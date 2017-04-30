using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Mastodon.Model;
using Windows.Storage.Pickers;
using System.Net.Http;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace Tuuto.Common.Controls
{
    class MediaTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Image { get; set; }
        public DataTemplate Video { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null)
                return null;
            var model = item as AttachmentModel;
            switch (model.Type)
            {
                case AttachmentModel.ATTACHMENTTYPE_VIDEO:
                    return Video;
                default:
                    return Image;
            }
        }
    }
    public sealed partial class MediaViewDialog : ContentDialog
    {
        public IList<AttachmentModel> ItemsSource
        {
            get { return (IList<AttachmentModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IList<AttachmentModel>), typeof(MediaViewDialog), new PropertyMetadata(null));
        private int _index;

        public MediaViewDialog(IList<AttachmentModel> items, int index = 0)
        {
            this.InitializeComponent();
            ItemsSource = items;
            _index = index;
            //SelectedIndex = index;
        }

        protected override void OnRightTapped(RightTappedRoutedEventArgs e)
        {
            e.Handled = true;
            //Hide();
            ImageMenu.ShowAt(this, e.GetPosition(this));
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Hide();
        }

        private async void ImageMenu_Save_Click(object sender, RoutedEventArgs e)
        {
            var name = Path.GetFileName((flipView.SelectedItem as AttachmentModel).Url);
            var picker = new FileSavePicker()
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeChoices.Add("Image file", new List<string>() { ".jpg", ".png", ".gif" });
            var file = await picker.PickSaveFileAsync();
            if (file == null)
                return;
            try
            {
                using (var client = new HttpClient())
                using (var fstream = await file.OpenStreamForWriteAsync())
                using (var stream = await client.GetStreamAsync((flipView.SelectedItem as AttachmentModel).Url))
                    await stream.CopyToAsync(fstream);

            }
            catch (Exception)
            {
                file.DeleteAsync();
            }
        }

        private void flipView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            flipView.SelectedIndex = _index;
        }
    }
}