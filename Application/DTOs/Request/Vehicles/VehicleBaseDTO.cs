using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Vehicles
{
    public class VehicleBaseDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(1, 100, ErrorMessage = "Select Vehicle Owner")]
        public int VehicleOwnerId { get; set; }
        [Range(1,100,ErrorMessage ="Select Vehicle Brand")]
        public int VehicleBrandId { get; set; }
    }
}
