using MeterTrackerApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<MeterService>();
builder.Services.AddScoped<PremiseService>();
builder.Services.AddScoped<ReadingService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// TEST — remove later
// await RunDeleteBehaviorTests(app);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();




static async Task RunDeleteBehaviorTests(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // var user = new User {Name="Александр Иванович", Role=Role.User, Password="Sosite1"};
    // db.Users.Add(user);
    // await db.SaveChangesAsync();

    var user = await db.Users.FindAsync(1);

    // var premise = new Premise{Address="улица пушкина", ResponsibleUserId=user.Id};
    // db.Add(premise);
    // await db.SaveChangesAsync();

    var premise = await db.Premises.FindAsync(1);

    // var meter = new Meter{PremiseId=1, MeterType=MeterType.electricity};
    // db.Add(meter);
    // await db.SaveChangesAsync();

    var meter = await db.Meters.FindAsync(1);

    // var reading = new Reading{MeterId=meter.Id, Value=12000, ReadingDate=DateTime.UtcNow, SubmittedById=user.Id};
    // db.Readings.Add(reading);
    // await db.SaveChangesAsync();

    var reading = await db.Readings.FindAsync(1);

    db.Readings.Remove(reading);
    await db.SaveChangesAsync();

    db.Users.Remove(user);
    await db.SaveChangesAsync();

    // var allUser = await db.Users.ToListAsync();
    // foreach (var el in allUser)
    // {
    //     System.Console.WriteLine($"Роль пользователя: {el.Role}, имя пользователя: {el.Name}, пароль пользователя: {el.Password}");
    // }

    // var allPremise = await db.Premises.Include(p=>p.ResponsibleUser).ToListAsync();
    // foreach (var el in allPremise)
    // {
    //     System.Console.WriteLine($"Адрес: {el.Address} - Ответственный {el.ResponsibleUser?.Name}");
    // }
}
}