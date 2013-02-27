using System.Linq;

namespace Entities.Core
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