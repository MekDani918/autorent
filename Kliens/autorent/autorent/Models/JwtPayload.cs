using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Models
{
    internal class JwtPayload
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public long Iat {  get; set; }
        public long Exp {  get; set; }
    }
}
