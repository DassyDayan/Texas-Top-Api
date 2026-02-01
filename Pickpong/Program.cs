using Microsoft.EntityFrameworkCore;
using Pickpong.BL.Carpet;
using Pickpong.BL.Classes;
using Pickpong.BL.Interfaces;
using Pickpong.DAL.Classes;
using Pickpong.DAL.Interfaces;
using Pickpong.Dto.Classes;
using Pickpong.Models;
using Pickpong.Services.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TexasTopContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TexasTopDb"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);

builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTexastopClient", policy =>
        policy.WithOrigins(
                "https://texastop.webit.systems",
                "http://texastop.webit.systems",
                "https://localhost:4200", // אם מפתחים עם Angular מקומית
                "http://localhost:4200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddHttpContextAccessor();//

builder.Services.AddScoped<ICarpetService, CarpetService>();
builder.Services.AddScoped<IPlayerServiceBL, PlayerServiceBL>();
builder.Services.AddScoped<IBoardSettingsDL, BoardSettingsDL>();
builder.Services.AddScoped<IBoardSettingsBL, BoardSettingsBL>();
builder.Services.AddScoped<ICustomizesDL, CustomizesDL>();
builder.Services.AddScoped<ICustomizesBL, CustomizesBL>();
builder.Services.AddScoped<IFileUploadBL, FileUploadBL>();
builder.Services.AddScoped<IFileUploadDL, FileUploadDL>();
builder.Services.AddScoped<ICarpetDL, CarpetDL>();
builder.Services.AddScoped<IOrderServiceBL, OrderServiceBL>();
builder.Services.AddScoped<ICartServiceBL, CartServiceBL>();
builder.Services.AddScoped<ICartDL, CartDL>();
builder.Services.AddScoped<IOrderDL, OrderDL>();
builder.Services.AddScoped<ILoginBL, LoginBL>();
builder.Services.AddScoped<IPlayerDL, PlayerDL>();
builder.Services.AddScoped<ILoginDL, LoginDL>();
builder.Services.AddScoped<IManagementBL, ManagementBL>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowTexastopClient");
app.UseAuthorization();
app.MapControllers();

app.Run();