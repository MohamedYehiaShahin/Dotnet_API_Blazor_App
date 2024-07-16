using Application.DTOs.Request.Vehicles;
using Application.DTOs.Response;
using Application.DTOs.Response.Vehicles;
using Application.Extensions;
using System.Net.Http.Json;

namespace Application.Services
{
    public class VehicleService(HttpClientService httpClientService) : IVehicleService
    {
        private async Task<HttpClient> PrivateClient() => (await httpClientService.GetPrivateClient());

        private static string CheckResponseStatus(HttpResponseMessage response)
        {
            if(!response.IsSuccessStatusCode)
                return $"Sorry unknown error occured.{Environment.NewLine}Error Description:{response}{Environment.NewLine}Status Code:{response.StatusCode}";
            else
                return null ;
        }

        private static GeneralResponse ErrorOperation(string message) => new(false, message);

        // Add
        public async Task<GeneralResponse> AddVehicle(CreateVehicleRequestDTO model)
        {
            var privateClient = await PrivateClient();
            var result = await privateClient.PostAsJsonAsync(Constant.AddVehicleRoute, model);
            var response = CheckResponseStatus(result);
            if(!string.IsNullOrEmpty(response)) return ErrorOperation(response);
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }

        public async Task<GeneralResponse> AddVehicleBrand(CreateVehicleBrandRequestDTO model)
        {
            var privateClient = await PrivateClient();
            var result = await privateClient.PostAsJsonAsync(Constant.AddVehicleBrandRoute, model);
            var response = CheckResponseStatus(result);
            if (!string.IsNullOrEmpty(response)) return ErrorOperation(response);
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }

        public async Task<GeneralResponse> AddVehicleOwner(CreateVehicleOwnerRequestDTO model)
        {
            var privateClient = await PrivateClient();
            var result = await privateClient.PostAsJsonAsync(Constant.AddVehicleOwnerRoute, model);
            var response = CheckResponseStatus(result);
            if (!string.IsNullOrEmpty(response)) return ErrorOperation(response);
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }

        //delete
        public async Task<GeneralResponse> DeleteVehicle(int id)
        {
            var privateClient = await PrivateClient();
            var result = await privateClient.DeleteAsync($"{Constant.DeleteVehicleRoute}/{id}");
            var response = CheckResponseStatus(result);
            if (!string.IsNullOrEmpty(response)) return ErrorOperation(response);
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();

        }

        public async Task<GeneralResponse> DeleteVehicleBrand(int id)
        {
            var privateClient = await PrivateClient();
            var result = await privateClient.DeleteAsync($"{Constant.DeleteVehicleBrandRoute}/{id}");
            var response = CheckResponseStatus(result);
            if(!string.IsNullOrEmpty(response)) return ErrorOperation(response);
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();

        }

        public async Task<GeneralResponse> DeleteVehicleOwner(int id)
        {
            var privateClient = await PrivateClient();
            var result = await privateClient.DeleteAsync($"{Constant.DeleteVehicleOwnerRoute}/{id}");
            var response = CheckResponseStatus(result);
            if(!string.IsNullOrEmpty(response)) return ErrorOperation(response);
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();

        }

        //get single
        public async Task<GetVehicleResponseDTO> GetVehicle(int id) =>
            await (await PrivateClient()).GetFromJsonAsync<GetVehicleResponseDTO>($"{Constant.GetVehicleRoute}/{id}");

        public async Task<GetVehicleBrandResponseDTO> GetVehicleBrand(int id) =>
            await (await PrivateClient()).GetFromJsonAsync<GetVehicleBrandResponseDTO>($"{Constant.GetVehicleBrandRoute}/{id}");

        public async Task<GetVehicleOwnerResponseDTO> GetVehicleOwner(int id) =>
            await (await PrivateClient()).GetFromJsonAsync<GetVehicleOwnerResponseDTO>($"{Constant.GetVehicleOwnerRoute}/{id}");

        //get list
        public async Task<IEnumerable<GetVehicleResponseDTO>> GetVehicles() =>
            await (await PrivateClient()).GetFromJsonAsync<IEnumerable<GetVehicleResponseDTO>>($"{Constant.GetVehiclesRoute}");


        public async Task<IEnumerable<GetVehicleBrandResponseDTO>> GetVehicleBrands() =>
            await (await PrivateClient()).GetFromJsonAsync<IEnumerable<GetVehicleBrandResponseDTO>>($"{Constant.GetVehiclesBrandRoute}");



        public async Task<IEnumerable<GetVehicleOwnerResponseDTO>> GetVehicleOwners() =>
            await (await PrivateClient()).GetFromJsonAsync<IEnumerable<GetVehicleOwnerResponseDTO>>($"{Constant.GetVehiclesOwnerRoute}");


        //update
        public async Task<GeneralResponse> UpdateVehicle(UpdateVehicleRequestDTO model)
        {
            var privateClient = await PrivateClient();
            var result = await privateClient.PutAsJsonAsync(Constant.UpdateVehicleRoute, model);
            var response = CheckResponseStatus(result);
            if(!string.IsNullOrEmpty(response)) return ErrorOperation(response);
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }

        public async Task<GeneralResponse> UpdateVehicleBrand(UpdateVehicleBrandRequestDTO model)
        {
            var privateClient = await PrivateClient();
            var result = await privateClient.PutAsJsonAsync(Constant.UpdateVehicleBrandRoute, model);
            var response = CheckResponseStatus(result);
            if (!string.IsNullOrEmpty(response)) return ErrorOperation(response);
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }

        public async Task<GeneralResponse> UpdateVehicleOwner(UpdateVehicleOwnerRequestDTO model)
        {
            var privateClient = await PrivateClient();
            var result = await privateClient.PutAsJsonAsync(Constant.UpdateVehicleOwnerRoute, model);
            var response = CheckResponseStatus(result);
            if (!string.IsNullOrEmpty(response)) return ErrorOperation(response);
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }
    }
}
