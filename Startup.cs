using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class Startup
{
    public Startup(WebApplicationBuilder builder, IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public Startup(ConfigurationManager configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void configureServices(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.DisableImplicitFromServicesParameters = true;
        });

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddDbContext<ApiDbContext>(options => options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")
                ));

        services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

        _ = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        });

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                        .AddEntityFrameworkStores<ApiDbContext>();

    }

    public void Configure(WebApplication app, IWebHostEnvironment environment)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.UseAuthentication();

        app.MapControllers();


    }
}