using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Manager;
using System.Security.Cryptography.X509Certificates;

namespace Service
{
	public class WCFService : IWCFContract
	{
		public void TestCommunication()
		{
			Console.WriteLine("Communication established.");
		}

	}
}
