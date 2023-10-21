using Projekat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string address = "net.tcp://localhost:4000/IService1";
                NetTcpBinding binding = new NetTcpBinding();

                ChannelFactory<IService1> channel = new ChannelFactory<IService1>(binding, address);

                IService1 proxy = channel.CreateChannel();

                Console.WriteLine("Klijent uspesno pokrenut.");

                Console.WriteLine("Unesite tekst:");

                string unos = Console.ReadLine();
                Console.WriteLine(proxy.GetData(unos));

                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
