using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PM.Model.Model
{
    public class UserProfile
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string CommunicationAddress { get; set; }

        [DisplayName("Active")]
        public bool Status { get; set; }
        public string UserName { get; set; }

    }
}
