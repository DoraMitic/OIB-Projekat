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
            try
            {
                /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
                string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

                this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
                this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();
                this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

                /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
                this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

                this.a = new MyAuthorizationManager(this.Credentials.ClientCertificate.Certificate);

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

        public bool OtvoriRacun(string clientGroup)
        {
            bool retValue = false;
            try
            {
                retValue = factory.OtvoriRacun(clientGroup);
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

        public bool ZatvoriRacun(string clientGroup, long broj)
        {
            bool retValue = false;
            try
            {
                retValue = factory.ZatvoriRacun(clientGroup, broj);
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

        public double ProveriStanje(string clientGroup, long broj)
        {
            double retValue = 0;
            try
            {
                retValue = factory.ProveriStanje(clientGroup, broj);
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

        public bool Uplata(string clientGroup, long broj, double iznos)
        {
            bool retValue = false;
            try
            {
                retValue = factory.Uplata(clientGroup, broj, iznos);
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

        public bool Isplata(string clientGroup, long broj, double iznos)
        {
            bool retValue = false;
            try
            {
                retValue = factory.Isplata(clientGroup, broj, iznos);
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

        public bool Opomena(string clientGroup, long broj)
        {
            bool retValue = false;
            try
            {
                retValue = factory.Opomena(clientGroup, broj);
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
