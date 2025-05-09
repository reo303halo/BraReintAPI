using System.Text;
using bra_reint_API.Data;
using bra_reint_API.Services.AuthServices;
using bra_reint_API.Services.BookingServices;
using bra_reint_API.Services.BookingTypeServices;
using bra_reint_API.Services.EmailServices;
using bra_reint_API.Services.PostalCodeServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration
                           .GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' not found.");

var jwtString = builder.Configuration
                           .GetSection("Jwt:Key").Value ??
                       throw new InvalidOperationException(
                           "Section or key 'Jwt:Key' not found.");


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB Services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Jwt Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        var byteKey = Encoding.UTF8.GetBytes(jwtString);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateActor = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,                                            // In the file appsettings.json:
            ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,     // Change to the location of the server issuing the token
            ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value, // Change to the location of the client
            IssuerSigningKey = new SymmetricSecurityKey(byteKey)
        };
    });

// Interfaces
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IBookingService, BookingService>();
builder.Services.AddTransient<IBookingTypeService, BookingTypeService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IPostalCodeService, PostalCodeService>();


// APP
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();