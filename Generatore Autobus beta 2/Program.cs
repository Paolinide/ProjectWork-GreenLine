using System;

namespace Generatore_Autobus_beta_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine(" ***********  INIZIO  *********** ");
            var v1 = new Veicolo();
            for (int i = 0; i < 20; i++)
            {
                System.Threading.Thread.Sleep(1000); // aspetta 1 secondo
                v1.Aggiorna();
            }
            Console.WriteLine(" ***********   FINE   *********** ");
        }
    }
}
