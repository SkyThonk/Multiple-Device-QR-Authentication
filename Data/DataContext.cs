namespace MultiDeviceQrLogin.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<UserModel> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().ToTable("Users");
        modelBuilder.Entity<UserModel>().HasKey(u => u.UserId);

        
        base.OnModelCreating(modelBuilder);
    }
}