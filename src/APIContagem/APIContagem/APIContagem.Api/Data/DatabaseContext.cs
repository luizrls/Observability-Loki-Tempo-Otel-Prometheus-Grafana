using APIContagem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace APIContagem.Api.Data;

public class DatabaseContext : DbContext
{
    public DbSet<CountHistory>? Historicos { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CountHistory>(entity =>
        {
            entity.ToTable("HistoricoContagem");
            entity.HasKey(c => c.Id);
        });
    }
}