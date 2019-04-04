using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using FaceBookChat.Controllers;
using FaceBookChat.CommonBL;
using FaceBookChat.Models;
using System.Data.Entity;
namespace FaceBookChat.Hubs
{
    public class MessageHub : Hub
    {
        ChatModelContext db = new ChatModelContext();
        String CurrentUser { get; set; }
        public String CurrentConnectionId { get; set; }
        User userFounded;
        public override Task OnConnected()
        {
            Clients.Others.broadCastConnected();

            CurrentUser = Users.GetUserName;
            CurrentConnectionId = Context.ConnectionId;
            userFounded = db.Users.SingleOrDefault(user => user.Name == CurrentUser);
            CheckMeIgGroup(userFounded.Id);
            db.UserConnections.Add(new UserConnection() { ConnectionId = CurrentConnectionId, UserId = userFounded.Id });

            db.SaveChanges();
            return base.OnConnected();
        }



        // Sned Message
        public void SendMessage(string from, string to, string message, bool isGroup)
        {
            string IdTo;
            dynamic  reciver = null;
            if (isGroup)
            {
                 IdTo = db.Groups.SingleOrDefault(group => group.Name == to).Id;
               //  reciver = db.GroupConnection.SingleOrDefault(m => m.GroupId == IdTo);

            }
            else
            {
                 IdTo = db.Users.SingleOrDefault(user => user.Name == to).Id;
                userFounded = db.Users.SingleOrDefault(user => user.Name == to);
                reciver = db.UserConnections.SingleOrDefault(m => m.UserId == userFounded.Id);

            }
            //BroadCast From

            Clients.Caller.broadCastMessageToSender(from,  message, DateTime.Now.ToShortDateString());
           var IdFrom = db.Users.SingleOrDefault(user => user.Name == from).Id;
           // //BroadCast To 
           // var IdTo = db.Users.SingleOrDefault(user => user.Name == to).Id;
           // userFounded = db.Users.SingleOrDefault(user => user.Name == to);
            //var reciver = db.UserConnections.SingleOrDefault(m => m.UserId == userFounded.Id);
            if(reciver != null)// Check if reciver is online or not
            {
                CurrentConnectionId = reciver.ConnectionId;
                Clients.Client(CurrentConnectionId).broadCastMessageToReciver(to, message, DateTime.Now.ToShortDateString());
            }
            if (isGroup)// Save To DB Group
            {
                Clients.OthersInGroup(to).broadCatSendedToGroup(from, to, message, DateTime.Now.ToShortDateString());
                //Save To DB
                db.GroupUsers.Add(new GroupUsers() {  GroupId=IdTo, UserId = IdFrom, Message=message});
                

            }
            else// Save To DB USers
            {
                IdTo = db.Users.SingleOrDefault(user => user.Name == to).Id;
                reciver = db.UserConnections.SingleOrDefault(m => m.UserId == userFounded.Id);
                int id = db.MessageUsers.Count();
                MessageUser messageUser = new MessageUser() { Id = (id + 1).ToString(), SenderId = IdFrom, ReciverId = IdTo, Message = message };
                db.MessageUsers.Add(messageUser);
            }
            // Save To DB
            
            db.SaveChanges();
        }// Send Message



        public void JoinGroup(string user, string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);// Add me to Group
            // Said to all I joined to Group
            Clients.OthersInGroup(groupName).broadCastToOtherInGroupWhoJoined(user, groupName);
            var groupId = db.Groups.SingleOrDefault(group=>group.Name == groupName).Id;
            var userId = db.Users.SingleOrDefault(use=>use.Name == user).Id;
            db.GroupUsers.Add(new GroupUsers() { GroupId = groupId, UserId= userId , Message="" });
            db.SaveChanges();
        }// Join Group

        public void CheckMeIgGroup(string userId)// Find Me in grouos and add me
        {
           List<GroupUsers> groups= db.GroupUsers.Where(user=>user.UserId == userId).GroupBy(group=>group.Group.Name).SelectMany(m=>m.ToList<GroupUsers>()).ToList<GroupUsers>();
            foreach (GroupUsers groupName in groups)
            {
                var x = groupName.Group.Name;
                Groups.Add(Context.ConnectionId, groupName.Group.Name);// Add me to Group

            }
        }
        // Sent to Group
        public void SendMessageToGroup(string groupName, string name, string message)
        {

            Clients.Group(groupName).SendedToGroup(name, groupName, message);
            var groupId = db.Groups.SingleOrDefault(group => group.Name == groupName).Id;
            var userId = db.Users.SingleOrDefault(use => use.Name == name).Id;
            db.GroupUsers.Add(new GroupUsers() { GroupId = groupId, UserId = userId , Message = message});
//            db.SaveChanges();

        }// Send To Group
        public void Hello()
        {
            Clients.All.hello();
        }


        //Disconeccted
        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.Others.broadCastDisconnected();
            CurrentConnectionId = Context.ConnectionId;
            userFounded = db.Users.SingleOrDefault(user => user.Name == CurrentUser);

            if (userFounded != null)
            {
                UserConnection userConnection = db.UserConnections.Where(user => user.UserId == userFounded.Id).SingleOrDefault(user=>user.ConnectionId == CurrentConnectionId);

                db.UserConnections.Remove(userConnection);// Remove User With connectionId
                db.SaveChanges();
            }


            return base.OnDisconnected(stopCalled);
        }
    }
}