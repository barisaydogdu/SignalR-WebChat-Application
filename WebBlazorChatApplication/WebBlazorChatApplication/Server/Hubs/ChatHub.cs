using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Xml.Linq;
using WebBlazorChatApplication.Server.Models;

namespace WebBlazorChatApplication.Server.Hubs
{
   
    public class ChatHub:Hub
    {
        private static Dictionary<string, string> Users=new Dictionary<string, string>();
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();


        public async Task InitializeUserList()
        {
            var list = _connections.GetUsers();

            await Clients.All.SendAsync("ReceiveInitializeUserList", list);
        }



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
             Users.Add(Context.ConnectionId, username);
            string name = Context.User.Identity.Name; 
            var name2 = Context.GetHttpContext().User;
            _connections.Add(username, Context.ConnectionId);
            await AddToMessageToChat(string.Empty, $"{username} joined to party!", DateTime.Now);
            await Task.CompletedTask;
            //    await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await AddToMessageToChatInfo(string.Empty, $"{username} has left!");
        }

        public async Task AddToMessageToChat(string user,string message,DateTime time)
        {
            await Clients.All.SendAsync("GetThatMessageDude", user, message,time);
        } 

        public async Task AddToMessageToChatInfo(string user,string message)
        {
            await Clients.All.SendAsync("GetThatMessageDudeInfo", user, message);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(string groupName, string user, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessageToGroup", user, message);
        }



    }

      

    }

