using System.Text;
using ElkoodProject.DependencyResolver;
using ElkoodProject.Task.DataAccess;
using ElkoodProject.User.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "add token",
        Name = "auth",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[] { }
    } });
});

string connS = builder.Configuration.GetConnectionString("defaultconnectionstring")!;

builder.Services.AddDbContext<TasksDbContext>(options => options.UseSqlServer(connS));
builder.Services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(connS));

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<UsersDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
});

builder.Services.
    AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GuestRolePolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == "Role" && c.Value == "GUEST")));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerRolePolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == "Role" && c.Value == "OWNER")));
});


builder.Services.RegisterTaskDependencies();
static async Task SeedIdentityDataAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = ["OWNER","GUEST"];

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var userEmail = "fajr.ma98@gmail.com";
    var user = await userManager.FindByEmailAsync(userEmail);
    if (user == null)
    {
        var newUser = new IdentityUser
        {
            Email = userEmail,
            UserName = "FajrSeed",
            EmailConfirmed = true,
            PhoneNumber = "0934343981"
        };

        var result = await userManager.CreateAsync(newUser, "12345678");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "OWNER");
        }
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
await SeedIdentityDataAsync(services);
