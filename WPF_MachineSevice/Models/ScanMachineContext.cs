using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WPF_MachineSevice.Models;

public partial class ScanMachineContext : DbContext
{
    public ScanMachineContext()
    {
    }

    public ScanMachineContext(DbContextOptions<ScanMachineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderImage> OrderImages { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ShopStore> ShopStores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=scanmachine.cxksysgcg0vx.ap-southeast-2.rds.amazonaws.com;Initial Catalog=ScanMachine;User ID=admin;Password=voquanghuy903506;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brand");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Brands)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Brand_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");

            entity.HasOne(d => d.Brand).WithMany(p => p.Categories)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Category_Brand");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.ToTable("Machine");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");

            entity.HasOne(d => d.Store).WithMany(p => p.Machines)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Machine_ShopStore");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.MachineId).HasColumnName("MachineID");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDetailsId).HasColumnName("OrderDetailsID");
            entity.Property(e => e.OrderImageId).HasColumnName("OrderImageID");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");

            entity.HasOne(d => d.Machine).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Machine");

            entity.HasOne(d => d.OrderDetails).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderDetailsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_OrderDetails");

            entity.HasOne(d => d.OrderImage).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_OrderImage");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_ShopStore");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetailsNavigation)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Product");
        });

        modelBuilder.Entity<OrderImage>(entity =>
        {
            entity.ToTable("OrderImage");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.ImageDetailsId).HasColumnName("ImageDetailsID");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.Image).WithMany(p => p.Products)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Image");
        });

        modelBuilder.Entity<ShopStore>(entity =>
        {
            entity.ToTable("ShopStore");

            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");

            entity.HasOne(d => d.Brand).WithMany(p => p.ShopStores)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopStore_Brand");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
