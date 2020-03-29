using employeeservice.Common;
using loginservice.Models;
using loginservice.Processors;
using loginservice.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class Startup
{
    public IConfigurationRoot Configuration { get; set; }

    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddJsonFile("vcap-local.json", optional: true) // when running locally, store VCAP_SERVICES credentials in vcap-local.json
            .AddEnvironmentVariables();

        Configuration = builder.Build();

        string vcapServices = Environment.GetEnvironmentVariable("VCAP_SERVICES");
        if (vcapServices != null)
        {
            dynamic json = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(vcapServices);

            // CF 'cloudantNoSQLDB' service
            if (json.ContainsKey("cloudantNoSQLDB"))
            {
                try
                {
                    Configuration["cloudantNoSQLDB:0:credentials:username"] = json["cloudantNoSQLDB"][0].credentials.username;
                    Console.WriteLine("username ");
                    Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:username"]);
                    Configuration["cloudantNoSQLDB:0:credentials:password"] = json["cloudantNoSQLDB"][0].credentials.password;
                    Console.WriteLine("password ");
                    Console.WriteLine(json["cloudantNoSQLDB"][0].credentials.password);
                    Configuration["cloudantNoSQLDB:0:credentials:host"] = json["cloudantNoSQLDB"][0].credentials.host;
                    Console.WriteLine("host ");
                    Console.WriteLine(json["cloudantNoSQLDB"][0].credentials.host);
                    Configuration["cloudantNoSQLDB:0:credentials:url"] = json["cloudantNoSQLDB"][0].credentials.url;
                    Console.WriteLine("url ");
                    Console.WriteLine(json["cloudantNoSQLDB"][0].credentials.url);
                }
                catch (Exception)
                {
                    // Failed to read Cloudant uri, ignore this and continue without a database
                }
            }
            // user-provided service with 'cloudant' in the name
            else if (json.ContainsKey("user-provided"))
            {
                foreach (var service in json["user-provided"])
                {
                    if (((String)service.name).Contains("cloudant"))
                    {
                        try
                        {
                            Configuration["cloudantNoSQLDB:0:credentials:username"] = json["cloudantNoSQLDB"][0].credentials.username;
                            Configuration["cloudantNoSQLDB:0:credentials:password"] = json["cloudantNoSQLDB"][0].credentials.password;
                            Configuration["cloudantNoSQLDB:0:credentials:host"] = json["cloudantNoSQLDB"][0].credentials.host;
                            Configuration["cloudantNoSQLDB:0:credentials:url"] = json["cloudantNoSQLDB"][0].credentials.url;
                        }
                        catch (Exception)
                        {
                            // Failed to read Cloudant uri, ignore this and continue without a database
                        }
                    }
                }
            }

        }
        else if (Configuration.GetSection("services").Exists())
        {
            try
            {
                Configuration["cloudantNoSQLDB:0:credentials:username"] = Configuration["services:cloudantNoSQLDB:0:credentials:username"];
                Console.WriteLine("username ");
                Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:username"]);
                Configuration["cloudantNoSQLDB:0:credentials:password"] = Configuration["services:cloudantNoSQLDB:0:credentials:password"];
                Console.WriteLine("password ");
                Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:password"]);
                Configuration["cloudantNoSQLDB:0:credentials:host"] = Configuration["services:cloudantNoSQLDB:0:credentials:host"];
                Console.WriteLine("host ");
                Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:host"]);
                Configuration["cloudantNoSQLDB:0:credentials:url"] = Configuration["services:cloudantNoSQLDB:0:credentials:url"];
                Console.WriteLine("url ");
                Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:url"]);
            }
            catch (Exception)
            {
                // Failed to read Cloudant uri, ignore this and continue without a database
            }

        }
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add framework services.
        //var allowedDomains = this.Configuration.GetSection("CorsSettings:AllowOrigins").Get<string[]>();
        //var allowedMethods = this.Configuration.GetSection("CorsSettings:AllowMethods").Get<string[]>();
        //var allowedHeaders = this.Configuration.GetSection("CorsSettings:AllowHeaders").Get<string[]>();
        //services.AddCors(
        //    o => o.AddPolicy(
        //        "CorsAllowAny",
        //        b =>
        //        {
        //            b//.WithOrigins(allowedDomains)
        //                 .WithMethods(allowedMethods)
        //                 .WithHeaders(allowedHeaders)
        //                 .AllowCredentials()
        //                 .AllowAnyOrigin();
        //        }));
        var allowedDomains = this.Configuration.GetSection("CorsSettings:AllowOrigins").Get<string[]>();
        var allowedMethods = this.Configuration.GetSection("CorsSettings:AllowMethods").Get<string[]>();
        var allowedHeaders = this.Configuration.GetSection("CorsSettings:AllowHeaders").Get<string[]>();
        services.AddCors(
            o => o.AddPolicy(
                "CorsAllowAny",
                b =>
                {
                    b//.WithOrigins(allowedDomains)
                         .WithMethods(allowedMethods)
                         .WithHeaders(allowedHeaders)
                         .AllowCredentials()
                         .AllowAnyOrigin();
                }));

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Info { Title = "Login master service", Version = "v1" });


            //Locate the XML file being generated by ASP.NET...
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile.ToLower());

            //... and tell Swagger to use those XML comments.
            c.IncludeXmlComments(xmlPath);

            // Swagger 2.+ support
            var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

            c.AddSecurityDefinition("Bearer", new ApiKeyScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = "header",
                Type = "apiKey"
            });
            c.AddSecurityRequirement(security);

            //c.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
            //c.AddSecurityDefinition("ApiKeyScheme", new ApiKeyScheme() { In = "header", Description = "Please insert AppID such as App-123456", Name = "AppID", Type = "apiKey" });
            //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[0] } });
            //c.AddSecurityDefinition("ApiKeyScheme", new ApiKeyScheme() { In = "header", Description = "Please insert ID", Name = "AppID", Type = "apiKey" });
            //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "ApiKeyScheme", new string[1] } });
        });
        var creds = new Creds()
        {
            username = Configuration["cloudantNoSQLDB:0:credentials:username"],
            password = Configuration["cloudantNoSQLDB:0:credentials:password"],
            host = Configuration["cloudantNoSQLDB:0:credentials:host"]
        };

        if (creds.username != null && creds.password != null && creds.host != null)
        {
            services.AddAuthorization();
            services.AddSingleton(typeof(Creds), creds);
            services.AddTransient<ICloudantService, CloudantService>();           
            
            services.AddTransient<IPostUserLoginProcessor, PostuserLoginProcessor>();
            services.AddTransient<IPutUserLoginProcessor, PutUserLoginProcessor>();
            services.AddTransient<IPostW3UserLoginProcessor, PostW3userLoginProcessor>();
            services.AddTransient<IPutW3UserLoginProcessor, PutW3UserLoginProcessor>();
            services.AddTransient<IPostUserRightsProcessor, PostuserRightsProcessor>();
            services.AddTransient<IPutUserRightsProcessor, PutUserRightsProcessor>();
            services.AddTransient<LoggingHandler>();
            services.AddHttpClient("cloudant", client =>
            {
                var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(creds.username + ":" + creds.password));

                client.BaseAddress = new Uri(Configuration["cloudantNoSQLDB:0:credentials:url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            })
            .AddHttpMessageHandler<LoggingHandler>();
        }

        services.AddMvc();

    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        var cloudantService = ((ICloudantService)app.ApplicationServices.GetService(typeof(ICloudantService)));

        loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        loggerFactory.AddDebug();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
                .AddCustomHeader("X-Content-Type-Options", "nosniff")
                .AddCustomHeader("Cache-Control", "no-cache, no-store, must-revalidate")
                .AddCustomHeader("Pragma", "no-cache")
                .AddCustomHeader("Expires", "-1")
                //.AddCustomHeader("Access-Control-Allow-Methods", "*")
                .AddCustomHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
                .AddCustomHeader("Access-Control-Allow-Headers", "Content-Type,Authorization,AppID")
                .AddCustomHeader("Access-Control-Allow-Origin", "https://employeemaster.mybluemix.net"));

        app.UseDefaultFiles();
        app.UseStaticFiles();
        //app.UseCors("CorsAllowAny");
        app.UseCors("CorsAllowAny");
        app.UseMvc(routes =>
        {
            routes.MapRoute(
             name: "default",
             template: "{controller=Home}/{action=Index}/{id?}");
        });

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Login Master Web API V1");
            c.RoutePrefix = "swagger";
            //c.SupportedSubmitMethods();
        });
    }

    class LoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine("{0}\t{1}", request.Method, request.RequestUri);
            var response = await base.SendAsync(request, cancellationToken);
            Console.WriteLine(response.StatusCode);
            return response;
        }
    }
}