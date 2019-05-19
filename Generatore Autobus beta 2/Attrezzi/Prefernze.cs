using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Generatore_Autobus_beta_2
{
    // Questa classe serve a gestire tutte le preferenze dell'applicazione
    public static class Preferences
    {
        // * * *    VARIABILI COI VALORI PREDEFINITI    * * * 
        // RETE
        public static string pingAddress = "8.8.8.8";
        public static string serverAddress = "http://127.0.0.1:4000/";
        // ARCHIVIO
        public static string preferencesFileName = "Preferences.pref"; // nome del file che archivia le preferenze
        public static string fileName = "RecordsSaved_{0}.save"; // nome del file che archivia i record non inviati
        public static int maxRecordsStacked = 5; // numero di record impilati prima di creare un nuovo file
        public static string dataPreferences
        {
            get => $"Destinatario del Ping: {pingAddress}\n"
                 + $"Indirizzo del Server di archiviazione dati: {serverAddress}\n"
                 + $"Nome di questo file: {preferencesFileName}\n"
                 + $"Nome del file di salvataggio dei record non iviati: {fileName}\n"
                 + $"Numero di record accumulati per ciascun file: {maxRecordsStacked}";
        }
        public static void SavePreferences() => Archive.SaveData(preferencesFileName, dataPreferences);
        public static void LoadPreferences()
        {
            string dataPreferences = Archive.LoadFile(preferencesFileName);
            //Console.WriteLine($"{dataPreferences}");
            string[] arrayPreferences = Regex.Split(dataPreferences, "\n");
            for (int i = 0; i < arrayPreferences.Length - 1; i++)
                arrayPreferences[i] = $"{Regex.Split(arrayPreferences[i], ": ")[1]}";
            pingAddress = arrayPreferences[0];
            serverAddress = arrayPreferences[1];
            preferencesFileName = arrayPreferences[2];
            fileName = arrayPreferences[3];
            int.TryParse(arrayPreferences[4], out maxRecordsStacked);
        }
    }
}