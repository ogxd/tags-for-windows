using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System.Linq;
using System.Drawing;
using TagsForWindows.Properties;

namespace TagsForWindows {

    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    public class ContextMenu : SharpContextMenu
    {
        private ContextMenuStrip menu = new ContextMenuStrip();

        protected override bool CanShowMenu() {
           
            if (SelectedItemPaths.Count() >= 1) {
                this.UpdateMenu();
                return true;
            }

            return false;
        }

        protected override ContextMenuStrip CreateMenu() {
            menu.Items.Clear();
            this.createSubMenus();
            return menu;
        }

        private void UpdateMenu() {
            menu.Dispose();
            menu = CreateMenu();
        }

        protected void createSubMenus() {

            var mainMenu = new ToolStripMenuItem();

            foreach (var tagAndLabel in Manager.GetTags(SelectedItemPaths.First()))
            {
                if (string.IsNullOrEmpty(mainMenu.Text))
                {
                    mainMenu.Image = GetBitmap(tagAndLabel.color.ToString());
                }
                else
                {
                    mainMenu.Text += ", ";
                }
                mainMenu.Text += tagAndLabel.label;
            }

            if (string.IsNullOrEmpty(mainMenu.Text)) {
                mainMenu.Text = "Tags...";
            }

            var menuGreen = new ToolStripMenuItem {
                Text = "Green",
                Image = GetBitmap("Green")
            };

            var menuYellow = new ToolStripMenuItem {
                Text = "Yellow",
                Image = GetBitmap("Yellow")
            };

            var menuRed = new ToolStripMenuItem {
                Text = "Red",
                Image = GetBitmap("Red")
            };

            var menuPurple = new ToolStripMenuItem {
                Text = "Purple",
                Image = GetBitmap("Purple")
            };

            var menuBlue = new ToolStripMenuItem {
                Text = "Blue",
                Image = GetBitmap("Blue")
            };

            var menuNone = new ToolStripMenuItem {
                Text = "None",
            };

            menuGreen.Click += (sender, args) => assignIcon(TagColor.Green);
            menuYellow.Click += (sender, args) => assignIcon(TagColor.Yellow);
            menuRed.Click += (sender, args) => assignIcon(TagColor.Red);
            menuPurple.Click += (sender, args) => assignIcon(TagColor.Purple);
            menuBlue.Click += (sender, args) => assignIcon(TagColor.Blue);
            menuNone.Click += (sender, args) => unassignIcon();

            mainMenu.DropDownItems.Add(menuGreen);
            mainMenu.DropDownItems.Add(menuYellow);
            mainMenu.DropDownItems.Add(menuRed);
            mainMenu.DropDownItems.Add(menuPurple);
            mainMenu.DropDownItems.Add(menuBlue);
            mainMenu.DropDownItems.Add(menuNone);

            menu.Items.Clear();
            menu.Items.Add(mainMenu);
        }

        private void assignIcon(TagColor tag) {
            foreach (string path in SelectedItemPaths) {
                Manager.AssignTag(path, tag, null);
            }
            Extensions.RefreshExplorer();
        }

        private void unassignIcon() {
            foreach (string path in SelectedItemPaths) {
                Manager.UnassignAllTags(path);
            }
            Extensions.RefreshExplorer();
        }

        public static Bitmap GetBitmap(string color) {
            if (Extensions.Dpi > 0.96f * 250 - 1) {
                return (Bitmap)Resources.ResourceManager.GetObject(color + "40", Resources.Culture);
            }
            if (Extensions.Dpi > 0.96f * 225 - 1) {
                return (Bitmap)Resources.ResourceManager.GetObject(color + "36", Resources.Culture);
            }
            if (Extensions.Dpi > 0.96f * 200 - 1) {
                return (Bitmap)Resources.ResourceManager.GetObject(color + "32", Resources.Culture);
            }
            if (Extensions.Dpi > 0.96f * 175 - 1) {
                return (Bitmap)Resources.ResourceManager.GetObject(color + "28", Resources.Culture);
            }
            if (Extensions.Dpi > 0.96f * 150 - 1) {
                return (Bitmap)Resources.ResourceManager.GetObject(color + "24", Resources.Culture);
            }
            if (Extensions.Dpi > 0.96f * 125 - 1) {
                return (Bitmap)Resources.ResourceManager.GetObject(color + "20", Resources.Culture);
            }
            return (Bitmap)Resources.ResourceManager.GetObject(color + "16", Resources.Culture);
        }
    }
}