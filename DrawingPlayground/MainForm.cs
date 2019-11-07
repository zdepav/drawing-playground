#nullable enable
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground {

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

            ShowCodeEditorForm();
            ShowLogForm();
            ShowErrorListForm();
            ShowCanvasForm();
            ShowHelpForm();

            LoadState();
        }

        private void ShowCodeEditorForm() {
            if (codeEditorForm == null) {
                codeEditorForm = new CodeEditorForm(jsRunner, this, log);
                codeEditorForm.Closed += (sender, e) => codeEditorForm = null;
            }
            if (codeEditorForm.IsHidden) {
                codeEditorForm.Show(dockPanel, DockState.Document);
            }
        }

        private void ShowErrorListForm() {
            if (errorListForm == null) {
                errorListForm = new ErrorListForm(this);
                errorListForm.Closed += (sender, e) => errorListForm = null;
            }
            if (errorListForm.IsHidden) {
                errorListForm.Show(dockPanel, DockState.DockBottom);
            }
        }

        private void ShowCanvasForm() {
            if (canvasForm == null) {
                canvasForm = new CanvasForm(jsRunner);
                canvasForm.Closed += (sender, e) => canvasForm = null;
            }
            if (canvasForm.IsHidden) {
                canvasForm.Show(dockPanel, DockState.DockRight);
            }
        }

        private void ShowLogForm() {
            if (logForm == null) {
                logForm = new LogForm();
                logForm.Closed += (sender, e) => log.Output = logForm = null;
                log.Output = logForm;
            }
            if (logForm.IsHidden) {
                logForm.Show(dockPanel, DockState.DockBottom);
            }
        }

        private void ShowHelpForm() {
            if (helpForm == null) {
                helpForm = new HelpForm();
                helpForm.Closed += (sender, e) => helpForm = null;
            }
            if (helpForm.IsHidden) {
                helpForm.Show(dockPanel, DockState.DockRightAutoHide);
            }
        }

        private void MainForm_Load(object sender, EventArgs e) { }

        private void codeEditorToolStripMenuItem_Click(object sender, EventArgs e) => ShowCodeEditorForm();

        private void canvasToolStripMenuItem_Click(object sender, EventArgs e) => ShowCanvasForm();

        private void errorListToolStripMenuItem_Click(object sender, EventArgs e) => ShowErrorListForm();

        private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e) => ShowLogForm();

        private void showHelpToolStripMenuItem_Click(object sender, EventArgs e) => ShowHelpForm();

        private void resetToolStripMenuItem_Click(object sender, EventArgs e) => jsRunner.Reset();

        private void newToolStripMenuItem_Click(object sender, EventArgs e) => NewProject();

        private void openToolStripMenuItem_Click(object sender, EventArgs e) => OpenProject();

        private void LoadState() {
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
            }
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

    }

    public class AppState {

        [JsonRequired]
        public string? CurrentProject { get; set; }

    }

}
