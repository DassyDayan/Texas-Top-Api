using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Pickpong.BL.Interfaces;
using Pickpong.Entities;
using Pickpong.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Pickpong.BL.Classes
{
    public class LoginBL : ILoginBL
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _authorizationServiceURL;
        private readonly HttpClient _httpClient;
        private readonly int _sessionTimeout;
        private readonly IMemoryCache _memoryCache;

        // Mail settings
        private readonly string _fromAddress;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _server;
        private readonly int _port;

        public LoginBL(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _secretKey = _configuration.GetValue<string>("SecretKey") ??
                throw new InvalidOperationException("SecretKey missing in config");
            _sessionTimeout = _configuration.GetValue<int>("SessionTimeout");
            _authorizationServiceURL = _configuration.GetValue<string>("ItServiceURL") ??
                throw new InvalidOperationException("ItServiceURL missing in config");

            HttpClientHandler handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (req, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_authorizationServiceURL),
                Timeout = TimeSpan.FromSeconds(30)
            };

            IConfigurationSection smtpSection = _configuration.GetSection("Smtp");
            _fromAddress = smtpSection.GetValue<string>("FromAddress") ?? "";
            _userName = smtpSection.GetValue<string>("UserName") ?? "";
            _password = smtpSection.GetValue<string>("Password") ?? "";
            _server = smtpSection.GetValue<string>("Server") ?? "";
            _port = smtpSection.GetValue<int>("Port");
        }

        private string WrapBodyWithStyle(string bodyContent) =>
            $"<div style='direction: rtl; text-align: right; font-size: large; color:#4d6f8e;'>{bodyContent}</div>";

        public async Task SendMailAsync(MailModel mailModel)
        {
            using MailMessage message = new MailMessage(
                new MailAddress(_fromAddress), new MailAddress(mailModel.MailTo))
            {
                IsBodyHtml = true,
                Subject = mailModel.Subject,
                Body = WrapBodyWithStyle(mailModel.Body)
            };

            try
            {
                using SmtpClient smtpClient = new SmtpClient(_server, _port)
                {
                    Credentials = new NetworkCredential(_userName, _password),
                    EnableSsl = true
                };
                await smtpClient.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw;
            }
        }


        public async Task SendSMSAsync(
            SMSModel smsModel,
            string username,
            string senderName,
            SmsType smsType)
        {
            try
            {
                string escapedMessage = SecurityElement.Escape(smsModel.messageText);
                string apiToken = _configuration.GetValue<string>("inforuToken") ??
                    throw new InvalidOperationException("inforuToken missing in config");
                string deliveryUrl = _configuration.GetValue<string>("urlService") ?? string.Empty;

                string xml = BuildSMSXml
                    (escapedMessage, smsModel.mobilePhone, username, apiToken,
                    senderName, smsType, deliveryUrl);
                string encodedXml = HttpUtility.UrlEncode(xml, Encoding.UTF8);

                var requestBody = new { InforuXML = encodedXml };

                HttpResponseMessage response = await _httpClient
                    .PostAsJsonAsync("https://api.inforu.co.il/SendMessageXml.ashx", requestBody);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"SMS sent successfully to {smsModel.mobilePhone}: {responseContent}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending SMS to {smsModel.mobilePhone}: {ex.Message}");
                throw;
            }
        }


        private string BuildSMSXml(string message,
            string phoneNumber, string username,
            string apiToken, string senderName,
            SmsType smsType, string deliveryUrl)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Inforu>");
            sb.Append("<User>");
            sb.Append($"<Username>{username}</Username>");
            sb.Append($"<ApiToken>{apiToken}</ApiToken>");
            sb.Append("</User>");
            sb.Append("<Content Type=\"sms\">");
            sb.Append($"<Message>{message}</Message>");
            sb.Append("</Content>");
            sb.Append("<Recipients>");
            sb.Append($"<PhoneNumber>{phoneNumber}</PhoneNumber>");
            sb.Append("</Recipients>");
            sb.Append("<Settings>");
            sb.Append($"<Sender>{senderName}</Sender>");

            if (smsType == SmsType.Regular)
            {
                sb.Append("<SenderNumber>0548538934</SenderNumber>");
                sb.Append("<CustomerMessageID>1</CustomerMessageID>");
                sb.Append("<MessageInterval>0</MessageInterval>");
                if (!string.IsNullOrWhiteSpace(deliveryUrl))
                    sb.Append($"<DeliveryNotificationUrl>{deliveryUrl}</DeliveryNotificationUrl>");
            }

            sb.Append("</Settings>");
            sb.Append("</Inforu>");

            return sb.ToString();
        }

        public string GenerateToken(ResponseModel userModel)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string jsonString = JsonSerializer.Serialize(userModel);
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Sid, jsonString) }),
                Expires = DateTime.UtcNow.AddMinutes(_sessionTimeout),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task SendMailCompleteOrderAsync(MailModel mailModel) => await SendMailAsync(mailModel);

        public async Task SendOTPAsync(LoginParams loginParams, TcustomerDetail customer)
        {
            loginParams.OTP = new Random().Next(100000, 999999).ToString();
            string message = $"הקוד החד פעמי שלך הוא: {loginParams.OTP}";

            if (loginParams.isMail)
            {
                MailModel mailModel = new MailModel
                {
                    MailTo = customer.NvEmailAddress!,
                    Subject = "קוד חד פעמי לכניסה למערכת",
                    Body = message
                };
                await SendMailAsync(mailModel);
            }
            else
            {
                SMSModel smsModel = new SMSModel
                {
                    messageText = message,
                    mobilePhone = customer.NvPhoneNumber!
                };
                await SendSMSAsync(smsModel, "myny", "Texas-Top", SmsType.Regular);
            }

            _memoryCache.Set(loginParams.mobileOrMail, loginParams, TimeSpan.FromMinutes(5));
        }

        public async Task NotifyCustomerOfCompletedOrderAsync(string? email, string? phone, string messageText)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                MailModel mailModel = new MailModel
                {
                    MailTo = email,
                    Subject = "ההזמנה שלך התקבלה בהצלחה!",
                    Body = messageText
                };
                await SendMailAsync(mailModel);
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                SMSModel smsModel = new SMSModel
                {
                    messageText = messageText,
                    mobilePhone = phone
                };
                await SendSMSAsync(smsModel, "myny", "Texas-Top", SmsType.CompleteOrder);
            }
        }

    }

    public enum SmsType
    {
        Regular,
        CompleteOrder
    }
}