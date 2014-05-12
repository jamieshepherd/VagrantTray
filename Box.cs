using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VagrantTray
{
    public class Box
    {
        public int boxID;
        public String boxName, boxPath;
        /*
        public Boolean boxStatus
        {
            get;
            set;
        }
         */

        public Boolean boxStatus = false;

        public Box(int id, String name, String path, Boolean status)
        {
            boxID = id;
            boxName = name;
            boxPath = path;
            boxStatus = status;
        }
    }
}
