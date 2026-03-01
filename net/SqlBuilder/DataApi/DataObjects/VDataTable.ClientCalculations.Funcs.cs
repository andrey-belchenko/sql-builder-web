using System;
using System.Collections.Generic;
namespace sql.builder.DataApi
{
    public static partial class VClientCalculations
    {

        public delegate object ClientFunction(FactParam[] pars);

        public static SortedList<string, ClientFunction> AllFuncs = new SortedList<string, ClientFunction>();
        private static bool _inited;
        class FalseResult
        {
        }
        public static void InitFuncsImpementation()
        {
            if (_inited) return;
            _inited = true;


            AllFuncs[TextConst.AVFunction.IsNull] = delegate (FactParam[] pars)
            {

                if (pars[0].Evaluate() == null)
                {
                    return true;
                }
                return false;
            };

            AllFuncs[TextConst.AVFunction.IsNotNull] = delegate (FactParam[] pars)
            {

                if (pars[0].Evaluate() == null)
                {
                    return false;
                }
                return true;
            };
            AllFuncs[TextConst.AVFunction.Or] = delegate (FactParam[] pars)
            {

                foreach (var par in pars)
                {
                    if ((bool)par.Evaluate())
                    {
                        return true;
                    }
                }
                return false;
            };

            AllFuncs[TextConst.AVFunction.And] = delegate (FactParam[] pars)
            {

                foreach (var par in pars)
                {
                    if (!(bool)par.Evaluate())
                    {
                        return false;
                    }
                }
                return true;
            };

            AllFuncs[TextConst.AVFunction.In] = delegate (FactParam[] pars)
            {

                object p0 = pars[0].Evaluate();
                object[] p1 = (object[])pars[1].Evaluate();
                foreach (var v in p1)
                {
                    if (v.ToString() == p0.ToString())
                    {
                        return true;
                    }
                }
                return false;
            };


            AllFuncs[TextConst.AVFunction.False] = delegate (FactParam[] pars)
            {


                return false;
            };

            AllFuncs[TextConst.AVFunction.Array] = delegate (FactParam[] pars)
            {

                var val = new List<object>();
                foreach (var par in pars)
                {
                    val.Add(par.Evaluate());
                }
                return val.ToArray();
            };

            AllFuncs[TextConst.AVFunction.Concat] = delegate (FactParam[] pars)
            {

                object val = "";
                foreach (var par in pars)
                {
                    var val1 = Cmn.Nvl(par.Evaluate(), "").ToString();
                    val += val1;
                }
                return val;
            };

            AllFuncs[TextConst.AVFunction.Coalesce] = delegate (FactParam[] pars)
            {

                object val = null;
                foreach (var par in pars)
                {
                    val = par.Evaluate();
                    if (val != null)
                    {
                        break;
                    }
                }
                return val;
            };



            AllFuncs[TextConst.AVFunction.Div] = delegate (FactParam[] pars)
            {
                if (HasNulls(pars)) return null;

                var val = Cmn.ToDecimal(pars[0].Evaluate()) / Cmn.ToDecimal(pars[1].Evaluate());


                return val;
            };

            AllFuncs[TextConst.AVFunction.Multiply] = delegate (FactParam[] pars)
            {
                if (HasNulls(pars)) return null;

                var val = (decimal)1;

                foreach (var par in pars)
                {
                    val = val * Cmn.ToDecimal(par.Evaluate());
                }
                return val;
            };

            AllFuncs[TextConst.AVFunction.PlusNvl] = delegate (FactParam[] pars)
            {

                var parVals = NullsToZero(pars);


                var val = (decimal)0;

                foreach (var par in parVals)
                {
                    val = val + Cmn.ToDecimal(par);
                }
                return val;
            };


            AllFuncs[TextConst.AVFunction.MinusNvl] = delegate (FactParam[] pars)
            {

                var parVals = NullsToZero(pars);
                var val = Cmn.ToDecimal(parVals[0]);
                int i = 0;
                foreach (var par in parVals)
                {
                    if (i > 0)
                    {
                        val = val - Cmn.ToDecimal(par);
                    }
                    i++;

                }
                return val;
            };

            AllFuncs[TextConst.AVFunction.Neg] = delegate (FactParam[] pars)
            {
                if (HasNulls(pars)) return null;
                return -Cmn.ToDecimal(pars[0].Evaluate()); ;
            };
            ClientFunction lt = delegate (FactParam[] pars)
            {
                if (HasNulls(pars)) return false;
                object v0 = pars[0].Evaluate();
                if (Cmn.IsNumeric(v0))
                {
                    return Cmn.ToDecimal(v0) < Cmn.ToDecimal(pars[1].Evaluate());
                }
                else
                {
                    throw new NotImplementedException(); // реализовать для дат и строк
                }
            };
            AllFuncs[TextConst.AVFunction.Less] = lt;
            AllFuncs["ls"] = lt;
            AllFuncs[TextConst.AVFunction.Greater] = delegate (FactParam[] pars)
            {
                if (HasNulls(pars)) return false;
                object v0 = pars[0].Evaluate();
                if (Cmn.IsNumeric(v0))
                {
                    return Cmn.ToDecimal(v0) > Cmn.ToDecimal(pars[1].Evaluate());
                }
                else
                {
                    throw new NotImplementedException(); // реализовать для дат и строк
                }
            };

            AllFuncs[TextConst.AVFunction.Equal] = delegate (FactParam[] pars)
            {

                return Cmn.Nvl(pars[0].Evaluate(), "").ToString() == Cmn.Nvl(pars[1].Evaluate(), "").ToString();


            };

            AllFuncs[TextConst.AVFunction.If] = delegate (FactParam[] pars)
            {
                if ((bool)pars[0].Evaluate())
                {
                    return pars[1].Evaluate();
                }
                else
                {
                    if (pars.Length > 2)
                    {
                        return pars[2].Evaluate();
                    }
                    else
                    {
                        return null;
                    }
                }

            };


            AllFuncs[TextConst.AVFunction.Case] = delegate (FactParam[] pars)
            {

                foreach (var par in pars)
                {
                    var res = par.Evaluate();
                    if (!(res is FalseResult))
                    {
                        return res;
                    }
                }
                return null;
            };


            AllFuncs[TextConst.AVFunction.When] = delegate (FactParam[] pars)
            {
                if ((bool)pars[0].Evaluate())
                {
                    return pars[1].Evaluate();
                }
                else
                {
                    return new FalseResult();
                }

            };

            AllFuncs[TextConst.AVFunction.Elese] = delegate (FactParam[] pars)
            {
                return pars[0].Evaluate();
            };

            AllFuncs[TextConst.AVFunction.Dummy] = delegate (FactParam[] pars)
            {
                return pars[0].Evaluate();
            };
        }
        private static bool HasNulls(FactParam[] pars)
        {
            return System.Array.Exists(pars, FactParam.ValueIsNull);
        }
        private static decimal[] NullsToZero(FactParam[] pars)
        {
            int count = pars.Length;
            decimal[] values = new decimal[count]; // NB: массив инициализирован нулями
            for (int index = 0; index < count; index++)
            {
                object value = pars[index].Evaluate();
                if (!Cmn.IsNullOrDBNull(value))
                {
                    values[index] = (decimal)value;
                }
            }
            return values;
        }
    }
}