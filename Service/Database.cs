using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Service
{
    public class Database
    {
        internal static Dictionary<string, Racun> racuni = new Dictionary<string, Racun>();

        static Database()
        {
            Racun r1 = new Racun(00001, 1000, -500, 0, DateTime.Now);
            Racun r2 = new Racun(00002, 1000, -500, 0, DateTime.Now);
            Racun r3 = new Racun(00003, 1000, -500, 0, DateTime.Now);

            racuni.Add("Korisnik1", r1);
            racuni.Add("Sluzbenik1", r2);
            racuni.Add("Korisnik2", r3);
        }
    }
}
