using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .Build();

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        builder.UseSqlServer(connectionString,
            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

        // Create a minimal service provider
        var services = new ServiceCollection();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DesignTimeDbContextFactory).Assembly));
        var serviceProvider = services.BuildServiceProvider();

        return new AppDbContext(
            builder.Options, 
            serviceProvider.GetRequiredService<IHttpContextAccessor>(), 
            serviceProvider.GetRequiredService<IPublisher>());
    }
}