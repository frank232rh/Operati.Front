using Microsoft.EntityFrameworkCore;

namespace Prueba.Data
{
    public class PruebaDbContext : DbContext
    {
        public PruebaDbContext()
        {
        }
        public PruebaDbContext(DbContextOptions<PruebaDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Helpers.ContextConfiguration.ConexionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            }

        }

    }
}