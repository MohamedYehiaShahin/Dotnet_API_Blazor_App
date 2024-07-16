using Application.Contracts;
using Application.DTOs.Request.Vehicles;
using Application.DTOs.Response;
using Application.DTOs.Response.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController(IVehicle vehicle) : ControllerBase
    {
        // Post /create
        [HttpPost("add-vehicle")]
        public async Task<ActionResult<GeneralResponse>> Create(CreateVehicleRequestDTO model) =>
            Ok(await vehicle.AddVehicle(model));

        [HttpPost("add-vehicle-brand")]
        public async Task<ActionResult<GeneralResponse>> CreateBrand(CreateVehicleBrandRequestDTO model) => 
            Ok(await vehicle.AddVehicleBrand(model));

        [HttpPost("add-vehicle-owner")]
        public async Task<ActionResult<GeneralResponse>> CreateOwner(CreateVehicleOwnerRequestDTO model) => 
            Ok(await vehicle.AddVehicleOwner(model));

        // Get / single
        [HttpGet("get-vehicle/{id}")]
        public async Task<ActionResult<GetVehicleResponseDTO>> Get(int id) =>
            Ok(await vehicle.GetVehicle(id));

        
        [HttpGet("get-vehicle-brand/{id}")]
        public async Task<ActionResult<GetVehicleBrandResponseDTO>> GetBrand(int id) =>
            Ok(await vehicle.GetVehicleBrand(id));

        
        [HttpGet("get-vehicle-owner/{id}")]
        public async Task<ActionResult<GetVehicleOwnerResponseDTO>> GetOwner(int id) =>
            Ok(await vehicle.GetVehicleOwner(id));

        // Get / list
        [HttpGet("get-vehicles")]
        public async Task<ActionResult<IEnumerable<GetVehicleResponseDTO>>> Gets() =>
            Ok(await vehicle.GetVehicles());

        [HttpGet("get-vehicle-brands")]
        public async Task<ActionResult<IEnumerable<GetVehicleBrandResponseDTO>>> GetBrands() =>
            Ok(await vehicle.GetVehicleBrands());

        [HttpGet("get-vehicle-owners")]
        public async Task<ActionResult<IEnumerable<GetVehicleOwnerResponseDTO>>> GetOwners() =>
            Ok((await vehicle.GetVehicleOwners()));

        //Update
        [HttpPut("Update-vehicle")]
        public async Task<ActionResult<GeneralResponse>> Update(UpdateVehicleRequestDTO model) =>
            Ok(await vehicle.UpdateVehicle(model));

        [HttpPut("Update-vehicle-brand")]
        public async Task<ActionResult<GeneralResponse>> UpdateBrand(UpdateVehicleBrandRequestDTO model) =>
            Ok(await vehicle.UpdateVehicleBrand(model));

        [HttpPut("Update-vehicle-owner")]
        public async Task<ActionResult<GeneralResponse>> UpdateOwner(UpdateVehicleOwnerRequestDTO model) =>
            Ok(await vehicle.UpdateVehicleOwner(model));

        //Delete
        [HttpDelete("Delete-vehicle/{id}")]
        public async Task<ActionResult<GeneralResponse>>Delete(int id)=>
            Ok(await vehicle.DeleteVehicle(id));

        [HttpDelete("Delete-vehicle-brand/{id}")]
        public async Task<ActionResult<GeneralResponse>> DeleteBrand(int id) =>
            Ok(await vehicle.DeleteVehicleBrand(id));

        [HttpDelete("Delete-vehicle-owner/{id}")]
        public async Task<ActionResult<GeneralResponse>> DeleteOwner(int id) =>
            Ok((await vehicle.DeleteVehicleOwner(id)));

    }
}
