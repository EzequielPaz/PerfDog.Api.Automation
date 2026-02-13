using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PerfDog.Tests.Utils
{
    public static class LoggerExtensions
    {
        public static void LogJsonDebug(
            this ILogger logger,
            string title,
            string json)
        {
            if (!logger.IsEnabled(LogLevel.Debug))
                return;

            string formattedJson = PrettyPrintJson(json);

            logger.LogDebug(
                "{Title}:\n{Json}",
                title,
                formattedJson
            );
        }

        private static string PrettyPrintJson(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                return JsonSerializer.Serialize(
                    doc,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
            }
            catch
            {
                return json;
            }
        }
    }

}
