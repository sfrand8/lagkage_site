using System.Data;
using Lagkage.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Lagkage.Database;

public static class DBServiceCollectionExtensions
{
   public static IServiceCollection AddRepositories(this IServiceCollection services)
   {
      services.AddScoped<IDbConnection>(sp =>
      {
         var config = sp.GetRequiredService<IConfiguration>();
         return new NpgsqlConnection(config.GetConnectionString("DefaultConnection"));
      });
      Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

      return services.AddScoped<ICakeLayerRepository, CakeLayerRepository>();
   }
}

