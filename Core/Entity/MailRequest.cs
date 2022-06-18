using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class MailRequest
    {
        public string Name { get; set; }
        public string userPhone { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
