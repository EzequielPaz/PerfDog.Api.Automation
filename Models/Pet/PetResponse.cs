using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PerfDog.Tests.Models.Pet
{
    public class PetResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; } 

        [JsonPropertyName("category")]
        public CategoryResponse Category { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("photoUrls")]
        public List<string> PhotoUrls { get; set; }

        [JsonPropertyName("tags")]
        public List<TagResponse> Tags { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class CategoryResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class TagResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
