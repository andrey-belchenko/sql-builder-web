using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
//using System.Windows.Forms;
using Devart.Data.Oracle;
using sql.builder.FieldInfo;
using System.Reflection;
using sql.builder.UI;

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

        public  string MakeMultidefinedPropertyValue(List<string> strings)
        {
            string s="";
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
