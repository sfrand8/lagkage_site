using Lagkage.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Lagkage.Database;

public static class DBServiceCollectionExtensions
{
   public static IServiceCollection AddRepositories(this IServiceCollection services)
   {
      Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

      return services.AddScoped<ICakeLayerRepository, CakeLayerRepository>();
   }
}

