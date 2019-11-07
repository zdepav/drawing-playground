#nullable enable
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Esprima;
using FastColoredTextBoxNS;
using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground {

    internal partial class CodeEditorForm : DockContent {

        private readonly JsRunner jsRunner;

        private readonly MainForm mainForm;

        private readonly ILog log;

        private readonly Style ErrorStyle = new WavyLineStyle(255, Color.Red);

        private volatile ParserException[] syntaxErrors;

        private volatile bool codeChangedSinceSave, codeChangedSinceRun;

        private volatile string code;

        private FileInfo? scriptFile;

        public FileInfo? ScriptFile {
            get => scriptFile;
            set {
                codeBox.Enabled = value != null;
                scriptFile = value;
            }
        }

        public CodeEditorForm(JsRunner jsRunner, MainForm mainForm, ILog log) {
            this.jsRunner = jsRunner;
            this.mainForm = mainForm;
            this.log = log;
            codeChangedSinceSave = codeChangedSinceRun = false;
            syntaxErrors = new ParserException[0];
            code = "";
            InitializeComponent();
            ScriptFile = null;
        }

        private void codeBox_TextChangedDelayed(object sender, TextChangedEventArgs e) {
            code = codeBox.Text;
            codeChangedSinceRun = codeChangedSinceSave = true;
            if (!backgroundWorker.IsBusy) {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            while (codeChangedSinceRun) {
                codeChangedSinceRun = false;
                syntaxErrors = jsRunner.RunCode(code);
                if (syntaxErrors.Length == 0) {
                    mainForm.canvasForm?.RefreshEvents();
                }
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            var range = codeBox.Range;
            range.ClearStyle(ErrorStyle);
            foreach (var error in syntaxErrors) {
                var index = error.Index;
                if (index < codeBox.TextLength) {
                    codeBox.GetRange(index, index + 1).SetStyle(ErrorStyle);
                }
            }
            mainForm.errorListForm?.SetErrors(syntaxErrors);
        }

        private void saveTimer_Tick(object sender, EventArgs e) {
            if (scriptFile != null && codeChangedSinceSave) {
                try {
                    FileUtils.SaveFile(scriptFile, code);
                    codeChangedSinceSave = false;
                } catch {
                    log.LogError("Unable to save script");
                }
            }
        }

        public void NavigateTo(ParserException error) {
            var index = error.Index;
            codeBox.Selection = codeBox.GetRange(
                index,
                index < codeBox.TextLength
                    ? index + 1
                    : index
            );
        }

        protected override string GetPersistString() => "CodeEditor";

        public void SetCode(string code) {
            codeBox.Text = code;
        }

    }

}
