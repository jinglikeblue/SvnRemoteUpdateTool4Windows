using SharpSvn;
using System;
using System.Collections.ObjectModel;

namespace QuickSvnCommit
{

    public delegate void SvnCommitCompleteEvent(SvnCommitCommand cmd, string error, SvnCommitResult result);

    public class SvnCommitCommand
    {
        public string svnPath { get; private set; }

        public event SvnCommitCompleteEvent onComplete;

        public SvnCommitCommand(string svnPath)
        {
            this.svnPath = svnPath;
        }

        public void Excute()
        {
            SvnCommitResult result;

            try
            {
                using (SvnClient client = new SvnClient())
                {
                    var statusArgs = new SvnStatusArgs();
                    statusArgs.Depth = SvnDepth.Infinity;
                    statusArgs.RetrieveAllEntries = true;
                    Collection<SvnStatusEventArgs> statuses;
                    client.GetStatus(svnPath, statusArgs, out statuses);
                    foreach (SvnStatusEventArgs statusEventArgs in statuses)
                    {
                        if (statusEventArgs.LocalContentStatus != SvnStatus.Normal)
                        {
                            switch (statusEventArgs.LocalContentStatus)
                            {
                                case SvnStatus.None:
                                    break;
                                case SvnStatus.Missing:
                                    client.Delete(statusEventArgs.Path);
                                    break;
                                case SvnStatus.Zero:
                                    break;
                                case SvnStatus.Incomplete:
                                    break;
                                case SvnStatus.External:
                                    break;
                                case SvnStatus.Obstructed:
                                    break;
                                case SvnStatus.Ignored:
                                    break;
                                case SvnStatus.Conflicted:
                                    break;
                                case SvnStatus.Merged:
                                    break;
                                case SvnStatus.Modified:
                                    break;
                                case SvnStatus.Replaced:
                                    break;
                                case SvnStatus.Deleted:
                                    break;
                                case SvnStatus.Added:
                                    break;
                                case SvnStatus.NotVersioned:
                                    client.Add(statusEventArgs.Path);
                                    break;
                            }

                            Console.WriteLine("File:{0} Status:{1}", statusEventArgs.Path, statusEventArgs.LocalContentStatus);
                        }
                    }

                    SvnCommitArgs args = new SvnCommitArgs();
                    args.LogMessage = "auto commit";
                    client.Commit(svnPath, args, out result);
                }

                onComplete?.Invoke(this, null, result);
            }
            catch (Exception e)
            {
                onComplete?.Invoke(this, e.Message, null);
            }
        }
    }
}
