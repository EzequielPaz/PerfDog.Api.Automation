using Microsoft.Extensions.Logging;

namespace PerfDog.Tests.Utils
{
    public class NUnitLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new NUnitLogger(categoryName);
        }

        public void Dispose() { }
    }

    public class NUnitLogger : ILogger
    {
        private readonly string _category;

        public NUnitLogger(string category)
        {
            // Solo el nombre final de la clase
            _category = category.Contains('.')
                ? category.Substring(category.LastIndexOf('.') + 1)
                : category;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            var prefix = GetLevelPrefix(logLevel);

            TestContext.Out.WriteLine(
                $"{DateTime.Now:HH:mm:ss.fff} {prefix} [{_category}] {message}"
            );

            if (exception != null)
            {
                TestContext.Out.WriteLine("💥 EXCEPTION:");
                TestContext.Out.WriteLine(exception);
            }
        }

        private static string GetLevelPrefix(LogLevel level)
        {
            return level switch
            {
                LogLevel.Trace => "🔍 TRACE",
                LogLevel.Debug => "🐛 DEBUG",
                LogLevel.Information => "ℹ️ INFO ",
                LogLevel.Warning => "⚠️ WARN ",
                LogLevel.Error => "❌ ERROR",
                LogLevel.Critical => "🔥 FATAL",
                _ => "LOG"
            };
        }

    }
}



