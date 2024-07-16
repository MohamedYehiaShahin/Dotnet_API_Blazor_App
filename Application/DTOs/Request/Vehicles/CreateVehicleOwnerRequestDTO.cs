
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Vehicles
{
    public class CreateVehicleOwnerRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
