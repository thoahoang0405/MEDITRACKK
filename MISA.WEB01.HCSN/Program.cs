
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


var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();





