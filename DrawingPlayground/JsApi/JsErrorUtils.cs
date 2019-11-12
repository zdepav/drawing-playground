#nullable enable

using Jint;
using Jint.Runtime;

namespace DrawingPlayground.JsApi {

    internal static class JsErrorUtils {
        
        public static JavaScriptException Error(Engine engine, string message) {
            return new JavaScriptException(engine.Error, message);
        }

        public static JavaScriptException NotSupported(Engine engine, string className, string memberName) {
            return new JavaScriptException(engine.Error, $"{className}.{memberName} is not supported");
        }

        public static JavaScriptException InvalidValue(Engine engine, string className, string propertyName, object? value) {
            return new JavaScriptException(
                engine.Error,
                $"{(value == null ? "null" : $"'{value}'")} is not a valid value for {className}.{propertyName}"
            );
        }

        public static JavaScriptException InvalidValue(Engine engine, string className, string methodName, string argumentName, object? value) {
            return new JavaScriptException(
                engine.Error,
                $"{(value == null ? "null" : $"'{value}'")} is not a valid value for argument {argumentName} of {className}.{methodName}"
            );
        }

        public static JavaScriptException TypeError(Engine engine, string message) {
            return new JavaScriptException(engine.TypeError, message, null);
        }

    }

}
