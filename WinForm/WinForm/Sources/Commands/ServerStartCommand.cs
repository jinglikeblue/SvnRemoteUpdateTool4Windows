using One;
using SharpSvn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 100;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Global.Ins.socket.Refresh();
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
            try
            {
                var msg = Encoding.UTF8.GetString(data);
                Log.CI(ConsoleColor.DarkGreen, "收到协议：{0}", msg);
                var vo  = LitJson.JsonMapper.ToObject<RemoteRequestVO>(msg);
                if (vo.secretCode.Equals("!QAZ2wsx3edc"))
                {
                    var resp = new RemoteResponseVO();

                    lock (SVNModel.Ins)
                    {
                        var item = SVNModel.Ins.Get(vo.key);                        
                        if(item != null)
                        {
                            var svnPath = item.dir;
                            if (false == string.IsNullOrEmpty(vo.subDir))
                            {
                                svnPath += "\\" + vo.subDir;
                            }

                            try
                            {
                                SvnUpdateResult result;
                                Log.CI(ConsoleColor.DarkBlue, "开始更新SVN：{0}", svnPath);
                                SVNModel.Ins.UpdateSVN(svnPath, out result);
                                Log.CI(ConsoleColor.DarkBlue, "更新SVN完成", result.Revision);
                                resp.isSuccess = true;
                            }
                            catch(Exception e)
                            {
                                resp.isSuccess = false;
                                resp.msg = e.Message;
                            }
                        }
                        else
                        {
                            resp.isSuccess = false;
                            resp.msg = "不存在的key";
                        }
                    }                    
                    
                    var respStr = LitJson.JsonMapper.ToJson(resp);

                    sender.Send(Encoding.UTF8.GetBytes(respStr));
                }
            }
            catch (Exception e)
            {
                Log.I("处理协议出错:{0}", e.ToString());
                sender.Close();
            }            
        }
    }
}
