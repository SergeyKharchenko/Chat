using System;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Chat.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer initializer;
        private static object initializerLock = new object();
        private static bool isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LazyInitializer.EnsureInitialized(ref initializer, ref isInitialized, ref initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                try
                {
                    WebSecurity.InitializeDatabaseConnection("AuthContext", "User", "UserId", "Login",
                                                             autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
