using Common;
using Manager;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

namespace Client
{
    public class WCFClient : ChannelFactory<IWCFContract>, IWCFContract, IDisposable
    {
        IWCFContract factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            try
            {
                /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
                string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

                this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
                this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();
                this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

                /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
                this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

                
                factory = this.CreateChannel();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }


        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
                Audit.AuthenticationSuccess(WindowsIdentity.GetCurrent().Name);
            }
            catch (Exception e)
            {
                Audit.AuthenticationFailed(WindowsIdentity.GetCurrent().Name, e.Message);
                
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public bool OtvoriRacun(string korisnik)
        {
            bool retValue = false;
            try
            {
                retValue = factory.OtvoriRacun(korisnik);
                if (retValue)
                {
                    Console.WriteLine("Racun uspesno otvoren.");
                }
                else
                {
                    Console.WriteLine("Trazeni korisnik vec ima otvoren racun.");
                }
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Greska kod funkcije OtvoriRacun() : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska kod funkcije OtvoriRacun() : {0}", e.Message);
            }
            return retValue;
        }

        public bool ZatvoriRacun(long broj)
        {
            bool retValue = false;
            try
            {
                retValue = factory.ZatvoriRacun(broj);
                if (retValue)
                {
                    Console.WriteLine("Racun uspesno zatvoren.");
                }
                else
                {
                    Console.WriteLine("Racun sa trazenim brojem racuna ne postoji.");
                }
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Greska prilikom zatvaranja racuna : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska prilikom zatvaranja racuna : {0}", e.Message);
            }
            return retValue;
        }

        public string ProveriStanje(long broj)
        {
            string retValue = "";
            try
            {
                retValue = factory.ProveriStanje(broj);
                Console.WriteLine(retValue);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Greska kod funkcije ProveriStanje() : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska kod funkcije ProveriStanje() : {0}", e.Message);
            }
            return retValue;
        }

        public string Uplata(long broj, double iznos)
        {
            string retValue = "";
            try
            {
                retValue = factory.Uplata(broj, iznos);
                Console.WriteLine(retValue);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Greska kod funkcije Uplata() : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska kod funkcije Uplata() : {0}", e.Message);
            }
            return retValue;
        }

        public string Isplata(long broj, double iznos)
        {
            string retValue = "";
            try
            {
                retValue = factory.Isplata(broj, iznos);
                Console.WriteLine(retValue);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Greska kod funkcije Isplata() : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska kod funkcije Isplata() : {0}", e.Message);
            }
            return retValue;
        }

        public string Opomena(long broj)
        {
            string retValue;
            try
            {
                retValue = factory.Opomena(broj);
                Console.WriteLine(retValue);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Greska kod funkcije Opomena() : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska kod funkcije Opomena() : {0}", e.Message);
            }
            return "Greska";
        }
    }
}
