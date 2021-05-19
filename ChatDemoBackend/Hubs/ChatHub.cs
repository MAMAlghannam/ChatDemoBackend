using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatDemoBackend.Hubs
{
    public class ChatHub : Hub
    {
        // userId: { connectionId, username }
        public static Dictionary<string, object> list = new Dictionary<string, object>();
        public static int numberOfConnected = 0;

        string userId; // connected user - no need, because a new instance is generated with every invoke !
        string username; // connected user - same ^

        public override Task OnConnectedAsync()
        {
            numberOfConnected++;
            return base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            numberOfConnected--;
            Console.WriteLine("userId going to left: " + Context.Items["userId"]);
            list.Remove(Context.Items["userId"]+"");
            await Clients.Others.SendAsync("SomeoneLeft", Context.Items["userId"]); // send
        }

        public async Task Subscribe(string userId, string username)
        {
            try
            {
                list.Add(userId, new { connectionId = Context.ConnectionId, username = username });
            }
            catch (Exception)
            {
                Context.Abort();
                throw new HubException("You're already logged in !");
            }

            Context.Items.Add("userId", userId);
            Context.Items.Add("username", username);
            Console.WriteLine("userId just joined: " + userId);
            await Clients.Caller.SendAsync("ReceiveConnectedUsers", list); // send
            await Clients.Others.SendAsync("SomeoneJoined", new { userId, connectionId = Context.ConnectionId, username }); // send
        }

        public async Task SendMessage(string messageContent)
        {
            object message = new 
            {
                content = messageContent,
                userId = Context.Items["userId"],
                username = Context.Items["username"],
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            Console.WriteLine(message);
            await Clients.Others.SendAsync("ReceiveMessage", message); // send
        }

        public async Task IsTyping()
        {
            await Clients.Others.SendAsync("SomeoneIsTyping", Context.Items["username"]); // send
        }
    }
}
