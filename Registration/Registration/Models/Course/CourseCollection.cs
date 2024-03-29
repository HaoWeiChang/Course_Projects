﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class CourseCollection
    {
        public string _id { get; set; } //課程編號
        public string Name { get; set; } // 課程名稱
        public string Description { get; set; } //課程內容        
        public string Professor { get; set; } //教授名稱
        public string ProfessorID { get; set; }//教授ID
        public string Periods { get; set; } //課程節數
        public int Count { get; set; }//選課人數
        public string Uplimit { get; set; }//上限人數
        public ChooseMember[] ChooseList { get; set; }
        

    }
    public class ChooseMember
    {
        public string student_ID { get; set; }
        public string student_Name { get; set; }

    }
}