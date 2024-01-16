using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Common;
using System.ServiceModel.Security;
using Manager;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel.Description;
using System.IdentityModel.Policy;

namespace Service
{
	public class Program
	{
		static void Main(string[] args)
		{
			Console.ReadKey();
			/// srvCertCN.SubjectName should be set to the service's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

			NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

			string address = "net.tcp://localhost:9999/Receiver";
			ServiceHost host = new ServiceHost(typeof(WCFService));
			host.AddServiceEndpoint(typeof(IWCFContract), binding, address);



			///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
			host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
			host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();

			///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
			host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
			host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

			// podesavamo custom polisu, odnosno nas objekat principala
			host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
			List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
			policies.Add(new CustomAuthorizationPolicy());
			host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

			// podesavanje AutidBehaviour-a
			Audit audit = new Audit();

			ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
			newAudit.AuditLogLocation = AuditLogLocation.Application;
			newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

			host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
			host.Description.Behaviors.Add(newAudit);

			try
			{
				host.Open();
				Console.WriteLine("WCFService je pokrenut.\nPritisnite <enter> da zaustavite ...");
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine("[ERROR] {0}", e.Message);
				Console.WriteLine("[StackTrace] {0}", e.StackTrace);
			}
			finally
			{
				host.Close();
			}
		}
	}
}
