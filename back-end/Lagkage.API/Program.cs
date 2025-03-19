using System.Data;
using Lagkage.Database;
using Npgsql;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddRepositories();
        builder.Services.AddControllers();

        var app = builder.Build();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}