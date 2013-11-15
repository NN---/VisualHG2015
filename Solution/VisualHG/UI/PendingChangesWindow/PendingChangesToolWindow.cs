using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HgLib;
using Microsoft.VisualStudio.Shell;

namespace VisualHg
{
    [Guid(Guids.HgPendingChangesToolWindowGuid)]
    public class PendingChangesToolWindow : ToolWindowPane
    {
        private PendingChangesControl pendingChangesControl;

        
        public override IWin32Window Window
        {
            get { return (IWin32Window)pendingChangesControl; }
        }


        public PendingChangesToolWindow() : base(null)
        {
            Caption = Resources.ResourceManager.GetString("100");
            pendingChangesControl = new PendingChangesControl();
        }


        public void SetFiles(HgFileInfo[] files)
        {
            pendingChangesControl.SetFiles(files);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && pendingChangesControl != null)
            {
                pendingChangesControl.Dispose();
                pendingChangesControl = null;
            }

            base.Dispose(disposing);
        }
    }
}