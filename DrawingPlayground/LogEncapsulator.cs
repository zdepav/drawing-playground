#nullable enable
using System;
using Jint.Runtime;

namespace DrawingPlayground {

    internal class LogEncapsulator : ILog {

        public ILog? Output { get; set; }

        public void Log(string message) {
            Output?.Log(message);
        }

        public void LogDebug(object? value) {
            Output?.LogDebug(value);
        }

        public void LogInfo(object? value) {
            Output?.LogInfo(value);
        }

        public void LogWarning(object? value) {
            Output?.LogWarning(value);
        }

        public void LogError(object? value) {
            Output?.LogError(value);
        }

        public void LogError(JavaScriptException error) {
            Output?.LogError(error);
        }

        public void LogError(MemoryLimitExceededException error){
            Output?.LogError(error);
        }

        public void LogError(TimeoutException error){
            Output?.LogError(error);
        }

        public void LogError(RecursionDepthOverflowException error){
            Output?.LogError(error);
        }


        public void LogFatal(object? value) {
            Output?.LogFatal(value);
        }

    }

}
