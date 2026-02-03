using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Web.Microondas.Infrastructure.DatabaseContext
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Web.Microondas.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var base64Conn = configuration.GetConnectionString("DefaultConnection");
            var decodedConn = Encoding.UTF8.GetString(Convert.FromBase64String(base64Conn));

            optionsBuilder.UseSqlServer(decodedConn);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}