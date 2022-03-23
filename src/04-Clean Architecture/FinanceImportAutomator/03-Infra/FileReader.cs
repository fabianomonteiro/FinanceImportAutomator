using CleanArchitectureFinanceImportAutomator._01_Application;
using CleanArchitectureFinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CleanArchitectureFinanceImportAutomator._03_Infra
{
    public class FileReader : Interactor<string, IEnumerable<string>>, IFileReader
    {
        public string Path => Input;

        private readonly INotification _notification;

        public FileReader(INotification notification)
        {
            _notification = notification;
        }

        protected override IEnumerable<string> ImplementExecute(string input)
        {
            StreamReader streamReader = null;
            List<string> lines = new List<string>();

            try
            {
                int lineIndex = -1;
                string line;

                // Lê o arquivo
                streamReader = File.OpenText(Path);

                // Percorre o arquivo linha a linha
                while ((line = streamReader.ReadLine()) != null)
                {
                    lineIndex++;

                    // Valida se o conteúdo do arquivo está em formato correto
                    if (!line.Contains(';'))
                    {
                        _notification.AddNotification("Incorrect format file.");
                        return lines;
                    }

                    // Pular cabeçalho
                    if (lineIndex == 0)
                        continue;

                    lines.Add(line);                    
                }
            }
            catch (Exception ex)
            {
                _notification.AddNotification($"An unexpected error occurred in the application. Error: {ex.Message}");
            }
            finally
            {
                streamReader?.Close();
            }

            return lines;
        }
    }
}
