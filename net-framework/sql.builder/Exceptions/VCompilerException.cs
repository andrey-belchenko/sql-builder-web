using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sql.builder;
using sql.builder.Controls;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using sql.builder.DataApi;
using sql.builder.UI;
namespace sql.builder.Exceptions
{
  
    

    class VCompilerException : Exception
    {

        public VCompilerException(string message,XElement elementInfo,XElement nodeInfo)
            : base(message)
        {
            if (UIStatic.ShowErrorsInSchemeEditor && elementInfo != null) {
                try {
                    var errorInfo = new VErrorInfo();
                    errorInfo.NodeInfo = nodeInfo;
                    errorInfo.ElementInfo = elementInfo;
                    //ucMainSqlBuilder.ShowErrorInSchemeEditor(errorInfo);
                } catch {
                    throw new System.Exception("Ошибка при компиляции. Не удалось ошибку в схеме. Сообщение: " + message + " " + nodeInfo + " " + elementInfo);
                }
            }
        }
    }

    class VErrorInfo
    {
        public XElement ElementInfo;
        public XElement NodeInfo;
    }


    class VExceptionController
    {
        private static Stack<XElement> _processedStack = new Stack<XElement>();
        public static void ClearProcessedStack()
        {
            _processedStack = new Stack<XElement>();
        }
        public static void BeginProcessingElement(XElement element)
        {
            _processedStack.Push(element);
        }

        public static void EndProcessingElement(XElement element)
        {
            var el = _processedStack.Pop();
            if (el != element)
            {
                throw new InvalidOperationException("Не соответствуют вызовы BeginProcessingElement EndProcessingElement");
            }
        }

        public static XElement GetProcessedElementAndClear()
        {
            if (!_processedStack.Any()) return null;
            var el = _processedStack.Peek();
            ClearProcessedStack();
            return el;
        }
    }
}
