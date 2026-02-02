using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql.builder.Clean
{
    public class CleanUtils
    {
        public static string GetRootPath()
        {
           return  AppDomain.CurrentDomain.BaseDirectory;
        } 
    }
}
