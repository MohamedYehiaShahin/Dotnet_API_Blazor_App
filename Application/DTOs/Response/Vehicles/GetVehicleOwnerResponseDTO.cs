using Application.DTOs.Request.Vehicles;
namespace Application.DTOs.Response.Vehicles
{
    public class GetVehicleOwnerResponseDTO : CreateVehicleOwnerRequestDTO
    {
        public int Id { get; set; }
        public virtual ICollection<GetVehicleResponseDTO> Vehicles { get; set; }
    }
}
