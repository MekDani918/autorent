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
                byte[] PayLoadBytes = Convert.FromBase64String(base64urlToBase64(encodedPayload));
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

        private string base64urlToBase64(string base64UrlString)
        {
            string base64 = base64UrlString;
            base64.Replace('-', '+');
            base64.Replace('_', '/');
            if(base64.Length % 4 != 0 )
            {
                base64 += new String('=', 4 - base64.Length % 4);
            }

            return base64;
        }
    }
}
