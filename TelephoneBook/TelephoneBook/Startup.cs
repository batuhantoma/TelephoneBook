using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelephoneBook.Models;
using TelephoneBook.Services;

namespace TelephoneBook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.Configure<KisilerDatabaseSettings>(Configuration.GetSection(nameof(KisilerDatabaseSettings)));
            services.AddSingleton<IKisilerDatabaseSettings>(sp => sp.GetRequiredService<IOptions<KisilerDatabaseSettings>>().Value);

            services.Configure<ReportStatusDatabaseSettings>(Configuration.GetSection(nameof(ReportStatusDatabaseSettings)));
            services.AddSingleton<IReportStatusDatabaseSettings>(sp => sp.GetRequiredService<IOptions<ReportStatusDatabaseSettings>>().Value);
            services.AddSingleton<KisilerApiService>();
            services.AddSingleton<ReportRequestService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
