using Application.DTOs.Request.Vehicles;
namespace Application.DTOs.Response.Vehicles
{
    public class GetVehicleBrandResponseDTO : CreateVehicleBrandRequestDTO
    {
        public int Id { get; set; }

        public virtual ICollection<GetVehicleResponseDTO> Vehicles { get; set; } = null;
    }
}
