using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Contact
    {
        public Contact(string id,string UserId, string name, string server)
        {
            this.id = id;
            this.UserId = UserId;
            this.name = name;
            this.server = server;
            last = null;
            lastdate = null;
            this.ContactMessages = new List<Message>();
        }

        [Key, Column(Order = 0)]
        public string id { get; set; }
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
        public string? name { get; set; }
        public string server { get; set; }
        public string? last { get; set; }
        public string? lastdate { get; set; }
        public List<Message> ContactMessages { get; set; }
    }
}
