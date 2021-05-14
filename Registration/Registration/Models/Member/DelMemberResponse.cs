using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class DelMemberResponse
    {
        public bool ok { get; set; }
        public string errmsg { get; set; }
        public DelMemberResponse()
        {
            this.ok = true;
            this.errmsg = "";
        }
    }
}