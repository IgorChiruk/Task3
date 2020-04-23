using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Chat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime MessageDate { get; set; }
        public string FromUser { get; set; }
        public string MessageText { get; set; }
    }
}