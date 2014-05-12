using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VagrantTray
{
    [Serializable()]
    public class BoxList
    {
        public int id = 0;
        public List<Box> list = new List<Box>();

        // They can just do list.Add(box) but this AddBox also refreshes the menu
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
