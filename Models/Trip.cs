using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public List<Itinerary> Itineraries { get; set; } = new List<Itinerary>();

        public List<TripImage> Images { get; set; } = new List<TripImage>();
    }
}
