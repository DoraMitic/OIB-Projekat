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
            //string subjectName = clientCertificate.Subject;
            this.identity = clientCertificate;
            Console.WriteLine(clientCertificate.Name);
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        //private readonly IIdentity identity;

        //public MyAuthorizationManager(X509Certificate2 clientCertificate)
        //{
        //    this.identity = new GenericIdentity(clientCertificate.SubjectName.Name);
        //}

        //public IIdentity Identity => this.identity;

        // Override IsInRole method
        public bool IsInRole(string role)
        {
            //if (this.identity is GenericIdentity genericIdentity)
            //{
                // Extract username from the certificate subject
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
            //}

        }

        private string ExtractUsernameFromCertificate(IIdentity genericIdentity)
        {
            // Replace this logic with your actual way of extracting the username from the certificate
            // Example: Assuming the subject is in the format "CN=username,O=organization"
            string[] subjectParts = genericIdentity.Name.Split(',');
            foreach (string part in subjectParts)
            {
                if (part.Trim().StartsWith("OU=", StringComparison.OrdinalIgnoreCase))
                {
                    if (part.Substring(4, 1) == "S")
                    {
                        Console.WriteLine(part.Substring(4, 9));
                        return part.Substring(4, 9);
                    }
                    else if(part.Substring(4, 1) == "K")
                    {
                        Console.WriteLine(part.Substring(4, 8));
                        return part.Substring(4, 8);
                    }
                }
            }

            return null;
        }


        // Other methods and properties of MyAuthorizationManager...
    }
    //public class MyAuthorizationManager : ServiceAuthorizationManager
    //{
    //    public static bool IsInRole(string clientGroup, string permission)
    //    {
    //        //foreach (IdentityReference group in this.identity.Groups)
    //        //{
    //            //SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
    //            //var name = sid.Translate(typeof(NTAccount));
    //            //string groupName = Formatter.ParseName(name.ToString());
    //            string[] permissions;
    //            if (RolesConfig.GetPermissions(clientGroup, out permissions))
    //            {
    //                foreach (string permision in permissions)
    //                {
    //                    if (permision.Equals(permission))
    //                        return true;
    //                }
    //            }
    //        //}
    //        return false;
    //    }
    //}
}
