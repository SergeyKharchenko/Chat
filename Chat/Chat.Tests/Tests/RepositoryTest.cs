using Chat.Infrastructure.Concrete;
using Chat.Tests.Dummy;
using Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Chat.Tests.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void AddTest()
        {
            var dbSet = new DummyDbSet<User>();
            var repository = new Repository<User>(dbSet);

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
            var repository = new Repository<User>(dbSet);

            repository.Remove(john);

            Assert.AreEqual(2, repository.Entities.Count());
            Assert.AreEqual("Tom", repository.Entities.First().Login);
        }

        [TestMethod]
        public void FindByIdTest()
        {
            var dbSet = new DummyDbSet<User>
                {
                    new User {Id = 1, Login = "John"},
                    new User {Id = 2, Login = "Tom"},
                    new User {Id = 3, Login = "Eric"}
                };
            var repository = new Repository<User>(dbSet);

            var user = repository.FindById(1);

            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("John", user.Login);
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
            var repository = new Repository<User>(dbSet);

            var user = repository.FindBy(u => u.Login.Contains("o"));

            Assert.AreEqual(2, user.Count());
            Assert.AreEqual("John", user.First().Login);
            Assert.AreEqual("Tom", user.Last().Login);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var dbSet = new DummyDbSet<User>
                {
                    new User {Id = 1, Login = "John"},
                    new User {Id = 2, Login = "Tom"},
                    new User {Id = 3, Login = "Eric"}
                };
            var repository = new Repository<User>(dbSet);

            var user = repository.FindById(1);
            user.Login = "Frank";
            var user2 = repository.FindById(1);

            Assert.AreEqual("Frank", user2.Login);
        }
    }
}