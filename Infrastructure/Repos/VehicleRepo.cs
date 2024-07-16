using Application.Contracts;
using Application.DTOs.Request.Vehicles;
using Application.DTOs.Response;
using Application.DTOs.Response.Vehicles;
using Domain.Entity.VehicleEntity;
using Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    
    public class VehicleRepo(AppDbContext context) : IVehicle
    {
        //class utilities
        private async Task<Vehicle> FindVehicleByName(string name) =>
            await context.Vehicles.FirstOrDefaultAsync(v=>v.Name.ToLower() == name.ToLower());
        private async Task<VehicleBrand> FindVehicleBrandByName(string name) =>
            await context.VehicleBrands.FirstOrDefaultAsync(v => v.Name.ToLower() == name.ToLower());
        private async Task<VehicleOwner> FindVehicleOwnerByName(string name) => 
            await context.VehicleOwners.FirstOrDefaultAsync(v => v.Name.ToLower() == name.ToLower());


        private async Task<Vehicle> FindVehicleById(int Id) =>
            await context.Vehicles.Include(v=>v.VehicleBrand).Include(v=>v.VehicleOwner).FirstOrDefaultAsync(v => v.Id == Id);
        private async Task<VehicleBrand> FindVehicleBrandById(int Id) => 
            await context.VehicleBrands.FirstOrDefaultAsync(v => v.Id == Id);
        private async Task<VehicleOwner> FindVehicleOwnerById(int Id) => 
            await context.VehicleOwners.FirstOrDefaultAsync(v => v.Id == Id);


        private async Task SaveChangesAsync()=> await context.SaveChangesAsync();
        private static GeneralResponse NullResponse(string message) => new(false, message);
        private static GeneralResponse AlreadyExistResponse(string message) => new(false, message);
        private static GeneralResponse OperationSuccessResponse(string message) => new(true, message);

        //interface methods CRUD 
        public async Task<GeneralResponse> AddVehicle(CreateVehicleRequestDTO model)
        {
            if (await FindVehicleByName(model.Name) is not null) return AlreadyExistResponse("Vehicle already exist");
            context.Vehicles.Add(model.Adapt(new Vehicle()));
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle data saved");
        }

        public async Task<GeneralResponse> AddVehicleBrand(CreateVehicleBrandRequestDTO model)
        {
            if (await FindVehicleBrandByName(model.Name) is not null) return AlreadyExistResponse("Vehicle brand already exist");
            
            context.VehicleBrands.Add(model.Adapt(new VehicleBrand()));
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle brand data saved");
        }

        public async Task<GeneralResponse> AddVehicleOwner(CreateVehicleOwnerRequestDTO model)
        {
            if (await FindVehicleOwnerByName(model.Name) is not null) return AlreadyExistResponse("Vehicle Owner already exist");
            context.VehicleOwners.Add(model.Adapt(new VehicleOwner()));
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle owner data saved");
        }

        public async Task<GeneralResponse> DeleteVehicle(int id)
        {
            if (await FindVehicleById(id) is null) return NullResponse("Vehicle not found");
            context.Vehicles.Remove(await FindVehicleById(id));
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle Deleted");
        }

        public async Task<GeneralResponse> DeleteVehicleBrand(int id)
        {
            if (await FindVehicleBrandById(id) is null) return NullResponse("Vehicle brand not found");
            context.VehicleBrands.Remove(await  FindVehicleBrandById(id));
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle brand deleted");
        }

        public async Task<GeneralResponse> DeleteVehicleOwner(int id)
        {
            if (await FindVehicleOwnerById(id) is null) return NullResponse("Vehicle owner not found");
            context.VehicleOwners.Remove(await FindVehicleOwnerById(id));
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle owner deleted");
        }

        // GET
        public async Task<GetVehicleResponseDTO> GetVehicle(int id) =>
            (await FindVehicleById(id)).Adapt(new GetVehicleResponseDTO());
       
        public async Task<GetVehicleBrandResponseDTO> GetVehicleBrand(int id)=>
            (await FindVehicleBrandById(id)).Adapt(new GetVehicleBrandResponseDTO());

        public async Task<GetVehicleOwnerResponseDTO> GetVehicleOwner(int id) =>
            (await FindVehicleOwnerById(id)).Adapt(new GetVehicleOwnerResponseDTO());


        // GET lists
        public async Task<IEnumerable<GetVehicleResponseDTO>> GetVehicles()
        {
            var data = (await context.Vehicles.Include(v =>v.VehicleBrand).Include(v =>v.VehicleOwner).ToListAsync());
            return data.Select(v => new GetVehicleResponseDTO
            {
                Id = v.Id,
                Name = v.Name,
                Description = v.Description,
                VehicleOwnerId = v.VehicleOwnerId,
                VehicleBrandId = v.VehicleBrandId,
                VehicleBrand = new GetVehicleBrandResponseDTO()
                {
                    Id = v.VehicleBrand.Id,
                    Name = v.VehicleBrand.Name,
                    Location = v.VehicleBrand.Location,
                },
                VehicleOwner = new GetVehicleOwnerResponseDTO()
                {
                    Id = v.VehicleOwner.Id,
                    Name = v.VehicleOwner.Name,
                    Address = v.VehicleOwner.Address
                }
            }).ToList();
        }

        public async Task<IEnumerable<GetVehicleBrandResponseDTO>> GetVehicleBrands() =>
            (await context.VehicleBrands.ToListAsync()).Adapt<List<GetVehicleBrandResponseDTO>>();
         
        public async Task<IEnumerable<GetVehicleOwnerResponseDTO>> GetVehicleOwners() =>
            (await context.VehicleOwners.ToListAsync()).Adapt<List<GetVehicleOwnerResponseDTO>>();



        public async Task<GeneralResponse> UpdateVehicle(UpdateVehicleRequestDTO model)
        {
            if (await FindVehicleById(model.Id) is null) return NullResponse("Vehicle not found");
            context.Entry(await FindVehicleById(model.Id)).State = EntityState.Detached;
            context.Vehicles.Update(model.Adapt(new Vehicle()));
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle data updated");
        }

        public async Task<GeneralResponse> UpdateVehicleBrand(UpdateVehicleBrandRequestDTO model)
        {
            if (await FindVehicleBrandById(model.Id) is null) return NullResponse("vehicle Brand not found");
            context.Entry(await FindVehicleBrandById(model.Id)).State = EntityState.Detached;
            context.VehicleBrands.Update(model.Adapt(new VehicleBrand()));
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle Brand data updated");
        }

        public async Task<GeneralResponse> UpdateVehicleOwner(UpdateVehicleOwnerRequestDTO model)
        {
            if (await FindVehicleOwnerById(model.Id) is null) return NullResponse("vehicle owner not found");
            context.Entry(await FindVehicleOwnerById(model.Id)).State = EntityState.Detached;
            context.VehicleOwners.Update(model.Adapt(new VehicleOwner()));
            await SaveChangesAsync();
            return OperationSuccessResponse("Vehicle owner data updated");
        }
    }
}
