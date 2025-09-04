using System;
using System.Collections.Generic;
using AccountManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountManagementSystem.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Use the Name= syntax to read from configuration instead of hardcoding
            optionsBuilder.UseSqlServer("Name=DefaultConnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.code);
            entity.HasIndex(e => e.id_number).IsUnique();
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.code);
            entity.HasIndex(e => e.account_number).IsUnique();
            entity.HasOne(d => d.person_codeNavigation)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.person_code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Person");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.code);
            entity.HasOne(d => d.account_codeNavigation)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.account_code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}