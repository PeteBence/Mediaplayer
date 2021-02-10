using System;
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
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Windows.Threading;

namespace Mediaplayer
{

    public partial class MainWindow : Window
    {


        MediaPlayer mediaPlayer = new MediaPlayer();
        DispatcherTimer timer = new DispatcherTimer();
        string fajlnev;
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        int isPlaying = 0;
        int playing = -1;
        int repeatType = 0;

        public MainWindow()
        {
            InitializeComponent();
        }
       

        private void betoltes_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog filedialog = new OpenFileDialog
            {
                Multiselect = true,
                DefaultExt = ".mp3"
            };
            {

                fajlnev = filedialog.FileName;
                box.Text = filedialog.SafeFileName;
                mediaPlayer.Open(new Uri(fajlnev));
            }
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
            sliProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            sliProgress.Value = mediaPlayer.Position.TotalSeconds;
            if (sliProgress.Value == mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds)
            {
                timer.Stop();
                sliProgress.Value = 0;
            }
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)volumeslider.Value;
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }
        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            playing = playing - 1;
            box.SelectedText = playing;
            string file = box.Items[box.SelectedText].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            sliProgress.IsEnabled = true;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            playing++;
            box.SelectedText = playing;
            string file = box.Items[box.SelectedText].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            sliProgress.IsEnabled = true;

        }
    }
}