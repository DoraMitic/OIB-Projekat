using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Common;
using System.Security.Cryptography.X509Certificates;

namespace Service
{
    public class WCFService : IWCFContract
    {
        public void TestCommunication()
        {
            Console.WriteLine("Communication established.");
        }

        public bool OtvoriRacun()
        {
            if (Thread.CurrentPrincipal.IsInRole("Sluzbenik"))
            {
                //foreach (KeyValuePair<string, Racun> racun in Database.racuni)
                //{
                //    if (racun.Value.Broj == 0)
                //    {
                        string name = Thread.CurrentPrincipal.Identity.Name;
                        long randomFiveDigitNumber = Database.racuni.Last().Value.Broj + 1;

                        Racun noviRacun = new Racun(randomFiveDigitNumber, 0, -500, 0, DateTime.Now);

                        //Database.racuni[racun.Key] = noviRacun;
                        Database.racuni.Add(name, noviRacun);
                    //}
                //}
            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call OtvoriRacun method (time: {1}). " +
                    "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }

            return false;
        }

        public bool ZatvoriRacun(long broj)
        {
            if (Thread.CurrentPrincipal.IsInRole("Sluzbenik"))
                foreach(KeyValuePair<string, Racun> racun in Database.racuni)
                {
                    if(racun.Value.Broj == broj)
                    {
                        return Database.racuni.Remove(racun.Key);
                    }
                }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call ZatvoriRacun method (time: {1}). " +
                    "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
                
            return false;
        }

        public double ProveriStanje(long broj)
        {
            if (Thread.CurrentPrincipal.IsInRole("Sluzbenik") || Thread.CurrentPrincipal.IsInRole("Korisnik"))
            {
                foreach (KeyValuePair<string, Racun> racun in Database.racuni)
                {
                    if (racun.Value.Broj == broj)
                    {
                        //Console.WriteLine($"Stanje na racunu sa brojem {broj} je : {racun.Value.Iznos} ");
                        return racun.Value.Iznos;
                    }
                }
            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call ZatvoriRacun method (time: {1}). " +
                    "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
            return 0;
        }

        public bool Uplata(long broj, double iznosUplate)
        {
            if (Thread.CurrentPrincipal.IsInRole("Sluzbenik") || Thread.CurrentPrincipal.IsInRole("Korisnik"))
            {
                foreach (KeyValuePair<string, Racun> racun in Database.racuni)
                {
                    double novoStanje = racun.Value.Iznos + iznosUplate;
                    if (racun.Value.Broj == broj)
                    {
                        if (novoStanje >= 0 && racun.Value.Iznos < 0)
                        {
                            if (racun.Value.Blokiran != 0)
                            {
                                racun.Value.Blokiran = 0; // Odblokiraj račun
                                Console.WriteLine($"Račun (Broj: {racun.Value.Broj}) je odblokiran nakon uplate.");
                                return true;
                            }
                        }
                    }
                    racun.Value.Iznos = novoStanje;

                    Console.WriteLine($"Uplata na račun (Broj: {racun.Value.Broj}). Novo stanje: {racun.Value.Iznos}");
                    return true;
                }

            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call ZatvoriRacun method (time: {1}). " +
                    "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
            return false;
        }

        public bool Isplata(long broj, double iznosIsplate)
        {
            if (Thread.CurrentPrincipal.IsInRole("Sluzbenik") || Thread.CurrentPrincipal.IsInRole("Korisnik"))
            {
                foreach (KeyValuePair<string, Racun> racun in Database.racuni)
                {
                    if (racun.Value.Broj == broj)
                    {
                        if (racun.Value.Blokiran > 0)
                        {
                            Console.WriteLine($"Isplata nije moguća. Račun (Broj: {racun.Value.Broj}) je blokiran.");
                            return false;
                        }
                        else if (iznosIsplate > racun.Value.DozvoljeniMinus)
                        {
                            Console.WriteLine($"Isplata nije moguća. Iznos isplate ({iznosIsplate}) je veći od dozvoljenog minusa ({racun.Value.DozvoljeniMinus}).");
                            return false;
                        }
                        else
                        {
                            double novoStanje = racun.Value.Iznos - iznosIsplate;

                            racun.Value.Iznos = novoStanje;

                            Console.WriteLine($"Isplata sa računa (Broj: {racun.Value.Broj}). Novo stanje: {racun.Value.Iznos}");
                            return true;
                        }

                    }
                }
            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call ZatvoriRacun method (time: {1}). " +
                    "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
            return false;
        }

        public bool Opomena(long broj)
        {
            if (Thread.CurrentPrincipal.IsInRole("Sluzbenik"))
            {
                foreach (KeyValuePair<string, Racun> racun in Database.racuni)
                {
                    if (racun.Value.Broj == broj)
                    {
                        if (racun.Value.Iznos < 0 && racun.Value.Blokiran == 0)
                        {
                            // Blokiranje računa ako korisnik ima dugovanje (minus) i račun nije već blokiran
                            racun.Value.Blokiran = 1;
                            Console.WriteLine($"Račun (Broj: {racun.Value.Broj}) je blokiran zbog duga.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Opomena nije potrebna. Račun (Broj: {racun.Value.Broj}) nije u minusu ili je već blokiran.");
                            return false;
                        }
                    }
                }
            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call Opomena method (time: {1}). " +
                    "For this method user needs to be a member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
            return false;
        }
    }
}

