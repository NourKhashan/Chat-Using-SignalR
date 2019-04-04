using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FaceBookChat.Models
{
    public class MessageUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public string Id { get; set; }
        public String Message { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public int SeenStatus { get; set; } = 0;

        [ForeignKey("UserSender")]
        public string SenderId { get; set; }
        [ForeignKey("UserReciver")]

        public string ReciverId { get; set; }


        public virtual User UserSender { get; set; }
        public virtual User UserReciver { get; set; }

    }
}