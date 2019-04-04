using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FaceBookChat.Models
{
    public class GroupUsers
    {
        [Key]
        public int Id { set; get; }
        [Column(Order =1)]
        [ForeignKey("Group")]
        public string GroupId { get; set; }
        [Column(Order = 2)]

        [ForeignKey("User")]
        public string UserId { get; set; }

        public string Message { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}