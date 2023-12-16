using Manager;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

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
            //Manager.ClientCertValidator.(srvCert);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"),
                                      new X509CertificateEndpointIdentity(srvCert));



            using (WCFClient proxy = new WCFClient(binding, address))
            {
                // Extract OU from the client's certificate
                //string clientOu = ExtractOuFromCertificate(proxy.Credentials.ClientCertificate.Certificate);
                //Console.WriteLine("OU from client's certificate: " + clientOu);

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
        private static string ExtractOuFromCertificate(X509Certificate2 certificate)
        {
            string[] subjectParts = certificate.Subject.Split(',');
            foreach (string part in subjectParts)
            {
                if (part.Trim().StartsWith("OU=", StringComparison.OrdinalIgnoreCase))
                {
                    return part.Substring(4).Trim();
                }
            }
            return null;
        }
    }
}
