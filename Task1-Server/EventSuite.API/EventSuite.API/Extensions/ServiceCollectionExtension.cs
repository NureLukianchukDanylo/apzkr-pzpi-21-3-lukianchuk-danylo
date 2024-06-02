﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EventSuite.Core.Mappings;
using EventSuite.Core.Models;
using EventSuite.DAL.Repositories.Implementations;
using EventSuite.DAL.Repositories.Interfaces;
using EventSuite.BLL.Services.Implementations;
using EventSuite.BLL.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace EventSuite.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        // Method for configuring swagger
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        // Method for registering services
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventResourceService, EventResourceService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IMallService, MallService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<ISmartBraceletService, SmartBraceletService>();
        }

        // Method for registering repositories
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IGenericRepository<Event>, GenericRepository<Event>>();
            services.AddTransient<IGenericRepository<Location>, GenericRepository<Location>>();
            services.AddTransient<IGenericRepository<Mall>, GenericRepository<Mall>>();
            services.AddTransient<IGenericRepository<Resource>, GenericRepository<Resource>>();
            services.AddTransient<IGenericRepository<Registration>, GenericRepository<Registration>>();
            services.AddTransient<IGenericRepository<Ticket>, GenericRepository<Ticket>>();
            services.AddTransient<IGenericRepository<Venue>, GenericRepository<Venue>>();
            services.AddTransient<IGenericRepository<EventResource>, GenericRepository<EventResource>>();
            services.AddTransient<IGenericRepository<Reservation>, GenericRepository<Reservation>>();
            services.AddTransient<IGenericRepository<SmartBracelet>, GenericRepository<SmartBracelet>>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // Method for configuring jwt authentication
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        // Method for configuring mappings
        public static void ConfigureMapping(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            var mapperConfig = new MapperConfiguration(map =>
            {
                map.AddProfile<UserMappingProfile>();
                map.AddProfile<LocationMappingProfile>();
                map.AddProfile<EventMappingProfile>();
                map.AddProfile<EventResourceMappingProfile>();
                map.AddProfile<MallMappingProfile>();
                map.AddProfile<RegistrationMappingProfile>();
                map.AddProfile<ReservationMappingProfile>();
                map.AddProfile<ResourceMappingProfile>();
                map.AddProfile<TicketMappingProfile>();
                map.AddProfile<VenueMappingProfile>();
                map.AddProfile<SmartBraceletMappingProfile>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }

        // Method for configurinf localization
        public static void ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(x => x.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US")
                        {
                            DateTimeFormat =
                            {
                                LongTimePattern = "MM/DD/YYYY",
                                ShortTimePattern = "MM/DD/YYYY"
                            }
                        },
                        new CultureInfo("uk-UA")
                        {
                            DateTimeFormat =
                            {
                                LongTimePattern = "DD/MM/YYYY",
                                ShortTimePattern = "DD/MM/YYYY"
                            }
                        }
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });
        }
    }
}
