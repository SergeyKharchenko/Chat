using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Chat.Infrastructure.Abstract;
using Entities.Models;
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
            kernel.Bind<IEntityRepository<Entities.Models.Chat>>().To<EntityRepository<Entities.Models.Chat>>();
            kernel.Bind<IEntityRepository<Record>>().To<EntityRepository<Record>>();
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