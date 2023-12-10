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
		bool OtvoriRacun(string clientGroup);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool ZatvoriRacun(string clientGroup, long broj);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		double ProveriStanje(string clientGroup, long broj);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool Uplata(string clientGroup, long broj, double iznos);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool Isplata(string clientGroup, long broj, double iznos);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool Opomena(string clientGroup, long broj);

	}
}
