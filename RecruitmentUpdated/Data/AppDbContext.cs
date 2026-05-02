using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<InterviewRound> InterviewRounds { get; set; }
        public DbSet<Interview> Interviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Roles ----------
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            // ---------- Users → Roles ----------
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Candidates → Users (1-to-1) ----------
            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.UserId)
                .IsUnique();

            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.User)
                .WithOne(u => u.Candidate)
                .HasForeignKey<Candidate>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- JobPositions → Companies ----------
            modelBuilder.Entity<JobPosition>()
                .HasOne(j => j.Company)
                .WithMany(c => c.JobPositions)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- JobPositions → Users (CreatedBy) ----------
            modelBuilder.Entity<JobPosition>()
                .HasOne(j => j.CreatedByUser)
                .WithMany()
                .HasForeignKey(j => j.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Applications → JobPositions ----------
            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobPosition)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Applications → Candidates ----------
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Candidate)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Applications: Unique(JobId, CandidateId) ----------
            modelBuilder.Entity<Application>()
                .HasIndex(a => new { a.JobId, a.CandidateId })
                .IsUnique();

            // ---------- InterviewRounds ----------
            modelBuilder.Entity<InterviewRound>()
                .HasIndex(r => r.RoundName)
                .IsUnique();

            // ---------- Interviews → Applications ----------
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Application)
                .WithMany(a => a.Interviews)
                .HasForeignKey(i => i.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Interviews → InterviewRounds ----------
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Round)
                .WithMany(r => r.Interviews)
                .HasForeignKey(i => i.RoundId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Interviews → Users (Interviewer) ----------
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Interviewer)
                .WithMany()
                .HasForeignKey(i => i.InterviewerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Interviews → Users (UpdatedBy) ----------
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.UpdatedByUser)
                .WithMany()
                .HasForeignKey(i => i.UpdatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
