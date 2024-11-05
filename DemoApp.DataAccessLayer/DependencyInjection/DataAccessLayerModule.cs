using Autofac;
using DemoApp.Core.Configuration;
using DemoApp.DataAccessLayer.DbContext;
using DemoApp.DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.DataAccessLayer.DependencyInjection;

public class DataAccessLayerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context =>
        {
            var dbConfig = context.Resolve<IDbConfiguration>();
            Console.WriteLine(dbConfig.ConnectionString);
            var optionsBuilder = new DbContextOptionsBuilder<PostgresDbContext>();
            optionsBuilder.UseNpgsql(dbConfig.ConnectionString);
            return new PostgresDbContext(optionsBuilder.Options);
        }).AsSelf().InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(Repository<>))
            .As(typeof(IRepository<>))
            .InstancePerLifetimeScope();
    }
}