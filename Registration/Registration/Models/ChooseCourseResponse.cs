using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class ChooseCourseResponse
    {
        public bool ok { get; set; }
        public string errmsg { get; set; }
        public List<Courses> CoursesList { get; set; }
        public ChooseCourseResponse()
        {
            this.ok = true;
            this.errmsg = "";
            this.CoursesList = new List<Courses>();
        }
    }
}