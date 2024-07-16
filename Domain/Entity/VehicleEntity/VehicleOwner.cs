using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entity.VehicleEntity
{
    public class VehicleOwner : BaseClass
    {
        public string? Address { get; set; }

        [JsonIgnore]
        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}
