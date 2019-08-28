using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVNTool
{
    public class UpdateItemVO
    {
        public string key;
        public string dir;

        public override string ToString()
        {
            return string.Format("Key: {0}    Dir: {1}", key, dir);
        }
    }

    public class SVNConfigVO
    {
        public List<UpdateItemVO> list = new List<UpdateItemVO>();
    }
}
