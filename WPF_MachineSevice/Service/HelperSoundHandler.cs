using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_MachineSevice.Service
{
    public class HelperSoundHandler
    {
        private SoundPlayer _soundScanSuccessHandler;
        public HelperSoundHandler()
        {
           _soundScanSuccessHandler = new SoundPlayer();
        }
        public void PlayThankYouSound()
        {
            try
            {
                System.Media.SoundPlayer soundThankYou = new System.Media.SoundPlayer();
                string soundThankYouFilePath = "D:\\FPT\\SWD392\\ProjectSWD392\\WebsiteMachine\\WPF_MachineSevice\\SoundHandler\\payment_successfully_have_a_good_day.wav";
                soundThankYou.SoundLocation = soundThankYouFilePath;
                soundThankYou.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing sound: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void CheckPaymentSound()
        {
            try
            {
                System.Media.SoundPlayer soundPayments = new System.Media.SoundPlayer();
                string soundPaymentsFilePath = "D:\\FPT\\SWD392\\ProjectSWD392\\WebsiteMachine\\WPF_MachineSevice\\SoundHandler\\pleasescanning_the_product_to_payment.wav";
                soundPayments.SoundLocation = soundPaymentsFilePath;
                soundPayments.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing sound: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void ScanningProductsSound()
        {
            try
            {
                System.Media.SoundPlayer soundScanning = new System.Media.SoundPlayer();
                string soundScanningFilePath = "D:\\FPT\\SWD392\\ProjectSWD392\\WebsiteMachine\\WPF_MachineSevice\\SoundHandler\\scanning_successfully_please_pay_product.wav";
                soundScanning.SoundLocation = soundScanningFilePath;
                soundScanning.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing sound: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
