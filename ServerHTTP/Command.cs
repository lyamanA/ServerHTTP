using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerHTTP
{
    public  class Command
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
        public const string EXIT = "EXIT";
        public string HttpMethod { get; set; }
        public Car Value { get; set; }
    }
}
