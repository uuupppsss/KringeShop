using KringeShopLib.Model;
using Microsoft.EntityFrameworkCore;

namespace KringeShopApi.HomeModel;

//public partial class KrinageShopDbContext : DbContext
//{
//    public KrinageShopDbContext()
//    {
//    }

//    public KrinageShopDbContext(DbContextOptions<KrinageShopDbContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<BasketItem> BasketItems { get; set; }

//    public virtual DbSet<Order> Orders { get; set; }

//    public virtual DbSet<OrderItem> OrderItems { get; set; }

//    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

//    public virtual DbSet<Product> Products { get; set; }

//    public virtual DbSet<ProductType> ProducTtypes { get; set; }

//    public virtual DbSet<SavedProduct> SavedProducts { get; set; }

//    public virtual DbSet<User> Users { get; set; }

//    public virtual DbSet<UserRole> UserRoles { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=localhost;port=3307;user=root;password=root;database=krinageshopdb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder
//            .UseCollation("utf8mb4_0900_ai_ci")
//            .HasCharSet("utf8mb4");

//        modelBuilder.Entity<BasketItem>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("basketitem");

//            entity.HasIndex(e => e.ProductId, "FK_basketitem_product_id");

//            entity.HasIndex(e => e.UserId, "FK_basketitem_user_id");

//            entity.Property(e => e.Id).HasColumnName("id");
//            entity.Property(e => e.Cost).HasPrecision(19, 2);

//            entity.HasOne(d => d.Product).WithMany(p => p.Basketitems)
//                .HasForeignKey(d => d.ProductId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_basketitem_product_id");

//            entity.HasOne(d => d.User).WithMany(p => p.BasketItems)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_basketitem_user_id");
//        });

//        modelBuilder.Entity<Order>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("order");

//            entity.HasIndex(e => e.StatusId, "FK_order_orderstatus_id");

//            entity.HasIndex(e => e.UserId, "FK_order_user_id");

//            entity.Property(e => e.Id).HasColumnName("id");
//            entity.Property(e => e.Adress).HasMaxLength(255);
//            entity.Property(e => e.CreateDate).HasColumnType("datetime");
//            entity.Property(e => e.FullCost).HasPrecision(19, 2);
//            entity.Property(e => e.RecieveDate).HasColumnType("datetime");

//            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.StatusId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_order_orderstatus_id");

//            entity.HasOne(d => d.User).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_order_user_id");
//        });

//        modelBuilder.Entity<OrderItem>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("orderitem");

//            entity.HasIndex(e => e.OrdeId, "FK_orderitem_order_id");

//            entity.HasIndex(e => e.ProductId, "FK_orderitem_product_id");

//            entity.Property(e => e.Id).HasColumnName("id");
//            entity.Property(e => e.Cost).HasPrecision(19, 2);

//            entity.HasOne(d => d.Orde).WithMany(p => p.Orderitems)
//                .HasForeignKey(d => d.OrdeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_orderitem_order_id");

//            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
//                .HasForeignKey(d => d.ProductId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_orderitem_product_id");
//        });

//        modelBuilder.Entity<OrderStatus>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("orderstatus");

//            entity.Property(e => e.Id).HasColumnName("id");
//            entity.Property(e => e.Title).HasMaxLength(255);
//        });

//        modelBuilder.Entity<Product>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("product");

//            entity.HasIndex(e => e.TypeId, "FK_product_producttype_id");

//            entity.Property(e => e.Id).HasColumnName("id");
//            entity.Property(e => e.Description).HasMaxLength(255);
//            entity.Property(e => e.Name).HasMaxLength(50);
//            entity.Property(e => e.Price).HasPrecision(19, 2);

//            entity.HasOne(d => d.Type).WithMany(p => p.Products)
//                .HasForeignKey(d => d.TypeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_product_producttype_id");
//        });

//        modelBuilder.Entity<ProductType>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("producttype");

//            entity.Property(e => e.Id).HasColumnName("id");
//            entity.Property(e => e.Title).HasMaxLength(255);
//        });

//        modelBuilder.Entity<SavedProduct>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("savedproduct");

//            entity.HasIndex(e => e.ProductId, "FK_savedproduct_product_id");

//            entity.HasIndex(e => e.UserId, "FK_savedproduct_user_id");

//            entity.Property(e => e.Id).HasColumnName("id");

//            entity.HasOne(d => d.Product).WithMany(p => p.Savedproducts)
//                .HasForeignKey(d => d.ProductId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_savedproduct_product_id");

//            entity.HasOne(d => d.User).WithMany(p => p.SavedProducts)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_savedproduct_user_id");
//        });

//        modelBuilder.Entity<User>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("user");

//            entity.HasIndex(e => e.RoleId, "FK_user_userrole_id");

//            entity.Property(e => e.Id).HasColumnName("id");
//            entity.Property(e => e.ContactPhone).HasMaxLength(255);
//            entity.Property(e => e.Email).HasMaxLength(50);
//            entity.Property(e => e.Password).HasMaxLength(255);
//            entity.Property(e => e.Username).HasMaxLength(255);

//            entity.HasOne(d => d.Role).WithMany(p => p.Users)
//                .HasForeignKey(d => d.RoleId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_user_userrole_id");
//        });

//        modelBuilder.Entity<UserRole>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("userrole");

//            entity.Property(e => e.Id).HasColumnName("id");
//            entity.Property(e => e.Title).HasMaxLength(255);
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
