using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime.Interop;

namespace DrawingPlayground.JsApi {

    public class Path2DConstructor : FunctionInstance, IConstructor {

        public Path2DConstructor(Engine engine) : base(engine, "Path2D", new[] { "path" }, null, strict: false) { }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments) => Construct(arguments);

        public ObjectInstance Construct(JsValue[] arguments) {
            if (arguments.Length == 0) {
                return new ObjectWrapper(Engine, new Path2D(Engine));
            } else if (arguments[0] is ObjectWrapper ow && ow.Target is Path2D path) {
                return new ObjectWrapper(Engine, new Path2D(path));
            } else {
                throw JsErrorUtils.InvalidValue(Engine, nameof(Path2D), nameof(Path2D), "path", arguments[0]);
            }
        }
    }

}
