using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Racun
    {
        long broj;
        double iznos;
        double dozvoljeniMinus;
        double blokiran;
        DateTime poslednjaTransakcija;

        public Racun(long broj, double iznos, double dozvoljeniMinus, double blokiran, DateTime poslednjaTransakcija)
        {
            this.Broj = broj;
            this.Iznos = iznos;
            this.DozvoljeniMinus = dozvoljeniMinus;
            this.Blokiran = blokiran;
            this.PoslednjaTransakcija = poslednjaTransakcija;
        }

        [DataMember]
        public long Broj { get => broj; set => broj = value; }
        [DataMember]
        public double Iznos { get => iznos; set => iznos = value; }
        [DataMember]
        public double DozvoljeniMinus { get => dozvoljeniMinus; set => dozvoljeniMinus = value; }
        [DataMember]
        public double Blokiran { get => blokiran; set => blokiran = value; }
        [DataMember]
        public DateTime PoslednjaTransakcija { get => poslednjaTransakcija; set => poslednjaTransakcija = value; }

        public override string ToString()
        {
            return String.Format("Broj : {0}, iznos : {1}, DozvoljeniMinus : {2}, Blokiran : {3}, Poslednja transakcija : {4}", broj, iznos, dozvoljeniMinus, blokiran, poslednjaTransakcija);
        }
    }
}
