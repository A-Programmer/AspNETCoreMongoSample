using MongoDB.Driver;

namespace SchoolApi.ExtensionMethods;

public static class MongoConnectionStringExtensionMethod
{
    public static WebApplicationBuilder AddMongoDbConnectionString(
        this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddSingleton<IMongoClient>(_ =>
            {

                var connectionString = builder.Configuration
                    .GetSection("SchoolDatabaseSettings:ConnectionString")?
                    .Value;
                return new MongoClient(connectionString);
            });

        return builder;
    }
}