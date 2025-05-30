﻿using DDDFinanceImportAutomator._01_Application;
using DDDFinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace DDDFinanceImportAutomator._03_Infra
{
    public class TransactionInfraService : ITransactionInfraService
    {
        private readonly INotification _notification;

        public TransactionInfraService(INotification notification)
        {
            _notification = notification;
        }

        public IEnumerable<string> ReadTransactionsToImport(string path)
        {
            LogHelper.LogStart(nameof(ReadTransactionsToImport));

            StreamReader streamReader = null;
            List<string> lines = new List<string>();

            try
            {
                int lineIndex = -1;
                string line;

                // Lê o arquivo
                streamReader = File.OpenText(path);

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
                LogHelper.LogError(nameof(ReadTransactionsToImport), ex);

                _notification.AddNotification($"An unexpected error occurred in the application. Error: {ex.Message}");
            }
            finally
            {
                streamReader?.Close();

                LogHelper.LogEnd(nameof(ReadTransactionsToImport));
            }

            return lines;
        }
    }
}
