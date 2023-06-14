using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMMOSystem
{
    public class CustomLevels
    {

        //1: 500
        //2: 1000

        private void generateXPCustomFile()
        {
           // UseCustomXPTable
        }

        private string GenerateXPTableString(Dictionary<string, int> xpTable)
        {
            int num = 0;
            string text = "";
            foreach (KeyValuePair<string, int> item in xpTable)
            {
                text += ((num != 0) ? ", " : "");
                text = text + item.Key + ":" + item.Value;
                num++;
            }
            return text;
        }

    }
}
