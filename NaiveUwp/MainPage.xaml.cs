using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
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
            mediaElement.Volume = Volumn.Value;
        }
        private void VoiceButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Volume = 0;
            //mediaElement.Volume = {Binding value, ElementName=Volumn};
        }

        private void Play2_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Source = new Uri("http://www.neu.edu.cn/indexsource/neusong.mp3", UriKind.Absolute);
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            var buffer = await httpClient.GetBufferAsync(new Uri("http://www.neu.edu.cn/indexsource/neusong.mp3"));
            if (buffer == null) return;
            //创建本地资源
            FileSavePicker fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary;
            fileSavePicker.FileTypeChoices.Add("校歌", new List<string>() { ".mp3" });
            var storageFile = await fileSavePicker.PickSaveFileAsync();
            if (storageFile == null) return;
            //写入本地资源
            CachedFileManager.DeferUpdates(storageFile);
            await FileIO.WriteBufferAsync(storageFile, buffer);
            await CachedFileManager.CompleteUpdatesAsync(storageFile);
            MessageDialog msg = new MessageDialog("Welcome!");//.....
            //写入MediaElement
            var stream = await storageFile.OpenAsync(FileAccessMode.Read);
            mediaElement.SetSource(stream, "");
        }
    }
}