using System;

namespace Generatore_di_dati
{
    class Program
    {
        static void Main(string[] args)
        {
            Random sticazzi = new Random();

            Console.Clear();
            var m = new Mezzo();
            for (int i = 0; i < 10; i++){
                // Console.WriteLine(sticazzi.Next(100));
                Console.WriteLine(m.posizione);
            }
        }
    }
}
