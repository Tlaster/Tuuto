using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Mastodon.Model;
using Mastodon.Api;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Toolkit.Uwp;
using Windows.Storage;

namespace Tuuto.Model
{
    class MediaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SavedFile { get; set; }
        [NotMapped]
        public byte[] Data { get; }
        [NotMapped]
        public BitmapImage Image { get; }

        public MediaData()
        {

        }

        public MediaData(byte[] data, BitmapImage image)
        {
            Data = data;
            Image = image;
        }

        internal async Task SaveToFile()
        {
            var file = await StorageFileHelper.WriteBytesToLocalFileAsync(Data, new Random().Next().ToString(), CreationCollisionOption.GenerateUniqueName);
            SavedFile = file.Name;
        }

        internal async Task DeleteFile()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            if (!await localFolder.FileExistsAsync(SavedFile))
                return;
            var file = await localFolder.GetFileAsync(SavedFile);
            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
    }
}
