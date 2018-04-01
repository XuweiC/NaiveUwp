using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace NaiveUwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayButton.Content.ToString() == "Play")
            {
                PlayButton.Content = "Pause";
                mediaElement.Play();
            }
            else
            {
                PlayButton.Content = "Play";
                mediaElement.Pause();
            }
        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mp3");
            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                // 指定需要让 MediaElement 播放的媒体流
                mediaElement.SetSource(stream, file.ContentType);
            }
            mediaElement.Play();
        }

        public void Volumn_ValueChanged(object sender, RoutedEventArgs e)
        {
            
        }
        private void VoiceButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Volume = Volumn.Value;
            //mediaElement.Volume = {Binding value, ElementName=Volumn};
        }
    }
}