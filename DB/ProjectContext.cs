using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DB.Models;

namespace DB
{
    public partial class ProjectContext : DbContext
    {
        public ProjectContext()
        {
        }

        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Consultant> Consultants { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LOCALHOST; Database=Projekt; Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Consultant>(entity =>
            {
                entity.ToTable("Consultant");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasMany(d => d.Projects)
                    .WithMany(p => p.Consultants)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProjectConsultant",
                        l => l.HasOne<Project>().WithMany().HasForeignKey("ProjectId").HasConstraintName("FK__ProjectCo__Proje__5F7E2DAC"),
                        r => r.HasOne<Consultant>().WithMany().HasForeignKey("ConsultantId").HasConstraintName("FK__ProjectCo__Consu__5E8A0973"),
                        j =>
                        {
                            j.HasKey("ConsultantId", "ProjectId").HasName("PK__ProjectC__F2D994B66C2CDE7A");

                            j.ToTable("ProjectConsultant");
                        });

                entity.HasMany(d => d.Skills)
                    .WithMany(p => p.Consultants)
                    .UsingEntity<Dictionary<string, object>>(
                        "ConsultantSkill",
                        l => l.HasOne<Skill>().WithMany().HasForeignKey("SkillId").HasConstraintName("FK__Consultan__Skill__5BAD9CC8"),
                        r => r.HasOne<Consultant>().WithMany().HasForeignKey("ConsultantId").HasConstraintName("FK__Consultan__Consu__5AB9788F"),
                        j =>
                        {
                            j.HasKey("ConsultantId", "SkillId").HasName("PK__Consulta__884236410DCD97ED");

                            j.ToTable("ConsultantSkill");
                        });
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__Project__Company__57DD0BE4");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__Project__StatusI__56E8E7AB");

                entity.HasMany(d => d.Skills)
                    .WithMany(p => p.Projects)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProjectSkill",
                        l => l.HasOne<Skill>().WithMany().HasForeignKey("SkillId").HasConstraintName("FK__ProjectSk__Skill__634EBE90"),
                        r => r.HasOne<Project>().WithMany().HasForeignKey("ProjectId").HasConstraintName("FK__ProjectSk__Proje__625A9A57"),
                        j =>
                        {
                            j.HasKey("ProjectId", "SkillId").HasName("PK__ProjectS__1BE0B7E8CFA5CA60");

                            j.ToTable("ProjectSkill");
                        });
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("Skill");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
