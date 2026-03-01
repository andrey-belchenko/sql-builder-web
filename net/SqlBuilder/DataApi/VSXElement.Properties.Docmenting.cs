using System.Collections.Generic;
//using System.Windows.Forms;

namespace sql.builder.DataApi
{
    public partial class VSXElement : VXElement
    {

        public bool IsStringAddision(string s)
        {
            if (s.Contains(TextConst.AVTitle.Add))
            {
                return true;
            }
            return false;
        }

        public string MakeMultidefinedPropertyValue(List<string> strings)
        {
            string s = "";
            foreach (string s1 in strings)
            {
                if (s != "")
                {
                    if (!s.Contains(TextConst.AVTitle.Add))
                    {
                        break;
                    }
                }
                if (s1 != "")
                {
                    if (s.Contains(TextConst.AVTitle.Add))
                    {
                        s = s.Replace(TextConst.AVTitle.Add, s1);
                        // s+=s1.Substring(1,s1.Length-1);
                    }
                    else
                    {
                        s = s1;
                    }
                }
            }
            return s;
        }
    }








}
