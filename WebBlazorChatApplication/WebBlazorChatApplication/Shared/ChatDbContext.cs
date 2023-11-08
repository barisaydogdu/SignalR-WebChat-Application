using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WebBlazorChatApplication.Shared.SharedModels;

namespace WebBlazorChatApplication.Shared
{
    public class ChatDbContext:DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        public DbSet<MessageUser> UserMessages { get; set; }
    }
}
