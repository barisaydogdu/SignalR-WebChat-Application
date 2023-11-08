using Microsoft.EntityFrameworkCore;
using WebBlazorChatApplication.Server.Models;

namespace WebBlazorChatApplication.Server.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        public DbSet<MessageUser> UserMessages { get; set; }
    }
}
