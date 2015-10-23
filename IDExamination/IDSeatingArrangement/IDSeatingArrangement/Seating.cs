using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSeatingArrangement
{
    class Seating
    {
        public string Id { get; set; }
        public string Course { get; set; }
        public string T1Date { get; set; }
        public string T2Date { get; set; }
        public string Time { get; set; }
        public string Room { get; set; }
        public string Students { get; set; }
        public Boolean Compre { get; set; }
        public Seating(string c1, string t1, string t2, string time, string room, string students,Boolean compre )
        {
            Course = c1;T1Date = t1;T2Date = t2;Time = time;Room = room;Students = students;Compre = compre;
        }
    }
}
