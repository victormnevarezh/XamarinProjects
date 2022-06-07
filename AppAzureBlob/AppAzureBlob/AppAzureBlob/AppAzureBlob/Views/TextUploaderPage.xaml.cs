using AppAzureBlob.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAzureBlob.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextUploaderPage : ContentPage
    {
        public TextUploaderPage()
        {
            InitializeComponent();
        }

        private async void buttonUpload_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(editorText.Text))
                {
                    buttonUpload.IsEnabled = false;
                    activityIndicator.IsRunning = true;

                    var byteData = Encoding.UTF8.GetBytes(editorText.Text);
                    await new AzureService().UploadFileAsync(AzureContainer.Text, new MemoryStream(byteData));
                }
                else
                {
                    labelMessage.Text = "Debes capturar un texto para subirlo al contenedor de Azure Blobs";
                    await Task.Delay(5000);
                }
            }
            catch(Exception exc)
            {
                labelMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                //labelMessage.Text = string.Empty;
                buttonUpload.IsEnabled = true;
                activityIndicator.IsRunning = false;
            }
        }

        private async void buttonUpload12_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(editorText.Text))
                {
                    buttonUpload.IsEnabled = false;
                    activityIndicator.IsRunning = true;

                    var byteData = Encoding.UTF8.GetBytes(editorText.Text);
                    await new AzureService12().UploadFileAsync(AzureContainer.Text, new MemoryStream(byteData));
                }
                else
                {
                    labelMessage.Text = "Debes capturar un texto para subirlo al contenedor de Azure Blobs";
                    await Task.Delay(5000);
                }
            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                //labelMessage.Text = string.Empty;
                buttonUpload.IsEnabled = true;
                activityIndicator.IsRunning = false;
            }
        }
    }
}