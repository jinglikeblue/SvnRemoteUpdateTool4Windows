using SharpSvn;
using SVNTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {            
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            RefreshList();
            One.Log.onLog += Log_onLog;
        }

        List<string> _consoleLog = new List<string>();

        private void Log_onLog(string content)
        {
            _consoleLog.Add(content);
            while (_consoleLog.Count > 1000)
            {
                _consoleLog.RemoveAt(0);
            }
            textConsole.Lines = _consoleLog.ToArray();            
            textConsole.SelectionStart = textConsole.Text.Length;
            textConsole.SelectionLength = 0;
            textConsole.ScrollToCaret();
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void FolderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void OnPortKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar!= (char)Keys.Back)
            {                
                e.Handled = true;
            }
        }

        private void OnBtnStartClick(object sender, EventArgs e)
        {
            textPort.Enabled = false;
            textSecretCode.Enabled = false;

            int port = int.Parse(textPort.Text);
            Global.Ins.secretCode = textSecretCode.Text.Trim();
            new ServerStartCommand(port).Excute();
            btnStart.Enabled = false;
        }

        private void BtnSelectDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = "选择一个SVN目录";
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            var result = folderBrowserDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                textDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var key = textKey.Text.Trim();
            var dir = textDir.Text.Trim();

            if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(dir))
            {
                MessageBox.Show(this, "输入内容错误");
                return;
            }

            SVNModel.Ins.Set(key, dir);
            RefreshList();
        }

        private void OnListSelectIndexChanged(object sender, EventArgs e)
        {
            Console.Write(list.SelectedItem);
            if (null == list.SelectedItem)
            {
                textKey.Text = "";
                textDir.Text = "";
            }
            else
            {
                var obj = list.SelectedItem as UpdateItemVO;
                textKey.Text = obj.key;
                textDir.Text = obj.dir;
            }
        }

        void RefreshList()
        {
            
            var cfgList = SVNModel.Ins.cfg.list;
            var lastSelected = list.SelectedItem;
            list.BeginUpdate();
            list.Items.Clear();
            for (int i = 0; i < cfgList.Count; i++)
            {
                list.Items.Add(cfgList[i]);
            }
            list.SelectedItem = lastSelected;
            list.EndUpdate();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {            
            SVNModel.Ins.Del(list.SelectedItem as UpdateItemVO);
            RefreshList();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if(null == list.SelectedItem)
            {
                MessageBox.Show("选中要更新的内容");
                return;
            }

            var vo = (list.SelectedItem as UpdateItemVO);
            string svnPath = vo.dir;

            SvnUpdateResult result;
            var args = SVNModel.Ins.UpdateSVN(svnPath, out result);
            args.SvnError += Args_SvnError;
            args.Conflict += Args_Conflict;
            MessageBox.Show("更新完成");            
        }

        private void Args_Conflict(object sender, SvnConflictEventArgs e)
        {           
            MessageBox.Show("更新发生冲突");
        }

        private void Args_SvnError(object sender, SvnErrorEventArgs e)
        {
            MessageBox.Show("更新发生错误");
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            //注意判断关闭事件Reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;    //取消"关闭窗口"事件
                this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
                //notifyIcon.Visible = true;
                this.Hide();
                return;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)    //最小化到系统托盘
            {
                //notifyIcon.Visible = true;    //显示托盘图标
                this.Hide();    //隐藏窗口
            }
        }

        private void OnNotifyIconClick(object sender, EventArgs e)
        {
            //notifyIcon.Visible = false;
            //EventArgs继承自MouseEventArgs,所以可以强转
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                this.Show();
                WindowState = FormWindowState.Normal;
                this.Focus();
            }
        }

        private void OnItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == exitItem)
            {
                var result = MessageBox.Show("确认要关闭程序吗?", "", MessageBoxButtons.OKCancel);
                if(result == DialogResult.OK)
                {
                    Application.Exit();
                }
            }           
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }
    }
}
