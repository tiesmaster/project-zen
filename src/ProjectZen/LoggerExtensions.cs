using System.Diagnostics;

using Serilog;

namespace Tiesmaster.ProjectZen
{
    public static class LoggerExtensions
    {
        public static void StartImport(this ILogger logger)
        {
            logger.Information("Start import");
        }

        public static void FinishedImport(this ILogger logger, Stopwatch totalElapsed)
        {
            logger.Information("Finished Import in {TotalElapsed}", totalElapsed.Elapsed);
        }
    }
}