using IronPython.Runtime;
using IronPython.Runtime.Operations;
using IronPython.Runtime.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domo.Modules
{
    [PythonType]
    public class TestDecorator : PythonTypeSlot
    {
        internal object func;

        public TestDecorator(CodeContext/*!*/ context, object func)
        {
            if (!PythonOps.IsCallable(context, func))
                throw PythonOps.TypeError("{0} object is not callable", func.ToString());
            this.func = func;
        }

        internal bool TryGetValue(CodeContext context, object instance, PythonType owner, out object value)
        {
            value = __get__(instance, PythonOps.ToPythonType(owner));
            return true;
        }

        internal bool GetAlwaysSucceeds
        {
            get
            {
                return true;
            }
        }

        public object __func__
        {
            get
            {
                return func;
            }
        }

        #region IDescriptor Members

        public object __get__(object instance) { return __get__(instance, null); }

        public object __get__(object instance, object owner)
        {
            if (owner == null)
            {
                if (instance == null) throw PythonOps.TypeError("__get__(None, None) is invalid");
                owner = DynamicHelpers.GetPythonType(instance);
            }
            return new Method(func, owner, DynamicHelpers.GetPythonType(owner));
        }

        #endregion
    }
}
