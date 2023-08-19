
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace InvoiceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Invoice Management API",
                    Description = "The Invoice Management API is a simple CRUD (Create, Read, Update, Delete) web service designed to handle operations related to invoices. It provides endpoints for managing invoice records, including creation, retrieval, update, and deletion. The API follows RESTful principles and is documented using the OpenAPI specification, with interactive documentation available through Swagger UI.",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Simon Adam",
                        Email = "simon.adam.job@gmail.com",
                    },
                });

                //'C:\Users\simon\Desktop\InvoiceApi\InvoiceApi\bin\Debug\net7.0\InvoiceApi.xml'.

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice Api V1");
                });
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}