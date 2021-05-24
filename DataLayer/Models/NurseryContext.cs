using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models
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
        public virtual DbSet<TblFieldRegexRel> TblFieldRegexRel { get; set; }
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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=103.216.62.27;Initial Catalog=Nursery;User ID=Yanal;Password=1710ahmad.fard");
        //    }
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          => optionsBuilder
         .UseLazyLoadingProxies()
         .UseSqlServer("Data Source=103.216.62.27;Initial Catalog=Nursery;User ID=Yanal;Password=1710ahmad.fard");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblField>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsRequired).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblFieldRegexRel>(entity =>
            {
                entity.HasOne(d => d.Field)
                    .WithMany(p => p.TblFieldRegexRel)
                    .HasForeignKey(d => d.FieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblFieldRegexRel_TblField");

                entity.HasOne(d => d.Regex)
                    .WithMany(p => p.TblFieldRegexRel)
                    .HasForeignKey(d => d.RegexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblFieldRegexRel_TblRegex");
            });

            modelBuilder.Entity<TblForm>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblFormFieldRel>(entity =>
            {
                entity.HasKey(e => e.FormFieldId)
                    .HasName("PK_TblFormFieldId");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

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

            modelBuilder.Entity<TblPage>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblPageFormRel>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

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
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblRolePageRel>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

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

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
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
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

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

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.FormField)
                    .WithMany(p => p.TblValue)
                    .HasForeignKey(d => d.FormFieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblValue_TblFormFieldId");

                entity.HasOne(d => d.Kid)
                    .WithMany(p => p.TblValue)
                    .HasForeignKey(d => d.KidId)
                    .HasConstraintName("FK_TblValue_TblKid");

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
