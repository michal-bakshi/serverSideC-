using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using TodoApi;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ToDoDB"),
        ServerVersion.Parse("8.0.40-mysql")));

builder.Services.AddTransient<ToDoDbContext>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()  
            .AllowAnyMethod()     
            .AllowAnyHeader();   
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["Jwt:SecretKey"] ?? "DefaultSecretKey123456"; // Fallback key
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,  
            ValidIssuer = builder.Configuration["Jwt:Issuer"],    
            ValidAudience = builder.Configuration["Jwt:Audience"], 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();



var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
string GenerateJwtToken(User user, IConfiguration config)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
        issuer: config["Jwt:Issuer"],
        audience: config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(120),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapGet("/", () => "Hello World!");


app.MapGet("/get", async (ToDoDbContext context) =>
{
    var items = await context.Items.ToListAsync();
    return Results.Ok(items);
});
app.MapPost("/", async(ToDoDbContext context,Item newItem) => {
        await context.Items.AddAsync(newItem);
        await context.SaveChangesAsync();
        return Results.Created($"/{newItem.Id}",newItem);

});

app.MapPut("/{id}/{isComplete}",async (ToDoDbContext context,int id,bool isComplete) =>{
     var existingItem = await context.Items.FindAsync(id);

    if (existingItem == null)
    {
        return Results.NotFound($"Item with ID {id} not found.");
    }
    existingItem.IsComplete = isComplete;

    await context.SaveChangesAsync();

    return Results.Ok(existingItem);
});
app.MapDelete("/{id}", async(ToDoDbContext context,int id) => {
       var itemToDelete = await context.Items.FindAsync(id);
    if (itemToDelete == null)
        return Results.NotFound($"Item with ID {id} not found.");
     context.Items.Remove(itemToDelete);
     await context.SaveChangesAsync();
    return Results.Ok($"Item with ID {id} has been deleted successfully");

});

app.MapPost("/login", async (ToDoDbContext context,User user, IConfiguration config) =>
{
    var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
    if (existingUser == null)
    {
        return Results.NotFound("User not found");
    }
   var token = GenerateJwtToken(user, config);
    return Results.Ok(new { token });
});

app.MapPost("/register", async (ToDoDbContext context,User newUser,IConfiguration config) => {
    var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
    if (existingUser != null)
    {
        return Results.BadRequest("User with this email already exists.");
    }
    await context.Users.AddAsync(newUser);
    await context.SaveChangesAsync();
    var token = GenerateJwtToken(newUser, config);
    return Results.Ok(new { token });

});


app.MapMethods("/api", new[] { "OPTIONS", "HEAD" }, 
                  () => "This is an options or head request ");








app.Run();



