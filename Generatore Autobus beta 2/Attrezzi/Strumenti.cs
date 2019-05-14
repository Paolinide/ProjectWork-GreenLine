using System;
using static Generatore_Autobus_beta_2.Trasmissione;
using static Generatore_Autobus_beta_2.Archivio;

namespace Generatore_Autobus_beta_2
{
    class Veicolo
    {
        private int _contaInvii = 0;
        public void Aggiorna()
        {
            JsonDataRecord jsdata = new JsonDataRecord();
            Manda(jsdata.SetId(++_contaInvii));
        }
    }
    public class JsonDataRecord
    {
        public int id = 0;
        public JsonDataRecord SetId(int id)
        {
            this.id = id;
            return this;
        }
    }
}
