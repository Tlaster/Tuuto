using Mastodon.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Tuuto.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using PropertyChanged;
using Windows.Graphics.Imaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.ComponentModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace Tuuto.Common.Controls
{
    [ImplementPropertyChanged]
    public sealed partial class TootDialog : ContentDialog, INotifyPropertyChanged
    {
        private const int MAX_IMAGE_COUNT = 4;
        private int _inReplyToId = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<MediaData> Medias { get; } = new ObservableCollection<MediaData>();
        public int SelectedVisibilityIndex { get; set; } = 0;
        public bool IsSensitive { get; set; } = false;

        public int TextCount => 500 - Text.Length - ContentWarningText.Length;
        
        public string ContentWarningText { get; set; } = "";

        public string Text
        {
            get
            {
                richEditBox.Document.GetText(Windows.UI.Text.TextGetOptions.NoHidden, out string value);
                return value;
            }
            set
            {
                richEditBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, value);
            }
        }
        void TextChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextCount)));
        }

        public TootDialog()
        {
            this.InitializeComponent();
        }

        #region Media
        private async void AddMultipleImage(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".bmp");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");
            var files = await picker.PickMultipleFilesAsync();
            if (files != null)
                for (var i = 0; i < 9 && i < files.Count; i++)
                    await AddMediaDataFromFile(files[i]);
        }

        private async void AddSingleImage(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".bmp");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");
            var file = await picker.PickSingleFileAsync();
            if (file != null)
                await AddMediaDataFromFile(file);
        }

        private async void TakePhoto(object sender, RoutedEventArgs e)
        {
            var camera = new CameraCaptureUI();
            camera.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            var file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file != null)
                await AddMediaDataFromFile(file);
        }

        private async Task AddMediaDataFromFile(StorageFile file, bool deleteAfterUsed = false)
        {
            byte[] data;
            using (var stream = await file.OpenReadAsync())
            using (var dataReader = new DataReader(stream))
            {
                data = new byte[stream.Size];
                await dataReader.LoadAsync((uint)stream.Size);
                dataReader.ReadBytes(data);
            }
            await AddMediaData(data);
            if (deleteAfterUsed)
                await file.DeleteAsync();
        }

        private async Task AddMediaData(byte[] data)
        {
            if ((data.Length / 1024f / 1024f > 8f) || Medias.Count >= MAX_IMAGE_COUNT)//The media is larger than 8MB
                return;
            using (var stream = new MemoryStream(data))
            {
                var bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                Medias.Add(new MediaData(data, bitmap));
            }
        }

        private async void TextBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            if (Medias.Count >= MAX_IMAGE_COUNT)
                return;
            var dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
            if (dataPackageView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Bitmap))
            {
                e.Handled = true;
                var bitmap = await dataPackageView.GetBitmapAsync();
                var file = await GetFileFromBitmap(bitmap);
                await AddMediaDataFromFile(file, true);
            }
            else if (dataPackageView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.StorageItems))
            {
                e.Handled = true;
                var files = (await dataPackageView.GetStorageItemsAsync()).Where(item => item is StorageFile && (item as StorageFile).ContentType.Contains("image")).ToList();
                for (var i = 0; i < 9 && i < files.Count; i++)
                    await AddMediaDataFromFile(files[i] as StorageFile);
            }
        }
        private static async Task<StorageFile> GetFileFromBitmap(RandomAccessStreamReference bitmap)
        {
            var file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync($"{new Random().Next()}.png", CreationCollisionOption.GenerateUniqueName);
            using (var fstream = await file.OpenStreamForWriteAsync())
            using (var stream = await bitmap.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var pixels = await decoder.GetPixelDataAsync();
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fstream.AsRandomAccessStream());
                encoder.SetPixelData(decoder.BitmapPixelFormat, BitmapAlphaMode.Ignore,
                    decoder.OrientedPixelWidth, decoder.OrientedPixelHeight,
                    decoder.DpiX, decoder.DpiY,
                    pixels.DetachPixelData());
                await encoder.FlushAsync();
            }
            return file;
        }

        private async void TextBox_DragOver(object sender, DragEventArgs e)
        {
            var def = e.GetDeferral();
            e.Handled = true;
            if (e.DataView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Bitmap))
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            else if (e.DataView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.StorageItems))
            {
                var files = (await e.DataView.GetStorageItemsAsync()).Where(item => item is StorageFile && (item as StorageFile).ContentType.Contains("image")).ToList();
                e.AcceptedOperation = files?.Count > 0 && Medias.Count < MAX_IMAGE_COUNT ?
                    Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy :
                    Windows.ApplicationModel.DataTransfer.DataPackageOperation.None;
            }
            def.Complete();
        }

        private async void TextBox_Drop(object sender, DragEventArgs e)
        {
            var def = e.GetDeferral();
            e.Handled = true;
            if (e.DataView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Bitmap))
                await AddMediaDataFromFile(await GetFileFromBitmap(await e.DataView.GetBitmapAsync()));
            else if (e.DataView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.StorageItems))
            {
                var files = (await e.DataView.GetStorageItemsAsync()).Where(item => item is StorageFile && (item as StorageFile).ContentType.Contains("image")).ToList();
                for (var i = 0; i < 9 && i < files.Count; i++)
                    await AddMediaDataFromFile(files[i] as StorageFile);
            }
            def.Complete();
        }

        private void DeleteMedia(object sender, RoutedEventArgs e)
        {
            Medias.Remove((sender as FrameworkElement).DataContext as MediaData);
        }

        private void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Menu_DeleteMedia.DataContext = e.ClickedItem as MediaData;
            MediaTapFlyout.ShowAt((sender as AdaptiveGridView).ContainerFromItem(e.ClickedItem) as FrameworkElement);
        }

        #endregion

        public void Toot()
        {

            DraftManager.Add(new DraftModel
            {
                Domain = Settings.CurrentAccount.Domain,
                AccessToken = Settings.CurrentAccount.AccessToken,
                AccountId = Settings.CurrentAccount.Id,
                Status = Text.Replace("\r", "\n"),
                InReplyToId = _inReplyToId,
                Sensitive = IsSensitive,
                SpoilerText = ContentWarningText,
                Visibility = TootVisibilityList.VisibilityList[SelectedVisibilityIndex].VisibilityCode,
                Medias = Medias.ToList()
            });
            Hide();
        }

    }
}
