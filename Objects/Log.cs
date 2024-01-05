using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.Objects
{
    public class Log
    {
        public int WoodID { get; set; }
        public int MonkeyID { get; set; }
        public string Message { get; set; }

        public Log(int woodID, int monkeyID, string message)
        {
            this.WoodID = woodID;
            this.MonkeyID = monkeyID;
            this.Message = message;
        }
    }
}
