using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Travel.Web.Filters
{
    public class TravelPrincipal : IPrincipal
    {
        public IIdentity Identity
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }

    public class TravelIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }
    }
}