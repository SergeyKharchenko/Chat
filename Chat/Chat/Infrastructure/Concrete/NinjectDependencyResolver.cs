using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Entities.Core;
using Chat.Infrastructure.Abstract;
using Ninject;

namespace Chat.Infrastructure.Concrete
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindigs();
        }

        private void AddBindigs()
        {
            kernel.Bind<IAuthorizationService>().To<WebSecurityAuthorizationService>();
            kernel.Bind<IChatRepository>().To<ChatRepository>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}