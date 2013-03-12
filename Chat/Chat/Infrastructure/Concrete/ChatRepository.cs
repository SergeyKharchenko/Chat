using System.Data;
using System.Linq;
using Chat.Infrastructure.Abstract;
using Entities.Core.Concrete;
using Entities.Models;

namespace Chat.Infrastructure.Concrete
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatContext context = new ChatContext();

        public IQueryable<Entities.Models.Chat> Chats
        {
            get { return context.Chats; }
        }

        public Entities.Models.Chat GetChatById(int id)
        {
            return context.Chats.Find(id);
        }

        public void CreateChat(Entities.Models.Chat chat)
        {
            context.Chats.Add(chat);
            Save();
        }

        public void CreateRecord(Record record)
        {
            context.Records.Add(record);
            Save();
        }

        public void UpdateChat(Entities.Models.Chat chat)
        {
            context.Entry(chat).State = EntityState.Modified;
            Save();
        }

        public void UpdateRecord(Record record)
        {
            context.Entry(record).State = EntityState.Modified;
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public User GetUserById(int id)
        {
            return context.Users.Find(id);
        }
    }
}