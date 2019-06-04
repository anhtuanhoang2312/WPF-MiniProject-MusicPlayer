﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;

namespace MiniProject_MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //static MainWindow()
        //{
        //    Telerik.Windows.Controls.StyleManager.ApplicationTheme = new Telerik.Windows.Controls.FluentTheme();
        //    Telerik.Windows.Controls.RadRibbonWindow.IsWindowsThemeEnabled = false;
        //}


        public static MediaPlayer _audio = new MediaPlayer();
        public static DispatcherTimer _timer = new DispatcherTimer();
        public static BindingList<Info> _infoList = new BindingList<Info>();
        public static BindingList<Playlist> _playlistList = new BindingList<Playlist>();
        public static string currentlyPlayingSong = null;
        public static bool _isDragging = false;
        public static bool _isPlaying = false;
        public static bool _isExist = false;
        public static MyMusicPage mymusicpg = new MyMusicPage();
        public static PlaylistPage playlistpg = new PlaylistPage();




        public MainWindow()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromSeconds(0);
            _timer.Tick += timer_Tick;

            Slider.ApplyTemplate();
            System.Windows.Controls.Primitives.Thumb thumb = (Slider.Template.FindName("PART_Track", Slider) as System.Windows.Controls.Primitives.Track).Thumb;
            thumb.MouseEnter += new MouseEventHandler(thumb_MouseEnter);
        }

        public void thumb_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed
                && e.MouseDevice.Captured == null)
            {
                // the left button is pressed on mouse enter
                // but the mouse isn't captured, so the thumb
                // must have been moved under the mouse in response
                // to a click on the track.
                // Generate a MouseLeftButtonDown event.
                MouseButtonEventArgs args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                args.RoutedEvent = MouseLeftButtonDownEvent;
                (sender as System.Windows.Controls.Primitives.Thumb).RaiseEvent(args);
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if(!_isDragging)
            {
                Slider.Value = _audio.Position.TotalSeconds;
                Slider.Maximum = _audio.NaturalDuration.TimeSpan.TotalSeconds;
            }
            Current.Text = String.Format(_audio.Position.ToString(@"mm\:ss"));
            Total.Text = String.Format(_audio.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isPlaying)
            {
                _audio.Play();
                _isPlaying = true;
                _timer.Start();
            }
            else
            {
                _audio.Pause();
                _isPlaying = false;
                _timer.Stop();
            }
        }

        public static void SetNowPlaying(string song)
        {
            currentlyPlayingSong = song;
            _audio.Open(new Uri(song));
        }
        public static string GetNowPlaying()
        {
            return currentlyPlayingSong;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _isDragging = true;
        }

        public void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _isDragging = false;
            _audio.Position = TimeSpan.FromSeconds(Slider.Value);
        }    

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Playlist newPlaylist = new Playlist("untitled playlist", new List<Info>());
            _playlistList.Add(newPlaylist);
            PlaylistListView.ItemsSource = _playlistList;
        }

        private void PlaylistItem_Click(object sender, RoutedEventArgs e)
        {
            Control.Show(MainContent, playlistpg);
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MyListView.SelectedItems.Clear();

            ListViewItem item = sender as ListViewItem;
            if (item != null)
            {
                item.IsSelected = true;
                MyListView.SelectedItem = item;
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                if (item.Name == "MyMusic")
                {
                    Control.Show(MainContent, mymusicpg);
                }
                else if (item.Name == "NowPlaying")
                {
                    Control.Show(MainContent, new NowPlayingPage());
                }
                else
                {

                }
            }
        }
    }
}