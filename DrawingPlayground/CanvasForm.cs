#nullable enable
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jint.Native.Object;
using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground {

    internal partial class CanvasForm : DockContent {

        private static readonly string[] eventNames = {
            "step", "draw",
            "keyDown", "keyUp",
            "mouseDown", "mouseUp", "mouseMove"
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
            if (!enabledCheckBox.Checked) {
                return;
            }
            if (jsEvents.TryGetValue(name, out var func)) {
                jsRunner.InvokeFunction(func, args);
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
            RunEvent("step");
            RunEvent("draw", e.Graphics);
        }

        protected override string GetPersistString() => "Canvas";

        private void timer_Tick(object sender, System.EventArgs e) {
            Refresh();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e) {
            mouseLocation = e.Location;
            RunEvent("mouseMove", e.Location);
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e) {
            if (pressedMouseButtons.Add(e.Button)) {
                RunEvent("mouseDown", e.Button.ToString());
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e) {
            pressedMouseButtons.Remove(e.Button);
            RunEvent("mouseUp", e.Button.ToString());
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e) {
            if (pressedKeys.Add(e.KeyCode)) {
                RunEvent("keyDown", e.KeyCode.ToString());
            }
        }

        private void canvas_KeyUp(object sender, KeyEventArgs e) {
            pressedKeys.Remove(e.KeyCode);
            RunEvent("keyUp", e.KeyCode.ToString());
        }

    }

}
