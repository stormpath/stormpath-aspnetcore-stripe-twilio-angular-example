using Microsoft.EntityFrameworkCore;

namespace WebApiAngularStorm.Models
{
  public class ApiContext : DbContext
  {
    public ApiContext (DbContextOptions<ApiContext> options) : base(options){ }

    public DbSet<Todo> Todos { get; set; }
  }
}