using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using PerfDog.Tests.Utils;

namespace PerfDog.Tests.Base
{
    /// <summary>
    /// Base class for API testing. 
    /// Handles Playwright context initialization and logging configuration.
    /// </summary>
    public class BaseApiTest : PlaywrightTest
    {
        protected IAPIRequestContext RequestContext { get; private set; }
        protected ILogger<BaseApiTest> Logger { get; private set; }
        protected ILoggerFactory LoggerFactory { get; private set; }

        [SetUp]
        public async Task BaseSetup()
        {
            // 1. Initialize the Log Factory using the custom NUnitLoggerProvider
            LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
                builder.AddProvider(new NUnitLoggerProvider());
                // Set to Debug level to ensure the LogJsonDebug extension works
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            Logger = LoggerFactory.CreateLogger<BaseApiTest>();

            // 2. Configure Playwright API Request Context
            var contextOptions = new APIRequestNewContextOptions
            {
                BaseURL = "https://petstore.swagger.io/v2/",
                ExtraHTTPHeaders = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                    { "Content-Type", "application/json" },
                    { "api_key", "special-key" } 
                }
            };

            RequestContext = await Playwright.APIRequest.NewContextAsync(contextOptions);

            Logger.LogInformation("🚀 Starting Scenario: {TestName}", TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public async Task BaseTearDown()
        {
            Logger.LogInformation("🏁 Finishing Scenario: {Status}", TestContext.CurrentContext.Result.Outcome.Status);

            // Clean up resources to prevent memory leaks
            if (RequestContext != null)
            {
                await RequestContext.DisposeAsync();
            }

            LoggerFactory?.Dispose();
        }
    }
}