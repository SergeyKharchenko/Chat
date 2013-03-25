using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
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
            kernel.Bind<IRoomUnitOfWork>().To<RoomUnitOfWork>();

            kernel.Bind<DbContext>().To<ChatContext>();
            kernel.Bind(typeof(IDbSet<>)).To(typeof(IDbSet<>));

            kernel.Bind<IAuthorizationService>().To<WebSecurityAuthorizationService>();

            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));            
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