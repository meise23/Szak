using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
  public DbSet <Product> Products {get;set;}
  public DbSet <Partner> Partners {get;set;}
  public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
  public DbSet<Package> Packages => Set<Package>();
  public DbSet<PackageItem> PackageItems => Set<PackageItem>();

  public DbSet<Sale> Sales => Set<Sale>();
  public DbSet<SaleLine> SaleLines => Set<SaleLine>();

  public DbSet<Promotion> Promotions => Set<Promotion>();
  public DbSet<PromotionProduct> PromotionProducts => Set<PromotionProduct>();
  public DbSet<PromotionGroup> PromotionGroups => Set<PromotionGroup>();

  public DbSet<Company> Companies {get;set;}

  /*protected override void  OnConfiguring(DbContextOptionsBuilder options) {
     options.UseSqlite("Data Source=demo.db");
  }*/
  protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<PromotionProduct>()
        .HasKey(pp => new { pp.PromotionId, pp.ProductId });

    modelBuilder.Entity<PromotionGroup>()
        .HasKey(pg => new { pg.PromotionId, pg.ProductGroupId });
}

}