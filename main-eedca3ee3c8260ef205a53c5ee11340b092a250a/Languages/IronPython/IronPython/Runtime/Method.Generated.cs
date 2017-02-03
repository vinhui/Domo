﻿/* ****************************************************************************
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

#if FEATURE_CORE_DLR
using System.Linq.Expressions;
#else
using Microsoft.Scripting.Ast;
#endif

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Dynamic;
using System.Reflection;

using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

using IronPython.Runtime.Operations;
using IronPython.Runtime.Types;
using Microsoft.Scripting.Utils;
using IronPython.Runtime.Binding;
using Microsoft.Scripting.Actions;

namespace IronPython.Runtime {
    public sealed partial class Method {
        Binding.FastBindResult<T> Binding.IFastInvokable.MakeInvokeBinding<T>(CallSite<T> site, Binding.PythonInvokeBinder binder, CodeContext context, object[] args) {
            // TODO: We can process any signature that isn't * or ** args for the 1st argument
            if (binder.Signature.IsSimple) {
                BaseMethodBinding binding = null;

                if (__self__ == null) {
                    if (args.Length != 0) {
                        binding = GetMethodBinding<T>(binder, GetTypeArgsSelfless<T>(), binding);

                        if (binding != null) {
                            return new FastBindResult<T>(
                                (T)(object)binding.GetSelflessTarget(),
                                true
                            );
                        }
                    }
                } else {
                    var selfBinder = GetSelfBinder(binder, context);

                    if (args.Length == 0) {
                        binding = new MethodBinding(selfBinder);
                    } else {
                        binding = GetMethodBinding<T>(selfBinder, GetTypeArgs<T>(), binding);
                    }

                    if (binding != null) {
                        return new FastBindResult<T>(
                            (T)(object)binding.GetSelfTarget(),
                            true
                        );
                    }
                }
            }
            return new Binding.FastBindResult<T>();
        }

        private static BaseMethodBinding GetMethodBinding<T>(Binding.PythonInvokeBinder binder, Type[] typeArgs, BaseMethodBinding binding) where T : class {
            #region Generated Python Selfless Method Caller Switch

            // *** BEGIN GENERATED CODE ***
            // generated by function: selfless_method_caller_switch from: generate_calls.py

            switch (typeArgs.Length) {
                case 1: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<>).MakeGenericType(typeArgs), binder); break;
                case 2: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,>).MakeGenericType(typeArgs), binder); break;
                case 3: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,>).MakeGenericType(typeArgs), binder); break;
                case 4: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,,>).MakeGenericType(typeArgs), binder); break;
                case 5: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,,,>).MakeGenericType(typeArgs), binder); break;
                case 6: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,,,,>).MakeGenericType(typeArgs), binder); break;
                case 7: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,,,,,>).MakeGenericType(typeArgs), binder); break;
                case 8: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,,,,,,>).MakeGenericType(typeArgs), binder); break;
                case 9: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,,,,,,,>).MakeGenericType(typeArgs), binder); break;
                case 10: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,,,,,,,,>).MakeGenericType(typeArgs), binder); break;
                case 11: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,,,,,,,,,>).MakeGenericType(typeArgs), binder); break;
                case 12: binding = (BaseMethodBinding)Activator.CreateInstance(typeof(MethodBinding<,,,,,,,,,,,>).MakeGenericType(typeArgs), binder); break;
            }

            // *** END GENERATED CODE ***

            #endregion
            return binding;
        }

        private static Type[] GetTypeArgs<T>() where T : class {
            return ArrayUtils.ShiftLeft(ArrayUtils.ConvertAll(typeof(T).GetMethod("Invoke").GetParameters(), pi => pi.ParameterType), 3);
        }

        private static Type[] GetTypeArgsSelfless<T>() where T : class {
            return ArrayUtils.ShiftLeft(ArrayUtils.ConvertAll(typeof(T).GetMethod("Invoke").GetParameters(), pi => pi.ParameterType), 4);
        }

        private static PythonInvokeBinder GetSelfBinder(Binding.PythonInvokeBinder binder, CodeContext context) {
            return context.LanguageContext.Invoke(
                new CallSignature(ArrayUtils.Insert(new Argument(ArgumentType.Simple), binder.Signature.GetArgumentInfos()))
            );
        }

        abstract class BaseMethodBinding {
            public abstract Delegate GetSelfTarget();
            public abstract Delegate GetSelflessTarget();
        }

        class MethodBinding : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object>>)site).Update(site, context, target);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                throw new InvalidOperationException();
            }
        }

        #region Generated Python Method Callers

        // *** BEGIN GENERATED CODE ***
        // generated by function: method_callers from: generate_calls.py


        class MethodBinding<T0> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, object>>)site).Update(site, context, target, arg0);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, object>>)site).Update(site, context, target, arg0, arg1);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, object>>)site).Update(site, context, target, arg0, arg1);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, object>>)site).Update(site, context, target, arg0, arg1, arg2);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, object>>)site).Update(site, context, target, arg0, arg1, arg2);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2, T3> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2, T3 arg3) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2, arg3);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, T3, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3, T3 arg4) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3, arg4);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, T3, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2, T3, T4> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2, arg3, arg4);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3, T3 arg4, T4 arg5) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3, arg4, arg5);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2, T3, T4, T5> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2, arg3, arg4, arg5);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3, T3 arg4, T4 arg5, T5 arg6) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3, arg4, arg5, arg6);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2, T3, T4, T5, T6> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3, T3 arg4, T4 arg5, T5 arg6, T6 arg7) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2, T3, T4, T5, T6, T7> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3, T3 arg4, T4 arg5, T5 arg6, T6 arg7, T7 arg8) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2, T3, T4, T5, T6, T7, T8> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3, T3 arg4, T4 arg5, T5 arg6, T6 arg7, T7 arg8, T8 arg9) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3, T3 arg4, T4 arg5, T5 arg6, T6 arg7, T7 arg8, T8 arg9, T9 arg10) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3, T3 arg4, T4 arg5, T5 arg6, T6 arg7, T7 arg8, T8 arg9, T9 arg10, T10 arg11) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>(SelflessTarget);
            }
        }

        class MethodBinding<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : BaseMethodBinding {
            private CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>> _site;

            public MethodBinding(PythonInvokeBinder binder) {
                _site = CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>>.Create(binder);
            }

            public object SelfTarget(CallSite site, CodeContext context, object target, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
                Method self = target as Method;
                if (self != null && self._inst != null) {
                    return _site.Target(_site, context, self._func, self._inst, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            }

            public object SelflessTarget(CallSite site, CodeContext context, object target, object arg0, T0 arg1, T1 arg2, T2 arg3, T3 arg4, T4 arg5, T5 arg6, T6 arg7, T7 arg8, T8 arg9, T9 arg10, T10 arg11, T11 arg12) {
                Method self = target as Method;
                if (self != null && self._inst == null) {
                    return _site.Target(_site, context, self._func, PythonOps.MethodCheckSelf(context, self, arg0), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
                }

                return ((CallSite<Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>>)site).Update(site, context, target, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            }

            public override Delegate GetSelfTarget() {
                return new Func<CallSite, CodeContext, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>(SelfTarget);
            }

            public override Delegate GetSelflessTarget() {
                return new Func<CallSite, CodeContext, object, object, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>(SelflessTarget);
            }
        }

        // *** END GENERATED CODE ***

        #endregion
    }
}
