using AForge.Video.DirectShow;
using AForge.Video;
using AForge.Imaging;
using FireSharp;
using FireSharp.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System;
using System.Windows.Media.Media3D;
using System.Diagnostics.Tracing;
namespace WPF_MachineSevice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FilterInfoCollection? videoDevices;
        private VideoCaptureDevice[]? videoSources;
        private VideoCaptureDevice selectedVideoSource;
        private int selectedCameraIndex = 0;
        private int captureCount = 1;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MachineWindow_Loaded;
            Closing += MachineWindow_Closing;
        }
        /// <summary>
        /// Load Machine Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MachineWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSources = new VideoCaptureDevice[videoDevices.Count];
                for (int i = 0; i < videoDevices.Count; i++)
                {
                    videoSources[i] = new VideoCaptureDevice(videoDevices[i].MonikerString);
                    videoSources[i].NewFrame += VideoSource_BitMapFrame;
                    videoSources[i].VideoResolution = videoSources[i].VideoCapabilities.Last();
                    videoSources[i].Start();
                }
                selectedVideoSource = videoSources[0];
                selectedVideoSource.NewFrame += VideoSource_BitMapFrame;
            }
        }
        /// <summary>
        /// Close Machine Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MachineWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (videoSources != null)
                {
                    foreach (var videoSource in videoSources)
                    {
                        if (videoSource != null && videoSource.IsRunning)
                        {
                            await Task.Run(() =>
                            {
                                videoSource.SignalToStop();
                                videoSource.WaitForStop();
                            });
                        }
                    }
                    videoSources = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi dừng video source: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Display image
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="imageView"></param>
        private void DisplayImageInImageView(Bitmap frame, System.Windows.Controls.Image imageView)
        {
            System.Windows.Media.Imaging.BitmapImage bitmapImage = ToBitMapImage(frame);

            if (bitmapImage != null)
            {
                imageView.Source = bitmapImage;
            }
        }
        /// <summary>
        /// Load image in bitmap save image in memogy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void VideoSource_BitMapFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
            camVideoView.Dispatcher.Invoke(() =>
            {
                Dispatcher.Invoke(() => DisplayImageInImageView(frame, camVideo1));
                Dispatcher.Invoke(() => DisplayImageInImageView(frame, camVideo2));
                Dispatcher.Invoke(() => DisplayImageInImageView(frame, camVideo3));
                Dispatcher.Invoke(() => DisplayImageInImageView(frame, camVideoView));

            });
        }
        /// <summary>
        /// Convert To Bitmap Image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private System.Windows.Media.Imaging.BitmapImage ToBitMapImage(Bitmap bitmap)
        {
            System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
        /// <summary>
        /// Take Picture Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTakePictureImage_Click(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                if (videoSources != null)
                {
                    if (selectedCameraIndex >= 0 && selectedCameraIndex < videoSources.Length)
                    {
                        var videoSource = videoSources[selectedCameraIndex];
                        if (videoSource != null && videoSource.IsRunning)
                        {
                            BitmapSource capturedBitmapSource = (BitmapSource)camVideoView.Source;
                            if (capturedBitmapSource != null)
                            {
                                Bitmap capturedBitmap = HelpToBitMapImage(capturedBitmapSource);
                                string subfolderName = DateTime.Now.ToString("yyyyMMdd");
                                string subfolderPath = System.IO.Path.Combine("D:\\FPT\\SWD392\\ProjectSWD392\\WebsiteMachine\\WPF_MachineSevice\\Picture", subfolderName);
                                if (!Directory.Exists(subfolderPath))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(subfolderPath);
                                       // System.Windows.MessageBox.Show($"Subfolder created: {subfolderPath}");
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Windows.MessageBox.Show($"Error creating subfolder: {ex.Message}");
                                        return;
                                    }
                                }
                                string folderName = $"{DateTime.Now:yyyyMMdd}_{captureCount++}";
                                string folderPath = System.IO.Path.Combine(subfolderPath, folderName);
                                if (!Directory.Exists(folderPath))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(folderPath);
                                       // System.Windows.MessageBox.Show($"Folder created: {folderPath}");
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Windows.MessageBox.Show($"Error creating folder: {ex.Message}");
                                        return;
                                    }
                                }
                                string fileName = $"capture_{DateTime.Now:HHmmss}.png";
                                string filePath = System.IO.Path.Combine(folderPath, fileName);
                                try
                                {
                                    capturedBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                                  //  System.Windows.MessageBox.Show($"Capture saved to {filePath}");

                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show($"Error saving capture: {ex.Message}");
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Bitmap source is null.");
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Selected video source is not running.");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Invalid camera index.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error capturing frame: {ex.Message}");
            }
        }
        /// <summary>
        /// Help to bit Map Image
        /// </summary>
        /// <param name="capturedBitmapSource"></param>
        /// <returns></returns>
        private Bitmap HelpToBitMapImage(BitmapSource capturedBitmapSource)
        {
            Bitmap bitmap;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(capturedBitmapSource));
                enc.Save(memoryStream);
                bitmap = new Bitmap(memoryStream);
            }
            return bitmap;
        }
        /// <summary>
        /// Update View Image By button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button3_Click(object sender, RoutedEventArgs eventArgs)
        {

            
        }

    }
}