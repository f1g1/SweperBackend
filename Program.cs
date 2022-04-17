using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SweperBackend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SweperBackendContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("SweperBackendContext") ?? throw new InvalidOperationException("Connection string 'SweperBackendContext' not found.")));
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("127.0.0.0.1").AllowAnyMethod().AllowAnyHeader();
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

app.MapControllers();

app.Run();
