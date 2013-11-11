﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using HgLib;

namespace VisualHg
{
    partial class SccProvider
    {
        // ------------------------------------------------------------------------
        // show an wait for exit of required dialog
        // update state for given files
        // ------------------------------------------------------------------------
        void QueueDialog(string[] files, string command)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try {
                HgLib.TortoiseHg.ShowSelectedFilesWindow(files, command);
                sccService.StatusTracker.CacheUpdateRequired=false;
                sccService.StatusTracker.Enqueue(new HgLib.UpdateFileStatusHgCommand(files));
                }catch{}
            });
        }

        // ------------------------------------------------------------------------
        // commit selected files dialog
        // ------------------------------------------------------------------------
        public void CommitDialog(string[] files)
        {
            QueueDialog(files, " --nofork commit ");
        }

        // ------------------------------------------------------------------------
        // add files to repo dialog
        // ------------------------------------------------------------------------
        void AddFilesDialog(string[] files)
        {
            QueueDialog(files, " --nofork add ");
        }

        // ------------------------------------------------------------------------
        // show an wait for exit of required dialog
        // update state for given files
        // ------------------------------------------------------------------------
        void QueueDialog(string root, string command)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try{
                    Process process = HgLib.TortoiseHg.Start(command, root);
                if (process != null)
                    process.WaitForExit();

                sccService.StatusTracker.CacheUpdateRequired = false;
                sccService.StatusTracker.Enqueue(new HgLib.UpdateRootStatusHgCommand(root));
                }catch{}
            });
        }

        // ------------------------------------------------------------------------
        // show TortoiseHg commit dialog
        // ------------------------------------------------------------------------
        void CommitDialog(string directory)
        {
            QueueDialog(directory, "commit");
        }

        // ------------------------------------------------------------------------
        // show TortoiseHg revert dialog
        // ------------------------------------------------------------------------
        void RevertDialog(string[] files)
        {
            QueueDialog(files, " --nofork revert ");
        }

        // ------------------------------------------------------------------------
        // show TortoiseHg repo browser dialog
        // ------------------------------------------------------------------------
        public void RepoBrowserDialog(string root)
        {
            QueueDialog(root, "log");
        }

        // ------------------------------------------------------------------------
        // show TortoiseHg file log dialog
        // ------------------------------------------------------------------------
        public void LogDialog(string file)
        {
            String root = HgProvider.FindRepositoryRoot(file);
            if (root != string.Empty)
            {
                file = file.Substring(root.Length + 1);
                QueueDialog(root, "log \"" + file + "\"");
            }
        }

        // ------------------------------------------------------------------------
        // show file diff window
        // ------------------------------------------------------------------------
        void DiffDialog(string sccFile, string file, string commandMask)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try{
                Process process= HgLib.TortoiseHg.DiffDialog(sccFile, file, commandMask);
                if (process != null)
                    process.WaitForExit();

                sccService.StatusTracker.CacheUpdateRequired = false;
                sccService.StatusTracker.Enqueue(new HgLib.UpdateFileStatusHgCommand(new string[]{file}));
                }catch{}
            });
        }


        // ------------------------------------------------------------------------
        // show TortoiseHg synchronize dialog
        // ------------------------------------------------------------------------
        public void SyncDialog(string directory)
        {
            QueueDialog(directory, "synch");
        }

        // ------------------------------------------------------------------------
        // show TortoiseHg status dialog
        // ------------------------------------------------------------------------
        public void StatusDialog(string directory)
        {
            QueueDialog(directory, "status");
        }
    }
}
