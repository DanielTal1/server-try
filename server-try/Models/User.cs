using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class User
    {
        public User(string id, string userName, string password, string nickname )
        {
            this.Id = id;
            UserName = userName;
            Password = password;
            Image = null;
            Nickname = nickname;
            ContactsList= new List<Contact>();
        }
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        public string Nickname { get; set; }
        public List<Contact> ContactsList { get; set; }

    }
}
