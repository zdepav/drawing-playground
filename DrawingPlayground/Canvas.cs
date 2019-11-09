#nullable enable
using System.Windows.Forms;
using SkiaSharp.Views.Desktop;

namespace DrawingPlayground {
    
    #if RENDER_BACKEND_SYSTEM_DRAWING
    public partial class Canvas : UserControl {
    #elif RENDER_BACKEND_SKIA
    public partial class Canvas : SKControl {
    #endif

        public Canvas() {
            InitializeComponent();
        }

    }

}
