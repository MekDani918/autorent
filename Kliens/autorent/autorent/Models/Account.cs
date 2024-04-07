using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Models
{
    public class Account
    {
        public string Username { get; set; }
        public string Token { get; }

        public Account(string username, string token)
        {
            Username = username;
            Token = token;
        }
    }
}
