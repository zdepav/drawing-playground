#nullable enable
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Jint.Native.Object;
using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground {

    internal partial class CanvasForm : DockContent {

        private static readonly string[] eventNames = {
            "step", "draw",
            "keyPress", "keyRelease",
            "mousePress", "mouseRelease", "mouseMove", "mouseScroll"
        };

        private readonly HashSet<Keys> pressedKeys;

        private readonly HashSet<MouseButtons> pressedMouseButtons;

        private Point mouseLocation;

        private readonly JsRunner jsRunner;

        private readonly ConcurrentDictionary<string, ObjectInstance> jsEvents;

        public CanvasForm(JsRunner jsRunner) {
            this.jsRunner = jsRunner;
            mouseLocation = Point.Empty;
            pressedKeys = new HashSet<Keys>();
            pressedMouseButtons = new HashSet<MouseButtons>();
            jsEvents = new ConcurrentDictionary<string, ObjectInstance>();
            InitializeComponent();
        }

        private void RunEvent(string name, params object[] args) {
            if (jsEvents.TryGetValue(name, out var func)) {
                jsRunner.InvokeFunction(func);
            }
        }

        public void RefreshEvents() {
            foreach (var eventName in eventNames) {
                var func = jsRunner.GetFunction(eventName);
                if (func != null) {
                    jsEvents[eventName] = func;
                } else {
                    jsEvents.TryRemove(eventName, out _);
                }
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e) {
            RunEvent("draw", e.Graphics);
        }

        protected override string GetPersistString() => "Canvas";
    }

}
