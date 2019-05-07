using System;
//using System.Device.Location;
namespace Generatore_di_dati
{
    class Mezzo
    {
        static private double[] centro = new double[2] { 45.9562503, 12.6597197 };
        static Random caso = new Random();
        double variazione { get => (double)(caso.Next(200)-100)/100; }
        public string posizione { get => (centro[0] + variazione) + ", " + (centro[1] + variazione); }
    }
}
