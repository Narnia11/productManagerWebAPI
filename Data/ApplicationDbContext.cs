using Microsoft.EntityFrameworkCore;
using ProductManagerWebAPI.Domain;

namespace ProductManagerWebAPI.Data;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options)
  { }

  public DbSet<Product> Products { get; set; }
}
