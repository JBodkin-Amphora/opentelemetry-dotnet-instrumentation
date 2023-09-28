// <copyright file="DispatcherOperationIntegration.cs" company="OpenTelemetry Authors">
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

using OpenTelemetry.AutoInstrumentation.CallTarget;

namespace OpenTelemetry.AutoInstrumentation.Instrumentations.WindowsPresentationFoundation;

/// <summary>
/// System.Windows.Threading.DispatcherOperation calltarget instrumentation
/// </summary>
[InstrumentMethod(
assemblyName: "WindowsBase",
typeName: "System.Windows.Threading.DispatcherOperation",
methodName: ".ctor",
returnTypeName: "System.Windows.Threading.DispatcherOperation",
parameterTypeNames: new string[] { "System.Windows.Threading.Dispatcher", "System.Delegate", "System.Windows.Threading.DispatcherPriority", "System.Object", "System.Int32", "System.Windows.Threading.DispatcherOperationTaskSource", "System.Boolean" },
minimumVersion: "4.0.0",
maximumVersion: "6.65535.65535",
integrationName: "WindowsPresentationFoundation",
type: InstrumentationType.Trace)]
public static class DispatcherOperationIntegration
{
    internal static CallTargetState OnMethodBegin<TTarget, TDispatcher, TDispatcherPriority, TArgs, TNumArgs, TDispatcherOperationTaskSource, TUseAsyncSemantics>(TTarget instance, TDispatcher dispatcher, ref Delegate callback, TDispatcherPriority priority, TArgs args, TNumArgs numArgs, TDispatcherOperationTaskSource taskSource, TUseAsyncSemantics useAsyncSemantics)
    {
        callback = callback.Wrap();
        return CallTargetState.GetDefault();
    }
}
