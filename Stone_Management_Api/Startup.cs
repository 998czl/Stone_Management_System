using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebCore_Entity;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace Stone_Management_Api
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
			services.AddControllers();
			//���ð汾
			services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			//����httpcontext
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			//����session
			services.AddSession();

			Container.RegisterAll();

			//�����������
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins",
						builder => //builder.AllowAnyOrigin()
						builder.WithOrigins("localhost:5000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
			});

			//ע��Swagger������������һ���Ͷ��Swagger �ĵ�
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "API �ĵ�",
					Version = "v1",
					Description = "��Ŀ���нӿ�˵��"
				});

				//Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				
			}		
			//����Https
			app.UseHttpsRedirection();
			//����Session
			app.UseSession();
			//�����������
			app.UseCors("AllowAllOrigins");

			//ʹ�м����������Swagger��ΪJSON�˵�
			app.UseSwagger();
			////�����м�����ṩ�û����棨HTML��js��CSS�ȣ����ر���ָ��JSON�˵�
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");				
			});

			//����Mvc
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "api/{controller}/{action}/{id?}",
					defaults: new { controller = "home", action = "index" });			
			});

			//ע�����
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}
	}
}
