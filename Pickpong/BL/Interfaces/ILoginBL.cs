using Pickpong.BL.Classes;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.BL.Interfaces
{
    public interface ILoginBL
    {
        Task SendMailAsync(MailModel mailModel);
        Task SendSMSAsync(SMSModel smsModel, string username, string senderName, SmsType smsType);
        string GenerateToken(ResponseModel userModel);
        Task SendMailCompleteOrderAsync(MailModel mailModel);
        Task SendOTPAsync(LoginParams loginParams, TcustomerDetail customer);
        Task NotifyCustomerOfCompletedOrderAsync(string? email, string? phone, string messageText);

    }
}