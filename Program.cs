
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Steeltoe.Discovery.Client;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//For Microsoft SQL server:
//builder.Services.AddDbContext<CSRAgentAuthServiceContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("CSRAgentAuthServiceContext") ?? throw new InvalidOperationException("Connection string 'CSRAgentAuthServiceContext' not found.")));

//builder.Services.AddDbContext<AdminAuthServiceContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("AdminAuthServiceContext") ?? throw new InvalidOperationException("Connection string 'AdminAuthServiceContext' not found.")));

//builder.Services.AddDbContext<UserAuthServiceContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("UserAuthServiceContext") ?? throw new InvalidOperationException("Connection string 'UserAuthServiceContext' not found.")));


//For mySQL:
//Configuration file - appsettings
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

//MySql connection configuration
//builder.Services.AddDbContext<CSRAgentAuthServiceContext>
//    (option => option.UseMySql(configuration.GetConnectionString("CSRAgentAuthServiceContext"), ServerVersion.Parse("8.0.32-mysql")));


//builder.Services.AddDbContext<AdminAuthServiceContext>
//    (option => option.UseMySql(configuration.GetConnectionString("AdminAuthServiceContext"), ServerVersion.Parse("8.0.32-mysql")));


//builder.Services.AddDbContext<UserAuthServiceContext>
//    (option => option.UseMySql(configuration.GetConnectionString("UserAuthServiceContext"), ServerVersion.Parse("8.0.32-mysql")));




//-------------------------------------EUREKA with mySQL-------------------------
var connectionstring = configuration["ConnectionStrings:UserAuthServiceContext"];
builder.Services.AddDbContext<UserAuthServiceContext>
    (option => option.UseMySql(
    connectionstring,
    ServerVersion.Parse("8.0.32-mysql"),
    option =>
    {
        option.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: System.TimeSpan.FromSeconds(5),
        errorNumbersToAdd: null);
    }
));

var connectionstring2 = configuration["ConnectionStrings:AdminAuthServiceContext"];
builder.Services.AddDbContext<AdminAuthServiceContext>
    (option => option.UseMySql(
    connectionstring,
    ServerVersion.Parse("8.0.32-mysql"),
    option =>
    {
        option.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: System.TimeSpan.FromSeconds(5),
        errorNumbersToAdd: null);
    }
));

var connectionstring3 = configuration["ConnectionStrings:CSRAgentAuthServiceContext"];
builder.Services.AddDbContext<CSRAgentAuthServiceContext>
    (option => option.UseMySql(
    connectionstring,
    ServerVersion.Parse("8.0.32-mysql"),
    option =>
    {
        option.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: System.TimeSpan.FromSeconds(5),
        errorNumbersToAdd: null);
    }
));


//----------------------------------------------------------------------------



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//this is for microsoft SQL
//builder.Services.AddDbContext<UserAuthServiceContext>(options =>
//{
//    options.UseSqlServer(configuration.GetConnectionString("AuthContext"));
//});


//Adding JWT Authentication 
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes("cgicanadatelecomkey"); //the key is changed with the TokenBuilder.cs key
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = false,
        //ValidIssuer = configuration["JWT:Telcom_Issuer"],
        //ValidIssuer = configuration["JWT:Telcom_Issuer"],
        //ValidAudience = configuration["JWT:Admin_Audience"],
        //ValidAudience = configuration["JWT:Admin_Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key)
    };
});

////Adding JWT Authentication for multiple endpoints using multiple secret keys
//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(admin =>
//{
//    var adminKey = Encoding.UTF8.GetBytes("admin-cgicanadatelecomkey"); //the key is changed with the TokenBuilder.cs key
//    admin.SaveToken = true;
//    admin.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        RequireExpirationTime = false,
//        //ValidIssuer = configuration["JWT:Issuer"],
//        //ValidAudience = configuration["JWT:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(adminKey)
//    };
//}).AddJwtBearer(user =>
//{
//    var userKey = Encoding.UTF8.GetBytes("user-cgicanadatelecomkey"); //the key is changed with the TokenBuilder.cs key
//    user.SaveToken = true;
//    user.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        RequireExpirationTime = false,
//        //ValidIssuer = configuration["JWT:Issuer"],
//        //ValidAudience = configuration["JWT:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(userKey)
//    };
//}).AddJwtBearer(agent =>
//{
//    var agentKey = Encoding.UTF8.GetBytes("agent-cgicanadatelecomkey"); //the key is changed with the TokenBuilder.cs key
//    agent.SaveToken = true;
//    agent.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        RequireExpirationTime = false,
//        //ValidIssuer = configuration["JWT:Issuer"],
//        //ValidAudience = configuration["JWT:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(agentKey)
//    };
//});


builder.Services.AddScoped<ITokenBuilderAdmin, TokenBuilderAdmin>();
builder.Services.AddScoped<ITokenBuilderUser, TokenBuilderUser>();
builder.Services.AddScoped<ITokenBuilderCSRAgent, TokenBuilderCSRAgent>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCors", builder =>
    {
        //builder.WithOrigins("http://localhost:5124")
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });

});

// Adding custom filter
//builder.Services.AddScoped<AddTokenToHeader>();

//applies the filter to all my action methods
//builder.Services.AddControllers(config => config.Filters.Add(typeof(AddTokenToHeaderAttribute)));

builder.Services.AddControllers();

//eureka
builder.Services.AddDiscoveryClient(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//eureka
//app.UseDiscoveryClient();

app.UseRouting();
app.UseCors("EnableCors");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CorsMiddleware>();



app.MapControllers();

app.Run();
//add these:
//app.UseMiddleware<CorsMiddleware>();
//app.UseCors("EnableCors");
//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

//app.Run();


//finished the exception hadling
//everything is working in my service.
//authentication and registration is working in all endpoints 
//working on the eureka and docker image,





//test your app by trying to break it with all kinds of inputs
//jwt
//we need backend validations