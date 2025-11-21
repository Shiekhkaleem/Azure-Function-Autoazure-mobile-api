using AutoAzureMob.API.Helper;
using AutoAzureMob.DAL.DAL;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using AutoAzureMob.API.ActionFilter;
using AutoAzureMob.BLL.BLL;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Google.Protobuf.WellKnownTypes;
using AutoAzureMob.API.ExceptionHandling;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using AutoAzureMob.API.Logger;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults((context,builder)=>
    {
        //builder.UseMiddleware<LoggingMiddleware>();
      //  builder.UseMiddleware<ErrorHandlerMiddleware>();
        builder.UseWhen<ErrorHandlerMiddleware>(context =>
        {
            // We want to use this middleware only for http trigger invocations.
            return context.FunctionDefinition.InputBindings.Values
                          .First(a => a.Type.EndsWith("Trigger")).Type == "httpTrigger";
        });
        builder.UseWhen<JWTMiddleware>(context => 
        {
            // We want to use this middleware only for http trigger invocations.
            return context.FunctionDefinition.InputBindings.Values
                          .First(a => a.Type.EndsWith("Trigger")).Type == "httpTrigger";
        });
        builder.UseFunctionExecutionMiddleware();
        //builder.UseNewtonsoftJson();
        builder.Services.Configure<JsonSerializerOptions>(options =>
        {
            options.AllowTrailingCommas = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.PropertyNameCaseInsensitive = true;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        });
    })
    .ConfigureOpenApi()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ExecuteContext>();
        services.AddScoped<UserHandler>();
        //Swagger Configuration

        //IConfiguration 
        var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddEnvironmentVariables().Build();
        ConfigurationHelper.Initialize(configuration);
        var connectionString = configuration.GetConnectionString("AutoAzure-DEV");
        var fileName = configuration.GetSection("FirebaseGoogleAuthFile").Value;
        // FireBase
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(fileName),
        });

        services.AddSingleton(sp =>
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return new ExecuteContext
            {
                Connection = connection,
                Transaction = null
            };
        });
        //Authentication
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer("JwtBearer", jwtBearerOptions =>
        {
            jwtBearerOptions.RequireHttpsMetadata = false;
            jwtBearerOptions.SaveToken = true;
            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtToken:Key").Value)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromDays(365)
            };
        });
        services.AddAuthorization();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConfiguration(configuration.GetSection("logging"));
        });
       
    })
    .Build();

host.Run();
