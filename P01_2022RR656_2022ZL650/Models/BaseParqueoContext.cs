using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace P01_2022RR656_2022ZL650.Models
{
    public class BaseParqueoContext: DbContext
    {
        public BaseParqueoContext(DbContextOptions<BaseParqueoContext> options) : base(options)



        {



        }


        public DbSet<Usuarios> Usuarios { get; set; }
        
        public DbSet<Reservas> Reservas { get; set; }

        public DbSet<EspaciosParqueo> EspaciosParqueos { get; set; }

        public DbSet<Sucursales> Sucursales { get; set; }




    }
}
