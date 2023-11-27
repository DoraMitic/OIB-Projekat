using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Manager;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
            string srvCertCN = "wcfservice";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"),
                                      new X509CertificateEndpointIdentity(srvCert));

            using (WCFClient proxy = new WCFClient(binding, address))
            {
                /// 1. Communication test
                proxy.TestCommunication();
                Console.WriteLine("TestCommunication() finished. Press <enter> to continue ...");
                Console.ReadLine();

                Console.WriteLine("Otvaranje racuna...");
                proxy.OtvoriRacun();

                Console.WriteLine("Zatvaranje racuna...");
                Console.WriteLine("Unesite broj racuna:");
                long broj = long.Parse(Console.ReadLine());
                proxy.ZatvoriRacun(broj);

                Console.WriteLine("Provera stanja racuna...");
                Console.WriteLine("Unesite broj racuna:");
                broj = long.Parse(Console.ReadLine());
                proxy.ProveriStanje(broj);

                Console.WriteLine("Uplata na racun...");
                Console.WriteLine("Unesite broj racuna:");
                broj = long.Parse(Console.ReadLine());
                Console.WriteLine("Unesite iznos uplate:");
                double uplata = double.Parse(Console.ReadLine());
                proxy.Uplata(broj, uplata);

                Console.WriteLine("Isplata sa racuna...");
                Console.WriteLine("Unesite broj racuna:");
                broj = long.Parse(Console.ReadLine());
                Console.WriteLine("Unesite iznos isplate:");
                double isplata = double.Parse(Console.ReadLine());
                proxy.Isplata(broj, isplata);

                Console.WriteLine("Provera opomene...");
                Console.WriteLine("Unesite broj racuna:");
                broj = long.Parse(Console.ReadLine());
                proxy.Opomena(broj);

            }
        }
    }
}
