namespace Lagkage.Database;

public static class DBServiceCollectionExtensions
{
   public static IServiceCollection AddRepositories(this IServiceCollection services)
   {
      Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

      return services.AddScoped<CakeLayerRepository>();
   }
}

