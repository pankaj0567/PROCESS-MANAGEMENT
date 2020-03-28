using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.MVC.Utility
{
    public class APIUrl : IAPIUrl
    {
        public string BaseAddress { get; set; }

        public APIUrl(string baseUri)
        {
            BaseAddress = baseUri;
        }
    }
}
