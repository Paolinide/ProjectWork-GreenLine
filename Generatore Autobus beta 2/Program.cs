using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Generatore_Autobus_beta_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            // var j = new JsonDataRecord();

            // j.SetId(3).SetIndex(69);
            // j.Print();

            // MemoryStream stream = j.CreateJsonStream();
            // // MemoryStream stream = new MemoryStream();
            // // DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(JsonDataRecord));
            // // jsonSerializer.WriteObject(stream, j);

            // Console.Write("JSON form of JsonDataRecord object: ");
            // Console.WriteLine(JsonDataRecord.JsonToString(stream));

            // var j2 = JsonDataRecord.ReadJsonStream(stream);
            // // jsonSerializer = new DataContractJsonSerializer(typeof(JsonDataRecord));
            // // //jsonSerializer.WriteObject(stream, new JsonDataRecord());
            // // stream.Position = 0;
            // // var j2 = (JsonDataRecord)jsonSerializer.ReadObject(stream);

            // j.SetId(4).SetIndex(46);
            // j.Print();
            // j2.Print();
            // var j3 = j.Clone();
            // j3.Print();
            // Preferences.SavePreferences();
            Preferences.LoadPreferences(); // carichiamo le preferenze dal file di preferenze
            Trasmission.ConnectionAUTO(); // impostata su AUTO la connessione viene verificata ad ogni ciclo
            // Trasmission.DisattivaConnessione();
            // Console.WriteLine(Archive.dataFileName(1));
            // int id = 0;
            // var j1 = (new JsonDataRecord().SetId(++id).UpdateIdRecord());
            // var j2 = (new JsonDataRecord().SetId(++id).UpdateIdRecord());
            // var j3 = (new JsonDataRecord().SetId(++id).UpdateIdRecord());
            // ICollection<JsonDataRecord> list = new List<JsonDataRecord>();
            // list.Add(j1);
            // list.Add(j2);
            // list.Add(j3);
            // Console.WriteLine(JsonDataRecord.JsonToString(JsonDataRecord.StackToStream(list)));

            Console.WriteLine(" ***********  INIZIO  *********** ");
            var v1 = new Vehicle(); // creazione di un veicolo virtuale che simula un percorso casuale
            for (int i = 1; i <= 40; i++)
            {
                Console.Write($"{i,2} ");
                if (i == 10) Trasmission.ConnectionOFF(); // viene simulata una disconnessione
                if (i == 30) Trasmission.ConnectionON(); // la connessione viene riattivata
                System.Threading.Thread.Sleep(200); // aspetta 1 secondo
                v1.Aggiorna(); // il veicolo si sposta ed aggiorna i suoi valori e li trasmette
            }
            // adesso controlliamo se i record sono stati spediti tutti o qualcuno è rimasto in attesa nello stack
            if (!Trasmission.connessioneAttiva) Console.WriteLine();
            if (!Archive.savedRecords) Console.WriteLine("Nessun record rimasto in coda.");
            else foreach (var item in Archive.stack)
                    Console.WriteLine("Nello stack: " + ((JsonDataRecord)item).ToString());
            Console.WriteLine(" ***********   FINE   *********** ");
        }
    }
}
