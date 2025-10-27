using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.Models
{
    public class Itinerary
    {
        [Key]
        public int ItineraryId { get; set; }

        [Required]
        public int TripId { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;  // Place, Restaurant, Activity

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTime? Date { get; set; }

        [ForeignKey("TripId")]
        public Trip Trip { get; set; } = null!;
    }
}
