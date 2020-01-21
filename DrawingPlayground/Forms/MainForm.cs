#nullable enable
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground.Forms {

    internal partial class MainForm : Form {

        public CodeEditorForm? codeEditorForm { get; private set; }
        public ErrorListForm? errorListForm { get; private set; }
        public CanvasForm? canvasForm { get; private set; }
        public HelpForm? helpForm { get; private set; }
        public LogForm? logForm { get; private set; }

        private readonly JsRunner jsRunner;

        private readonly LogEncapsulator log;
        private bool shouldClose;

        public DirectoryInfo DataDirectory { get; }

        public DirectoryInfo ProjectsDirectory { get; }

        public MainForm() {
            DataDirectory = Directory.CreateDirectory(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "DrawingPlayground"
#if DEBUG
                  , "Debug"
#endif
                )
            );
            ProjectsDirectory = Directory.CreateDirectory(
                Path.Combine(DataDirectory.FullName, "Projects")
            );
            codeEditorForm = null;
            errorListForm = null;
            canvasForm = null;
            logForm = null;
            shouldClose = false;
            log = new LogEncapsulator();
            jsRunner = new JsRunner(log);

            InitializeComponent();

            dockPanel.Theme = vs2015LightTheme;

            dockPanel.DockLeftPortion = 0.5;
            dockPanel.DockRightPortion = 0.5;

            AddCodeEditorForm();
            AddLogForm();
            AddErrorListForm();
            AddCanvasForm();
            AddHelpForm(false);
        }

        private void AddCodeEditorForm(bool show = true) {
            if (codeEditorForm == null) {
                codeEditorForm = new CodeEditorForm(jsRunner, this, log);
                codeEditorForm.Closed += (sender, e) => codeEditorForm = null;
            }
            if (codeEditorForm.IsHidden && show) {
                codeEditorForm.Show(dockPanel, DockState.Document);
            }
        }

        private void AddErrorListForm(bool show = true) {
            if (errorListForm == null) {
                errorListForm = new ErrorListForm(this);
                errorListForm.Closed += (sender, e) => errorListForm = null;
            }
            if (errorListForm.IsHidden && show) {
                errorListForm.Show(dockPanel, DockState.DockBottom);
            }
        }

        private void AddCanvasForm(bool show = true) {
            if (canvasForm == null) {
                canvasForm = new CanvasForm(jsRunner);
                canvasForm.Closed += (sender, e) => canvasForm = null;
            }
            if (canvasForm.IsHidden && show) {
                canvasForm.Show(dockPanel, DockState.DockRight);
            }
        }

        private void AddLogForm(bool show = true) {
            if (logForm == null) {
                logForm = new LogForm();
                logForm.Closed += (sender, e) => log.Output = logForm = null;
                log.Output = logForm;
            }
            if (logForm.IsHidden && show) {
                logForm.Show(dockPanel, DockState.DockBottom);
            }
        }

        private void AddHelpForm(bool show = true) {
            if (helpForm == null) {
                helpForm = new HelpForm();
                helpForm.Closed += (sender, e) => helpForm = null;
            }
            if (helpForm.IsHidden && show) {
                helpForm.Show(dockPanel, DockState.DockRightAutoHide);
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            LoadState();
        }

        private void codeEditorToolStripMenuItem_Click(object sender, EventArgs e) => AddCodeEditorForm();

        private void canvasToolStripMenuItem_Click(object sender, EventArgs e) => AddCanvasForm();

        private void errorListToolStripMenuItem_Click(object sender, EventArgs e) => AddErrorListForm();

        private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e) => AddLogForm();

        private void showHelpToolStripMenuItem_Click(object sender, EventArgs e) => AddHelpForm();

        private void resetToolStripMenuItem_Click(object sender, EventArgs e) => jsRunner.Reset();

        private void newToolStripMenuItem_Click(object sender, EventArgs e) => NewProject();

        private void openToolStripMenuItem_Click(object sender, EventArgs e) => OpenProject();

        private void LoadState() {
            if (!NewProject()) {
                shouldClose = true;
            } /*
            try {
                var stateString = FileUtils.LoadFile(DataDirectory, "state.json");
                if (!string.IsNullOrWhiteSpace(stateString)) {
                    var state = JsonConvert.DeserializeObject<AppState>(stateString);
                    if (!string.IsNullOrWhiteSpace(state.CurrentProject) &&
                        File.Exists(Path.Combine(DataDirectory.FullName, state.CurrentProject + ".js"))
                    ) {
                        OpenProject(state.CurrentProject);
                    } else if (!NewProject()) {
                        shouldClose = true;
                    }
                }
                var dpsStateFilePath = Path.Combine(DataDirectory.FullName, "dps.xml");
                if (File.Exists(dpsStateFilePath)) {
                    dockPanel.LoadFromXml(
                        dpsStateFilePath,
                        persistString => persistString switch {
                            "CodeEditor" => codeEditorForm,
                            "Canvas" => canvasForm,
                            "Help" => helpForm,
                            "Console" => logForm,
                            "ErrorList" => errorListForm,
                            _ => default(DockContent?)
                        }
                    );
                }
            } catch {
                log.LogError("Could not load editor state");
            }*/
        }

        private bool NewProject() {
            using var newProjectForm = new NewProjectForm(ProjectsDirectory);
            if (newProjectForm.ShowDialog() == DialogResult.OK) {
                codeEditorForm!.ScriptFile = new FileInfo(Path.Combine(ProjectsDirectory.FullName, newProjectForm.ProjectName + ".js"));
                codeEditorForm!.SetCode("");
                return true;
            } else {
                return false;
            }
        }

        private void OpenProject(string? projectName = null) {
            throw new NotImplementedException();
        }

        private void timer_Tick(object sender, EventArgs e) {
            using var mstream = new MemoryStream();
            dockPanel.SaveAsXml(mstream, Encoding.UTF8);
            FileUtils.SaveFile(DataDirectory, "dps.xml", mstream.ToArray());
        }

        private void MainForm_Shown(object sender, EventArgs e) {
            if (shouldClose) {
                Close();
            }
        }

        private void RunCodeToolStripMenuItem_Click(object sender, EventArgs e) {
            canvasForm?.ClearEvents();
            jsRunner.Reset();
            var errs = jsRunner.Run();
            errorListForm?.SetRuntimeErrors(errs);
            if (errs.Length == 0) {
                canvasForm?.RefreshEvents();
            }
        }

        private void licensesToolStripMenuItem_Click(object sender, EventArgs e) {
            using var licenses = new Licenses();
            licenses.ShowDialog();
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e) =>
            Process.Start("https://github.com/zdepav/drawing-playground");

    }

    public class AppState {

        [JsonRequired]
        public string? CurrentProject { get; set; }

    }

}
