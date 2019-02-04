using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LabelsForWindows
{
    public class LabelsForWindows : SharpIconOverlayHandler {

        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes) {
            return true;
            //try {
            //    //  Get the file attributes.
            //    var fileAttributes = new FileInfo(path);

            //    //  Return true if the file is read only, meaning we'll show the overlay.
            //    return fileAttributes.IsReadOnly;
            //}
            //catch (Exception) {
            //    return false;
            //}
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
