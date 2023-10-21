using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Service1)))
            {
                string address = "net.tcp://localhost:4000/IService1";
                NetTcpBinding binding = new NetTcpBinding();

                host.AddServiceEndpoint(typeof(IService1), binding, address);

                host.Open();
                Console.WriteLine($"Servis je uspesno pokrenut na adresi : {address}");
                Console.ReadKey();
                host.Close();
            }
        }
    }
}
