using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VagrantTray
{
    public class BoxList
    {
        public int id = 0;
        public List<Box> list = new List<Box>();
        public void AddBox(Box Box)
        {
            list.Add(Box);
            Program.mainWindow.RefreshMenu();
        }
        public IEnumerator<Box> GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
