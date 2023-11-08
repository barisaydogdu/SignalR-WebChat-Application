using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Xml.Linq;
using WebBlazorChatApplication.Client.Pages;
using WebBlazorChatApplication.Server.Data;
using WebBlazorChatApplication.Server.Models;

namespace WebBlazorChatApplication.Server.Hubs
{

    public class ChatHub : Hub
    {
        private readonly ChatDbContext dbContext;


        public ChatHub(ChatDbContext _dbContext)
        {
            dbContext = _dbContext;
        }



        private static Dictionary<string, string> Users = new Dictionary<string, string>();
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private static List<string> connectedUsers = new List<string>();
        private static Dictionary<string, List<string>> groupUsers = new Dictionary<string, List<string>>();
        //private static Dictionary<string, (string Username, DateTime LastActivity)> onlineUsers = new Dictionary<string, (string, DateTime)>();
        public static Dictionary<string, (string Username, DateTime LastActivity, DateTime? DisconnectedTime)> onlineUsers = new Dictionary<string, (string, DateTime, DateTime?)>();



        public async Task InitializeUserList()
        {
            var list = _connections.GetUsers();

            await Clients.All.SendAsync("ReceiveInitializeUserList", list);
        }

        public List<(string Username, DateTime LastActivity, DateTime? DisconnectedTime)> GetOnlineUsers()
        {
            return onlineUsers.Values.ToList();
        }
        //public void UpdateOnlineUsers()
        //{
        //    Clients.All.SendAsync("UpdateOnlineUsers", onlineUsers.Select(u => new OnlineUser
        //    {
        //        Username = u.Value.Username,
        //        LastActivity = u.Value.LastActivity,
        //        DisconnectedTime = u.Value.DisconnectedTime
        //    }).ToList());
        //}


        //public override async Task OnConnectedAsync()
        //{
        //    string username = Context.GetHttpContext().Request.Query["username"];   
        //    Users.Add(Context.ConnectionId, username);
        //    await AddToMessageToChat(string.Empty, $"{username} joined to party!",DateTime.Now);
        //    await base.OnConnectedAsync();
        //}
        public string username = "";
      

        public override async Task OnConnectedAsync()
        {

            username = Context.GetHttpContext().Request.Query["username"];
            var connectionId = Context.ConnectionId;
            Users.Add(Context.ConnectionId, username);
            var lastActivity = DateTime.UtcNow;
            if (!connectedUsers.Contains(connectionId))
            {
                connectedUsers.Add(connectionId);
                onlineUsers[connectionId] = (username, lastActivity, null);

            }
            _connections.Add(username, Context.ConnectionId);
            await SendConnectedUsers();
            await AddToMessageToChat(string.Empty, $"{username} joined to party!", DateTime.Now);
            await Clients.All.SendAsync("UserConnected", username);

            await Task.CompletedTask;
            //    await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            username = Context.GetHttpContext().Request.Query["username"];
            var connectionId = Context.ConnectionId;
            if (connectedUsers.Contains(connectionId))
            {
                await UpdateConnectedUsers(connectionId);
                connectedUsers.Remove(connectionId);

            }
            await AddToMessageToChat(string.Empty, $"{username} joined to party!", DateTime.Now);

            await Clients.All.SendAsync("UserDisconnected", username);
            await SendConnectedUsers();
            //  Context.Abort(); // Bağlantıyı sonlandır
        }

        public async Task SendConnectedUsers()
        {
            var usernames = new List<string>();

            foreach (var connectionId in connectedUsers)
            {
                var username = Context.GetHttpContext().Request.Query["username"];
                usernames.Add(username);
            }

            await Clients.All.SendAsync("UserConnected", usernames);
        }


        public async Task UpdateConnectedUsers(string connectionId)
        {

            onlineUsers[connectionId] = (onlineUsers[connectionId].Username, onlineUsers[connectionId].LastActivity, DateTime.UtcNow);
            await Clients.All.SendAsync("ReceiveUpdatedUsers", onlineUsers);

        }
        public async Task AddToMessageToChat(string user, string message, DateTime time)
        {
            await Clients.All.SendAsync("GetThatMessageDude", user, message, time);
        }

        public async Task AddToMessageToChatInfo(string user, string message)
        {
            await Clients.All.SendAsync("GetThatMessageDudeInfo", user, message);
        }

        public async Task JoinGroup(string groupName, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(string groupName, string user, string message)
        {
            var newMessage = new MessageUser
            {
                IdGuid = Guid.NewGuid(),
                User = user,
                Text = message,
                GroupName = groupName,
                Timestamp = DateTime.Now,
            };
            dbContext.UserMessages.Add(newMessage);
            await dbContext.SaveChangesAsync();
            await Clients.Group(groupName).SendAsync("ReceiveMessageToGroup", user, message);
        }




    }



}
