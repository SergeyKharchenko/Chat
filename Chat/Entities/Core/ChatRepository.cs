using System.Linq;

namespace Entities.Core
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatContext context = new ChatContext();

        public IQueryable<Core.Chat> Chats
        {
            get { return context.Chats; }
        }


    }
}