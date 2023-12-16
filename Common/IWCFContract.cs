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
		bool OtvoriRacun();

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool ZatvoriRacun(long broj);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		double ProveriStanje(long broj);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool Uplata(long broj, double iznos);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool Isplata(long broj, double iznos);

		[OperationContract]
		[FaultContract(typeof(SecurityException))]
		bool Opomena(long broj);

	}
}
