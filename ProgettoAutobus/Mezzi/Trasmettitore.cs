using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Http;
using System.Text;
using System.IO;
using static ProgettoAutobus.Archiviatore;

namespace ProgettoAutobus
{
    static class Trasmettitore // l'elemento che trasmette i dati al server
    {
        static private int limiteRecord = 5; // indica quanti record vengono messi nella pila prima di creare un file nuovo con essa
        private static readonly HttpClient client = new HttpClient();
        static string indirizzoServer = "http://127.0.0.1:3000/";
        static Stack pila = new Stack(); // il contenitore dei record
        static private bool? _connessione = null; // per forzare a credere la connessione aperta o chiusa, automatica di default
        static public void AttivaConnessione() => _connessione = true;
        static public void DisattivaConnessione() => _connessione = false;
        static public void ConnessioneAutomatica() => _connessione = null;
        static public bool connessioneAttiva
        {
            get
            {
                // verifichiamo lo stato della connessione
                if (_connessione != null) return (bool)_connessione;
                try
                {
                    Ping ping = new Ping();
                    PingReply pingresult = ping.Send("8.8.8.8");//indirizzoServer);
                    Console.WriteLine("*** connessione attiva!");
                    return (pingresult.Status.ToString() == "Success");
                }
                catch (System.Exception)
                {
                    Console.WriteLine("*** connessione non attiva!");
                    return false;
                    throw;
                }
            }
        }
        static public bool InoltraRecord(JsonDataRecord jsonData)
        {
            Console.WriteLine("Cerco di inviare: " + jsonData);
            // se la connessione è attiva, spediamo il dato al server
            if (connessioneAttiva)
            {
                InoltraAlServer(jsonData);
                return true;
            }
            else
            {
                // altrimenti la rete non è raggiungibile e memoriziamo i dati in locale
                Trattieni(jsonData);
                return false;
            }
        }
        static void InoltraAlServer(JsonDataRecord jsonData, bool trattieni = true)
        {
            // non ci sono impedimenti e quindi spediamo il nostro dato al server
            if (!Spedisci("[ " + jsonData.ToString() + " ]") && trattieni) Trattieni(jsonData);
            // poi verifichiamo se la pila ha altri record da inviare
            if (pila.Count > 0)
            {
                // spediamone il contenuto al server e  poi svuotiamo la pila
                if (Spedisci(JsonDataRecord.StackToString(pila))) pila.Clear();
                else return; // se ci sono problemi non se ne fa niente
            }
            // verifichiamo se ci sono file salvati da inviare
            while (fileInArchivio)
            {
                // cerchiamo di spedire l'ultimo file recuperato
                if (Spedisci(Pull())) ThrowLast();
                else return; // se ci sono problemi non se ne fa niente
                // // recuperiamo i dati archiviati
                // string stringaPila = Archiviatore.Pop();
                // // e spediamo al server anche i file archiviati
                // Spedisci(stringaPila);
            };
        }
        static bool Spedisci(string stringaPila)
        { // trasmissione di pile di record
            // l'oggetto jsonData è stato pensato come array di record, pertanto può essere inviata l'intera pila senza dover ciclare i suoi elementi
            try
            {
                Console.WriteLine("Inoltro:");
                Console.WriteLine(stringaPila);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(indirizzoServer);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //streamWriter.Write("{\"uno\": \"uno\"}");
                    streamWriter.Write(stringaPila);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

                Console.WriteLine("*** Invio Riuscito!");
                return true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("*** Invio Fallito:\n" + e);
                return false;
                throw;
            }
        }
        static public void InviaMessaggio(string messaggio)
        { // trasmissione di pile di record
          // l'oggetto jsonData è stato pensato come array di record, pertanto può essere inviata l'intera pila senza dover ciclare i suoi elementi
            Console.WriteLine("Inoltro:");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(indirizzoServer);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write("{ \"messaggio\": " + "\"" + messaggio + "\" }");
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
        static void Trattieni(JsonDataRecord jsonData)
        {
            // infila il dato in una pila
            pila.Push(jsonData);
            // se la pila raggiunge una certa dimensione crea un file coi dati e svuota la pila
            if (pila.Count >= limiteRecord) Push(ref pila); // la pila viene passata per iferimento per poter essere manipolata
        }
    }
}