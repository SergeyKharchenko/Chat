using System.Linq;
using Entities.Core.Abstract;
using Entities.Models;

namespace Entities.Core.Concrete
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatContext context = new ChatContext();

        public IQueryable<Chat> Chats
        {
            get { return null; }
            //get { return context.Chats; }
        }


    }
}