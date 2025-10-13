using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SpoofTest.Models;

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

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }

    public virtual DbSet<StudentTest> StudentTests { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3214EC07A32457FA");

            entity.ToTable("Answer");

            entity.Property(e => e.Content).HasMaxLength(100);

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Answer__Question__44FF419A");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Group__3214EC07866EB3E2");

            entity.ToTable("Group");

            entity.HasIndex(e => e.Name, "UQ__Group__737584F68C99F66E").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC073DF8A84A");

            entity.ToTable("Question");

            entity.Property(e => e.Content).HasMaxLength(100);

            entity.HasOne(d => d.Test).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Question__TestId__4222D4EF");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC07CADFAEC2");

            entity.ToTable("Student");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Patronymic).HasMaxLength(100);

            entity.HasOne(d => d.Group).WithMany(p => p.Students)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__GroupId__3C69FB99");
        });

        modelBuilder.Entity<StudentAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName(" PK_StudetAnswer_Id");

            entity.ToTable("StudentAnswer");

            entity.HasOne(d => d.Answer).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudetAnswer_AnswerId");

            entity.HasOne(d => d.StudentTest).WithMany(p => p.StudentAnswers)
                .HasForeignKey(d => d.StudentTestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudetAnswer_StudentTestId");
        });

        modelBuilder.Entity<StudentTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StudentT__3214EC0716BDC4D4");

            entity.ToTable("StudentTest");

            entity.HasIndex(e => new { e.StudentId, e.TestId }, "UQ__StudentT__2A09188E04EB8AA0").IsUnique();

            entity.Property(e => e.PassDate).HasColumnType("datetime");
            entity.Property(e => e.SetDate).HasColumnType("datetime");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentTests)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentTe__Stude__5070F446");

            entity.HasOne(d => d.Test).WithMany(p => p.StudentTests)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentTe__TestI__5165187F");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teacher__3214EC07118651E6");

            entity.ToTable("Teacher");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test__3214EC076D1FA9AB");

            entity.ToTable("Test");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.LimitMinutes).HasDefaultValue(5);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Teacherp).WithMany(p => p.Tests)
                .HasForeignKey(d => d.TeacherpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Test__TeacherpId__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
