#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DrawingPlayground {

    public partial class NewProjectForm : Form {

        private readonly DirectoryInfo projectsDirectory;

        public string? ProjectName { get; private set; }

        public NewProjectForm(DirectoryInfo projectsDirectory) {
            this.projectsDirectory = projectsDirectory;
            ProjectName = null;
            InitializeComponent();
        }

        private void projectNameTextBox_TextChanged(object sender, EventArgs e) {
            var error = false;
            var name = projectNameTextBox.Text.Trim();
            if (name.Any(c => c != '_' &&
                              (c < 'a' || c > 'z') &&
                              (c < 'A' || c > 'Z') &&
                              (c < '0' || c > '9'))
            ) {
                errorLabel.Text = "Only letters, digits and underscores are allowed.";
                error = true;
            } else if (File.Exists(Path.Combine(projectsDirectory.FullName, name + ".js"))) {
                errorLabel.Text = "Project with this name already exists.";
                error = true;
            }
            okButton.Enabled = !error;
            ProjectName = error ? null : name;
        }

        private void okButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void NewProjectForm_Load(object sender, EventArgs e) {

        }

        private void errorLabel_Resize(object sender, EventArgs e) {
            var w = (Width - 52) / 2;
            okButton.Width = w;
            cancelButton.Width = w;
            cancelButton.Left = okButton.Left + okButton.Width + 12;
        }
    }

}
