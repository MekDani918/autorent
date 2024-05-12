using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace autorent.Models
{
    class WebsocketMessage
    {
        public string Action { get; set; }
        public string Message { get; set; }
        public JsonNode Data { get; set; }
    }
}
