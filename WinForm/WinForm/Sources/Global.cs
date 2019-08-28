using One;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVNTool
{
    public class Global
    {
        public static Global Ins = new Global();
        private Global()
        {

        }

        public readonly WebSocketServer socket = new WebSocketServer();
    }
}
