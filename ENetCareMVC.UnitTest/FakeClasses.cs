using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Principal;

namespace ENetCareMVC.UnitTest
{
    class FakeClasses
    {

        public class FakeHttpContext : HttpContextBase
        {
            private readonly FakePrincipal _principal;
            public FakeHttpContext(FakePrincipal principal)
            {
                _principal = principal;
            }
            public override IPrincipal User
            {
                get
                {
                    return _principal;
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }
        }
        public class FakePrincipal : IPrincipal
        {
            private readonly IIdentity _identity;
            private readonly string[] _roles;
            public FakePrincipal(IIdentity identity, string[] roles)
            {
                _identity = identity;
                _roles = roles;
            }
            public IIdentity Identity
            {
                get { return _identity; }
            }
            public bool IsInRole(string role)
            {
                if (_roles == null)
                    return false;
                return _roles.Contains(role);
            }
        }

        public class FakeIdentity : IIdentity
        {
            private readonly string _name;
            public FakeIdentity(string userName)
            {
                _name = userName;
            }
            public string AuthenticationType
            {
                get { throw new System.NotImplementedException(); }
            }
            public bool IsAuthenticated
            {
                get { return !String.IsNullOrEmpty(_name); }
            }
            public string Name
            {
                get { return _name; }
            }
        }

        public class FakeControllerContext : ControllerContext
        {
            public FakeControllerContext(ControllerBase controller, string username, string[] roles)
                : base(new FakeHttpContext(new FakePrincipal(new FakeIdentity(username), roles)), new RouteData(), controller)
            {
            }
        }
    }
}
