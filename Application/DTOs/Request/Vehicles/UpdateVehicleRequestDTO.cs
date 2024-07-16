using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Vehicles
{
    public class UpdateVehicleRequestDTO :CreateVehicleRequestDTO
    {
        public int Id { get; set; }
    }
}
