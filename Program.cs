// See https://aka.ms/new-console-template for more information

using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace LogcatFormatter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");
                string inputDir = Path.Combine(BaseDirectory, "InputLogs");
                string outputDir = Path.Combine(BaseDirectory, "OutputLogs");

                Directory.CreateDirectory(outputDir);

                string[] logFiles = Directory.GetFiles(inputDir, "*.logcat");
                if (logFiles.Length == 0)
                {
                    Console.WriteLine("No log files found in the input directory.");
                    return;
                }

                foreach (var filePath in logFiles)
                {
                    Console.WriteLine($"Processing file: {Path.GetFileName(filePath)}");

                    // Read content from the file
                    string jsonContent = File.ReadAllText(filePath);
                    JObject logcatObject = JObject.Parse(jsonContent);

                    if (logcatObject == null)
                    {
                        Console.WriteLine($"Error parsing JSON in file: {Path.GetFileName(filePath)}");
                        continue;
                    }

                    List<string> formattedLogs = [];

                    var logEntries = logcatObject["logcatMessages"];
                    if (logEntries == null)
                    {
                        Console.WriteLine($"Empty log file: {Path.GetFileName(filePath)}");
                        continue;
                    }

                    string? logHeader = FormatHeader(logcatObject["metadata"]);
                    if (logHeader != null)
                    {
                        formattedLogs.Add(logHeader);
                    }

                    foreach (var logEntry in logEntries)
                    {
                        string? formattedLine = FormatLogEntry(logEntry);
                        if (formattedLine != null)
                        {
                            formattedLogs.Add(formattedLine);
                        }
                    }

                    string outputPath = $"{Path.GetFileNameWithoutExtension(filePath)}.log";
                    outputPath = Path.Combine(outputDir, outputPath);
                    File.WriteAllLines(outputPath, formattedLogs);
                    Console.WriteLine($"Operation completed, output path: {outputPath}");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message}");
                throw;
            }

        }

        private static string? FormatLogEntry(JToken? logEntry)
        {
            if (logEntry == null) return null;
            try
            {
                var header = logEntry["header"];
                string? logLevel = GetLogLevelText(header?["logLevel"]?.ToString() ?? "N/A");
                string? tag = header?["tag"]?.ToString();
                string? timestamp = ConvertToISO8601(header?["timestamp"]);

                string? message = logEntry?["message"]?.ToString();

                return $"[{logLevel}] {timestamp} [{tag}] {message}";
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message}");
                throw;
            }
        }

        private static string GetLogLevelText(string logLevel)
        {
            return logLevel switch
            {
                "INFO" => "I",
                "WARN" => "W",
                "ERROR" => "E",
                "DEBUG" => "D",
                "VERBOSE" => "V",
                _ => logLevel
            };
        }

        private static string? ConvertToISO8601(JToken? timestamp)
        {
            if (timestamp == null) return null;
            try
            {
                long? timestampSeconds = (long?)timestamp?["seconds"] ?? 0L;
                int? timestampNanos = (int?)timestamp?["nanos"] ?? 0;

                // Convert timestamp to DateTime
                DateTime datetime = DateTimeOffset.FromUnixTimeSeconds((long)timestampSeconds).DateTime;
                return datetime.ToString("yyyy-MM-dd HH:mm:ss") + $".{timestampNanos / 1000000:D3}";
            }
            catch
            {
                return null; // Return original if parsing fails
            }
        }

        private static string? FormatHeader(JToken? metadata)
        {
            if (metadata == null) return null;
            try
            {
                string? formattedHeader = "";
                JToken? device = metadata["device"];
                if (device != null)
                {
                    // Device information

                    // Table header
                    string tableHeader = "Device Info:\n" +
                                         "| ID              | Name           | Serial Number    | Online | Release | SDK | Feature Level | Model | Type     | Emulator |\n" +
                                         "|-----------------|----------------|------------------|--------|---------|-----|---------------|-------|----------|----------|\n";

                    // Sample row (You can replace this with real data)
                    string tableRow = $"| {device["deviceId"],-15} | {device["name"],-14} | {device["serialNumber"],-16} | {device["isOnline"],-6} | {device["release"],-7} | {device["sdk"],-3} | {device["featureLevel"],-13} | {device["model"],-5} | {device["type"],-8} | {device["isEmulator"],-8} |\n";

                    // Combine the device info and the table to return the final result
                    formattedHeader += tableHeader + tableRow;
                }

                return formattedHeader;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Cannot read metadata: {ex.Message}");
                return null;
            }
        }
    }
}