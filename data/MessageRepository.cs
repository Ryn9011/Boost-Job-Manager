using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobTracker.API.data;
using JobTracker.API.Helpers;
using JobTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.API.Data
{
    public class MessageRepository
    {

        private readonly DataContext _context;
        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public IQueryable<Message> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient).AsQueryable();
            
            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                    break;
                default: 
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false && u.RecipientDeleted == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);
            

            return messages;
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
             var messages = await _context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .Where(m => (m.RecipientId == userId && m.RecipientDeleted == false && m.SenderId == recipientId)
                    || (m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false))
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();
            return messages;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

    }
}