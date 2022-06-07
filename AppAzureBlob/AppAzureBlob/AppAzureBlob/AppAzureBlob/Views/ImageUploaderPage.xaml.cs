using AppAzureBlob.Services;
using Plugin.Media;
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
    public partial class ImageUploaderPage : ContentPage
    {
        byte[] ByteData;

        public ImageUploaderPage()
        {
            InitializeComponent();
        }

        private async void buttonTakePicture_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppPets", "No existe cámara disponible en el dispositivo", "Ok");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "AppAzureBlob",
                    Name = "AzureBlobImage.jpg"
                });

                if (file == null)
                    return;

                ByteData = await ConverImageFilePathToByteArray(file.Path);
                ImagePicture.Source = ImageSource.FromStream(() => new MemoryStream(ByteData));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppPets", $"Se generó un error al tomar la fotografía ({ex.Message})", "Ok");
            }
        }

        private async void buttonSelectPicure_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppPets", ":( No es posible seleccionar fotografías en el dispositivo.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                ByteData = await ConverImageFilePathToByteArray(file.Path);
                ImagePicture.Source = ImageSource.FromStream(() => new MemoryStream(ByteData));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppPets", $":( Se generó un error al tomar la fotografía: ({ex.Message})", "OK");
                throw;
            }
        }

        private async void buttonUpload_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ByteData != null && ByteData.Length >0)
                {
                    buttonUpload.IsEnabled = false;
                    activityIndicator.IsRunning = true;

                    
                    await new AzureService().UploadFileAsync(AzureContainer.Image, new MemoryStream(ByteData));

                    ImagePicture.Source = null;
                }
                else
                {
                    labelMessage.Text = "Debes seleccionar una imagen para subir al contenedor de Azure Blobs";
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

        private async Task<byte[]> ConverImageFilePathToByteArray(string filePath)
        {
            //Convierte una imagen desde la ruta del archivo a texto en un arreglo de bytes
            if (!string.IsNullOrEmpty(filePath))
            {
                FileStream stream = File.Open(filePath, FileMode.Open);
                byte[] bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                return bytes;
            }
            else
            {
                return null;
            }
        }

        private async void buttonUpload12_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ByteData != null && ByteData.Length > 0)
                {
                    buttonUpload.IsEnabled = false;
                    activityIndicator.IsRunning = true;


                    await new AzureService12().UploadFileAsync(AzureContainer.Image, new MemoryStream(ByteData));

                    ImagePicture.Source = null;
                }
                else
                {
                    labelMessage.Text = "Debes seleccionar una imagen para subir al contenedor de Azure Blobs";
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