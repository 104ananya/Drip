using Drip.Models;
using Microsoft.EntityFrameworkCore;        // so we can use DbContext, DbSet, ModelBuilder, etc.

namespace Drip.Data;

// EF Core's brain — maps C# classes to database tables
public class AppDbContext : DbContext       // INHERITANCE
{
    // Constructor — receives connection info from Program.cs via DI
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }     // base refers to the parent class (DbContext in this case).

    // Each DbSet = one table in the database
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Expense> Expenses => Set<Expense>();

    // Fluent API — configure how C# models map to PostgreSQL tables
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map to snake_case table names (PostgreSQL convention)
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Name).HasColumnName("name").HasMaxLength(100);
            entity.Property(u => u.Email).HasColumnName("email").HasMaxLength(255);
            entity.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(255);
            entity.Property(u => u.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.Name).HasColumnName("name").HasMaxLength(100);
            entity.Property(c => c.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.HasIndex(c => c.Name).IsUnique();
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("expenses");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Amount).HasColumnName("amount").HasColumnType("decimal(10,2)");
            entity.Property(e => e.Merchant).HasColumnName("merchant").HasMaxLength(200);
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").HasMaxLength(50);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ExpenseDate).HasColumnName("expense_date").HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");

            // Relationships
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Expenses)
                  .HasForeignKey(e => e.UserId);

            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Expenses)
                  .HasForeignKey(e => e.CategoryId);
        });
    }
}
