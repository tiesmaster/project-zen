using System.Diagnostics;

using Serilog;

namespace Tiesmaster.ProjectZen.BagImporter
{
    public static class LoggerExtensions
    {
        public static void StartReadingBagFile(this ILogger logger, string fileName, int fileIndex, int totalFilesToRead)
        {
            logger
                .ForContext("BagObjectFileName", fileName)
                .Debug("Processing file {CurrentFileIndex} / {TotalFilesCount}", fileIndex + 1, totalFilesToRead);
        }

        public static void FinishedReadingBagFile(
            this ILogger logger,
            Stopwatch singleFileElapsed,
            Stopwatch totalElapsed,
            int totalFilesRead,
            int totalCountRead)
        {
            logger.Debug(
                "Processed in: {MsPerFile} ms (Average: {MsPerFileAverage} ms) | Total: {TotalCountRead:N0}",
                singleFileElapsed.ElapsedMilliseconds,
                totalElapsed.ElapsedMilliseconds / totalFilesRead,
                totalCountRead);
        }

        public static void StartReadingBagObjects(this ILogger logger, string bagObjectNamePlural, int totalFilesToRead)
        {
            logger.Information("Start reading {BagObjectName} over {TotalFilesCount} files", bagObjectNamePlural, totalFilesToRead);
        }

        public static void FinishReadingBagObjects(
            this ILogger logger,
            string bagObjectNamePlural,
            int totalCountRead,
            int totalFilesRead,
            Stopwatch totalElapsed)
        {
            logger.Information(
                "Finished reading {BagObjectName} ({TotalCountRead:N0} objects across {TotalFilesRead:N0} files " +
                "in {TotalElapsed} | {MsPerFile} ms / file)",
                bagObjectNamePlural,
                totalCountRead,
                totalFilesRead,
                totalElapsed.Elapsed,
                totalElapsed.ElapsedMilliseconds / totalFilesRead);
        }
    }
}