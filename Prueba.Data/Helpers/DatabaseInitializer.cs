using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba.Data.Helpers
{
    public class DatabaseInitializer
    {
        /// <summary>
        /// Inicializa los registros default en la base de datos.
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        public static void Inicializar(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<PruebaDbContext>();
            context.Database.EnsureCreated();


            context.SaveChanges();
        }
    }
}
