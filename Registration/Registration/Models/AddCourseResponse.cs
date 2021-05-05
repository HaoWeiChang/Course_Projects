using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class AddCourseResponse
    {
        public bool ok { get; set; }
        public string errmsg { get; set; }
        public AddCourseResponse()
        {
            this.ok = true;
            this.errmsg = "";
        }
    }
}