using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Message
    {
        public Message( string content, bool sent)
        {
            this.content = content;
            this.created = DateTime.Now.ToString("ISO 8601");
            this.sent = sent;
        }
        public int id { get; set; }
        public string content { get; set; }
        public string created { get; set; }
        public bool sent { get; set; }

    }
}
