using APIMDevOpsCodeGenerator.OAuth;
using APIMDevOpsCodeGenerator.Web.AutoMapper;
using AutoMapper;
using DevOpsResourceCreator;
using DevOpsResourceCreator.Repository;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace APIMDevOpsCodeGenerator.Web
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
            services.AddMvc();
            services.AddControllersWithViews().AddJsonOptions(options => 
                options.JsonSerializerOptions.Converters.Add(new IntToStringConverter()));
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRepoRepository, RepoRepsitory>();
            services.AddScoped<IPipelineRepository, PipelineRepsitory>();
            services.AddScoped<IDevOpsRestApi, DevOpsRestApi>();
            services.AddScoped<IDevOpsRepository, DevOpsRepository>();
            services.AddScoped<IVarGroupRepository, VarGroupRepsitory>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddApplicationInsightsTelemetry();
            

             services.AddScoped<IExtensionRepository, ExtensionRepository>();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = AzureDevOpsAuthenticationDefaults.AuthenticationScheme;
            })

            .AddCookie(options =>
            {
                options.LoginPath = "/signin";
                options.LogoutPath = "/signout";
            })

            .AddAzureDevOps(options =>
            {
                // These represent the App ID and Client Secret portions of your Azure DevOps Application
                // settings
                options.ClientId = Configuration["AzureAppId"];
                options.ClientSecret = Configuration["AzureClientSecret"];

                // These must be the *exact* scopes defined in your application
                options.Scope.Add("vso.build_execute");
                options.Scope.Add("vso.code_full"); 
                options.Scope.Add("vso.code_status"); 
                options.Scope.Add("vso.dashboards_manage"); 
                options.Scope.Add("vso.environment_manage"); 
                options.Scope.Add("vso.extension.data_write"); 
                options.Scope.Add("vso.extension_manage"); 
                options.Scope.Add("vso.gallery_manage"); 
                options.Scope.Add("vso.graph_manage"); 
                options.Scope.Add("vso.identity_manage"); 
                options.Scope.Add("vso.machinegroup_manage"); 
                options.Scope.Add("vso.notification_manage"); 
                options.Scope.Add("vso.profile_write");
                options.Scope.Add("vso.project_manage"); 
                options.Scope.Add("vso.release_manage"); 
                options.Scope.Add("vso.serviceendpoint_manage"); 
                options.Scope.Add("vso.taskgroups_manage"); 
                options.Scope.Add("vso.threads_full"); 
                options.Scope.Add("vso.tokenadministration"); 
                options.Scope.Add("vso.variablegroups_manage"); 
                options.Scope.Add("vso.work_full");

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

           app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapDefaultControllerRoute();
            //    // endpoints.MapRazorPages();
            // });
        }
    }
}
