using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace autorent.Models
{
    public class Account
    {
        public string Username { get; set; }
        public string Token { get; }
        public string Role { 
            get
            {
                if(Token == null)
                {
                    return "";                   
                }
                string encodedPayload = Token.Split('.')[1];
                int padding = 4 - encodedPayload.Length % 4;
                encodedPayload += new String('=', padding);
                byte[] PayLoadBytes = Convert.FromBase64String(encodedPayload);
                string PayLoadString= Encoding.UTF8.GetString(PayLoadBytes);
                JwtPayload PayLoad = JsonSerializer.Deserialize<JwtPayload>(PayLoadString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return PayLoad.Role;
            } 
        }          

        public Account(string username, string token)
        {
            Username = username;
            Token = token;
        }
    }
}
