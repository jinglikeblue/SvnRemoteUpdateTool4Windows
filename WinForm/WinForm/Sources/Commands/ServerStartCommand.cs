using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SVNTool
{
    public class ServerStartCommand
    {
        int _port;

        public ServerStartCommand(int port)
        {
            _port = port;
        }

        public void Excute()
        {
            Global.Ins.socket.onClientEnter += Socket_onClientEnter;
            Global.Ins.socket.onClientExit += Socket_onClientExit;
            Global.Ins.socket.Start(_port, 4096);

            new Thread(LoopThread).Start();
        }

        void LoopThread()
        {
            while (true)
            {
                Global.Ins.socket.Refresh();
                Thread.Sleep(100);
            }            
        }

        private void Socket_onClientExit(One.IChannel obj)
        {
            
        }

        private void Socket_onClientEnter(One.IChannel obj)
        {
            obj.onReceiveData += Obj_onReceiveData;
        }

        private void Obj_onReceiveData(One.IChannel sender, byte[] data)
        {
            Console.WriteLine(Encoding.UTF8.GetString(data));
        }
    }
}
