using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Http;
using System.Text;
using System.IO;

namespace ProgettoAutobus
{
    static class Trasmettitore // l'elemento che trasmette i dati al server
    {
        static public bool connessione = false; /////////////////// QUESTA SERVE SOLO PER IL DEBUG, POI VA ELIMINATA
        static private int limiteRecord = 5; // indica quanti record vengono messi nella pila prima di creare un file nuovo con essa
        private static readonly HttpClient client = new HttpClient();
        static string indirizzoServer = "127.0.0.1";
        static bool connessioneAttiva => VerificaConnessione();
        static Stack pila = new Stack(); // il contenitore dei record
        //Archiviatore a; // il sistema di archiviazione che però non ha senso sia un'istanza
        static public bool VerificaConnessione()
        {
            // verifichiamo lo stato della connessione
            return connessione;
            try
            {
                Ping ping = new Ping();
                PingReply pingresult = ping.Send(indirizzoServer);
                return (pingresult.Status.ToString() == "Success");
            }
            catch (System.Exception)
            {
                return false;
                throw;
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
            // altrimenti la rete non è raggiungibile e memoriziamo i dati in locale
            Trattieni(jsonData);
            return false;
        }
        static void InoltraAlServer(JsonDataRecord jsonData)
        {
            // non ci sono impedimenti e quindi spediamo il nostro dato al server
            if (!Spedisci("{ \"VettoreDati\": [ " + jsonData.ToString() + " ]}")) Trattieni(jsonData);
            // poi verifichiamo se la pila ha altri record da inviare
            if (pila.Count > 0)
            {
                // spediamone il contenuto al server e  poi svuotiamo la pila
                if (Spedisci(JsonDataRecord.StackToString(pila))) pila.Clear();
                else return; // se ci sono problemi non se ne fa niente
            }
            // verifichiamo se ci sono file salvati da inviare
            while (Archiviatore.fileInArchivio)
            {
                // cerchiamo di spedire l'ultimo file recuperato
                if (Spedisci(Archiviatore.Pull())) Archiviatore.ThrowLast();
                else return; // se ci sono problemi non se ne fa niente
                // // recuperiamo i dati archiviati
                // string stringaPila = Archiviatore.Pop();
                // // e spediamo al server anche i file archiviati
                // Spedisci(stringaPila);
            };
        }

        static bool Spedisci(string stringaPila)
        { // trasmissione di pile di record
            // l'oggetto jsonData è stato previsto anche come array di record, pertanto può essere inviata l'intera pila senza dover ciclare i suoi elementi
            try
            {
                Console.WriteLine("Inoltro:");
                Console.WriteLine(stringaPila);

                var request = (HttpWebRequest)WebRequest.Create(indirizzoServer);

                var data = Encoding.ASCII.GetBytes(stringaPila);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return true;
            }
            catch (System.Exception)
            {
                return false;
                throw;
            }
        }
        static void Trattieni(JsonDataRecord jsonData)
        {
            // infila il dato in una pila
            pila.Push(jsonData);
            // se la pila raggiunge una certa dimensione crea un file coi dati e svuota la pila
            if (pila.Count >= limiteRecord) Archiviatore.Push(ref pila); // la pila viene passata per iferimento per poter essere manipolata
        }
    }
}