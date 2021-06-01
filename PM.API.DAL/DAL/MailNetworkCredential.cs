using System;
using System.Collections.Generic;
using System.Text;

namespace PM.API.DAL.DAL
{
    public class MailNetworkCredential : IMailNetworkCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
