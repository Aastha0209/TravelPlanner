using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Models
{
    public class TripImage
    {
        [Key]
        public int TripImageId { get; set; }

        [Required]
        public int TripId { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public Trip? Trip { get; set; }
    }
}
