/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the  Apache License, Version 2.0, please send an email to 
 * dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Apache License, Version 2.0.
 *
 * You must not remove this notice, or any other, from this software.
 *
 *
 * ***************************************************************************/

using System;
using System.Runtime.Serialization;

namespace IronPython.Runtime.Exceptions {
#if !FEATURE_WARNING_EXCEPTION
    #region Generated WarningException

    // *** BEGIN GENERATED CODE ***
    // generated by function: gen_one_exception_specialized from: generate_exceptions.py


    [Serializable]
    public class WarningException : Exception {
        public WarningException() : base() { }
        public WarningException(string msg) : base(msg) { }
        public WarningException(string message, Exception innerException)
            : base(message, innerException) {
        }
#if FEATURE_SERIALIZATION
        protected WarningException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }


    // *** END GENERATED CODE ***

    #endregion
#endif
}
