using System.Text.Json.Serialization;
using Lagkage.Database;
using Lagkage.MinimalApis;
using Scalar.AspNetCore;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddRepositories();
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
        });
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Lagkage.MinimalAPI")
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                    .WithTheme(ScalarTheme.Saturn);
            });
        }
        
        app.MapMinimalAPIs();

        app.UseHttpsRedirection();
        app.Run();
    }
}
