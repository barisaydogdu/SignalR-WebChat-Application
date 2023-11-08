using System.ComponentModel.DataAnnotations;

namespace WebBlazorChatApplication.Server.Models
{
    public class UserMessage
    {
        //[Key]
        //public int Id { get; set; }
        [Key]
        public Guid IdGuid { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public string GroupName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
