// <copyright file="ButtonIntegration.cs" company="OpenTelemetry Authors">
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
using OpenTelemetry.AutoInstrumentation.CallTarget;

namespace OpenTelemetry.AutoInstrumentation.Instrumentations.WindowsForms;

/// <summary>
/// System.Windows.Forms.Button calltarget instrumentation
/// </summary>
[InstrumentMethod(
    assemblyName: "System.Windows.Forms",
    typeName: "System.Windows.Forms.Button",
    methodName: "OnClick",
    returnTypeName: ClrNames.Void,
    parameterTypeNames: new string[] { "System.EventArgs" },
    minimumVersion: "4.0.0",
    maximumVersion: "6.65535.65535",
    integrationName: "WindowsForms",
    type: InstrumentationType.Trace)]
public static class ButtonIntegration
{
    private static readonly ActivitySource ActivitySource = new("WindowsForms");

    internal static CallTargetState OnMethodBegin<TTarget, TEventArgs>(TTarget instance, TEventArgs args)
    {
        var activity = ActivitySource.StartActivity("System.Windows.Forms.Button.OnClick", ActivityKind.Internal, parentId: null);

        var type = instance?.GetType();

        var nameProperty = type?.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
        var name = nameProperty?.GetValue(instance) as string ?? string.Empty;

        activity?.SetTag("component.name", name);
        activity?.SetTag("component.action", "click");
        activity?.SetTag("component.type", "button");

        return activity is not null ? new CallTargetState(activity) : CallTargetState.GetDefault();
    }

    internal static CallTargetReturn OnMethodEnd<TTarget>(TTarget instance, Exception exception, in CallTargetState state)
    {
        state.Activity?.Dispose();

        return CallTargetReturn.GetDefault();
    }
}
