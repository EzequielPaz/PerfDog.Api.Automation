using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PerfDog.Tests.Models.Order
{
    public class OrderRequest
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("petId")]
        public long PetId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("shipDate")]
        public string ShipDate { get; set; } 

        [JsonPropertyName("status")]
        public string Status { get; set; } 

        [JsonPropertyName("complete")]
        public bool Complete { get; set; }
    }

    public class OrderResponse : OrderRequest { } // En este caso son idénticos
}
