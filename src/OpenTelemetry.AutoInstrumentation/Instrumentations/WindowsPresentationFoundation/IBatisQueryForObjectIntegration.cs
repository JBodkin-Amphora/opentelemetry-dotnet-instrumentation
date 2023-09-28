// <copyright file="IBatisQueryForObjectIntegration.cs" company="OpenTelemetry Authors">
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
using OpenTelemetry.AutoInstrumentation.CallTarget;

namespace OpenTelemetry.AutoInstrumentation.Instrumentations.WindowsPresentationFoundation;

/// <summary>
/// IBatisNet.DataMapper.SqlMapper calltarget instrumentation
/// </summary>
[InstrumentMethod(
assemblyName: "IBatisNet.DataMapper",
typeName: "IBatisNet.DataMapper.SqlMapper",
methodName: "QueryForObject",
returnTypeName: "System.Object",
parameterTypeNames: new string[] { "System.String", "System.Object" },
minimumVersion: "1.0.0",
maximumVersion: "1.65535.65535",
integrationName: "WindowsPresentationFoundation",
type: InstrumentationType.Trace)]
public static class IBatisQueryForObjectIntegration
{
    private static readonly ActivitySource ActivitySource = new("WindowsPresentationFoundation");

    internal static CallTargetState OnMethodBegin<TTarget, TStatementName, TParameter>(TTarget instance, TStatementName statementName, TParameter parameter)
    {
        var activity = ActivitySource.StartActivity("IBatisNet.DataMapper.SqlMapper.QueryForObject");

        activity?.SetTag("query.statement", statementName as string ?? string.Empty);

        return activity is not null ? new CallTargetState(activity) : CallTargetState.GetDefault();
    }

    internal static CallTargetReturn<TReturn> OnMethodEnd<TTarget, TReturn>(TTarget instance, TReturn returnValue, Exception exception, in CallTargetState state)
    {
        state.Activity?.Dispose();

        return new CallTargetReturn<TReturn>(returnValue);
    }
}
