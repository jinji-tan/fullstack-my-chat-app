using api.Data;
using api.Models;
using api.Repositories.interfaces;
using Dapper;


namespace api.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MyChatAppDapperContext _dapper;

        public MessageRepository(MyChatAppDapperContext context)
        {
            _dapper = context;
        }

        public async Task<IEnumerable<Message>> GetConversationAsync(int currentUserId, int otherUserId)
        {
            var sql = @"
                SELECT Id, SenderId, ReceiverId, Content, Timestamp 
                FROM MyChatAppSchema.Messages 
                WHERE (SenderId = @CurrentUserId AND ReceiverId = @UserId) 
                   OR (SenderId = @UserId AND ReceiverId = @CurrentUserId)
                ORDER BY Timestamp ASC";

            return await _dapper.LoadData<Message>(sql, new { CurrentUserId = currentUserId, UserId = otherUserId });
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            var sql = @"
                INSERT INTO MyChatAppSchema.Messages (SenderId, ReceiverId, Content, Timestamp) 
                VALUES (@SenderId, @ReceiverId, @Content, @Timestamp);
                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var id = await _dapper.ExecuteSqlScalar(sql, message);
            message.Id = id;
            return message;
        }
    }
}