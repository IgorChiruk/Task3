using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Web_Chat.Models
{
    public class MessageContext : DbContext
    {
        public MessageContext()
            : base("DbConnection")
        { }

        public DbSet<Message> Messages { get; set; }
    }
    
    
}