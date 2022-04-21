using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SignalRChat.Hubs;
using SweperBackend.Automap;
using SweperBackend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SweperBackendContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("SweperBackendContext"), x => x.UseNetTopologySuite()));
var services = builder.Services;
var configuration = builder.Configuration;


builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
services.AddAutoMapper(typeof(SweperProfile));
services.AddSingleton<ChatHub>();


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("192.168.1.184").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

using (StreamReader r = new StreamReader(@"C:\Users\George\source\repos\SweperBackend\Firebase\firebase.json"))
{
    string json = r.ReadToEnd();
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromJson(json)
    }); ;
}

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["Jwt:Firebase:ValidIssuer"];
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Firebase:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:Firebase:ValidAudience"]
    };
});



var app = builder.Build();
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//todo for prod turn this on
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub", options =>
    {
        options.Transports =
            HttpTransportType.WebSockets |
            HttpTransportType.LongPolling;
    });
});

app.MapControllers();

app.UseCors();


app.Run();
