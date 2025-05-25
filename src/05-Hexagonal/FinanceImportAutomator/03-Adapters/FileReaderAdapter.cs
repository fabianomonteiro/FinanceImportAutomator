using HexagonalFinanceImportAutomator._02_Ports;
using HexagonalFinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HexagonalFinanceImportAutomator._03_Adapters
{
    /// <summary>
    /// Adapter for file reading operations - implements the IFileReaderPort
    /// This is a driven adapter (secondary adapter)
    /// </summary>
    public class FileReaderAdapter : IFileReaderPort
    {
        private readonly INotificationPort _notificationPort;

        public FileReaderAdapter(INotificationPort notificationPort)
        {
            _notificationPort = notificationPort ?? throw new ArgumentNullException(nameof(notificationPort));
        }

        public IEnumerable<string> ReadLines(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _notificationPort.AddMessage("File path is required.");
                return Enumerable.Empty<string>();
            }

            if (!File.Exists(filePath))
            {
                _notificationPort.AddMessage($"File not found: {filePath}");
                return Enumerable.Empty<string>();
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                
                if (!lines.Any())
                {
                    _notificationPort.AddMessage("File is empty.");
                    return Enumerable.Empty<string>();
                }

                // Validate file format
                foreach (var line in lines)
                {
                    if (!line.Contains(';'))
                    {
                        _notificationPort.AddMessage("Incorrect format file. Expected semicolon-separated values.");
                        return Enumerable.Empty<string>();
                    }
                }

                return lines;
            }
            catch (Exception ex)
            {
                _notificationPort.AddMessage($"Error reading file: {ex.Message}");
                return Enumerable.Empty<string>();
            }
        }
    }
}
