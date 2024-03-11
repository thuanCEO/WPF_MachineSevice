using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_MachineSevice.Repository;


namespace WPF_MachineSevice
{
    /// <summary>
    /// Interaction logic for ScanQR.xaml
    /// </summary>
    public partial class ScanQR : UserControl
    {
        public UnitOfWork unitOfWork = new UnitOfWork();
        private readonly MainWindow _mainWindow;
   
        public ScanQR(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }
        public void UpdateQRCode(Bitmap qrCode)
        {
            if (qrCode != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    qrCode.Save(memory, ImageFormat.Png);
                    memory.Position = 0;

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    qrCodeImageView.Source = bitmapImage;
                }
            }
        }
        private void btConfirmQR_Click(object sender, RoutedEventArgs e)
        {
            if (_mainWindow != null)
            {
                _mainWindow.ProcessPaymentQRSuccess();
            }
            else
            {
                MessageBox.Show("MainWindow instance is null. Cannot process payment.");
            }
            Window.GetWindow(this).Close();
        }
    }
}
