using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;

namespace download_progress_changed
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetWindowTheme(progressBar1.Handle, string.Empty, string.Empty);
            progressBar1.ForeColor = Color.Aqua;
            progressBar1.BackColor = Color.Gray;
            var rnd1 = 10;
            var path =
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Application.ProductName,
                    "DB.Browser.for.SQLite-3.12.2-win32.msi"
                );
            Directory.CreateDirectory( Path.GetDirectoryName(path));

            using (WebClient webClient = new WebClient())
            {
                // Attach event BEFORE downloading to receive progress
                webClient.DownloadProgressChanged += (sender, e) =>
                {
                    progressBar1.Value = e.ProgressPercentage;
                    label1updateinf.Text = e.ProgressPercentage + "%";
                    Console.WriteLine(e.ProgressPercentage + "%");
                };
                try
                {
                    webClient.DownloadFileAsync(new Uri("https://download.sqlitebrowser.org/DB.Browser.for.SQLite-3.12.2-win32.msi"), path);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }
        [DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public extern static Int32 SetWindowTheme(IntPtr hWnd,
                        String textSubAppName, String textSubIdList);

    }
}