using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using PerfDog.Tests.Models.Order;
using PerfDog.Tests.Models.Pet;
using PerfDog.Tests.Utils;
using System.Text.Json;

namespace PerfDog.Tests.Service
{
    public class PetService
    {
        private readonly IAPIRequestContext _api;
        private readonly ILogger _logger;

        public PetService(IAPIRequestContext api, ILogger logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <summary>
        /// Sends a POST request to create a new pet in the store.
        /// </summary>
        public async Task<PetResponse?> CreatePetAsync(PetRequest petRequest)
        {
            _logger.LogInformation("🚀 POST -> pet | Creating pet: {Name}", petRequest.Name);

            var response = await _api.PostAsync("pet", new APIRequestContextOptions { DataObject = petRequest });
            var json = await response.TextAsync();

            _logger.LogJsonDebug($"Response POST /pet ({response.Status})", json);

            return response.Ok ? JsonSerializer.Deserialize<PetResponse>(json, GetOptions()) : null;
        }

        /// <summary>
        /// Retrieves pet details by its unique ID.
        /// </summary>
        public async Task<PetResponse?> GetPetByIdAsync(long id)
        {
            _logger.LogInformation("🔍 GET -> pet/{Id} | Fetching pet details", id);

            var response = await _api.GetAsync($"pet/{id}");
            var json = await response.TextAsync();

            _logger.LogJsonDebug($"Response GET /pet/{id} ({response.Status})", json);

            return response.Ok ? JsonSerializer.Deserialize<PetResponse>(json, GetOptions()) : null;
        }

        /// <summary>
        /// Returns a list of pets filtered by their current status (available, pending, sold).
        /// </summary>
        public async Task<IEnumerable<PetResponse>> GetPetsByStatusAsync(string status)
        {
            _logger.LogInformation("📋 GET -> pet/findByStatus?status={Status}", status);

            var response = await _api.GetAsync("pet/findByStatus", new APIRequestContextOptions
            {
                // In Playwright .NET, we use 'Params' for query string parameters
                Params = new Dictionary<string, object> { { "status", status } }
            });

            var json = await response.TextAsync();
            _logger.LogJsonDebug($"Response GET /pet/findByStatus ({response.Status})", json);

            if (!response.Ok) return Enumerable.Empty<PetResponse>();

            return JsonSerializer.Deserialize<IEnumerable<PetResponse>>(json, GetOptions()) ?? Enumerable.Empty<PetResponse>();
        }

        /// <summary>
        /// Places an order for a specific pet.
        /// </summary>
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest orderRequest)
        {
            _logger.LogInformation("🛒 POST -> store/order | Creating order for PetId: {PetId}", orderRequest.PetId);

            var response = await _api.PostAsync("store/order", new APIRequestContextOptions
            {
                DataObject = orderRequest
            });

            var json = await response.TextAsync();
            _logger.LogJsonDebug($"Response POST /store/order ({response.Status})", json);

            return response.Ok ? JsonSerializer.Deserialize<OrderResponse>(json, GetOptions()) : null;
        }

        /// <summary>
        /// Common JSON serializer options for the service.
        /// </summary>
        private JsonSerializerOptions GetOptions() => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
}