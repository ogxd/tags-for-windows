using System;
using System.Runtime.InteropServices;
using SharpShell.Attributes;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

namespace TagsForWindows {

    [ComVisible(true)]
    [RegistrationName("  TagsForWindows_Blue")]
    [Guid("a259c04f-ffa8-310b-864c-fe602840399e")]
    public class OverlayIconBlue : SharpIconOverlayHandler {

        protected override int GetPriority() {
            return 90;
        }

        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes) {
            return Manager.GetTag(path).color == TagColor.Blue;
        }

        protected override System.Drawing.Icon GetOverlayIcon() {
            return Properties.Resources.Blue;
        }
    }
}
