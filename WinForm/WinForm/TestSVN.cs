using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using SharpSvn;
using System.Collections.ObjectModel;

namespace WinForm
{
    class TestSVN
    {
        public TestSVN()
        {
            using (SvnClient client = new SvnClient())
            {
                SvnUpdateResult result;
                string path = @"F:\yfy\adam\pc\Unity\Hall\Prefab";
                //client.Update(path, out result);
                //SvnUriTarget sut = new SvnUriTarget("http://39.98.171.8:81/svn/wallstreet/code/clients/trunk/Adam/Unity/Hall/Prefab");

                //client.CheckOut(sut, "./SVNTest",out result);
                //Console.WriteLine(result.ToString());

                //var scp = new SvnCommitArgs();
                //scp.LogMessage = "没用了";
                //scp.Depth = SvnDepth.Infinity;
                //client.Committing += Client_Committing;
                //client.Committed += Client_Committed;
                //scp.Committing += Scp_Committing;

                //var sdp = new SvnDeleteArgs();
                //if (client.Delete("./SVNTest"))
                //{
                //    Console.WriteLine("提交成功");
                //}
                //else
                //{
                //    Console.WriteLine("提交失败");
                //}  

                var statusArgs = new SvnStatusArgs();
                statusArgs.Depth = SvnDepth.Infinity;
                statusArgs.RetrieveAllEntries = true;
                Collection<SvnStatusEventArgs> statuses;
                client.GetStatus("./SVNTest", statusArgs, out statuses);
                foreach (SvnStatusEventArgs statusEventArgs in statuses)
                {
                    if (statusEventArgs.LocalContentStatus == SvnStatus.Modified)
                        Console.WriteLine("Modified file: " + statusEventArgs.Path);
                }
            }
        }

        private void Scp_Committing(object sender, SvnCommittingEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }

        private void Client_Committing(object sender, SvnCommittingEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }




    //class Demo
    //{
    //    public static void Main()
    //    {
    //        using (SvnClient client = new SvnClient())
    //        {
    //            // Checkout the code to the specified directory
    //            client.CheckOut(new Uri("http://sharpsvn.googlecode.com/svn/trunk/"),
    //                                    "c:\\sharpsvn");

    //            // Update the specified working copy path to the head revision
    //            client.Update("c:\\sharpsvn");
    //            SvnUpdateResult result;
    //            client.Update("c:\\sharpsvn", out result);


    //            client.Move("c:\\sharpsvn\\from.txt", "c:\\sharpsvn\\new.txt");

    //            // Commit the changes with the specified logmessage
    //            SvnCommitArgs ca = new SvnCommitArgs();
    //            ca.LogMessage = "Moved from.txt to new.txt";
    //            client.Commit("c:\\sharpsvn", ca);
    //        }
    //    }
    //}
}
