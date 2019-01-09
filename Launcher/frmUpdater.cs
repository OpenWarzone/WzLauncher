using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Newtonsoft.Json;
using FolderSelect;
using File = System.IO.File;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Intersect_Updater
{
    public partial class frmUpdater : Form
    {
        public Settings settings;
        private static readonly string[] Scopes = new[] { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
        private static ConcurrentQueue<Update> UpdateList = new ConcurrentQueue<Update>();
        private static long TotalUpdateSize = 0;
        private static long DownloadedBytes = 0;
        private static long FilesToDownload = 0;
        private static long FilesDownloaded = 0;
        private static long SpeedDownloadedBytes = 0;
        private static DateTime SpeedStartTime = DateTime.Now;
        //private static bool CheckingForUpdates = true;
        private static int DotCount = 0;
        private static Thread[] UpdateThreads = new Thread[4/*10*/];
        private TransparentLabel lbl;
        private TransparentLabel lblPercent;
        private TransparentLabel lblCloseButton;
        private TransparentLabel lblMinimizeButton;

        /*
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }

        internal enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        internal void EnableBlur()
        {
            var accent = new AccentPolicy();
            var accentStructSize = Marshal.SizeOf(accent);
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(this.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);

            //SetWindowCompositionAttribute(this.Handle, )
        }

        public static void EnableBlur(IntPtr HWnd, bool hasFrame = true)
        {

            AccentPolicy accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
            if (hasFrame)
                accent.AccentFlags = 0x20 | 0x40 | 0x80 | 0x100;

            int accentStructSize = Marshal.SizeOf(accent);

            IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            WindowCompositionAttributeData data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(HWnd, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }*/

        public frmUpdater()
        {
            InitializeComponent();

            //EnableBlur();
            //this.BackColor = Color.FromArgb(128, 0, 0, 0);
            //EnableBlur(this.Handle, false);

            lbl = new TransparentLabel(lblStatus);
            lbl.ForeColor = Color.White;

            lblPercent = new TransparentLabel(percentLabel);
            lblPercent.ForeColor = Color.LightBlue;

            lblCloseButton = new TransparentLabel(CloseButton);
            lblCloseButton.Click += CloseButton_Click;
            lblMinimizeButton = new TransparentLabel(MinimizeButton);
            lblMinimizeButton.Click += MinimizeButton_Click;

            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void frmUpdater_Load(object sender, EventArgs e)
        {
            //Try to load up icon
            var icon = Icon.ExtractAssociatedIcon(EntryAssemblyInfo.ExecutablePath);
            if (icon != null) this.Icon = icon;
            
            var settingsToTry = new List<string>();
            settingsToTry.Add("settings.json");
            var loadedSettings = false;
            foreach (var directory in Directory.GetDirectories(Directory.GetCurrentDirectory()))
            {
                if (File.Exists(Path.Combine(directory, "settings.json")))
                {
                    settingsToTry.Add(Path.Combine(directory, "settings.json"));
                }
            }
            foreach (var settingsFile in settingsToTry)
            {
                try
                {
                    settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFile));
                    loadedSettings = true;
                    break;
                }
                catch (Exception ex)
                {
                    
                }
            }

            if (!loadedSettings)
            {
                MessageBox.Show(@"Could not find settings.json in updater folder or subdirectories! Closing now!");
                this.Close();
                return;
            }
            if (loadedSettings && string.IsNullOrEmpty(settings.FolderId) || string.IsNullOrEmpty(settings.ApiKey))
            {
                MessageBox.Show(@"Missing folder id or api key in settings. Updater will now close!");
                this.Close();
                return;
            }
            if (!string.IsNullOrEmpty(settings.Background))
            {
                if (File.Exists(settings.Background))
                {
                    var launcherImage = File.ReadAllBytes(settings.Background);
                    try
                    {
                        using (var ms = new MemoryStream(launcherImage))
                        {
                            picBackground.BackgroundImage = Bitmap.FromStream(ms);

                            picBackground.Controls.Add(icon_discord);
                            icon_discord.BackColor = Color.Transparent;

                            picBackground.Controls.Add(icon_jkhub);
                            icon_jkhub.BackColor = Color.Transparent;

                            picBackground.Controls.Add(icon_moddb);
                            icon_moddb.BackColor = Color.Transparent;
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
            this.Text = settings.UpdaterTitle;
            Task.Run(Run);
        }

        private async Task Run()
        {
            bool updatecheck = true;

            while (updatecheck)
            {
                lbl.MeasureString = @"Checking for updates, please wait" + new string('.', 3);
                ConcurrentQueueExtensions.Clear(UpdateList);

                Google.Apis.Services.BaseClientService.Initializer bcs = new Google.Apis.Services.BaseClientService.Initializer();
                bcs.ApiKey = settings.ApiKey;
                bcs.ApplicationName = "Warzone Updater";
                bcs.GZipEnabled = true;

                Google.Apis.Drive.v3.DriveService service = new Google.Apis.Drive.v3.DriveService(bcs);
                await CheckFilesRecursively(service, "", settings.FolderId);

                lbl.MeasureString = null;

                var updating = UpdateList.Count > 0;
                FilesToDownload = UpdateList.Count;
                //CheckingForUpdates = false;
                if (UpdateList.Count > 0)
                {
                    List<Update> updates = new List<Update>();
                    updates.AddRange(UpdateList);

                    UpdateList = new ConcurrentQueue<Update>();
                    var updatePaths = new HashSet<string>();
                    for (int i = 0; i < updates.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(settings.Background))
                        {
                            var backgroundpath = Path.GetFullPath(settings.Background);
                            var updatePath = Path.GetFullPath(updates[i].FilePath);
                            if (backgroundpath == updatePath)
                            {
                                var update = updates[i];
                                updates.Remove(updates[i]);
                                updatePaths.Add(update.FilePath);
                                UpdateList.Enqueue(update);
                                break;
                            }
                        }
                    }


                    var updatesToRemove = new List<Update>();
                    foreach (var update in updates)
                    {
                        if (!updatePaths.Contains(update.FilePath))
                        {
                            updatePaths.Add(update.FilePath);
                        }
                        else
                        {
                            updatesToRemove.Add(update);
                        }
                    }

                    foreach (var update in updatesToRemove)
                    {
                        updates.Remove(update);
                    }

                    foreach (var update in updates)
                    {
                        UpdateList.Enqueue(update);
                    }

                    SpeedStartTime = DateTime.Now;

                    BeginInvoke((Action)(() => UpdateStatus()));
                    for (int i = 0; i < UpdateThreads.Length; i++)
                    {
                        UpdateThreads[i] = new Thread(DownloadFiles);
                        UpdateThreads[i].Start();
                    }
                }

                var threadsRunning = true;
                while (threadsRunning && updating)
                {
                    threadsRunning = false;
                    foreach (var thread in UpdateThreads)
                    {
                        if ((thread.IsAlive || thread.ThreadState == System.Threading.ThreadState.Running) && thread.ThreadState != System.Threading.ThreadState.Stopped) threadsRunning = true;
                        break;
                    }
                    Application.DoEvents();

                    // UQ1: Sleep for a moment to not max out CPU core...
                    System.Threading.Thread.Sleep(1);
                }

                lblPercent.Visible = false;
                lblPercent.CreateGraphics().Clear(Color.Transparent);
                lblPercent.Text = "";

                progressBar1.Value = 100;

                System.Threading.Thread.Sleep(1000);

                if (updating)
                {
                    //BeginInvoke((Action)(() => lbl.Text = @"Updates complete. Launching."));
                    updatecheck = true;
                }
                else
                {
                    BeginInvoke((Action)(() => lbl.Text = @"Game is up to date. Launching."));
                    updatecheck = false;
                }
            }

            // Make sure we have a link to JKA base folder...
            string LauncherPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString();
            string GameDataPath = LauncherPath + @"/gameData";
            if (!MakeBaseDirLink(GameDataPath))
            {// Failed to make a symbolic link.
                await Wait();
                BeginInvoke((Action)(() => Close()));
            }
            else
            {// Launch Game
                await Wait();
                string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString();
                var path = Path.Combine(AssemblyPath, settings.LaunchApplication);
                var workingDir = Path.GetDirectoryName(path);
                var psi = new ProcessStartInfo(path);
                psi.WorkingDirectory = workingDir;
                psi.Arguments = "+set r_renderer rd-warzone +set fs_game warzone +set g_gametype 11 +set logfile 2";
                Process.Start(psi);
                BeginInvoke((Action)(() => Close()));
            }
        }

        /*[System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }*/

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);
        static int SYMBOLIC_LINK_FLAG_DIRECTORY = 0x1;
        static int SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE = 0x2;
        static int symlinkFlags = 0x0;

        private bool MakeBaseDirLink(string gameDataFolder)
        {
            if (!Directory.Exists(gameDataFolder + @"/base"))
            {
                // Default "old skool" default JKA base directory...
                string symLink = @"C:/Program Files (x86)/LucasArts/Star Wars Jedi Knight Jedi Academy/GameData/base";
                // Default "steam" default JKA base directory...
                string steamSymbolicLink = @"C:/Program Files (x86)/Steam/steamapps/common/Jedi Academy/GameData/base";
                // Default "old skool" default JKA base directory...
                string DsymbolicLink = @"D:/Program Files (x86)/LucasArts/Star Wars Jedi Knight Jedi Academy/GameData/base";
                // Default "steam" default JKA base directory...
                string DsteamSymbolicLink = @"D:/Program Files (x86)/Steam/steamapps/common/Jedi Academy/GameData/base";
                // Our downloaded warzone gameData folder...
                string fileName = gameDataFolder + @"/base";

                if (!Directory.Exists(symLink))
                {// If old skool directory does not exist, check steam's default directory, so we can skip the dialog where possible...
                    if (Directory.Exists(steamSymbolicLink))
                    {// Exists in steam, awesome! Use it...
                        symLink = steamSymbolicLink;
                    }
                }

                if (!Directory.Exists(symLink))
                {// Try old skool but on D: drive...
                    if (Directory.Exists(DsymbolicLink))
                    {// Exists on D: drive, awesome! Use it...
                        symLink = DsymbolicLink;
                    }
                }

                if (!Directory.Exists(symLink))
                {// Try steam but on D: drive...
                    if (Directory.Exists(DsteamSymbolicLink))
                    {// Exists in steam, awesome! Use it...
                        symLink = DsteamSymbolicLink;
                    }
                }

                string steamDir = string.Empty;

                using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam"))
                {
                    if (registryKey != null)
                    {
                        steamDir = registryKey.GetValue("InstallPath").ToString();
                        registryKey.Close();

                        string fullDir = steamDir + @"/steamapps/common/Jedi Academy/GameData/base";

                        if (Directory.Exists(fullDir))
                        {
                            symLink = fullDir;
                        }
                    }
                }

                while (true)
                {
                    bool failed = false;

                    if (!Directory.Exists(symLink))
                    {// We didn't see JKA in any default places, ask the player via dialog to select their base dir...
                        var folderDialog = new FolderSelectDialog();
                        folderDialog.Title = @"Please select your Jedi Academy 'base' or 'gamedata' folder.";

                        if (folderDialog.ShowDialog())
                        {
                            if (folderDialog.FileName.EndsWith("base", StringComparison.CurrentCultureIgnoreCase))
                                symLink = folderDialog.FileName;
                            else if (folderDialog.FileName.EndsWith("gamedata", StringComparison.CurrentCultureIgnoreCase))
                            {
                                string finalDir = folderDialog.FileName + @"/base";

                                if (Directory.Exists(finalDir))
                                    symLink = finalDir;
                                else
                                    failed = true;
                            }
                            else
                                failed = true;
                        }
                        else
                        {
                            string dialogMessageBoxText = "The installer was unable to create a dialog to select your Jedi Academy 'base' folder.\n\nPlease copy your Jedi Academy 'base' folder into the GameData folder.";
                            string dialogCaption = "We had a small issue.";
                            MessageBox.Show(dialogMessageBoxText, dialogCaption);
                            return false;
                        }
                    }

                    if (!failed)
                        break;

                    string messageBoxText = "Your selected folder does not look correct.\n\nPlease try again to select your Jedi Academy 'base' or 'gamedata' folder.";
                    string caption = "ERROR!";
                    MessageBox.Show(messageBoxText, caption);
                }

                symlinkFlags = SYMBOLIC_LINK_FLAG_DIRECTORY | SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE;
                CreateSymbolicLink(fileName, symLink, symlinkFlags/*SymbolicLink.Directory*/);
            }

            return true;
        }

        static readonly string[] BytesPerSecondBinaryPrefix = { "b/s", "KB/s", "MB/s", "GB/s", "TB/s" }; // , "PB/s", "EB/s", "ZB/s", "YB/s"
        string GetBytesPerSecondString(double bytes)
        {
            int counter = 0;
            double value = bytes;
            string text = "";
            do
            {
                text = value.ToString("0.0") + " " + BytesPerSecondBinaryPrefix[counter];
                value /= 1024;
                counter++;
            }
            while (Math.Floor(value) > 0 && counter < BytesPerSecondBinaryPrefix.Length);
            return text;
        }

        static readonly string[] BytesBinaryPrefix = { "bytes", "KB", "MB" };//, "GB", "TB" }; // , "PB", "EB", "ZB", "YB"
        string GetBytesString(double bytes)
        {
            int counter = 0;
            double value = bytes;
            string text = "";
            do
            {
                text = value.ToString("0.0") + " " + BytesBinaryPrefix[counter];
                value /= 1024;
                counter++;
            }
            while (Math.Floor(value) > 0 && counter < BytesBinaryPrefix.Length);
            return text;
        }

        string GetBytesCompletedString(double currentbytes, double totalbytes)
        {
            int counter = 0;
            string text = "";
            string text2 = "";
            do
            {
                text = totalbytes.ToString("0.0") + " " + BytesBinaryPrefix[counter];
                text2 = currentbytes.ToString("0.0");
                totalbytes /= 1024;
                currentbytes /= 1024;
                counter++;
            }
            while (Math.Floor(totalbytes) > 0 && counter < BytesBinaryPrefix.Length);
            while (Math.Floor(currentbytes) > 0 && counter < BytesBinaryPrefix.Length) ;
            return text2 + "/" + text;
        }

        private void UpdateStatus()
        {
            double currentSpeed = 0;
            var timer = DateTime.Now - SpeedStartTime;

            currentSpeed += (double)SpeedDownloadedBytes / (timer.TotalSeconds);
            SpeedStartTime = DateTime.Now;
            SpeedDownloadedBytes = 0;

            //var percentage = ((float) FilesDownloaded / (float) FilesToDownload) * 100f; // based on just number of updates...
            double percentage = ((double)DownloadedBytes / (double)TotalUpdateSize) * 100.0f; // based on updates bytes sizes...

            if (currentSpeed <= 0 || double.IsInfinity(currentSpeed) || double.IsNaN(currentSpeed))
            {
                lbl.Text = "Downloading " + (int)(FilesToDownload - FilesDownloaded) + " updates..." + GetBytesCompletedString(DownloadedBytes, TotalUpdateSize);
            }
            else
            {
                lbl.Text = "Downloading " + (int)(FilesToDownload - FilesDownloaded) + " updates... " + GetBytesCompletedString(DownloadedBytes, TotalUpdateSize) + " at " + GetBytesPerSecondString(currentSpeed);
            }

            progressBar1.Visible = true;
            progressBar1.Value = (int)percentage;

            lblPercent.Visible = true;
            lblPercent.CreateGraphics().Clear(Color.Transparent);
            lblPercent.Text = (int)percentage + "% complete";

            if (currentSpeed <= 0 || double.IsInfinity(currentSpeed) || double.IsNaN(currentSpeed))
            {
                // FFS, 64 character limit... lol!
                //notifyIcon.Text = "Star Wars: Warzone - Updating - " + GetBytesCompletedString(DownloadedBytes, TotalUpdateSize) + " at " + GetBytesPerSecondString(0) + " " + lblPercent.Text;
                notifyIcon.Text = "Star Wars: Warzone - Updating - " + lblPercent.Text + " - " + GetBytesPerSecondString(0);
            }
            else
            {
                // FFS, 64 character limit... lol!
                //notifyIcon.Text = "Star Wars: Warzone - Updating - " + GetBytesCompletedString(DownloadedBytes, TotalUpdateSize) + " at " + GetBytesPerSecondString(currentSpeed) + " " + lblPercent.Text;
                notifyIcon.Text = "Star Wars: Warzone - Updating - " + lblPercent.Text + " - " + GetBytesPerSecondString(currentSpeed);
            }
        }

        private async Task Wait()
        {
            System.Threading.Thread.Sleep(5000);
        }

        private void DownloadFiles()
        {
            var backoff = 1000;
            Google.Apis.Services.BaseClientService.Initializer bcs = new Google.Apis.Services.BaseClientService.Initializer();
            bcs.ApiKey = settings.ApiKey;
            bcs.ApplicationName = "Warzone Updater";
            bcs.GZipEnabled = true;

            Google.Apis.Drive.v3.DriveService service = new Google.Apis.Drive.v3.DriveService(bcs);
            while (!UpdateList.IsEmpty)
            {
                Update update;

                if (UpdateList.TryDequeue(out update))
                {
                    if (DownloadUpdate(service, update))
                    {
                        
                    }
                    else
                    {
                        //Back off
                        UpdateList.Enqueue(update);
                        //backoff = backoff * 2;
                        System.Threading.Thread.Sleep(backoff);
                    }
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        private bool DownloadUpdate(DriveService service,Update update)
        {
            var request = service.Files.Get(update.UpdateFile.Id);
            var updatePath = Path.GetFullPath(update.FilePath);
            if (updatePath == Path.GetFullPath(EntryAssemblyInfo.ExecutablePath))
            {
                return true; //Don't try to update self -- it won't work!
            }
            if (File.Exists(update.FilePath)) File.Delete(update.FilePath);
            using (var stream = new FileStream(update.FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {

                // Add a handler which will be notified on progress changes.
                // It will notify on each chunk download and when the
                // download is completed or failed.
                long lastProgress = 0;
                var succeeded = true;
                request.MediaDownloader.ProgressChanged +=
                    (IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                            {
                                var diff = progress.BytesDownloaded - lastProgress;

                                SpeedDownloadedBytes += diff;

                                DownloadedBytes += diff;
                                lastProgress = progress.BytesDownloaded;
                                break;
                            }
                            case DownloadStatus.Completed:
                            {
                                var diff = progress.BytesDownloaded - lastProgress;

                                SpeedDownloadedBytes += diff;

                                DownloadedBytes += diff;
                                lastProgress = progress.BytesDownloaded;
                                if (!string.IsNullOrEmpty(settings.Background))
                                {
                                    var backgroundpath = Path.GetFullPath(settings.Background);

                                    if (backgroundpath == updatePath)
                                    {
                                        picBackground.BackgroundImage = Bitmap.FromStream(stream);

                                        picBackground.Controls.Add(icon_discord);
                                        icon_discord.BackColor = Color.Transparent;

                                        picBackground.Controls.Add(icon_jkhub);
                                        icon_jkhub.BackColor = Color.Transparent;

                                        picBackground.Controls.Add(icon_moddb);
                                        icon_moddb.BackColor = Color.Transparent;
                                    }
                                }
                                FilesDownloaded++;
                                break;
                            }
                            case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                succeeded = false;
                                break;
                            }
                        }
                        BeginInvoke((Action) (() => UpdateStatus()));
                    };
                request.Download(stream);
                System.Threading.Thread.Sleep(1000);
                return succeeded;
            }
        }

        private async Task CheckFilesRecursively(DriveService service, string currentFolder, string folderId, string nextPageToken = null)
        {
            // Define parameters of request.
            Google.Apis.Drive.v3.FilesResource.ListRequest listRequest = service.Files.List();
            string folderID = folderId; //Change this for your folder ID.
            listRequest.Q = "'" + folderID + "' in parents";
            listRequest.PageSize = 100;
            if (!string.IsNullOrEmpty(nextPageToken)) listRequest.PageToken = nextPageToken;
            listRequest.Fields = "nextPageToken, files(id, name, md5Checksum, size, mimeType)";

            //Start at root, look for all files that we don't have or files that need updating and grab them!
            // List files.
            FileList fileList = listRequest.Execute();
            IList<Google.Apis.Drive.v3.Data.File> files = fileList.Files;

            if (files != null && files.Count > 0)
            {
                DotCount++;
                if (DotCount > 3) DotCount = 0;

                if (currentFolder.Length > 0)
                    UpdateStatus(@"Checking for updates, please wait " + @"(" + currentFolder + @")", DotCount);
                else
                    UpdateStatus(@"Checking for updates, please wait", DotCount);

                foreach (var file in files)
                {
                    if (IsFolder(file.MimeType))
                    {
                        if (file.Name.Equals(".git"))
                            continue; // Skip .git dir(s)...

                        if (file.Name.Equals("html"))
                            continue; // Skip html dir(s)...

                        if (file.Name.Equals("Warzone-Nvidia-Profiles"))
                                continue; // Skip these dir(s)...

                        if (!Directory.Exists(Path.Combine(currentFolder, file.Name)))
                            Directory.CreateDirectory(Path.Combine(currentFolder, file.Name));
                        await CheckFilesRecursively(service, Path.Combine(currentFolder, file.Name), file.Id, null);
                    }
                    else if (IsFile(file.MimeType))
                    {
                        if (file.Name.StartsWith(".git"))
                            continue; // Skip .git files...

                        if (file.Name.Equals("INSTALL-HOWTO.txt"))
                            continue; // Skip old howto readme...

                        if (!File.Exists(Path.Combine(currentFolder, file.Name)))
                        {
                            //Queue File Up
                            UpdateList.Enqueue(new Update(Path.Combine(currentFolder,file.Name),file));
                            TotalUpdateSize += file.Size ?? 0;
                        }
                        else
                        {
                            //if file size or md5 doesn't match, queue file up
                            long length = new System.IO.FileInfo(Path.Combine(currentFolder, file.Name)).Length;
                            string md5Hash = "";
                            using (var md5 = MD5.Create())
                            {
                                using (var stream =
                                    new BufferedStream(File.OpenRead(Path.Combine(currentFolder, file.Name)), 1200000))
                                {
                                    md5Hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                                }
                            }
                            if (md5Hash != file.Md5Checksum || length != file.Size)
                            {
                                UpdateList.Enqueue(new Update(Path.Combine(currentFolder, file.Name), file));
                                TotalUpdateSize += file.Size ?? 0;
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(fileList.NextPageToken))
                CheckFilesRecursively(service, currentFolder, folderID, fileList.NextPageToken);
        }

        private bool IsFolder(string mimeType)
        {
            if (mimeType == "application/vnd.google-apps.folder") return true;
            return false;
        }

        private bool IsFile(string mimeType)
        {
            if (!mimeType.Contains("vnd.google-apps")) return true;
            return false;
        }

        private void lbl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Transparent);
        }

        private void percentLabel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Transparent);
        }

        private void picBackground_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(175, 0, 0, 0)), 0, lblStatus.Top, this.Width, /*lblStatus.Height*/this.Height);
        }

        private void UpdateStatus(string text, int DotCount)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, int>(UpdateStatus), new object[] { text, DotCount });
                return;
            }
            
            lblStatus.Text = text + new string('.', DotCount);
            lbl.Text = text + new string('.', DotCount);
            lbl.MeasureString = text + new string('.', 3);

            notifyIcon.Text = "Star Wars: Warzone - Checking for updates...";
        }

        private void tmrChecking_Tick(object sender, EventArgs e)
        {
            /*if (CheckingForUpdates)
            {
                DotCount++;
                if (DotCount > 3) DotCount = 0;
                UpdateStatus(@"Checking for updates, please wait" + new string('.', DotCount));
            }*/
        }

        //
        // Moddb, jkhub, discord icons...
        //

        private void pictureBox1_Click(object sender, EventArgs e)
        {// Discord
            System.Diagnostics.Process.Start("https://discord.gg/zQ4CB9S");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {// JKHub
            System.Diagnostics.Process.Start("https://jkhub.org/forum/134-star-wars-warzone/");
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {// Moddb
            System.Diagnostics.Process.Start("https://www.moddb.com/mods/star-wars-warzone");
        }

        //
        // Window close control...
        //

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();

            try
            {
                foreach (Process proc in Process.GetProcessesByName("Star Wars: Warzone - Launcher"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //
        // System tray and minimize button support...
        //

        private void Form1_Resize(object sender, EventArgs e)
        {
            //if the form is minimized  
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            Hide();
            this.WindowState = FormWindowState.Minimized;
            notifyIcon.Visible = true;

            var timer = DateTime.Now - SpeedStartTime;

            if (timer.TotalSeconds > 0)
            {
                UpdateStatus();
            }
            else
            {
                notifyIcon.Text = "Star Wars: Warzone - Checking for updates...";
            }
        }

        //
        // Borderless window moving controls...
        //

        int     mouseX = 0, mouseY = 0;
        bool    mouseDown = false;

        private void WindowMovePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseX = MousePosition.X - (WindowMovePanel.Size.Width / 2);
                mouseY = MousePosition.Y - (WindowMovePanel.Size.Height / 2);

                this.SetDesktopLocation(mouseX, mouseY);
            }
        }

        private void WindowMovePanel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void WindowMovePanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
        }
    }
}

internal static class ConcurrentQueueExtensions
{
    public static void Clear<T>(this ConcurrentQueue<T> queue)
    {
        T item;
        while (queue.TryDequeue(out item))
        {
            // do nothing
            System.Threading.Thread.Sleep(10);
        }
    }
}
