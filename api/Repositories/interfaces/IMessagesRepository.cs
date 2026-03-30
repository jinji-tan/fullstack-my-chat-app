using api.Models;

namespace api.Repositories.interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetConversationAsync(int currentUserId, int otherUserId);
        Task<Message> CreateMessageAsync(Message message);
    }
}