
using MEDITRACK.BL;
using MEDITRACK.DL;
using MEDITRACK.BL.BaseBL;
using MEDITRACK.BL.AppointmentBL;
using MEDITRACK.DL.AppointmentDL;
using MEDITRACK.BL.PrescriptionBL;
using MEDITRACK.DL.PrescriptionDL;
using MEDITRACK.BL.AccountBL;
using MEDITRACK.DL.AccountDL;
using MEDITRACK.BL.RecordBL;
using MEDITRACK.DL.RecordDL;
using MEDITRACK.DL.FamilyMemberDL;
using MEDITRACK.BL.NoticeBL;
using MEDITRACK.DL.NoticeDL;
using MEDITRACK.BL.FamilyMemberBL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using MEDITRACK.Controllers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
// Add services to the container.
builder.Services.AddScoped<IAppointmentDL, AppointmentDL>();
builder.Services.AddScoped<IAppointmentBL, AppointmentBL>();
builder.Services.AddScoped<IPrescriptionBL, PrescriptionBL>();
builder.Services.AddScoped<IPrescriptionDL, PrescriptionDL>();
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserDL, UserDL>();
builder.Services.AddScoped<IRecordBL, RecordBL>();
builder.Services.AddScoped<IRecordDL, RecordDL>();
builder.Services.AddScoped<IFamilyMemberDL, FamilyMemberDL>();
builder.Services.AddScoped<IFamilyMemberBL, FamilyMemberBL>();
builder.Services.AddScoped<INotificationBL, NotificationBL>();
builder.Services.AddScoped<INotificationDL, NotificationDL>();

builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
UsersController.configuration = builder.Configuration;

DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySqlConnection");
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                      });
});
// cấu hình swagger authorize
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
});

// authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey
       (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        //(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false
    };
});
builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();





