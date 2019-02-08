using System;
using System.Runtime.InteropServices;
using SharpShell.Attributes;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

namespace LabelsForWindows {

    [ComVisible(true)]
    [RegistrationName("  LabelsForWindows_Green")]
    [Guid("a259c04f-ffa8-310b-864c-fe602840399a")]
    public class OverlayIconGreen : SharpIconOverlayHandler {

        protected override int GetPriority() {
            return 90;
        }

        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes) {
            if (Manager.FilesToIcons.ContainsKey(path)) {
                return Manager.FilesToIcons[path] == Properties.Resources.green_ico;
            }
            return false;
        }

        protected override System.Drawing.Icon GetOverlayIcon() {
            return Properties.Resources.green_ico;
        }
    }
}
