using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;

namespace Manager
{
    public class MyAuthorizationManager : ServiceAuthorizationManager, IPrincipal
    {
        private readonly IIdentity identity;
        public MyAuthorizationManager(IIdentity clientCertificate)
        {
            this.identity = clientCertificate;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            string group = ExtractUsernameFromCertificate(Identity);

            // Check if the username is associated with the specified role in the roles configuration
            string[] roles;
            RolesConfig.GetPermissions(group, out roles);
            foreach (string permision in roles)
            {
                if (permision.Equals(role))
                    return true;
            }
            return false;

        }

        private string ExtractUsernameFromCertificate(IIdentity genericIdentity)
        {
            string[] subjectParts = genericIdentity.Name.Split(',');
            foreach (string part in subjectParts)
            {
                if (part.Trim().StartsWith("OU=", StringComparison.OrdinalIgnoreCase))
                {
                    if (part.Substring(4, 1) == "S")
                    {
                        return part.Substring(4, 9);
                    }
                    else if(part.Substring(4, 1) == "K")
                    {
                        return part.Substring(4, 8);
                    }
                }
            }

            return null;
        }
    }
}
