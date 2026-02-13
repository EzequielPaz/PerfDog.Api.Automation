using Microsoft.Extensions.Logging;
using PerfDog.Tests.Base;
using PerfDog.Tests.Models.Order;
using PerfDog.Tests.Models.Pet;
using PerfDog.Tests.Service;

namespace PerfDog.Tests.Test
{
    [TestFixture]
    public class PetstoreTests : BaseApiTest // Inheriting from Base to use centralized Context and Loggers
    {
        private PetService _petService;

        [SetUp]
        public void SetUp()
        {
            // Initialize the service using the Base class LoggerFactory and RequestContext
            var serviceLogger = LoggerFactory.CreateLogger<PetService>();
            _petService = new PetService(RequestContext, serviceLogger);
        }

        /// <summary>
        /// Part 1: Creates 10 pets with a specific status distribution 
        /// and verifies the details of the last 'sold' pet.
        /// </summary>
        [Test]
        public async Task CreateTenPetsAndVerifySoldPetDetails()
        {
            // 1. Define the status distribution: 5 available, 4 pending, 1 sold
            var distribution = new List<string>();
            distribution.AddRange(Enumerable.Repeat("available", 5));
            distribution.AddRange(Enumerable.Repeat("pending", 4));
            distribution.Add("sold");

            PetResponse? lastSoldPet = null;

            // 2. Loop to create the 10 pets
            for (int i = 0; i < distribution.Count; i++)
            {
                var request = new PetRequest
                {
                    // Generate unique ID based on current timestamp
                    Id = DateTimeOffset.Now.ToUnixTimeMilliseconds() + i,
                    Name = $"PerfDog_{distribution[i]}_{i}",
                    Status = distribution[i],
                    PhotoUrls = new List<string> { "https://perfdog.com/dog.jpg" }
                };

                var response = await _petService.CreatePetAsync(request);
                Assert.That(response, Is.Not.Null, $"Failed to create pet at index {i}");

                // Store the reference of the 'sold' pet for later verification
                if (distribution[i] == "sold") lastSoldPet = response;
            }

            // 3. Fetch details of the 'sold' pet by ID and validate its status
            Assert.That(lastSoldPet, Is.Not.Null, "No pet with 'sold' status was created.");
            var verifiedPet = await _petService.GetPetByIdAsync(lastSoldPet.Id);

            Assert.That(verifiedPet, Is.Not.Null, "Could not retrieve the sold pet from the API.");
            Assert.That(verifiedPet.Status, Is.EqualTo("sold"), "The pet status does not match 'sold'.");

            Logger.LogInformation("✅ Sold Pet verified successfully: {Id}", verifiedPet.Id);
        }

        /// <summary>
        /// Part 2: Fetches pets with 'available' status, stores 5 in a list,
        /// and creates a store order for each one.
        /// </summary>
        [Test]
        public async Task ListAvailablePetsAndCreateStoreOrders()
        {
            // 1. Retrieve pets with 'available' status from the API
            var availablePets = await _petService.GetPetsByStatusAsync("available");

            // 2. Take the first 5 pets and store them in a list
            var selectedPets = availablePets.Take(5).ToList();
            Assert.That(selectedPets.Count, Is.EqualTo(5), "Insufficient available pets found to perform the test.");

            // 3. Iterate through the selected pets and create a store order for each
            foreach (var pet in selectedPets)
            {
                var orderReq = new OrderRequest
                {
                    Id = DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000, // Simplified unique ID
                    PetId = pet.Id,
                    Quantity = 1,
                    ShipDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Status = "placed",
                    Complete = true
                };

                var orderResponse = await _petService.CreateOrderAsync(orderReq);

                // Validation: Ensure the order was created and references the correct PetId
                Assert.That(orderResponse, Is.Not.Null, $"Failed to create order for Pet ID {pet.Id}");
                Assert.That(orderResponse.PetId, Is.EqualTo(pet.Id), "The Order response PetId does not match.");

                Logger.LogInformation("✅ Order successfully created: PetId {PetId} -> OrderId {OrderId}", pet.Id, orderResponse.Id);
            }
        }
    }
}