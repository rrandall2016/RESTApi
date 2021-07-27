using DataStore.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebAPIYouTube
{
    public class Startup
    {

        private readonly IWebHostEnvironment _env;
        public Startup(IWebHostEnvironment env)
        {
            this._env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        //Adds Dependencies 
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsDevelopment())
            {
                services.AddDbContext<BugsContext>(options =>
                {
                    options.UseInMemoryDatabase("Bugs");
                });
            }
            //Options will be passed to BugsContext class to constructor, then to constructor of DB context base class
            services.AddDbContext<BugsContext>(options =>
            {
                options.UseInMemoryDatabase("Bugs");
            });
            //Adding dependencies and options for using Filters globally 
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //Contains all middleware added to the pipeline 
        //Configure middleware 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BugsContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Create the in-memory database for dev environment
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();

            app.UseRouting();//Always will see.

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }
}

//Entity Core Notes
//added class library, data store EF for all EF stuff in the library, decouple the EF technology 
//Added package, added BugsContext, added bugscontext tables, db set = table, override onmodelcreating
//OnmodelCreating makes the schema inside db with SQL server
//Added schema relationship and seeded data
//WebAPI added inmemory package, and configured db context in startup with BugsContext with DI, and made in memory db
