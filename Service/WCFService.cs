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

        //[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        //public bool Delete(int key)
        //{
        //    if (Thread.CurrentPrincipal.IsInRole("Admin"))
        //        return Database.cars.Remove(key);
        //    else
        //    {
        //        string name = Thread.CurrentPrincipal.Identity.Name;
        //        DateTime time = DateTime.Now;
        //        string message = String.Format("Access is denied. User {0} tried to call Delete method (time: {1}). " +
        //            "For this method user needs to be member of group Admin.", name, time.TimeOfDay);
        //        throw new FaultException<SecurityException>(new SecurityException(message));
        //    }
        //}

        ////[PrincipalPermission(SecurityAction.Demand, Role = "Modifier")]
        //public bool Modify(int key, Car car)
        //{
        //    if (Thread.CurrentPrincipal.IsInRole("Modifier"))
        //    {
        //        if (Database.cars.ContainsKey(key))
        //        {
        //            Database.cars[key] = car;
        //            return true;
        //        }
        //        return false;
        //    }
        //    else
        //    {
        //        string name = Thread.CurrentPrincipal.Identity.Name;
        //        DateTime time = DateTime.Now;
        //        string message = String.Format("Access is denied. User {0} tried to call Modify method (time: {1}). " +
        //            "For this method user needs to be member of group Modifier.", name, time.TimeOfDay);
        //        throw new FaultException<SecurityException>(new SecurityException(message));
        //    }
        //}

        ////[PrincipalPermission(SecurityAction.Demand, Role = "Reader")]
        //public Car Read(int key)
        //{
        //    if (Database.cars.ContainsKey(key))
        //    {
        //        return Database.cars[key];
        //    }

        //    return null;

        // ovo nema potrebe proveravati ako se proverava u CheckAccessCore
        //if (Thread.CurrentPrincipal.IsInRole("Reader"))
        //{
        //    if (Database.cars.ContainsKey(key))
        //    {
        //        return Database.cars[key];
        //    }

        //    return null;
        //}
        //else
        //{
        //    string name = Thread.CurrentPrincipal.Identity.Name;
        //    DateTime time = DateTime.Now;
        //    string message = String.Format("Access is denied. User {0} tried to call Read method (time: {1}). " +
        //        "For this method user needs to be member of group Reader.", name, time.TimeOfDay);
        //    throw new FaultException<SecurityException>(new SecurityException(message));
        //}
        //}

        public bool OtvoriRacun()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Uplata(long broj, double iznos)
        {
            throw new NotImplementedException();
        }

        public bool Isplata(long broj, double iznos)
        {
            throw new NotImplementedException();
        }

        public bool Opomena(long broj)
        {
            throw new NotImplementedException();
        }
    }
}

