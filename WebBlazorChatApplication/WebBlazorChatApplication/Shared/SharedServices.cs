using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WebBlazorChatApplication.Shared.SharedModels;

namespace WebBlazorChatApplication.Shared
{
    public class SharedServices
    {
        public class ChatService
        {
            private readonly ChatDbContext dbContext;

            public ChatService(ChatDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<List<MessageUser>> GetMessagesAsync()
            {
                return await dbContext.UserMessages.ToListAsync();
            }

            // Diğer veritabanı işlemleri metotlarını buraya ekleyebilirsiniz
        }
    }
}
