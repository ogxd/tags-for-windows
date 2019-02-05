using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ogxd {

    [ComVisible(true)]
    [Guid("f694f487-881a-449e-9120-a49671f3d99e")]
    public class LabelsForWindows : SharpIconOverlayHandler {

        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes) {
            return path.Contains(".pdf");
            //return Path.GetFileName(path).ToLower().Contains("a");
        }

        protected override System.Drawing.Icon GetOverlayIcon() {
            //  Return the read only icon.
            return Properties.Resources.Green;
        }

        protected override int GetPriority() {
            //  The read only icon overlay is very low priority.
            return 90;
        }
    }
}
