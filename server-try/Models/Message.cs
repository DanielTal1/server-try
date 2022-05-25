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
            DateTime date1 = DateTime.UtcNow;
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
            DateTime date2 = TimeZoneInfo.ConvertTime(date1, tz);
            this.created = date2.ToString("o");
            this.sent = sent;
        }
        public int id { get; set; }
        public string content { get; set; }
        public string created { get; set; }
        public bool sent { get; set; }

    }
}
