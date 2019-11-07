#nullable enable
using System;
using Jint.Runtime;

namespace DrawingPlayground {

    internal interface ILog {

        void Log(string message);

        void LogDebug(object? value);

        void LogInfo(object? value);

        void LogWarning(object? value);

        void LogError(object? value);

        void LogError(JavaScriptException error);

        void LogError(MemoryLimitExceededException error);

        void LogError(TimeoutException error);

        void LogError(RecursionDepthOverflowException error);

        void LogFatal(object? value);

    }

}
