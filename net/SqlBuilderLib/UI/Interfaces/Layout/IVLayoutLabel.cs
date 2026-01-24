using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public interface IVLayoutLabel : IVLayoutNode
    {
        void SetText(string text);
		void SetHint(string text);
        void SetBold(bool value);
        void SetTextAlignment(bool isLeft);
        int GetTextWidth();
    }
}
