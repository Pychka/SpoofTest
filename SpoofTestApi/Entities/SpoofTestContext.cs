using Microsoft.EntityFrameworkCore;

namespace SpoofTestApi.Entities;

public partial class SpoofTestContext : DbContext
{
    public SpoofTestContext()
    {
    }

    public SpoofTestContext(DbContextOptions<SpoofTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Submitted> Submitteds { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=SpoofTest;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3214EC0774817339");

            entity.ToTable("Answer");

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Answer__Question__3F466844");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC076D28E85A");

            entity.ToTable("Question");

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Test).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Question__TestId__3C69FB99");
        });

        modelBuilder.Entity<Submitted>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Submitte__3214EC07266A1908");

            entity.ToTable("Submitted");

            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
            entity.Property(e => e.SessionId).HasMaxLength(100);

            entity.HasOne(d => d.Test).WithMany(p => p.Submitteds)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Submitted__TestI__398D8EEE");

            entity.HasMany(d => d.Answers).WithMany(p => p.Submitteds)
                .UsingEntity<Dictionary<string, object>>(
                    "QuestionAnswer",
                    r => r.HasOne<Answer>().WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__QuestionA__Answe__4316F928"),
                    l => l.HasOne<Submitted>().WithMany()
                        .HasForeignKey("SubmittedId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__QuestionA__Submi__4222D4EF"),
                    j =>
                    {
                        j.HasKey("SubmittedId", "AnswerId").HasName("PK__Question__D97D753D6FDC3AB3");
                        j.ToTable("QuestionAnswer");
                    });
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test__3214EC07FFD36F64");

            entity.ToTable("Test");

            entity.Property(e => e.Title).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
