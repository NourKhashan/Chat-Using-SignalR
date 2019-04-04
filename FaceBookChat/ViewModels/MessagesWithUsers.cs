using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceBookChat.Models;

namespace FaceBookChat.ViewModels
{
    public class MessagesWithUsers
    {
        public List<User> Users { get; set; }
        public List<Group> Groups { get; set; }

        public MessageUser messageUser { get; set; }
        public GroupUsers groupUsers { get; set; }
    }
}