using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Manisai_PL_App.Models;

public partial class PlappContext : DbContext
{
    public PlappContext() { }

    public PlappContext(DbContextOptions<PlappContext> options) : base(options) { }

    public virtual DbSet<Aadharmaster> Aadharmasters { get; set; }
    public virtual DbSet<Bankdetail> Bankdetails { get; set; }
    public virtual DbSet<Basicdetail> Basicdetails { get; set; }
    public virtual DbSet<Companydetail> Companydetails { get; set; }
    public virtual DbSet<Companymaster> Companymasters { get; set; }
    public virtual DbSet<Doctypemaster> Doctypemasters { get; set; }
    public virtual DbSet<Docuploaddetail> Docuploaddetails { get; set; }
    public virtual DbSet<Loandetail> Loandetails { get; set; }
    public virtual DbSet<Otpmaster> Otpmasters { get; set; }
    public virtual DbSet<Panmaster> Panmasters { get; set; }
    public virtual DbSet<Personaldetail> Personaldetails { get; set; }
    public virtual DbSet<Pincodemaster> Pincodemasters { get; set; }
    public virtual DbSet<Rulesmaster> Rulesmasters { get; set; }
    public virtual DbSet<Usermaster> Usermasters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Basicdetail>(entity =>
        {
            entity.ToTable("BasicDetail");
        });

        modelBuilder.Entity<Personaldetail>(entity =>
        {
            entity.HasOne(d => d.App).WithMany(p => p.Personaldetails).HasForeignKey(d => d.Appid);
        });

        modelBuilder.Entity<Companydetail>(entity =>
        {
            entity.HasOne(d => d.App).WithMany(p => p.Companydetails).HasForeignKey(d => d.AppId);
        });

        modelBuilder.Entity<Bankdetail>(entity =>
        {
            entity.HasOne(d => d.App).WithMany(p => p.Bankdetails).HasForeignKey(d => d.Appid);
        });

        modelBuilder.Entity<Loandetail>(entity =>
        {
            entity.HasOne(d => d.App).WithMany(p => p.Loandetails).HasForeignKey(d => d.Appid);
            entity.Property(e => e.Roi).HasPrecision(5, 2);
            entity.Property(e => e.Emi).HasPrecision(12, 2);
            entity.Property(e => e.Approvedroi).HasPrecision(5, 2);
            entity.Property(e => e.Approvedemi).HasPrecision(12, 2);
        });
        modelBuilder.Entity<Docuploaddetail>(entity =>
        {
            entity.HasOne(d => d.App).WithMany(p => p.Docuploaddetails).HasForeignKey(d => d.Appid);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}