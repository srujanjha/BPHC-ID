using System;
using System.Collections.Generic;

namespace IDSeatingArrangement
{
    class Course
    {
        public bool Added = false;
        public string courseCode = "", courseNo = "";
        public List<string> IDs = new List<string>();
        public List<string> Rooms = new List<string>();
        public List<string> Invigilators = new List<string>();
        public string T1Date = "" , T1Time="" ;
        public string T2Date="" , T2Time="" ;
        public string CompreDate="" , CompreTime="" ;
        public Course(string code, string no)
        {
            courseCode = code; courseNo = no;
        }
        public override string ToString()
        {
            return courseCode + " " + courseNo;
        }
    }
}
