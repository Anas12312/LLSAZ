using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLS.Response
{ 
    class Result
    {
        public Object Data { get; set; }
        public bool Status { get; set; }
        public Object Error { get; set; }

    }
}
