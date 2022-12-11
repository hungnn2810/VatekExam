using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Document.EntityFramework
{
    public partial class DocumentDbContext : DbContext
    {
        public DocumentDbContext()
        {
        }

        public DocumentDbContext(DbContextOptions<DocumentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookMarks> BookMarks { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<PhysialFiles> PhysialFiles { get; set; }
        public virtual DbSet<S3Buckets> S3Buckets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=VatekExam_Document;User Id=sa;Password=Hung2001@");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookMarks>(entity =>
            {
                entity.HasKey(e => e.BookMarkId)
                    .HasName("PK__book_mar__E6E31C96743F80F9");

                entity.ToTable("book_marks");

                entity.Property(e => e.BookMarkId).HasColumnName("book_mark_id");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.PageNumber).HasColumnName("page_number");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.BookMarks)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK__book_mark__docum__68487DD7");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__categori__D54EE9B457E935A9");

                entity.ToTable("categories");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.HasKey(e => e.DocumentId)
                    .HasName("PK__document__9666E8AC60C4563F");

                entity.ToTable("documents");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__documents__categ__5CD6CB2B");
            });

            modelBuilder.Entity<PhysialFiles>(entity =>
            {
                entity.HasKey(e => e.PhysicalFileId)
                    .HasName("PK__physial___217D551008CDBFC6");

                entity.ToTable("physial_files");

                entity.Property(e => e.PhysicalFileId).HasColumnName("physical_file_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.FileLengthInBytes).HasColumnName("file_length_in_bytes");

                entity.Property(e => e.PageNumber).HasColumnName("page_number");

                entity.Property(e => e.PhysicalFileExtention)
                    .IsRequired()
                    .HasColumnName("physical_file_extention")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PhysicalFileName)
                    .IsRequired()
                    .HasColumnName("physical_file_name")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.S3BucketId).HasColumnName("s3_bucket_id");

                entity.Property(e => e.S3FileKey)
                    .IsRequired()
                    .HasColumnName("s3_file_key")
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.PhysialFiles)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__physial_f__docum__619B8048");

                entity.HasOne(d => d.S3Bucket)
                    .WithMany(p => p.PhysialFiles)
                    .HasForeignKey(d => d.S3BucketId)
                    .HasConstraintName("FK__physial_f__s3_bu__60A75C0F");
            });

            modelBuilder.Entity<S3Buckets>(entity =>
            {
                entity.HasKey(e => e.S3BucketId)
                    .HasName("PK__s3_bucke__F0809874AED06CC9");

                entity.ToTable("s3_buckets");

                entity.Property(e => e.S3BucketId)
                    .HasColumnName("s3_bucket_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.S3BucketName)
                    .IsRequired()
                    .HasColumnName("s3_bucket_name")
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
