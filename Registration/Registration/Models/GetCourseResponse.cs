using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class GetCourseResponse
    {
        public bool ok { get; set; }
        public string errmsg { get; set; }
        public List<Courses> CoursesList { get; set; }
        public GetCourseResponse()
        {
            this.ok = true;
            this.errmsg = "";
            this.CoursesList = new List<Courses>();
        }

    }
    public class Courses
    {
        public string _id { get; set; } //課程編號
        public string Name { get; set; } // 課程名稱
        public string Description { get; set; } //課程內容        
        public string Professor { get; set; } //教授名稱
        public string Periods { get; set; } //課程節數
        /*public string Count { get; set; }//選課人數*/
        public string Uplimit { get; set; }//上限人數
    }
}