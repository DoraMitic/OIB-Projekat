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
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"),
                                      new X509CertificateEndpointIdentity(srvCert));



            using (WCFClient proxy = new WCFClient(binding, address))
            {
                TestCommunication(proxy);

                while (true)
                {
                    Console.WriteLine("Izaberite opciju:");
                    Console.WriteLine("1. Otvori Racun");
                    Console.WriteLine("2. Zatvori Racun");
                    Console.WriteLine("3. Proveri Stanje Racuna");
                    Console.WriteLine("4. Uplata na Racun");
                    Console.WriteLine("5. Isplata sa Racuna");
                    Console.WriteLine("6. Provera Opomene");
                    Console.WriteLine("7. Izlaz");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            OtvoriRacun(proxy);
                            break;
                        case "2":
                            ZatvoriRacun(proxy);
                            break;
                        case "3":
                            ProveriStanje(proxy);
                            break;
                        case "4":
                            Uplata(proxy);
                            break;
                        case "5":
                            Isplata(proxy);
                            break;
                        case "6":
                            Opomena(proxy);
                            break;
                        case "7":
                            return; // Exit the program
                        default:
                            Console.WriteLine("Izbor nije validan. Izaberite ponovo.");
                            break;
                    }
                }
            }




        }
        static void TestCommunication(WCFClient proxy)
        {
            proxy.TestCommunication();
            Console.WriteLine("TestCommunication() zavrsen. Pritisnite <enter> da nastavite dalje ...");
            Console.ReadLine();
        }

        static void OtvoriRacun(WCFClient proxy)
        {
            Console.WriteLine("Otvaranje racuna...");
            Console.WriteLine("Unesite ime korisnika kojem otvarate racun:");
            string korisnik = Console.ReadLine();
            proxy.OtvoriRacun(korisnik);
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
    }
}
