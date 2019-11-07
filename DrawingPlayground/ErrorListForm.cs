#nullable enable
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Esprima;
using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground {

    internal partial class ErrorListForm : DockContent {

        private readonly MainForm mainForm;

        private readonly List<ParserException> errors;

        public ErrorListForm(MainForm mainForm) {
            this.mainForm = mainForm;
            errors = new List<ParserException>();
            InitializeComponent();
        }

        private void ErrorListForm_Load(object sender, EventArgs e) {
            errorList.ColumnCount = 3;
            errorList.Columns[0].Name = "Description";
            errorList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            errorList.Columns[1].Name = " Line ";
            errorList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            errorList.Columns[2].Name = "Column";
            errorList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void errorList_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0 && e.RowIndex < errors.Count) {
                mainForm.codeEditorForm?.NavigateTo(errors[e.RowIndex]);
            }
        }

        public void SetErrors(IEnumerable<ParserException> newErrors) {
            errors.Clear();
            errorList.Rows.Clear();
            foreach (var error in newErrors) {
                errors.Add(error);
                errorList.Rows.Add(error.Description, error.LineNumber, error.Column);
            }
        }

        protected override string GetPersistString() => "ErrorList";

    }

}
