// <copyright file="DelegateExtensions.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Diagnostics;
using System.Reflection;

namespace OpenTelemetry.AutoInstrumentation.Instrumentations.WindowsPresentationFoundation;

internal static class DelegateExtensions
{
    private static readonly ActivitySource ActivitySource = new("WindowsPresentationFoundation");

    internal static Delegate Wrap(this Delegate value)
    {
        var method = value.GetMethodInfo();
        var returnType = method.ReturnType;
        var parameters = method.GetParameters();

        switch (parameters.Length)
        {
            case 0:
                {
                    if (returnType == typeof(void))
                    {
                        return new Action0Wrapper(value).Wrap();
                    }
                    else
                    {
                        return ((Wrapper)Activator.CreateInstance(typeof(Func0Wrapper<>).MakeGenericType(returnType), value)!).Wrap();
                    }
                }

            case 1:
                {
                    if (returnType == typeof(void))
                    {
                        return ((Wrapper)Activator.CreateInstance(typeof(Action1Wrapper<>).MakeGenericType(parameters[0].ParameterType), value)!).Wrap();
                    }
                    else
                    {
                        return ((Wrapper)Activator.CreateInstance(typeof(Func1Wrapper<,>).MakeGenericType(parameters[0].ParameterType, returnType), value)!).Wrap();
                    }
                }

            case 2:
                {
                    if (returnType == typeof(void))
                    {
                        return ((Wrapper)Activator.CreateInstance(typeof(Action2Wrapper<,>).MakeGenericType(parameters[0].ParameterType, parameters[1].ParameterType), value)!).Wrap();
                    }
                    else
                    {
                        return ((Wrapper)Activator.CreateInstance(typeof(Func2Wrapper<,,>).MakeGenericType(parameters[0].ParameterType, parameters[1].ParameterType, returnType), value)!).Wrap();
                    }
                }

            default:
                return value;
        }
    }

    private class Wrapper
    {
        private readonly Delegate target;
        private Activity? activity;

        public Wrapper(Delegate target)
        {
            this.target = target;

            var method = target.Method;
            var name = method.DeclaringType + "." + method.Name;
            activity = ActivitySource.CreateActivity(name, ActivityKind.Internal);
        }

        public Delegate Wrap()
        {
            return Delegate.CreateDelegate(target.GetType(), this, "Invoke");
        }

        internal void OnMethodBegin()
        {
            activity?.Start();
        }

        internal void OnMethodEnd()
        {
            activity?.Dispose();
        }
    }

    private class Action0Wrapper : Wrapper
    {
        private readonly Action _delegate;

        public Action0Wrapper(Delegate target)
            : base(target)
        {
            _delegate = (Action)target.Method.CreateDelegate(typeof(Action), target.Target);
        }

        private void Invoke()
        {
            try
            {
                try
                {
                    OnMethodBegin();
                }
                catch (Exception)
                {
                    // Skip
                }

                _delegate();
            }
            finally
            {
                try
                {
                    OnMethodEnd();
                }
                catch (Exception)
                {
                    // Skip
                }
            }
        }
    }

    private class Action1Wrapper<TArg1> : Wrapper
    {
        private readonly Action<TArg1> _delegate;

        public Action1Wrapper(Delegate target)
            : base(target)
        {
            _delegate = (Action<TArg1>)target.Method.CreateDelegate(typeof(Action<>).MakeGenericType(typeof(TArg1)), target.Target);
        }

        private void Invoke(TArg1 arg1)
        {
            try
            {
                try
                {
                    OnMethodBegin();
                }
                catch (Exception)
                {
                    // Skip
                }

                _delegate(arg1);
            }
            finally
            {
                try
                {
                    OnMethodEnd();
                }
                catch (Exception)
                {
                    // Skip
                }
            }
        }
    }

    private class Action2Wrapper<TArg1, TArg2> : Wrapper
    {
        private readonly Action<TArg1, TArg2> _delegate;

        public Action2Wrapper(Delegate target)
            : base(target)
        {
            _delegate = (Action<TArg1, TArg2>)target.Method.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(TArg1), typeof(TArg2)), target.Target);
        }

        private void Invoke(TArg1 arg1, TArg2 arg2)
        {
            try
            {
                try
                {
                    OnMethodBegin();
                }
                catch (Exception)
                {
                    // Skip
                }

                _delegate(arg1, arg2);
            }
            finally
            {
                try
                {
                    OnMethodEnd();
                }
                catch (Exception)
                {
                    // Skip
                }
            }
        }
    }

    private class Func0Wrapper<TResult> : Wrapper
    {
        private readonly Func<TResult> _delegate;

        public Func0Wrapper(Delegate target)
            : base(target)
        {
            _delegate = (Func<TResult>)target.Method.CreateDelegate(typeof(Func<>).MakeGenericType(typeof(TResult)), target.Target);
        }

        private TResult? Invoke()
        {
            try
            {
                try
                {
                    OnMethodBegin();
                }
                catch (Exception)
                {
                    // Skip
                }

                return _delegate();
            }
            finally
            {
                try
                {
                    OnMethodEnd();
                }
                catch (Exception)
                {
                    // Skip
                }
            }
        }
    }

    private class Func1Wrapper<TArg1, TResult> : Wrapper
    {
        private readonly Func<TArg1, TResult> _delegate;

        public Func1Wrapper(Delegate target)
            : base(target)
        {
            _delegate = (Func<TArg1, TResult>)target.Method.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(TArg1), typeof(TResult)), target.Target);
        }

        private TResult? Invoke(TArg1 arg1)
        {
            try
            {
                try
                {
                    OnMethodBegin();
                }
                catch (Exception)
                {
                    // Skip
                }

                return _delegate(arg1);
            }
            finally
            {
                try
                {
                    OnMethodEnd();
                }
                catch (Exception)
                {
                    // Skip
                }
            }
        }
    }

    private class Func2Wrapper<TArg1, TArg2, TResult> : Wrapper
    {
        private readonly Func<TArg1, TArg2, TResult> _delegate;

        public Func2Wrapper(Delegate target)
            : base(target)
        {
            _delegate = (Func<TArg1, TArg2, TResult>)target.Method.CreateDelegate(typeof(Func<,,>).MakeGenericType(typeof(TArg1), typeof(TArg2), typeof(TResult)), target.Target);
        }

        private TResult? Invoke(TArg1 arg1, TArg2 arg2)
        {
            try
            {
                try
                {
                    OnMethodBegin();
                }
                catch (Exception)
                {
                    // Skip
                }

                return _delegate(arg1, arg2);
            }
            finally
            {
                try
                {
                    OnMethodEnd();
                }
                catch (Exception)
                {
                    // Skip
                }
            }
        }
    }
}
