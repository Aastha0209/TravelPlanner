using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? ProfilePicture { get; set; } // 👈 added for image

        public List<Trip> Trips { get; set; } = new List<Trip>();
    }
}
