// <copyright file="MediaContextIntegration.cs" company="OpenTelemetry Authors">
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
/// System.Windows.Media.MediaContext calltarget instrumentation
/// </summary>
[InstrumentMethod(
assemblyName: "PresentationCore",
typeName: "System.Windows.Media.MediaContext",
methodName: "AddLoadedOrUnloadedCallback",
returnTypeName: "MS.Internal.LoadedOrUnloadedOperation",
parameterTypeNames: new string[] { "System.Windows.Threading.DispatcherOperationCallback", "System.Windows.DependencyObject" },
minimumVersion: "4.0.0",
maximumVersion: "6.65535.65535",
integrationName: "WindowsPresentationFoundation",
type: InstrumentationType.Trace)]
public static class MediaContextIntegration
{
    internal static CallTargetState OnMethodBegin<TTarget, TDependencyObject>(TTarget instance, ref Delegate callback, TDependencyObject dependencyObject)
    {
        callback = callback.Wrap();
        return CallTargetState.GetDefault();
    }
}
