using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AutenticacaoJWT
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            }).AddJwtBearer("Jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    // ValidateAudience = "a audiencia que voce quer validar",
                    ValidateIssuer = false,
                    // ValidateIssuer = "o emissor que voce quer validar",

                    ValidateIssuerSigningKey = true,
                    // a chave deve ter no minimo 17 caracteres isso é uma regra.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("minhachavesecreta")),

                    ValidateLifetime = true,
                    // ValidateLifetime = "valida os valores de expiração e não antes(notBefore)",

                    ClockSkew = TimeSpan.FromMinutes(5) // 5 minutos de tolerancia para expiração do token
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();


            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Pagina não encontrada");
            });
        }
    }
}
