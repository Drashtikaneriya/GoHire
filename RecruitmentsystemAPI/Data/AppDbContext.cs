using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Models;
using RecruitmentsystemAPI.Controllers;

using YourProjectName.Models;

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
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Company> Company { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Users → Roles ----------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- JobPositions → Users (CreatedBy) ----------
            modelBuilder.Entity<JobPosition>()
                .HasOne(j => j.User)
                .WithMany()
                .HasForeignKey(j => j.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Applications → JobPositions ----------
            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobPosition)
                .WithMany()
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Applications → Candidates ----------
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Candidate)
                .WithMany()
                .HasForeignKey(a => a.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Interviews → Applications ----------
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Application)
                .WithMany()
                .HasForeignKey(i => i.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Interviews → Users (Interviewer) ----------
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Interviewer)
                .WithMany()
                .HasForeignKey(i => i.InterviewerId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
