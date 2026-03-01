using System;
using System.Collections.Generic;
using System.Linq;

namespace sql.builder.UI.CommandItems
{
    public static class UIFormsPool
    {
        public static int MaxInGroup = 1;
        private static Dictionary<string, List<UIFormInfo>> _pool = new Dictionary<string, List<UIFormInfo>>();
        public static UIFormC Get(string group_name)
        {
            List<UIFormInfo> forms_info = null;
            if (!_pool.TryGetValue(group_name, out forms_info))
            {
                forms_info = new List<UIFormInfo>();
                _pool[group_name] = forms_info;
            }
            UIFormInfo form_info = forms_info.FirstOrDefault(UIFormInfo.IsUnused);
            if (form_info == null)
            {
                if (forms_info.Count < MaxInGroup)
                {
                    return null;
                }
                else
                {
                    form_info = forms_info.OrderBy(UIFormInfo.LastGetTime).First();
                }
            }
            form_info.SetUsed();
            return form_info.Form;
        }
        public static void Add(UIFormC form)
        {
            List<UIFormInfo> forms_info;
            if (!_pool.TryGetValue(form.GroupName, out forms_info))
            {
                forms_info = new List<UIFormInfo>(1);
                _pool[form.GroupName] = forms_info;
            }
            if (forms_info.Count >= MaxInGroup)
            {
                throw new ArgumentOutOfRangeException(string.Format("Число форм типа \"{0}\" не должно превышать {1}", form.GroupName, MaxInGroup));
            }
            var form_info = new UIFormInfo(form, true);
            forms_info.Add(form_info);
        }
        public static void Free(UIFormC form)
        {
            List<UIFormInfo> forms_info;
            if (_pool.TryGetValue(form.GroupName, out forms_info) && forms_info != null)
            {
                for (int index = 0; index < forms_info.Count; index++)
                {
                    UIFormInfo form_info = forms_info[index];
                    if (form_info.Form == form)
                    {
                        form_info.SetUnused();
                        break;
                    }
                }
            }
        }
        public static void Release(UIFormC form, bool dispose = true)
        {
            List<UIFormInfo> forms_info;
            if (_pool.TryGetValue(form.GroupName, out forms_info) && forms_info != null)
            {
                for (int index = 0; index < forms_info.Count; index++)
                {
                    UIFormInfo form_info = forms_info[index];
                    if (form_info.Form == form)
                    {
                        forms_info.RemoveAt(index);
                        form_info.Release(dispose);
                        break;
                    }
                }
            }
        }
        public static void Clear()
        {
            _pool.Clear();
        }
        public static void Reset()
        {
            foreach (List<UIFormInfo> forms_info in _pool.Values)
            {
                for (int index = 0; index < forms_info.Count; index++)
                {
                    UIFormInfo form_info = forms_info[index];
                    form_info.Form.GroupName = null;
                    if (UIFormInfo.IsUnused(form_info))
                    {
                        //(form_info.Form.TmpGetControlAsWinFormCtrl() as IDisposable).Dispose();
                    }
                }
                forms_info.Clear();
            }

            _pool.Clear();
        }
        private class UIFormInfo
        {
            private UIFormC form;
            private bool used;
            private DateTime last_get_time;
            public UIFormC Form { get { return this.form; } }
            public UIFormInfo(UIFormC form, bool used)
            {
                this.form = form;
                this.used = used;
                this.last_get_time = DateTime.Now;
            }
            public void Release(bool dispose)
            {
                if (dispose)
                {
                    //(this.form.TmpGetControlAsWinFormCtrl() as IDisposable).Dispose();
                }
                this.form = null;
            }
            public void SetUsed()
            {
                this.used = true;
                this.last_get_time = DateTime.Now;
            }
            public void SetUnused()
            {
                this.used = false;
            }
            public static bool IsUnused(UIFormInfo fi)
            {
                return !fi.used;
            }
            public static DateTime LastGetTime(UIFormInfo fi)
            {
                return fi.last_get_time;
            }
        }
    }
}