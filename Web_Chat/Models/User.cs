﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Chat.Models
{
    public class User
    {
        public User() { }

        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public string Name { get; set; }
    }
}