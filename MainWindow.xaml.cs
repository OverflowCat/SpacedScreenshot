using System;
using System.Windows;
using System.Windows.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace SpacedScreenshot
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public int count = 0;
        public DispatcherTimer timer = new DispatcherTimer();
        private string filenamePrefix;
        private Bitmap bmpScreenshot;
        private Graphics gfxScreenshot;

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(TakeScreenshot);
        }

        private void TakeScreenshot(object sender, EventArgs e)
        {
            /* 
               Code copied from StackOverflow.
               https://stackoverflow.com/a/363008
               CC BY-SA 3.0
            */

            //Create a new bitmap.
            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height
                                           );

            // Create a graphics object from the bitmap.
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            string folderPath = path_tb.Text;
            if (!folderPath.EndsWith("\\")) folderPath += "\\";

            // If the folder does not exist, create it
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            bmpScreenshot.Save($"{folderPath}{filenamePrefix}-{count}-{DateTime.Now:yyyyMMdd-hhmmss}.png", System.Drawing.Imaging.ImageFormat.Png);
            count++;
            counter_label.Content = count.ToString();
            bmpScreenshot.Dispose();
            gfxScreenshot.Dispose();
        }

        private void Start_btn_Click(object sender, RoutedEventArgs e)
        {
            // 获取文本框中的时间间隔值
            int interval;
            try
            {
                interval = Int32.Parse(interval_tb.Text);
                Console.WriteLine(interval);
                filenamePrefix = filenamePrefix_tb.Text;
                timer.Interval = new TimeSpan(0, 0, interval);
                start_btn.IsEnabled = false;
                end_btn.IsEnabled = true;
                counter_label.Content = "0";
                count = 0;
                timer.Start();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("请输入正确的时间间隔！");
            }
        }

        private void End_btn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            end_btn.IsEnabled = false;
            start_btn.IsEnabled = true;
        }
    }
}