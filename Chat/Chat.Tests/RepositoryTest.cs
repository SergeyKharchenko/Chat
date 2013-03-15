using Chat.Infrastructure.Abstract;
using Chat.Infrastructure.Concrete;
using Chat.Tests.Dummy;
using Entities.Core.Concrete;
using Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Chat.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void AddTest()
        {
            var dbSet = new DummyDbSet<User>();
            var repository = new Repository<User>(new ChatContext(), dbSet);

            repository.Add(new User { Login = "John" });

            Assert.AreEqual(1, repository.Entities.Count());
            Assert.AreEqual("John", repository.Entities.First().Login);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var john = new User {Login = "John"};
            var dbSet = new DummyDbSet<User>
                {
                    john,
                    new User { Login = "Tom" },
                    new User { Login = "Eric" }
                };
            var repository = new Repository<User>(new ChatContext(), dbSet);

            repository.Remove(john);

            Assert.AreEqual(2, repository.Entities.Count());
            Assert.AreEqual("Tom", repository.Entities.First().Login);
        }

        [TestMethod]
        public void FindByTest()
        {
            var dbSet = new DummyDbSet<User>
                {
                    new User {Login = "John"},
                    new User { Login = "Tom" },
                    new User { Login = "Eric" }
                };
            var repository = new Repository<User>(new ChatContext(), dbSet);

            var user = repository.FindBy(u => u.Login.Contains("o"));

            Assert.AreEqual(2, user.Count());
            Assert.AreEqual("John", user.First().Login);
            Assert.AreEqual("Tom", user.Last().Login);
        }
    }
}