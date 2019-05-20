using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.IO;

namespace Generatore_Autobus_beta_2
{
    static public class Trasmission
    {
        // * * *     VARIABILI E RELATIVI GESTORI     * * *
        private static readonly HttpClient client = new HttpClient();
        static private bool? _connessione = null; // per forzare a credere la connessione aperta, chiusa o automatica (predefinito)
        static private bool? connessione
        {
            set
            {
                // if (value == null) Console.WriteLine("Connessione Auto.");
                // else Console.WriteLine("Connessione " + ((bool)value ? "attiva" : "inattiva"));
                _connessione = value;
            }
        }
        static public void ConnectionON() => connessione = true;
        static public void ConnectionOFF() => connessione = false;
        static public void ConnectionAUTO() => connessione = null;
        static public bool connessioneAttiva
        {
            get
            {
                // verifichiamo lo stato della connessione
                if (_connessione != null) return (bool)_connessione;
                try
                {
                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send(Preferences.pingAddress);//indirizzoServer);
                    //Console.WriteLine(" + + +  connessione attiva!  + + +");
                    return (pingresult.Status.ToString() == "Success");
                }
                catch (System.Exception)
                {
                    Console.WriteLine(" x x x  connessione non attiva!  x x x");
                    return false;
                    // throw;
                }
            }
        }
        // * * *   TRASMISSIONE   * * *
        // Riceviamo un record e vediamo se si può trasmettere...
        static public bool TrySend(JsonDataRecord jsonData, bool keep = true)
        {
            // Console.WriteLine("  * * Cerco di inviare: " + jsonData);
            if (!connessioneAttiva) // se la rete non è raggiungibile memoriziamo i dati in locale
            {
                Archive.KeepData(jsonData);
                return false;
            }
            else // altrimenti la connessione è attiva e cerchiamo di spediare il record al server
            {
                // non ci sono impedimenti e quindi spediamo il nostro dato al server
                if (keep && !Send("[ " + jsonData.ToJsonString() + " ]")) Archive.KeepData(jsonData);
                // poi verifichiamo se lo stack ha altri record da inviare
                if (Archive.stack.Count > 0)
                {
                    // spediamone il contenuto al server e  poi svuotiamo lo stack
                    if (Send(JsonDataRecord.StackToString(Archive.stack))) Archive.stack.Clear();
                    else return false; // se ci sono problemi non se ne fa niente
                }
                // verifichiamo se ci sono file salvati da inviare
                int fileCount = 0;
                while (Archive.savedFiles)
                {
                    // cerchiamo di spedire i file coi record archiviati
                    string fileName = Archive.dataFileName(++fileCount);
                    if (Send(Archive.LoadFile(fileName))) Archive.DeleteFile(fileName, true);
                    else return false; // se ci sono problemi non se ne fa niente
                };
                return true;
            }
        }

        static private bool Send(string stringaJson)
        {
            try
            {
                Console.WriteLine($"Inoltro:\n{stringaJson}\n");
                // parametri d'invio
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Preferences.serverAddress);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                // tentativo d'invio
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(stringaJson); // invio del file json attraverso uno stream
                    streamWriter.Flush(); // svuotamento dello stream
                    streamWriter.Close(); // chiusura dello stream
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null && result != "Ok") Console.WriteLine(result);
                }

                // Console.WriteLine("*** Invio Riuscito.");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invio Fallito." + e);
                return false;
                // throw;
            }
        }
    }
}
