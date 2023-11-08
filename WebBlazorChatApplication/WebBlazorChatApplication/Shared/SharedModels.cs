using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlazorChatApplication.Shared
{
    public class SharedModels
    {
        public class MessageUser
        {
            public int Id { get; set; }
            public string User { get; set; }
            public string Text { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}
