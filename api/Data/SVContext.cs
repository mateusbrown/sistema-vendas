using Microsoft.EntityFrameworkCore;
using SistemaVendasApi.Data.Models;

namespace SistemaVendasApi.Data;
public class SVContext(DbContextOptions<SVContext> options) : DbContext(options)
{
    public DbSet<Produtos> Produtos {get;set;}
    public DbSet<Vendas> Vendas {get;set;}
    public DbSet<VendasDetalhe> VendasDetalhe {get;set;}
}