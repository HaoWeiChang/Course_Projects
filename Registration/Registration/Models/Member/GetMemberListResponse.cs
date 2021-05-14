using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class GetMemberListResponse
    {
        public bool ok { get; set; }
        public string errmsg { get; set; }
        public List<Members> MembersList { get; set; }
        public GetMemberListResponse()
        {
            this.ok = true;
            this.errmsg = "";
            this.MembersList = new List<Members>();
        }
    }
    public class Members
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Faculty { get; set; }
        public string Identity { get; set; }        

    }
}