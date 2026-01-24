using System.Collections.Generic;

namespace sql.builder.DataApi
{
    internal class VLookupAnalyzer 
    {
        public VLookupAnalyzer()
        {
          
        }
        //public Result Check(VSXElement element)
        //{
        //    if (CheckReturn(element))
        //    {
        //        return Result.Return;
        //    }
        //    if (CheckStop != null)
        //    {
        //        if (CheckStop(element))
        //        {
        //            return Result.Stop;
        //        }
        //    }
        //    return Result.None;
            
        //}
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

    internal delegate bool AnalyzerCheckDelegate(VSXElement element);

    internal delegate List<VSXElement>   AnalyzerReturnDelegate(VSXElement element);
}
