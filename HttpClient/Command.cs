using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient
{
    class Command
    {
        public const string HttpGet = "GET";
        public const string HttpPost = "POST";
        public const string HttpPut = "PUT";
        public const string HttpDelete = "DELETE";
        public Car Value { get; set; }

        public string CmdName { get; set; }


    }
}
