namespace FaceBookChat.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ChatModelContext : DbContext
    {
        // Your context has been configured to use a 'ChatModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'FaceBookChat.Models.ChatModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ChatModel' 
        // connection string in the application configuration file.
        public ChatModelContext()
            : base("name=ChatModelContext")
        {
        }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<GroupConnection> GroupConnection { get; set; }
        public DbSet<GroupUsers> GroupUsers { get; set; }
        public DbSet<MessageUser> MessageUsers { get; set; }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}