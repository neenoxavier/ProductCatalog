using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Data;
using ProductCatalog.Data.Interfaces;
using ProductCatalog.Data.Repository;
using Swashbuckle.AspNetCore.Swagger;

namespace ProductCatalog
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
			// Add DBContext details
			services.AddDbContext<ProductDBContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("productConnectionStr"))
				);
			// Bind Repositories
			services.AddTransient<IProductRepository, ProductRepository>();
			services.AddSingleton<IConfiguration>(Configuration);

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/dist";
			});

			// Add Swagger Doc
			services.AddSwaggerGen(swag =>
				swag.SwaggerDoc("productV1", new Info { Title = "Product API", Description = "Product Catalog API Details" })
			);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

			// Add Swagger configurations
			app.UseSwagger();
			app.UseSwaggerUI(swag =>
			{
				swag.SwaggerEndpoint("/swagger/productV1/swagger.json", "Product API");

			});

			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
			
        }
    }
}
