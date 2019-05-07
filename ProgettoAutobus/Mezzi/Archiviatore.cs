using System;
using System.Collections;
using System.IO;

namespace ProgettoAutobus
{
    static class Archiviatore // istanza dell'oggetto che archivia, se necessario, i dati prodotti dal veicolo
    {
        private static int _fileSalvati = 0; // indice dei file salvati
        public static bool fileInArchivio => _fileSalvati > 0; // indicatore di file in attesa di essere inviati
        public static bool Push(ref Stack pila)
        {
            // creiamo un file che contiene la pila di record e lo salviamo
            // N.B. Al momento della lettura viene anche post-incrementato il contatore dei file creati
            using (StreamWriter scriviFile = new StreamWriter($"pila{++_fileSalvati}.txt"))
            {
                scriviFile.WriteLine(JsonDataRecord.StackToString(pila));
                Console.WriteLine("creato il file: “pila" + _fileSalvati + "”");
            }
            // dopodiché, se siamo riusciti a salvare il file, la pila viene svuotata
            pila.Clear();
            return true;
        }
        public static string Pull()
        {
            // procediamo alla lettura
            if (fileInArchivio)
            {
                // N.B. Al momento della lettura il contatore dei file creati NON viene post-derementato
                string nomeFile = $"pila{_fileSalvati}.txt";
                string risultato = nomeFile + " vuoto?!"; // se viene visualizzata 'sta stringa, qualcosa non funziona!
                if (File.Exists(nomeFile)) // se esiste...
                {
                    // ...carichiamo l'ultimo file salvato
                    using (StreamReader readtext = new StreamReader(nomeFile))
                    {
                        risultato = readtext.ReadToEnd(); // e lo mettiamo in una stringa
                        Console.WriteLine("letto il file: “" + nomeFile + "”");
                    }
                }
                return risultato;
            }
            else return null; // se non ci sono file salvati, non viene restituito niente
        }
        public static void ThrowLast()
        {
            // procediamo alla lettura
            if (fileInArchivio)
            {
                // N.B. Al momento della lettura viene anche post-derementato il contatore dei file creati
                string nomeFile = $"pila{_fileSalvati--}.txt";
                if (File.Exists(nomeFile)) // se esiste...
                {
                    // eliminiamo il file caricato
                    File.Delete(nomeFile);
                    Console.WriteLine(nomeFile + " eliminato...");
                }
            }
        }
        public static void ThrowAll()
        {
            // procediamo alla lettura
            while (fileInArchivio)
            {
                // N.B. Al momento della lettura viene anche post-derementato il contatore dei file creati
                string nomeFile = $"pila{_fileSalvati--}.txt";
                if (File.Exists(nomeFile)) // se esiste...
                {
                    // eliminiamo il file caricato
                    File.Delete(nomeFile);
                    Console.WriteLine(nomeFile + " eliminato...");
                }
            }
        }
        public static string Pop()
        {
            // procediamo alla lettura
            if (fileInArchivio)
            {
                // N.B. Al momento della lettura viene anche post-derementato il contatore dei file creati
                string nomeFile = $"pila{_fileSalvati--}.txt";
                string risultato = nomeFile + " vuoto?!"; // se viene visualizzata 'sta stringa, qualcosa non funziona!
                if (File.Exists(nomeFile)) // se esiste...
                {
                    // ...carichiamo l'ultimo file salvato
                    using (StreamReader readtext = new StreamReader(nomeFile))
                    {
                        risultato = readtext.ReadToEnd(); // e lo mettiamo in una stringa
                        Console.WriteLine("letto il file: “" + nomeFile + "”");
                    }
                    // eliminiamo il file caricato
                    File.Delete(nomeFile);
                    Console.WriteLine(nomeFile + " eliminato...");
                }
                return risultato;
            }
            else return null; // se non ci sono file salvati, non viene restituito niente
        }
    }
}