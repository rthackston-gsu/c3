using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;

namespace App3.Utility
{
    class PromptFilePicker
    {
        async Task chooseFile()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();

            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            var image = new BitmapImage();
            image.SetSource(stream);

            sr.WriteLine(stream.Size);
        }
    }
}
