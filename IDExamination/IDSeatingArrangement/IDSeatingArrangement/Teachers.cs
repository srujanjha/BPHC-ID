using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSeatingArrangement
{
    class Teachers
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public Teachers(string n,string e)
        {
            name = n;email = e;
        }

    }
}
