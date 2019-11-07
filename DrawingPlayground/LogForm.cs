#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;
using Jint.Runtime;
using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground {

    public partial class LogForm : DockContent, ILog {

        private readonly object textBoxLock;

        private readonly RichTextBox bufferTextBox;

        public LogForm() {
            textBoxLock = new object();
            InitializeComponent();
            bufferTextBox = new RichTextBox {
                Font = logTextBox.Font,
                ForeColor = logTextBox.ForeColor,
                BackColor = logTextBox.BackColor
            };
        }

        public void Log(string message) {
            AppendText(message, Color.Black);
        }

        public void Log(object? value) {
            Log(value?.ToString() ?? "null");
        }

        public void LogDebug(object? value) {
            AppendText(value?.ToString() ?? "null", Color.Gray);
        }

        public void LogInfo(object? value) {
            AppendText(value?.ToString() ?? "null", Color.Black);
        }

        public void LogWarning(object? value) {
            AppendText(value?.ToString() ?? "null", Color.Goldenrod);
        }

        public void LogError(object? value) {
            AppendText(value?.ToString() ?? "null", Color.Red);
        }

        public void LogFatal(object? value) {
            AppendText(value?.ToString() ?? "null", Color.DarkRed);
        }

        public void LogError(JavaScriptException error) {
            AppendText(error.ToString(), Color.Red);
        }

        public void LogError(MemoryLimitExceededException error){
            AppendText("Memory limit reached", Color.DarkRed);
        }

        public void LogError(TimeoutException error){
            AppendText("Execution time limit reached", Color.DarkRed);
        }

        public void LogError(RecursionDepthOverflowException error){
            AppendText("Stack overflow", Color.DarkRed);
        }

        private void AppendText(string text, Color color) {
            if (InvokeRequired) {
                Invoke(new Action(() => AppendTextActual(text, color)));
            } else AppendTextActual(text, color);
        }

        private void AppendTextActual(string text, Color color) {
            lock (textBoxLock) {
                var a = bufferTextBox.TextLength;
                bufferTextBox.AppendText(text + '\n');
                var b = bufferTextBox.TextLength;
                bufferTextBox.Select(a, b - a);
                bufferTextBox.SelectionColor = color;
                var (start, length) = (logTextBox.SelectionStart, logTextBox.SelectionLength);
                logTextBox.Rtf = bufferTextBox.Rtf;
                (logTextBox.SelectionStart, logTextBox.SelectionLength) = (start, length);
            }
        }

        protected override string GetPersistString() => "Console";

    }

}
