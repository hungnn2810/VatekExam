using Microsoft.EntityFrameworkCore;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EntityFramework.Document
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

        public virtual DbSet<Bookmarks> Bookmarks { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<DocumentPages> DocumentPages { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<PhysicalFiles> PhysicalFiles { get; set; }
        public virtual DbSet<S3Buckets> S3Buckets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bookmarks>(entity =>
            {
                entity.HasKey(e => e.BookmarkId)
                    .HasName("PK__book_mar__E6E31C96743F80F9");

                entity.ToTable("bookmarks");

                entity.Property(e => e.BookmarkId).HasColumnName("bookmark_id");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.PageNumber).HasColumnName("page_number");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.Bookmarks)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__bookmarks__docum__3C34F16F");
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

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<DocumentPages>(entity =>
            {
                entity.HasKey(e => new { e.DocumentId, e.PhysicalFileId })
                    .HasName("PK__document__84713DFD0F925164");

                entity.ToTable("document_pages");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.PhysicalFileId).HasColumnName("physical_file_id");

                entity.Property(e => e.PageNumber).HasColumnName("page_number");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentPages)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__document___docum__282DF8C2");

                entity.HasOne(d => d.PhysicalFile)
                    .WithMany(p => p.DocumentPages)
                    .HasForeignKey(d => d.PhysicalFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__document___physi__29221CFB");
            });

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.HasKey(e => e.DocumentId)
                    .HasName("PK__tmp_ms_x__9666E8ACC85A328E");

                entity.ToTable("documents");

                entity.Property(e => e.DocumentId).HasColumnName("document_id");

                entity.Property(e => e.AuthorId)
                    .IsRequired()
                    .HasColumnName("author_id")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

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
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__documents__categ__01142BA1");
            });

            modelBuilder.Entity<PhysicalFiles>(entity =>
            {
                entity.HasKey(e => e.PhysicalFileId)
                    .HasName("PK__physical__217D5510036708B4");

                entity.ToTable("physical_files");

                entity.Property(e => e.PhysicalFileId).HasColumnName("physical_file_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.FileLengthInBytes).HasColumnName("file_length_in_bytes");

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
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.S3Bucket)
                    .WithMany(p => p.PhysicalFiles)
                    .HasForeignKey(d => d.S3BucketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__physical___s3_bu__25518C17");
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
