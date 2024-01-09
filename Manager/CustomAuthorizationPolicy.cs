using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class CustomAuthorizationPolicy: IAuthorizationPolicy
    {
        public CustomAuthorizationPolicy()
        {
            Id = Guid.NewGuid().ToString();
            Console.WriteLine(Id);
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }
        public string Id
        {
            get;
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            if (!evaluationContext.Properties.TryGetValue("Certificates", out object list))
            {
                return false;
            }

            IList<X509Certificate2> certificates = list as IList<X509Certificate2>;
            if (list == null || certificates.Count <= 0)
            {
                return false;
            }

            evaluationContext.Properties["Principal"] =
                new MyAuthorizationManager(certificates[0]);
            return true;
        }
    }
}
