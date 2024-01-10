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
            Console.ReadKey();
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

                while (true)
                {
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1. Test Communication");
                    Console.WriteLine("2. Otvori Racun");
                    Console.WriteLine("3. Zatvori Racun");
                    Console.WriteLine("4. Proveri Stanje Racuna");
                    Console.WriteLine("5. Uplata na Racun");
                    Console.WriteLine("6. Isplata sa Racuna");
                    Console.WriteLine("7. Provera Opomene");
                    Console.WriteLine("8. Exit");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            TestCommunication(proxy);
                            break;
                        case "2":
                            OtvoriRacun(proxy);
                            break;
                        case "3":
                            ZatvoriRacun(proxy);
                            break;
                        case "4":
                            ProveriStanje(proxy);
                            break;
                        case "5":
                            Uplata(proxy);
                            break;
                        case "6":
                            Isplata(proxy);
                            break;
                        case "7":
                            Opomena(proxy);
                            break;
                        case "8":
                            return; // Exit the program
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }




        }
        static void TestCommunication(WCFClient proxy)
        {
            proxy.TestCommunication();
            Console.WriteLine("TestCommunication() finished. Press <enter> to continue ...");
            Console.ReadLine();
        }

        static void OtvoriRacun(WCFClient proxy)
        {
            Console.WriteLine("Otvaranje racuna...");
            proxy.OtvoriRacun();
        }

        static void ZatvoriRacun(WCFClient proxy)
        {
            Console.WriteLine("Zatvaranje racuna...");
            Console.WriteLine("Unesite broj racuna:");
            long broj = long.Parse(Console.ReadLine());
            proxy.ZatvoriRacun(broj);
        }

        static void ProveriStanje(WCFClient proxy)
        {
            Console.WriteLine("Provera stanja racuna...");
            Console.WriteLine("Unesite broj racuna:");
            long broj = long.Parse(Console.ReadLine());
            proxy.ProveriStanje(broj);
        }

        static void Uplata(WCFClient proxy)
        {
            Console.WriteLine("Uplata na racun...");
            Console.WriteLine("Unesite broj racuna:");
            long broj = long.Parse(Console.ReadLine());
            Console.WriteLine("Unesite iznos uplate:");
            double uplata = double.Parse(Console.ReadLine());
            proxy.Uplata(broj, uplata);
        }
        static void Isplata(WCFClient proxy)
        {
            Console.WriteLine("Isplata sa racuna...");
            Console.WriteLine("Unesite broj racuna:");
            long broj = long.Parse(Console.ReadLine());
            Console.WriteLine("Unesite iznos isplate:");
            double isplata = double.Parse(Console.ReadLine());
            proxy.Isplata(broj, isplata);
        }

        static void Opomena(WCFClient proxy)
        {
            Console.WriteLine("Provera opomene...");
            Console.WriteLine("Unesite broj racuna:");
            long broj = long.Parse(Console.ReadLine());
            proxy.Opomena(broj);
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
