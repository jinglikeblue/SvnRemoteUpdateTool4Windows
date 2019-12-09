using One;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVNTool
{
    public class Global
    {
        public static Global Ins = new Global();
        private Global()
        {

        }

        /// <summary>
        /// 通信密钥，如果该值不对，则不处理协议
        /// </summary>
        public string secretCode;

        public readonly WebSocketServer socket = new WebSocketServer();
        
    }
}
