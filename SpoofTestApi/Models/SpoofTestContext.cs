using Microsoft.EntityFrameworkCore;

namespace SpoofTestApi.Models;

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

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentTest> StudentTests { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=SpoofTest;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3214EC079EE2C0B8");

            entity.ToTable("Answer");

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Answer__Question__45F365D3");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC0737D786AC");

            entity.ToTable("Question");

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Test).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Question__TestId__4222D4EF");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC07E98B6AE3");

            entity.ToTable("Student");

            entity.Property(e => e.Group).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.HasIndex(e => e.Login, "UQ_Student_Login").IsUnique();
        });

        modelBuilder.Entity<StudentTest>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.TestId }).HasName("PK__StudentT__2A09188FBB68452F");

            entity.ToTable("StudentTest");

            entity.Property(e => e.DeadLine).HasColumnType("datetime");
            entity.Property(e => e.Result).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentTests)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentTe__Stude__3E52440B");

            entity.HasOne(d => d.Test).WithMany(p => p.StudentTests)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentTe__TestI__3F466844");

            entity.HasMany(d => d.Answers).WithMany(p => p.StudentTests)
                .UsingEntity<Dictionary<string, object>>(
                    "QuestionAnswer",
                    r => r.HasOne<Answer>().WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__QuestionA__Answe__48CFD27E"),
                    l => l.HasOne<StudentTest>().WithMany()
                        .HasForeignKey("StudentId", "TestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__QuestionAnswer__49C3F6B7"),
                    j =>
                    {
                        j.HasKey("StudentId", "TestId", "AnswerId").HasName("PK__Question__2FDD9ADF4F09ACE9");
                        j.ToTable("QuestionAnswer");
                    });
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teacher__3214EC07A9C387C9");

            entity.ToTable("Teacher");

            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.HasIndex(e => e.Login, "UQ_Teacher_Login").IsUnique();
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test__3214EC075FC60410");

            entity.ToTable("Test");

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Tests)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Test__TeacherId__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
