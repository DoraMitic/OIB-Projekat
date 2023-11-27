using Common;
using Manager;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;

namespace Client
{
    public class WCFClient : ChannelFactory<IWCFContract>, IWCFContract, IDisposable
    {
        IWCFContract factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
            }
            catch (Exception e)
            {
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

        public bool OtvoriRacun()
        {
            bool retValue = false;
            try
            {
                retValue = factory.OtvoriRacun();
                //Console.WriteLine("Delete allowed");
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
                Console.WriteLine("Delete allowed");
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Delete : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Delete : {0}", e.Message);
            }
            return retValue;
        }

        public double ProveriStanje(long broj)
        {
            double retValue = 0;
            try
            {
                retValue = factory.ProveriStanje(broj);
                //Console.WriteLine("Delete allowed");
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

        public bool Uplata(long broj, double iznos)
        {
            bool retValue = false;
            try
            {
                retValue = factory.Uplata(broj, iznos);
                //Console.WriteLine("Delete allowed");
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

        public bool Isplata(long broj, double iznos)
        {
            bool retValue = false;
            try
            {
                retValue = factory.Isplata(broj, iznos);
                //Console.WriteLine("Delete allowed");
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

        public bool Opomena(long broj)
        {
            bool retValue = false;
            try
            {
                retValue = factory.Opomena(broj);
                //Console.WriteLine("Delete allowed");
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Greska kod funkcije Opomena() : {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska kod funkcije Opomena() : {0}", e.Message);
            }
            return retValue;
        }
    }
}
