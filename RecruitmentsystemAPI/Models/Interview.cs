using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentsystemAPI.Models
{
    public class Interview
    {
        [Key]
        public int InterviewId { get; set; }

        // ---------- FOREIGN KEY COLUMNS ----------
        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public int InterviewerId { get; set; }

        // ---------- DATA ----------
        [Required]
        public DateTime InterviewDate { get; set; }

        [MaxLength(50)]
        public string? Mode { get; set; }   // Online / Offline

        public int? RoundNo { get; set; }

        public string? Feedback { get; set; }

        [MaxLength(50)]
        public string? Result { get; set; } // Pending / Selected / Rejected

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        // ---------- NAVIGATION PROPERTIES ----------
        [ForeignKey(nameof(ApplicationId))]
        public Application? Application { get; set; }

        [ForeignKey(nameof(InterviewerId))]

        public User? Interviewer { get; set; }

       

    }
}
