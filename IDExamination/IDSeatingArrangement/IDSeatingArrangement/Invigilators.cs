using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSeatingArrangement
{
    class Invigilators
    {
        public string Id { get; set; }
        public string course { get; set; }
        public string course_name { get; set; }
        public string names { get; set; }
        public string t1date { get; set; }
        public string t2date { get; set; }
        public string time { get; set; }
        public Invigilators(string cid, string ccid, string t1,string t2,string ti,string name)
        {
            course = cid; course_name = ccid; t1date = t1;t2date = t2;time = ti; names = name;
        }
    }
}
