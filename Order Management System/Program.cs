using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities.Identity;
using Order_Management_System.Core.Repositories;
using Order_Management_System.Core.Services;
using Order_Management_System.Extensions;
using Order_Management_System.Middlewares;
using Order_Management_System.Repository.Data;
using Order_Management_System.Services;
using OrderManagementSystem.Repository;
using OrderManagementSystem.Repository.Identity;
using Order_Management_System.Seeds;
using StackExchange.Redis;
using System.Text;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net;
using Stripe;
using TokenService = Order_Management_System.Services.TokenService;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Errors;

namespace OrderManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services


            builder.Services.AddControllers();
                ///.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            //    options.JsonSerializerOptions.WriteIndented = true; // Optional: for readable JSON
            //}); ;
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<OrderManagementDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });


            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnectionString"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {



                var connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connection);
            });


            builder.Services.AppServices();


            builder.Services.AddScoped<ITokenService, TokenService>();


            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };

                options.Events = new JwtBearerEvents
                {
                   
                    OnChallenge = context =>
                    {
                        
                        if (context.AuthenticateFailure != null)
                        {
                            context.HandleResponse(); 
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            var result = JsonSerializer.Serialize(new { message = "You are not authorized. Please provide a valid token." });
                            return context.Response.WriteAsync(result);
                        }
                        return Task.CompletedTask; 
                    },

                    OnForbidden = context =>
                    {
                        
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new { message = "You do not have permission." });
                        return context.Response.WriteAsync(result);
                    }
                };

            });



            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.RequireRole("Admin" ));
                options.AddPolicy("customer", policy => policy.RequireRole("Admin" , "Customer"));
            });


            #endregion

            builder.Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var error =actionContext.ModelState.Where(P=>P.Value.Errors.Count()>0 )
                    .SelectMany(P=>P.Value.Errors)
                    .Select(E=>E.ErrorMessage)
                    .ToArray();

                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = error
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);

                };
            });



            var app = builder.Build();



            #region Update Database

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var dbContext = services.GetRequiredService<OrderManagementDbContext>();
                    await dbContext.Database.MigrateAsync();

                    var identityDb = services.GetRequiredService<AppIdentityDbContext>();
                    await identityDb.Database.MigrateAsync();

                    // Seed roles and users
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await DefaultRoles.SeedAsync(roleManager);

                    var userManager = services.GetRequiredService<UserManager<User>>();
                    await DefaultUsers.SeedAdminUserAsync(userManager);
                    await DefaultUsers.SeedCustomerUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An Error occurred during database update");
                }
            }

            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseMiddleware<ExceptionMiddleWare>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
