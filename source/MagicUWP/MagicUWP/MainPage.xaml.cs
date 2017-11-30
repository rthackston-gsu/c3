using MagicUWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
/*
 * MVP
 * MainPage.xaml.cs:
 * Code contains methods to initiate the process by calling the async task(not on the UI thread)
 * By Beloved Egbedion
 * For Dr. Russell Thackston
 * MAGIC Project
 * 10/21/2017
*/


namespace MagicUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MagicServiceUtility msu = new MagicServiceUtility();


        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            //Calls EC2 AWS SDK
            /*
            Task<string> getMagicTask = MagicServiceUtility.GetServiceOutputAsync();
        
            string result = await getMagicTask;
            
            Debug.WriteLine("Characters received: " +result + "\n"+result.Length);
            if (result.Length > 0)
            {
                textBlock.Text = textBlock.Text + "\n" + result;
            }
            */


            Task<string> createS3Buccket = MagicServiceUtility.createS3();


            string resultBucket = "";

            resultBucket = await createS3Buccket;


            Debug.WriteLine("Characters received: " + resultBucket + "\n" + resultBucket.Length);
            if (resultBucket.Length > 0)
            {
                textBlock.Text = textBlock.Text + "\n" + resultBucket;
            }
            /*
            Task<string> createEc2V = MagicServiceUtility.createEc2();
            string res = await createEc2V;
            if (resultBucket.Length > 0)
            {

                //   textBlock.FontSize = 25;
                //    textBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                textBlock.Text = "\n" + resultBucket;



            }
            */
        }

        #region Button Click Event Handlers
        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            /*
            Fil dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                this.UploadFile = dlg.FileName;
            }
            */

        }
        #endregion

    }
}

