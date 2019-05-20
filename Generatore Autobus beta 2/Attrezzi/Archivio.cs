using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Generatore_Autobus_beta_2
{
    static public class Archive
    {
        static public ICollection<JsonDataRecord> stack = new List<JsonDataRecord>();
        static public bool savedRecords => stack.Count > 0;
        static private int _filesCounter = 0;
        static public bool savedFiles => _filesCounter > 0;
        static public string dataFileName(int num) => Preferences.fileName.Replace("{0}", num.ToString());
        public static void KeepData(JsonDataRecord jsonData)
        {
            // Console.Write($" >> >> count: {stack.Count} - max: {Preferences.maxRecordsStacked} - ");
            // Console.Write($" Inserisco il nuovo record: {jsonData.ToJsonString()}\n - dentro lo stack: {JsonDataRecord.JsonToString(JsonDataRecord.StackToStream(stack))}  >> Nuovo stack: ");
            stack.Add(jsonData.Clone());
            // Console.WriteLine(JsonDataRecord.JsonToString(JsonDataRecord.StackToStream(stack)));
            //Console.Write($" <(cnt:{stack.Count} - max:{Preferences.maxRecordsStacked})> ");
            if (stack.Count >= Preferences.maxRecordsStacked)
            {
                //Console.Write("@");
                string fileName = dataFileName(_filesCounter + 1);
                string fileData = JsonDataRecord.JsonToString(JsonDataRecord.StackToStream(stack));
                if (SaveData(fileName, fileData)) stack.Clear();
            }
        }
        static public bool SaveData(string fileName, string fileData)
        {
            Console.Write($" > > Salvataggio del file “{fileName}”...");
            try
            {
                // creiamo un file che contiene la pila di record e lo salviamo
                using (StreamWriter scriviFile = new StreamWriter(fileName))
                {
                    scriviFile.WriteLine(fileData);
                    // Console.WriteLine($" ... effettuato con successo.");
                }
                _filesCounter += 1; // dopo la scrittura viene anche incrementato il contatore dei file
                return true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine($" ... fallito.\n   x x x   ERRORE:\n “{e.Message}”");
                return false;
            }
        }
        static public string LoadFile(string fileName)
        {
            Console.Write($" > > Carico il file “{fileName}”...");
            try
            {
                // procediamo alla lettura
                // if (savedFiles)
                // {
                string result = fileName + " non esiste?!"; // se viene restituita 'sta stringa, qualcosa non funziona!
                if (File.Exists(fileName)) // se esiste...
                {
                    // ...carichiamo il file richiesto
                    using (StreamReader readtext = new StreamReader(fileName))
                    {
                        result = readtext.ReadToEnd(); // e lo mettiamo in una stringa
                    }
                    Console.WriteLine($" ... trovato");
                    return result;
                }
                // }
                else return null; // se non ci sono file salvati viene restituito valore nullo
            }
            catch (System.Exception e)
            {
                Console.Write($" ... non trovato.\nERRORE:\n “{e.Message}”");
                return null;
            }
        }
        static public void DeleteFile(string fileName, bool decrementa = false)
        {
            // procediamo alla lettura
            if (savedFiles)
            {
                // N.B. Al momento della lettura viene anche post-derementato il contatore dei file creati
                if (File.Exists(fileName)) // se esiste...
                {
                    // eliminiamo il file caricato
                    File.Delete(fileName);
                    if (decrementa) _filesCounter -= 1;
                    Console.WriteLine(fileName + " eliminato...");
                }
            }
        }
    }
}
