using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground {

    public partial class HelpForm : DockContent {

        public HelpForm() {
            InitializeComponent();
        }

        protected override string GetPersistString() => "Help";

    }

}
