using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class GetLoginResponse
    {
        public bool ok { get; set; }
        public string errmsg { get; set; }
        public string _id { get; set; }    
        public string token { get; set; }

        public GetLoginResponse()
        {
            this.ok = true;
            this.errmsg = "";
            this._id = "";
            this.token = "";
        }
    }
}