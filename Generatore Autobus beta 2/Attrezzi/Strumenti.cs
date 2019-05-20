using System;
using System.Collections;
using System.Collections.Generic;
using static Generatore_Autobus_beta_2.Trasmission;
using static Generatore_Autobus_beta_2.Archive;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Generatore_Autobus_beta_2
{
    class Vehicle // istanza generica di un mezzo, che produce i dati da inviare
    {
        static private int numeroDiMezzi = 0; // questo si ricorda del numero di istanze generate
        static private int seriale { get => numeroDiMezzi++; } // quando viene letto il valore, dopo la restituzione viene incrementato
        private JsonDataRecord _jsonData = new JsonDataRecord();// contenitore dei dati;
        private int direzione = 0; //  // direzione va da -1 a 2 (-1:sinistra 0:fermo 1:dritto 2:destra)
        private double angolo = 0.0; // angolo effettivo di spostamento
        private double velocità = 0.1; // velocità di spostamento
        private int passi = 10; // passi al termine della direzione attuale
        public Vehicle()
        {
            if (numeroDiMezzi < 5) ScegliCittà(numeroDiMezzi, ref _jsonData);
            else
            {
                ScegliCittà(0, ref _jsonData);
                _jsonData.SetDescription(_jsonData.getDescription + (numeroDiMezzi - 3));
            }
            _jsonData.SetId(seriale);
            // Console.WriteLine($" ***** Nuovo veicolo (id: {_jsonData.id}) “{_jsonData.getDescription}”");
        }
        public void Aggiorna()
        {
            if (passi-- == 0) CambiaDirezione();// finiti i passi si cambia direzione (N.B. l'operatore di post-incremento garantisce la variazione)
            if (direzione == 0) // quando ci siamo fermati
            {
                if (passi == 2) // appena fermati
                {
                    // si aprono le porte
                    _jsonData.SetDoors(true);
                }
                else if (passi == 1) // stiamo per ripartire
                {
                    // qualcuno potrebbe essere salito o sceso
                    _jsonData.AddPassenger((new Random()).Next(5) - 2);
                }
                else if (passi == 0) // stiamo per ripartire
                {
                    // si chiudono le porte
                    _jsonData.SetDoors(false);
                }
            }
            else // altrimenti siamo in movimmento
            {
                if (direzione != 1) // stiamo facendo una curva
                {
                    angolo += (direzione == 0) ? -0.05 : 0.05; // quindi variamo l'angolo di spostamento
                }
                // comunque stiamo andando avanti e modifichiamo le coordinate
                _jsonData.AddLatitude(velocità * Math.Sin(angolo));
                _jsonData.AddLongitude(velocità * Math.Cos(angolo));
            }
            // Console.WriteLine($"Attuale direzione: {direzione} - angolo: {angolo} - velocità: {velocità} - passi: {passi}");
            Trasmission.TrySend(_jsonData.UpdateIdRecord());
        }
        private void CambiaDirezione()
        {
            // Console.WriteLine($" * * Vecchia direzione: {direzione} - angolo: {angolo} - velocità: {velocità} - passi: {passi}");
            int nuovaDirezione = ((direzione + 3) % 4) - 1;
            direzione = nuovaDirezione;
            angolo += (nuovaDirezione == 2 ? 0.05 : (nuovaDirezione == -1 ? -0.05 : 0)); // nuova direzione += 2:+0.05 -1:-0.05 else:0
            velocità = (nuovaDirezione == 0 ? 0 : 0.1); // velocità = 0:0 else:0.1
            passi = (nuovaDirezione == 0 ? 3 : (nuovaDirezione == 1 ? 10 : 5)); // passi = 0:3 1:10 else:5
            // Console.WriteLine($" * * Nuova direzione: {direzione} angolo: {angolo} velocità: {velocità} passi: {passi}");
        }
        private void ScegliCittà(int indice, ref JsonDataRecord jsonData)
        {
            indice = Math.Max(0, Math.Min(4, indice)); // riportiamo l'indice entro i ranghi per sicurezza
            string[] des = { "Pordenone", "Udine", "Trieste", "Portogruaro", "Padova" };
            double[] lat = { 45.9568900, 46.0693000, 45.6432500, 45.7750, 45.4079700 };
            double[] lon = { 12.6605100, 13.2371500, 13.7903000, 12.8378, 11.8858600 };
            double[] alt = { 24, 108, 64, 5, 18 };
            jsonData.SetDescription(des[indice]);
            jsonData.SetLatitude(lat[indice]);
            jsonData.SetLongitude(lon[indice]);
            jsonData.SetAltitude(alt[indice]);
        }
    }



    [DataContract] // serve per effettuare la serializzazione, ovvero la conversione automatica, in jason
    public class JsonDataRecord
    {
        // * * *     VARIABILI E RELATIVI GESTORI     * * *
        // * * * (per accedere meglio alle variabili) * * *
        // Codice identificativo del veicolo
        [DataMember] // anche questi servono per la conversione automatica, in jason
        private int idVehicle = 0;
        public int id => idVehicle; // solo per leggere
        public JsonDataRecord SetId(int id) { this.idVehicle = id; return this; } // per modificarne il valore concatenando le funzioni

        // Contatore dei record inviati, fornisce un indice a ciascun record, utile durante il debug
        private static int _recordCounter = 0;
        public static int newRecord => _recordCounter++;
        // [DataMember] // nel json non serve
        private int idRecord = 0;
        public JsonDataRecord UpdateIdRecord() { idRecord = newRecord; return this; } // per modificarne il valore concatenando le funzioni

        [DataMember] // nel json non serve
        private string description; // descrizione, tratta o descrizione del veicolo
        public JsonDataRecord SetDescription(string descrizione) { description = descrizione; return this; }
        public string getDescription => description;

        [DataMember]
        private DateTime timeDate; // istante del record
        public JsonDataRecord SetIstant(DateTime istante) { timeDate = istante; return this; }
        public JsonDataRecord TimeUpdate() { timeDate = DateTime.Now; return this; }

        [DataMember]
        private double latitude, longitude, altitude; // coordinate geografiche
        public JsonDataRecord SetLatitude(double lat) { latitude = lat; return this; }
        public JsonDataRecord AddLatitude(double lat) { latitude += lat; return this; }
        public JsonDataRecord SetLongitude(double lon) { longitude = lon; return this; }
        public JsonDataRecord AddLongitude(double lon) { longitude += lon; return this; }
        public JsonDataRecord SetAltitude(double alt) { altitude = alt; return this; }
        public JsonDataRecord AddAltitude(double alt) { altitude += alt; return this; }

        [DataMember]
        private int passenger; // numero di passeggeri
        public JsonDataRecord SetPassenger(int gente) { passenger = gente; return this; }
        public JsonDataRecord AddPassenger(int gente) { passenger = Math.Max(0, passenger + gente); return this; }

        [DataMember]
        private bool theDoors; // stato delle porte
        public JsonDataRecord SetDoors(bool porte) { theDoors = porte; return this; }
        public JsonDataRecord InvertDoors() { theDoors = !theDoors; return this; }


        // * * * COSTRUTTORI * * *
        public JsonDataRecord() // record vuoto con valori di predefiniti
        {
            idVehicle = 0;
            description = "Nome, tratta o descrizione del mezzo";
            timeDate = DateTime.Now;
            latitude = 45.095;
            longitude = 12.564;
            altitude = 30.001;
            passenger = 0;
            theDoors = false;
        }
        public JsonDataRecord(int idVeicolo, string descrizione, double lat, double lon, double alt, int gente, bool porte) // record riempito
        {
            idVehicle = idVeicolo;
            description = descrizione;
            timeDate = DateTime.Now;
            latitude = lat;
            longitude = lon;
            altitude = alt;
            passenger = gente;
            theDoors = porte;
        }

        // * * * UTILITA' * * *
        // Rappresentazione in stringa dell'istanza
        override public string ToString() => $"VEICOLO id:{id}[{idRecord:000}] “{description}” {latitude:N2} {longitude:N2} {altitude:N1}";
        // Stringa della rappresentazioe json dell'istanza
        public string ToJsonString() => JsonToString(ToJsonStream());
        // Stampa in console della rappresentazione in stringa dell'istanza
        public void Print(bool aCapo = true) => Console.Write(ToString() + (aCapo ? "\n" : ""));
        // Stampa in console della rappresentazione in stringa dell'istanza
        public void PrintJson(bool aCapo = true) => Console.Write(ToJsonString() + (aCapo ? "\n" : ""));

        // * * * GESTIONE JSON * * *
        // Trasformiamo un oggetto JsonDataRecord nello strean json 
        // Versione record singolo, legato all'istanza
        public MemoryStream ToJsonStream()
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(JsonDataRecord));
            jsonSerializer.WriteObject(stream, this);
            return stream;
        }
        // Trasformiamo una lista di oggetti JsonDataRecord nello strean json 
        // a partire dalla lista dei record, statico e quindi legato alla classe
        static public MemoryStream StackToStream(ICollection<JsonDataRecord> stack)
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ICollection<JsonDataRecord>));
            jsonSerializer.WriteObject(stream, stack);
            return stream;
        }
        // Trasformiamo una lista di oggetti JsonDataRecord in una stringa json 
        static public string StackToString(ICollection<JsonDataRecord> stack) => JsonToString(StackToStream(stack));

        // Trasformiamo lo strean json in una stringa che rappresenta l'oggetto json
        static public string JsonToString(MemoryStream stream)
        {
            stream.Position = 0;
            return new StreamReader(stream).ReadToEnd();
        }
        // Trasformiamo uno strean json in oggetto JsonDataRecord
        static public JsonDataRecord ReadJsonStream(MemoryStream stream)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(JsonDataRecord));
            stream.Position = 0;
            return (JsonDataRecord)jsonSerializer.ReadObject(stream);
        }
        // Per creare una copia al volo del record
        public JsonDataRecord Clone() => JsonDataRecord.ReadJsonStream(this.ToJsonStream());
    }

}
