using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FaceBookChat.Models
{
    public class GroupConnection
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Group")]

        public string GroupId { get; set; }
        [Key]
        [Column(Order = 2)]

        public string ConnectionId { get; set; }


        public virtual Group Group { get; set; }
    }
}