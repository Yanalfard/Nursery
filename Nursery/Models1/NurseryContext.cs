﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Nursery.Models1
{
    public partial class NurseryContext : DbContext
    {
        public NurseryContext()
        {
        }

        public NurseryContext(DbContextOptions<NurseryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblConfig> TblConfig { get; set; }
        public virtual DbSet<TblField> TblField { get; set; }
        public virtual DbSet<TblForm> TblForm { get; set; }
        public virtual DbSet<TblFormFieldRel> TblFormFieldRel { get; set; }
        public virtual DbSet<TblKid> TblKid { get; set; }
        public virtual DbSet<TblPage> TblPage { get; set; }
        public virtual DbSet<TblPageFormRel> TblPageFormRel { get; set; }
        public virtual DbSet<TblRegex> TblRegex { get; set; }
        public virtual DbSet<TblRole> TblRole { get; set; }
        public virtual DbSet<TblRolePageRel> TblRolePageRel { get; set; }
        public virtual DbSet<TblUser> TblUser { get; set; }
        public virtual DbSet<TblUserLog> TblUserLog { get; set; }
        public virtual DbSet<TblUserRoleRel> TblUserRoleRel { get; set; }
        public virtual DbSet<TblValue> TblValue { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=103.216.62.27;Initial Catalog=Nursery;User ID=Yanal;Password=1710ahmad.fard");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblField>(entity =>
            {
                entity.Property(e => e.IsRequired).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblForm>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblFormFieldRel>(entity =>
            {
                entity.HasKey(e => e.FormFieldId)
                    .HasName("PK_TblFormFieldId");

                entity.HasOne(d => d.Field)
                    .WithMany(p => p.TblFormFieldRel)
                    .HasForeignKey(d => d.FieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblFormFieldId_TblField");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.TblFormFieldRel)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblFormFieldId_TblForm");
            });

            modelBuilder.Entity<TblPageFormRel>(entity =>
            {
                entity.HasOne(d => d.Form)
                    .WithMany(p => p.TblPageFormRel)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblPageFormRel_TblForm");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.TblPageFormRel)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblPageFormRel_TblPage");
            });

            modelBuilder.Entity<TblRegex>(entity =>
            {
                entity.HasOne(d => d.Field)
                    .WithMany(p => p.TblRegex)
                    .HasForeignKey(d => d.FieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblRegex_TblField");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.Property(e => e.Name).IsFixedLength();
            });

            modelBuilder.Entity<TblRolePageRel>(entity =>
            {
                entity.HasOne(d => d.Page)
                    .WithMany(p => p.TblRolePageRel)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblRolePageRel_TblPage");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblRolePageRel)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblRolePageRel_TblRole");
            });

            modelBuilder.Entity<TblUserLog>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_TblUserLog_TblUser");
            });

            modelBuilder.Entity<TblUserRoleRel>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblUserRoleRel)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblUserRoleRel_TblRole");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserRoleRel)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblUserRoleRel_TblUser");
            });

            modelBuilder.Entity<TblValue>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsAccepted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.FormField)
                    .WithMany(p => p.TblValue)
                    .HasForeignKey(d => d.FormFieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblValue_TblFormFieldId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblValue)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblValue_TblUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
