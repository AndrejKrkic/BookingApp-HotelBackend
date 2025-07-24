using HotelBackend;
using HotelBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Dodajte CORS konfiguraciju
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // URL frontend-a
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dodaj konfiguraciju za DbContext
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<IPromoCodeService, PromoCodeService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });


var app = builder.Build();

// Koristite CORS politiku
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

