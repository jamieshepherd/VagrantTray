using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VagrantTray
{
    public class Box
    {
        public int boxID { get; set; }
        public String boxName { get; set; }
        public String boxPath { get; set; }
        public Boolean boxStatus { get; set; }

        public Box()
        {
            boxStatus = false;
        }
        public Box(int id, String name, String path, Boolean status)
        {
            boxID = id;
            boxName = name;
            boxPath = path;
            boxStatus = status;
        }
    }
}
