using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Common
{
	[ServiceContract]
	public interface IWCFContract
	{
		[OperationContract]
		void TestCommunication();

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool OtvoriRacun(string korisnik);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool ZatvoriRacun(long broj);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		string ProveriStanje(long broj);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		string Uplata(long broj, double iznos);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		string Isplata(long broj, double iznos);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		string Opomena(long broj);

	}
}
