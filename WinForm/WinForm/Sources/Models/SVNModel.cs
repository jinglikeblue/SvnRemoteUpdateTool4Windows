using SharpSvn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVNTool
{
    public class SVNModel
    {
        const string CONFIG_FILE = "configs/svn.json";

        public static SVNModel Ins = new SVNModel();
        private SVNModel()
        {
            Load();
        }

        public SVNConfigVO cfg = new SVNConfigVO();

        void Load()
        {
            if (File.Exists(CONFIG_FILE))
            {
                var str = File.ReadAllText(CONFIG_FILE, Encoding.UTF8);
                cfg = LitJson.JsonMapper.ToObject<SVNConfigVO>(str);
            }
            else
            {
                cfg = new SVNConfigVO();
            }
        }

        void Save()
        {
            var str = LitJson.JsonMapper.ToPrettyJson(cfg);
            var dir = Path.GetDirectoryName(CONFIG_FILE);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(CONFIG_FILE, str, Encoding.UTF8);
        }

        public void Set(string key, string dir)
        {
            for(int i = 0; i < cfg.list.Count; i++)
            {
                if(cfg.list[i].key == key)
                {
                    cfg.list[i].dir = dir;
                    return;
                }
            }

            var vo = new UpdateItemVO();
            vo.key = key;
            vo.dir = dir;
            cfg.list.Add(vo);

            Save();
        }

        public void Del(int idx)
        {
            cfg.list.RemoveAt(idx);

            Save();
        }        

        public void Del(string key)
        {
            for(int i = 0; i < cfg.list.Count; i++)
            {
                if(cfg.list[i].key == key)
                {
                    Del(i);
                    return;
                }
            }
        }

        public void Del(UpdateItemVO vo)
        {
            cfg.list.Remove(vo);
            Save();
        }

        public SvnUpdateArgs UpdateSVN(string svnPath, out SvnUpdateResult result)
        {
            SvnUpdateArgs args = new SvnUpdateArgs();

            using (SvnClient client = new SvnClient())
            {                
                client.Update(svnPath, args, out result);
            }
            return args;
        }
    }
}
