// Data/ApplicationDbContext.cs
using LojaOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaOnline.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
    }
}
