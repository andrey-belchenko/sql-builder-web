using System;
using System.IO;
//using System.Windows.Forms;


namespace sql.builder.DataApi
{
    public class VFileGetter
    {
        public VFileGetter(string fileName)
        {
            FileName = fileName;
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
        public string FileName = null;


    }
}
