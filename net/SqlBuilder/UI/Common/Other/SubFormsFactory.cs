using System.Collections.Concurrent;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.UI
{
    public class SubFormsFactory
    {
        bool AsyncMode = false;

        private ConcurrentDictionary<string, UIFormC> _forms;

        public SubFormsFactory()
        {
            if (AsyncMode)
            {
                _forms = new ConcurrentDictionary<string, UIFormC>();
            }
        }

        public UIFormC Get(XElement xuseform, UIFormC parent, bool old = true)
        {
            UIFormC form = null;
            string name = xuseform.Attribute(AName.call).Value;
            if (AsyncMode)
            {
                if (_forms.TryRemove(name, out form))
                {
                    // переделать по-человечески
                    if (form.task != null) form.task.Wait();
                }
                else
                {
                    form = CreateForm(old);
                    form.PrepareAsync(xuseform, parent);
                    form.task.Wait();
                }

                // ставим готовиться новую форму
                var form_template = _forms[name] = CreateForm(old);
                form_template.PrepareAsync(xuseform, parent);
            }
            else
            {
                form = CreateForm(old);
                form.Prepare(xuseform, parent);
            }

            return form;
        }

        UIFormC CreateForm(bool old)
        {
            return (UIFormC)new UIFormC();
            //return (old) ? 
            //    (UIFormC)new UIFormC() { Visible = false } : 
            //    (UIFormC)new UIFormC2();
        }

    }
}
