using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSeatingArrangement
{
    class Students
    {
        public string Id { get; set; }
        public string Campus_ID { get; set; }
        public string Course_ID { get; set; }
        public string Name { get; set; }
        public Students(string cid,string ccid,string name)
        {
            Course_ID = ccid;Campus_ID = cid;Name = name;
        }
    }
}
