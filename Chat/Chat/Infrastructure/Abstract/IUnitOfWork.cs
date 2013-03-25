using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Chat.Infrastructure.Concrete;

namespace Chat.Infrastructure.Abstract
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}