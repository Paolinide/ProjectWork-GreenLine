using System;
using System.Collections;
using System.Collections.Generic;

namespace ProgettoAutobus
{
    class Veicolo // istanza generica di un mezzo, che produce i dati da inviare
    {
        static private int numeroDiMezzi = 0; // questo si ricorda del numero di istanze generate
        static private int seriale { get => numeroDiMezzi++; } // quando viene letto il valore, dopo la restituzione viene incrementato
        private JsonDataRecord _jsonData = new JsonDataRecord();// contenitore dei dati;
        private int direzione = 0; //  // direzione va da -1 a 2 (-1:sinistra 0:fermo 1:dritto 2:destra)
        private double angolo = 0.0; // angolo effettivo di spostamento
        private double velocità = 0.1; // velocità di spostamento
        private int passi = 10; // passi al termine della direzione attuale
        public Veicolo()
        {
            SetCittà(numeroDiMezzi, ref _jsonData);
            _jsonData.SetId(seriale);
            Console.WriteLine(_jsonData);
        }
        public void Aggiorna()
        {
            if (passi-- == 0) CambiaDirezione();// finiti i passi si cambia direzione (N.B. l'operatore di post-incremento garantisce la variazione)
            if (direzione == 0) // quando ci siamo fermati
            {
                if (passi == 2) // appena fermati
                {
                    // si aprono le porte
                    _jsonData.SetPorte(true);
                    // qualcuno potrebbe essere sceso
                    _jsonData.AddPasseggeri(-(new Random()).Next(5));
                }
                else if (passi == 0) // stiamo per ripartire
                {
                    // qualcuno potrebbe essere salito
                    _jsonData.AddPasseggeri((new Random()).Next(5));
                    // si chiudono le porte
                    _jsonData.SetPorte(false);
                }
            }
            else // altrimenti siamo in movimmento
            {
                if (direzione != 1) // stiamo facendo una curva
                {
                    angolo += (direzione == 0) ? -0.05 : 0.05; // quindi variamo l'angolo di spostamento
                }
                // comunque stiamo andando avanti e modifichiamo le coordinate
                _jsonData.AddLatitudine(velocità * Math.Sin(angolo));
                _jsonData.AddLongitudine(velocità * Math.Cos(angolo));
            }
            //Console.WriteLine($"Attuale direzione: {direzione} angolo: {angolo} velocità: {velocità} passi: {passi}");
            Trasmettitore.InoltraRecord(_jsonData);
        }
        private void CambiaDirezione()
        {
            int nuovaDirezione = ((direzione + 3) % 4) - 1;
            direzione = nuovaDirezione;
            angolo += (nuovaDirezione == 2 ? 0.05 : (nuovaDirezione == -1 ? -0.05 : 0)); // nuova direzione += 2:+0.05 -1:-0.05 else:0
            velocità = (nuovaDirezione == 0 ? 0 : 0.1); // velocità = 0:0 else:0.1
            passi = (nuovaDirezione == 0 ? 3 : (nuovaDirezione == 1 ? 10 : 5)); // passi = 0:3 1:10 else:5
            Console.WriteLine($"Nuova direzione: {direzione} angolo: {angolo} velocità: {velocità} passi: {passi}");
        }
        private void SetCittà(int indice, ref JsonDataRecord jsonData)
        {
            indice = Math.Max(0, Math.Min(4, indice)); // riportiamo l'indice entro i ranghi per sicurezza
            string[] des = { "Pordenone", "Udine", "Trieste", "Portogruaro", "Padova" };
            double[] lat = { 45.9568900, 46.0693000, 45.6432500, 45.7750, 45.4079700 };
            double[] lon = { 12.6605100, 13.2371500, 13.7903000, 12.8378, 11.8858600 };
            double[] alt = { 24, 108, 64, 5, 18 };
            jsonData.SetDescrizione(des[indice]);
            jsonData.SetLatitudine(lat[indice]);
            jsonData.SetLongitudine(lon[indice]);
            jsonData.SetAltitudine(alt[indice]);
        }
    }


    class JsonDataRecord
    {
        // VARIABILI
        private int _idVeicolo; // id del veicolo
        private string _descrizione; // descrizione, tratta o descrizione del veicolo
        private string _timeStamp; // istante del record
        private double _latitudine, _longitudine, _altitudine; // coordinate geografiche
        private int _passeggeri; // numero di passeggeri
        private bool _porteAperte; // stato delle porte

        public double latitudine => _latitudine;
        public double longitudine => _longitudine;

        // COSTRUTTORI
        public JsonDataRecord() // record vuoto con valori di default
        {
            _idVeicolo = 0;
            _descrizione = "Nome, tratta o descrizione del mezzo";
            _timeStamp = DateTime.Now.ToString();
            _latitudine = 45.095;
            _longitudine = 12.564;
            _altitudine = 30.001;
            _passeggeri = 0;
            _porteAperte = false;
        }
        public JsonDataRecord(int id, string descrizione, string istante, double lat, double lon, double alt, int gente, bool porte) // record riempito
        {
            _idVeicolo = id;
            _descrizione = descrizione;
            _timeStamp = istante;
            _latitudine = lat;
            _longitudine = lon;
            _altitudine = alt;
            _passeggeri = gente;
            _porteAperte = porte;
        }
        // MODIFICATORI (così si possono concatenare come cazzo vogliamo)
        public JsonDataRecord SetId(int id) { _idVeicolo = id; return this; }
        public JsonDataRecord SetDescrizione(string descrizione) { _descrizione = descrizione; return this; }
        public JsonDataRecord SetIstante(string istante) { _timeStamp = istante; return this; }
        public JsonDataRecord AggiornaIstante() { _timeStamp = DateTime.Now.ToString(); return this; }
        public JsonDataRecord SetLatitudine(double lat) { _latitudine = lat; return this; }
        public JsonDataRecord AddLatitudine(double lat) { _latitudine += lat; return this; }
        public JsonDataRecord SetLongitudine(double lon) { _longitudine = lon; return this; }
        public JsonDataRecord AddLongitudine(double lon) { _longitudine += lon; return this; }
        public JsonDataRecord SetAltitudine(double alt) { _altitudine = alt; return this; }
        public JsonDataRecord AddAltitudine(double alt) { _altitudine += alt; return this; }
        public JsonDataRecord SetPasseggeri(int gente) { _passeggeri = gente; return this; }
        public JsonDataRecord AddPasseggeri(int gente) { _passeggeri = Math.Max(0, _passeggeri + gente); return this; }
        public JsonDataRecord SetPorte(bool porte) { _porteAperte = porte; return this; }
        public JsonDataRecord InvertPorte() { _porteAperte = !_porteAperte; return this; }
        // UTILITA'
        public override string ToString()
        {
            // qui i dati del singolo record vengono convertiti in stringa
            return "{ \"id\": " + _idVeicolo +
                  ", \"descrizione\": \"" + _descrizione +
                  "\", \"timeStamp\": \"" + _timeStamp +
                  "\", \"latitudine\": " + _latitudine.ToString().Replace(',', '.') +
                  ", \"longitudine\": " + _longitudine.ToString().Replace(',', '.') +
                  ", \"altitudine\": " + _altitudine.ToString().Replace(',', '.') +
                  ", \"passeggeri\": " + _passeggeri +
                  ", \"porteAperte\": " + _porteAperte +
                  " }";
        }
        public static string StackToString(Stack pila)
        {
            // una pila intera viene convertita in stringa
            string risultato = "{ \"VettoreDati\": [\n";
            for (int i = 0; pila.Count > 0; i++)
                risultato += (i > 0 ? ",\n" : "") + pila.Pop().ToString();
            return risultato += "\n]}";
        }
        public Dictionary<string, string> ToDictionary()
        {
            // qui i dati del singolo record vengono convertiti in coppie di stringa <nome:dato>
            return new Dictionary<string, string> {
                        { "id", _idVeicolo.ToString() },
                        { "descrizione", _descrizione },
                        { "timeStamp", _timeStamp },
                        { "latitudine", _latitudine.ToString().Replace(',', '.') },
                        { "longitudine", _longitudine.ToString().Replace(',', '.') },
                        { "altitudine", _altitudine.ToString().Replace(',', '.') },
                        { "passeggeri", _passeggeri.ToString() },
                        { "porteAperte", _porteAperte.ToString() }
            };
        }
    }
}
