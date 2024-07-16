
namespace Application.DTOs.Response.Vehicles
{
    public class GetVehicleResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public GetVehicleOwnerResponseDTO? VehicleOwner { get; set; }
        public int VehicleOwnerId { get; set; }

        public GetVehicleBrandResponseDTO? VehicleBrand { get; set; }
        public int VehicleBrandId { get; set; }
    }
}