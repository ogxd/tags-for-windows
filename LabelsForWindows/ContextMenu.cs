using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System.Linq;
using System.Drawing;

namespace LabelsForWindows {

    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    public class ContextMenu : SharpContextMenu
    {
        private ContextMenuStrip menu = new ContextMenuStrip();

        protected override bool CanShowMenu() {
           
            if (SelectedItemPaths.Count() == 1)
            {
                this.UpdateMenu();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override ContextMenuStrip CreateMenu() {

            menu.Items.Clear();
            FileAttributes attr = File.GetAttributes(SelectedItemPaths.First());

            if (attr.HasFlag(FileAttributes.Directory))  {
                this.createSubMenus();
            } else {
                this.createSubMenus();
            }

            return menu;
        }

        private void UpdateMenu() {
            menu.Dispose();
            menu = CreateMenu();
        }

        protected void createSubMenus() {

            var mainMenu = new ToolStripMenuItem {
                Text = "Labels",
            };

            var menuGreen = new ToolStripMenuItem {
                Text = "Green",
                Image = Properties.Resources.green32x32
            };

            var menuYellow = new ToolStripMenuItem {
                Text = "Yellow",
                Image = Properties.Resources.yellow32x32
            };

            var menuRed = new ToolStripMenuItem
            {
                Text = "Red",
                Image = Properties.Resources.red32x32
            };

            var menuPurple = new ToolStripMenuItem
            {
                Text = "Purple",
                Image = Properties.Resources.purple32x32
            };

            var menuBlue = new ToolStripMenuItem
            {
                Text = "Blue",
                Image = Properties.Resources.blue32x32
            };

            var menuNone = new ToolStripMenuItem {
                Text = "None",
            };

            menuGreen.Click += (sender, args) => assignIcon(Properties.Resources.green_ico);
            menuYellow.Click += (sender, args) => assignIcon(Properties.Resources.yellow_ico);
            menuRed.Click += (sender, args) => assignIcon(Properties.Resources.red_ico);
            menuPurple.Click += (sender, args) => assignIcon(Properties.Resources.purple_ico);
            menuBlue.Click += (sender, args) => assignIcon(Properties.Resources.blue_ico);
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

        private void assignIcon(Icon icon) {
            foreach (string path in SelectedItemPaths) {
                if (Manager.FilesToIcons.ContainsKey(path)) {
                    Manager.FilesToIcons[path] = icon;
                } else {
                    Manager.FilesToIcons.Add(path, icon);
                }
            }
        }

        private void unassignIcon() {
            foreach (string path in SelectedItemPaths) {
                if (Manager.FilesToIcons.ContainsKey(path)) {
                    Manager.FilesToIcons.Remove(path);
                }
            }
        }
    }
}