using System;
using System.Runtime.InteropServices;
using SharpShell.Attributes;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;
using TagsForWindows.Properties;

namespace TagsForWindows {

    [ComVisible(true)]
    [RegistrationName("  TagsForWindows_Blue")]
    [Guid("a259c04f-ffa8-310b-864c-fe602840399e")]
    public class OverlayIconBlue : SharpIconOverlayHandler {

        protected override int GetPriority() {
            return 90;
        }

        private string _lastPath;

        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes) {
            return Database.GetTag(path) == "Blue";
            //Debug.Log($"CanShowOverlay path:{path}");
            //return !string.IsNullOrEmpty(Database.GetTag(_lastPath = path));
        }

        protected override System.Drawing.Icon GetOverlayIcon() {
            return Properties.Resources.Blue;
            //Debug.Log($"GetOverlayIcon lastPath:{_lastPath}");
            //return (System.Drawing.Icon)Resources.ResourceManager.GetObject(_lastPath, Resources.Culture);
        }
    }
}
