using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

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
            MyAuthorizationManager principal = Thread.CurrentPrincipal as MyAuthorizationManager;
            if (principal.IsInRole("OtvoriRacun"))
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                long randomFiveDigitNumber = Database.racuni.Last().Value.Broj + 1;

                Racun noviRacun = new Racun(randomFiveDigitNumber, 0, -500, 0, DateTime.Now);

                Database.racuni.Add(name, noviRacun);

                try
                {
                    Audit.AuthorizationSuccess(name,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                try
                {
                    Audit.AuthorizationFailed(name,
                        OperationContext.Current.IncomingMessageHeaders.Action, "OtvoriRacun method need OtvoriRacun permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                string message = String.Format("Access is denied. User {0} tried to call OtvoriRacun method (time: {1}). " +
                    "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));

            }

            return false;
        }

        public bool ZatvoriRacun(long broj)
        {
            MyAuthorizationManager principal = Thread.CurrentPrincipal as MyAuthorizationManager;
            if (principal.IsInRole("ZatvoriRacun"))
            {
                foreach (KeyValuePair<string, Racun> racun in Database.racuni)
                {
                    if (racun.Value.Broj == broj)
                    {
                        Database.racuni.Remove(racun.Key);
                        break;
                    }
                }

                string name = Thread.CurrentPrincipal.Identity.Name;

                try
                {
                    Audit.AuthorizationSuccess(name,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return true;

            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                try
                {
                    Audit.AuthorizationFailed(name,
                        OperationContext.Current.IncomingMessageHeaders.Action, "ZatvoriRacun method need ZatvoriRacun permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    string message = String.Format("Access is denied. User {0} tried to call ZatvoriRacun method (time: {1}). " +
                        "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                    throw new FaultException<SecurityException>(new SecurityException(message));
                }
            }

        }

        public string ProveriStanje(long broj)
        {
            MyAuthorizationManager principal = Thread.CurrentPrincipal as MyAuthorizationManager;
            if (principal.IsInRole("ProveriStanje"))
            {
                string name = Thread.CurrentPrincipal.Identity.Name;

                try
                {
                    Audit.AuthorizationSuccess(name,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                foreach (KeyValuePair<string, Racun> racun in Database.racuni)
                {
                    if (racun.Value.Broj == broj)
                    {
                        return $"Stanje na racunu sa brojem {broj} je : {racun.Value.Iznos} ";
                    }
                }
                return "Greska";
            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                try
                {
                    Audit.AuthorizationFailed(name,
                        OperationContext.Current.IncomingMessageHeaders.Action, "ProveriStanje method need ProveriStanje permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                string message = String.Format("Access is denied. User {0} tried to call ProveriStanje method (time: {1}). " +
                    "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }

        public string Uplata(long broj, double iznosUplate)
        {
            MyAuthorizationManager principal = Thread.CurrentPrincipal as MyAuthorizationManager;
            if (principal.IsInRole("Uplata"))
            {
                string name = Thread.CurrentPrincipal.Identity.Name;

                try
                {
                    Audit.AuthorizationSuccess(name,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if(iznosUplate <= 0)
                {
                    try
                    {
                        Audit.TransactionFailed(name,
                            OperationContext.Current.IncomingMessageHeaders.Action, "Transakcija neuspesna jer je iznos uplate u minusu ili nula.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    return "Iznos uplate ne sme biti negativan ili 0.";
                }

                string retVal = "";
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
                                retVal += $"Račun (Broj: {racun.Value.Broj}) je odblokiran nakon uplate.";
                            }
                        }
                        racun.Value.Iznos = novoStanje;
                        retVal += $"Uplata na račun (Broj: {racun.Value.Broj}). Novo stanje: {racun.Value.Iznos}";

                        try
                        {
                            Audit.TransactionSuccess(name,
                                OperationContext.Current.IncomingMessageHeaders.Action);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        return retVal;
                    }


                }

            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                try
                {
                    Audit.AuthorizationFailed(name,
                        OperationContext.Current.IncomingMessageHeaders.Action, "Uplata method need Uplata permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                string message = String.Format("Access is denied. User {0} tried to call Uplata method (time: {1}). " +
                    "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
            return "Greska";
        }

        public string Isplata(long broj, double iznosIsplate)
        {
            MyAuthorizationManager principal = Thread.CurrentPrincipal as MyAuthorizationManager;
            if (principal.IsInRole("Isplata"))
            {

                string name = Thread.CurrentPrincipal.Identity.Name;

                try
                {
                    Audit.AuthorizationSuccess(name,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                foreach (KeyValuePair<string, Racun> racun in Database.racuni)
                {
                    if (racun.Value.Broj == broj)
                    {
                        if (racun.Value.Blokiran > 0)
                        {
                            try
                            {
                                Audit.TransactionFailed(name,
                                    OperationContext.Current.IncomingMessageHeaders.Action, "Transakcija neuspesna jer je racun blokiran.");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            return $"Isplata nije moguća. Račun (Broj: {racun.Value.Broj}) je blokiran.";
                        }
                        else if (racun.Value.Iznos - iznosIsplate < racun.Value.DozvoljeniMinus)
                        {
                            try
                            {
                                Audit.TransactionFailed(name,
                                    OperationContext.Current.IncomingMessageHeaders.Action, "Transakcija neuspesna jer je iznos isplate veci od dozvoljenog minusa.");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            return $"Isplata nije moguća. Iznos isplate ({iznosIsplate}) je veći od dozvoljenog minusa ({racun.Value.DozvoljeniMinus}).";
                        }
                        else
                        {
                            double novoStanje = racun.Value.Iznos - iznosIsplate;

                            racun.Value.Iznos = novoStanje;

                            try
                            {
                                Audit.TransactionSuccess(name,
                                    OperationContext.Current.IncomingMessageHeaders.Action);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                            return $"Isplata sa računa (Broj: {racun.Value.Broj}). Novo stanje: {racun.Value.Iznos}";
                        }

                    }
                }
            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;
                try
                {
                    Audit.AuthorizationFailed(name,
                        OperationContext.Current.IncomingMessageHeaders.Action, "Isplata method need Isplata permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                string message = String.Format("Access is denied. User {0} tried to call Isplata method (time: {1}). " +
                    "For this method user needs to be member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
            return "Greska";
        }

        public string Opomena(long broj)
        {
            MyAuthorizationManager principal = Thread.CurrentPrincipal as MyAuthorizationManager;
            if (principal.IsInRole("Opomena"))
            {

                string name = Thread.CurrentPrincipal.Identity.Name;

                try
                {
                    Audit.AuthorizationSuccess(name,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                foreach (KeyValuePair<string, Racun> racun in Database.racuni)
                {
                    if (racun.Value.Broj == broj)
                    {
                        if (racun.Value.Iznos < 0 && racun.Value.Blokiran == 0)
                        {
                            // Blokiranje računa ako korisnik ima dugovanje (minus) i račun nije već blokiran
                            racun.Value.Blokiran = 1;
                            return $"Račun (Broj: {racun.Value.Broj}) je blokiran zbog duga.";
                        }
                        else
                        {
                            return $"Opomena nije potrebna. Račun (Broj: {racun.Value.Broj}) nije u minusu ili je već blokiran.";
                        }
                    }
                }
            }
            else
            {
                string name = Thread.CurrentPrincipal.Identity.Name;
                DateTime time = DateTime.Now;

                try
                {
                    Audit.AuthorizationFailed(name,
                        OperationContext.Current.IncomingMessageHeaders.Action, "Opomena method need Opomena permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


                string message = String.Format("Access is denied. User {0} tried to call Opomena method (time: {1}). " +
                    "For this method user needs to be a member of group Sluzbenik.", name, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
            return "Greska";
        }
    }
}

