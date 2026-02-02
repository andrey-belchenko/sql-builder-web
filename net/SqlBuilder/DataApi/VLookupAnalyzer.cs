using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public class VLookupAnalyzer 
    {
        public VLookupAnalyzer()
        {
          
        }
        public AnalyzerCheckDelegate CheckStop;
        public AnalyzerCheckDelegate CheckReturn;
        public AnalyzerReturnDelegate Return;
        public object Tag = null;
        public bool NeedStop = false;
        public List<int> AnalyzedElements = new List<int>();
        public bool CheckAndReturn(List<VSXElement> list, VSXElement element)
        {
            if (AnalyzedElements.Contains(element.GetHashCode()))
            {
                return false;
            }
            AnalyzedElements.Add(element.GetHashCode());
            if (CheckReturn==null || CheckReturn(element))
            {
                if (Return == null)
                {
                    list.Add(element);
                }
                else
                {
                    List<VSXElement> ret = Return(element);
                    if (ret != null)
                    {
                        list.AddRange(ret);
                    }
                }
            }

            if (NeedStop)
            {
                return false;
            }

            if (CheckStop != null && CheckStop(element))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public enum Result
        //{
        //    None,
        //    Stop,
        //    Return
        //}
    }

    public delegate bool AnalyzerCheckDelegate(VSXElement element);

    public delegate List<VSXElement>   AnalyzerReturnDelegate(VSXElement element);
}
