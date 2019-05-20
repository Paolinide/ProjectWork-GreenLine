using System;
using System.Collections;
using System.IO;
using static ProgettoAutobus.Trasmettitore;

namespace ProgettoAutobus
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Clear();
            for (int i = 10; i > 0; i--)
            {
                Console.Write("Inizio tra {0} secondi.", i);
                System.Threading.Thread.Sleep(1000); // aspetta 1 secondo
                Console.Clear();
            }

            // Stack pila = new Stack();
            // JsonDataRecord jsdata = new JsonDataRecord();
            // pila.Push(jsdata);
            // Console.WriteLine(JsonDataRecord.StackToString(pila));
            // return;
            // Console.WriteLine("{ \"" + "titolo" + "\": " + "\"" + "messaggio" + "\" }");
            Console.WriteLine(" ***********  INIZIO  *********** ");
            //return;
            var v1 = new Veicolo();
            //ConnessioneAutomatica();
            //DisattivaConnessione();
            // InviaMessaggio("--- INIZIO ---");
            for (int i = 0; i < 30; i++)
            {
                if (i > 6) AttivaConnessione();
                System.Threading.Thread.Sleep(1000); // aspetta 1 secondo
                v1.Aggiorna();
            }
            Archiviatore.ThrowAll();
            Console.WriteLine($"Connessione {(Trasmettitore.connessioneAttiva ? "" : "non ")}attiva");
            Console.WriteLine(" ***********   FINE   *********** ");

            // var v2 = new Veicolo();
            // var v3 = new Veicolo();
            // var v4 = new Veicolo();
            // var v5 = new Veicolo();

            // JsonDataRecord jr = new JsonDataRecord();
            // // //Console.WriteLine(jr);
            // Stack pila = new Stack();
            // pila.Push(jr.SetId(1));
            // var jr2 = new JsonDataRecord();
            // jr2.SetId(2).SetDescrizione("Portogruaro-Palse");
            // pila.Push(jr2);
            // var jr3 = new JsonDataRecord();
            // jr3.SetId(3).SetDescrizione("Udine-Tarvisio");
            // pila.Push(jr3);
            // // Console.WriteLine(JsonDataRecord.StackToString(pila));
            // // for (int i = 0; i < 10; i++)
            // // {
            // //     Console.WriteLine(((double)(new Random()).Next(10)-5.0) / 1000);
            // // }
            // // creazione file
            // // using (StreamWriter writetext = new StreamWriter("pila1.txt"))
            // // {
            // //     writetext.WriteLine(JsonDataRecord.StackToString(pila));
            // //     Console.WriteLine("creato...");
            // // }
            // // // lettura file
            // // using (StreamReader readtext = new StreamReader("pila1.txt"))
            // // {
            // //     string readMeText = readtext.ReadToEnd();
            // //     Console.WriteLine("letto...");
            // //     Console.WriteLine(readMeText);
            // // }
            // // // eliminazione file
            // // if (File.Exists("pila1.txt"))
            // // {
            // //     // File.Delete("pila1.txt");
            // //     Console.WriteLine("eliminato...");
            // // }
            // // Archiviatore.Push(ref pila);
            // // pila.Push(jr);
            // // pila.Push(jr2);
            // // pila.Push(jr3);
            // // Archiviatore.Push(ref pila);
            // // Console.WriteLine(JsonDataRecord.StackToString(pila));
            // // Console.WriteLine(Archiviatore.Pop());
            // // Console.WriteLine(Archiviatore.Pop());
            // Trasmettitore.InoltraRecord(jr);
            // Trasmettitore.InoltraRecord(jr2);
            // Trasmettitore.InoltraRecord(jr3);
            // Trasmettitore.connessione = true;
            // Trasmettitore.InoltraRecord(jr);
        }
    }
}
