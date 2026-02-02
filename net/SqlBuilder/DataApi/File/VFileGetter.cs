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


namespace sql.builder.DataApi
{
    public class VFileGetter
    {
        public VFileGetter(string fileName)
        {
          FileName=fileName;
        }

        public object OldId = null;
        public byte[] GetFile()
        {
            byte[] buff = null;
            FileStream fs = new FileStream(FileName,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(FileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }

        public String Name()
        {
            return this.FileName.SubstringAfter('\\').SubstringAfter('/');
        }
        public string FileName=null;

      
    }
}
