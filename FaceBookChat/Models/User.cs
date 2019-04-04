using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FaceBookChat.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }


        [Index(IsUnique = true)]
        [StringLength(50)]
        public String Name { get; set; }

        //Navigation Property
        //[ForeignKey("Groups")]
        //public int GroupId { get; set; }
        //public virtual List<Group> Groups { get; set; }
        [ForeignKey("GroupUsers")]
        public string GroupUsersId { get; set; }
        public virtual List<GroupUsers> GroupUsers { get; set; }

    }
}