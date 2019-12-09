using System;

namespace QuickSvnCommit
{
    static class Program
    {
        static void Main(string[] args)
        {
            var svnPath = args[0];
            //svnPath = @"D:\_assets\39.104.79.4\da_cai_shen_test";
            var cmd = new SvnCommitCommand(svnPath);
            cmd.onComplete += Cmd_onComplete;
            cmd.Excute();            
        }

        private static void Cmd_onComplete(SvnCommitCommand cmd, string error, SharpSvn.SvnCommitResult result)
        {
            if(error != null)
            {
                Console.WriteLine(error);
                Console.WriteLine("0");
            }
            else if(null == result)
            {
                Console.WriteLine("already latest");
                Console.WriteLine("1");
            }
            else
            {
                if (null == result.PostCommitError)
                {
                    Console.WriteLine(result.Revision);
                    Console.WriteLine("1");
                }
                else
                {
                    Console.WriteLine(result.PostCommitError);
                    Console.WriteLine("0");
                }
            }            
        }
    }
}
