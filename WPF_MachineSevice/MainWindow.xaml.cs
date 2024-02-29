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
using System.Threading.Tasks;
using System;
using System.Windows.Media.Media3D;
using System.Diagnostics.Tracing;
using System.Management;
using System.Linq;
using WPF_MachineSevice.Entities;
using System.Globalization;
using WPF_MachineSevice.DAO;

namespace WPF_MachineSevice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FilterInfoCollection? videoDevices;
        private VideoCaptureDevice[]? videoSources;
        private int selectedCameraIndex = 0;
        private int captureCount = 1;

        private readonly ScanMachineContext context;
        public List<Product> YourProductList { get; set; }
        public MainWindow()
        {
            context = new ScanMachineContext();
            InitializeComponent();
            Loaded += MachineWindow_Loaded;
            Closing += MachineWindow_Closing;
          
        }
        /// <summary>
        /// Load full product Data On ViewList
        /// </summary>
        private void LoadData()
        {
            using (var dbContext = new ScanMachineContext())
            {
                var productList = dbContext.Products.ToList();
                FileFolderListView.ItemsSource = productList;
            }
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
            if (videoDevices != null && videoDevices.Count >= 3)
            {
                videoSources = new VideoCaptureDevice[3];
                try
                {
                    Parallel.For(0, 3, i =>
                    {
                            videoSources[i] = new VideoCaptureDevice(videoDevices[i].MonikerString);
                            videoSources[i].VideoResolution = videoSources[i].VideoCapabilities.LastOrDefault();
                            switch (i)
                            {
                                case 0:
                                    videoSources[i].NewFrame += VideoSource1_BitMapFrame;
                                    break;
                                case 1:
                                    videoSources[i].NewFrame += VideoSource2_BitMapFrame;
                                    videoSources[i].NewFrame += VideoSource4_BitMapFrame;                    
                                    break;
                                case 2:
                                    videoSources[i].NewFrame += VideoSource3_BitMapFrame;
                                    break;
                            }
                            videoSources[i].Start();
                        });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi khởi động webcam: {ex.Message}");
                    Parallel.ForEach(videoSources, source =>
                    {
                        if (source != null && source.IsRunning)
                            source.Stop();
                    });
                }
            }
            else
            {
                MessageBox.Show("Không đủ thiết bị video.");
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
        private void VideoSource1_BitMapFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap frame1 = (Bitmap)eventArgs.Frame.Clone();
                camVideo1.Dispatcher.Invoke(() => DisplayImageInImageView(frame1, camVideo1));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in VideoSource_BitMapFrame: {ex.Message}");
            }
        }
        private void VideoSource2_BitMapFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap frame2 = (Bitmap)eventArgs.Frame.Clone();
                camVideo2.Dispatcher.Invoke(() => DisplayImageInImageView(frame2, camVideo2));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in VideoSource_BitMapFrame: {ex.Message}");
            }
        }
        private void VideoSource3_BitMapFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap frame3 = (Bitmap)eventArgs.Frame.Clone();
                camVideo3.Dispatcher.Invoke(() => DisplayImageInImageView(frame3, camVideo3));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in VideoSource_BitMapFrame: {ex.Message}");
            }
        }
        private void VideoSource4_BitMapFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap frame4 = (Bitmap)eventArgs.Frame.Clone();
                camVideoView.Dispatcher.Invoke(() => DisplayImageInImageView(frame4, camVideoView));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in VideoSource_BitMapFrame: {ex.Message}");
            }
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

                           
                                string subfilePath = System.IO.Path.Combine("D:\\FPT\\SWD392\\ProjectSWD392\\WebsiteMachine\\WPF_MachineSevice\\Picture", subfolderName);
                        
                                if (!Directory.Exists(subfolderPath))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(subfolderPath);
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
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Windows.MessageBox.Show($"Error creating folder: {ex.Message}");
                                        return;
                                    }
                                }
                                string fileName = $"capture_{DateTime.Now:HHmmss}.png";
                                string filePath = System.IO.Path.Combine(folderPath, fileName);
                                
                                //Path with train AI
                                string filePathPython = System.IO.Path.Combine("C:\\Yolov8\\ultralytics\\yolov8-silva\\inference\\images", fileName);
                                LoadData();
                                try
                                {
                                    capturedBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                                    
                                    //Save Image in file AI
                                    capturedBitmap.Save(filePathPython, System.Drawing.Imaging.ImageFormat.Bmp);
                                    UploadFolderToFirebase(folderPath);
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
        /// Upload folder to firebase
        /// </summary>
        /// <param name="folderPath"></param>
        private async void UploadFolderToFirebase(string folderPath)
        {
            try
            {
                var apiKey = "AIzaSyC7ug-zkAb2geK9rxGhDsagpGm5qrggRaE";
                var firebaseStorageBaseUrl = "https://firebasestorage.googleapis.com/v0/b/webid-6c809.appspot.com/o";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
                    string[] files = Directory.GetFiles(folderPath);
                    foreach (var filePath in files)
                    {
                        string relativePath = System.IO.Path.GetRelativePath(folderPath, filePath);
                        string fileName = string.Join("_", relativePath.Split(System.IO.Path.GetInvalidFileNameChars()));
                        var firebaseStorageUrl = $"{firebaseStorageBaseUrl}?uploadType=media&name={fileName}";
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        var content = new ByteArrayContent(fileBytes);
                        var fileResponse = await client.PostAsync(firebaseStorageUrl, content);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading folder: {ex.Message}");
            }
        }
        /// <summary>
        /// Upload file in to firebase
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        private async Task UploadFilesToFirebase(string folderPath)
        {
            try
            {
                var apiKey = "AIzaSyC7ug-zkAb2geK9rxGhDsagpGm5qrggRaE";
                var firebaseStorageBaseUrl = "https://firebasestorage.googleapis.com/v0/b/webid-6c809.appspot.com/o";
                string[] files = Directory.GetFiles(folderPath);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
                    foreach (var filePath in files)
                    {
                        string localFileName = System.IO.Path.GetFileName(filePath);
                        var firebaseStorageUrl = $"{firebaseStorageBaseUrl}?uploadType=media&name=images/{localFileName}";
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        var content = new ByteArrayContent(fileBytes);
                        var response = await client.PostAsync(firebaseStorageUrl, content);
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"File {localFileName} uploaded successfully to Firebase Storage!");
                        }
                        else
                        {
                            MessageBox.Show($"Error uploading file {localFileName} to Firebase Storage. Status Code: {response.StatusCode}\nContent: {await response.Content.ReadAsStringAsync()}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading files: {ex.Message}");
            }
        }
        /// <summary>
        /// Update View Image By button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            videoSources[1].NewFrame -= VideoSource4_BitMapFrame;
            videoSources[2].NewFrame -= VideoSource4_BitMapFrame;
            videoSources[0].NewFrame += VideoSource1_BitMapFrame;
            videoSources[0].NewFrame += VideoSource4_BitMapFrame;
            if (videoSources[0] != null)
            {
                videoSources[0].Start();
            }
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            videoSources[0].NewFrame -= VideoSource4_BitMapFrame;
            videoSources[2].NewFrame -= VideoSource4_BitMapFrame;
            videoSources[1].NewFrame += VideoSource2_BitMapFrame;
            videoSources[1].NewFrame += VideoSource4_BitMapFrame;
            if (videoSources[1] != null)
            {
                videoSources[1].Start();
            }
        }
        private void Button3_Click(object sender, RoutedEventArgs eventArgs)
        {
            videoSources[1].NewFrame -= VideoSource4_BitMapFrame;
            videoSources[0].NewFrame -= VideoSource4_BitMapFrame;
            videoSources[2].NewFrame += VideoSource3_BitMapFrame;
            videoSources[2].NewFrame += VideoSource4_BitMapFrame;
            if (videoSources[2] != null)
            {
                videoSources[2].Start();
            }
        }
        private void ResultTotolPrice(object sender, TextChangedEventArgs e)
        {
            
        }
       
    
    }
}