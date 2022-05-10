﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenshotTakerApp
{
    using System.Configuration;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading;
    using System.IO.Compression;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public partial class Form1 : Form
    {
        public string ApiBaseUrl { get; set; } = ConfigurationManager.AppSettings["url"];

        static string folder = "Screenshots";
        static Random random;
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        static int alarmCounter = 1;
        static bool exitFlag = false;
        static bool mouseHoverFlag = false;


        public Form1()
        {
            InitializeComponent();
        }


        private void TimerEventProcessor(Object myObject,
                                                EventArgs myEventArgs)
        {
            string image = AddImage();
            byte[] imageArray = File.ReadAllBytes($@"{image}");
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            UploadImage(base64ImageRepresentation);
            UploadMouseStatus();
            File.Delete(image);
            //int interval = random.Next(60000, 600000);
            //myTimer.Interval = interval;
            myTimer.Interval = TimeIntervalBar.Value * 1000 * 60;
        }

        private void UploadImage(string image)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();               
                HttpContent stringContent = new StringContent(image, Encoding.UTF8, "application/text");
                string uri = $"{this.ApiBaseUrl}/api/Screenshot";
                var response = client.PostAsync(uri, stringContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
            }
        }

        private void UploadMouseStatus()
        {
            string mouseStatus = mouseHoverFlag ? "Mouse moved." : "Mouse didn`t move.";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                HttpContent stringContent = new StringContent(mouseStatus, Encoding.UTF8, "application/text");
                string uri = $"{this.ApiBaseUrl}/api/MouseMoveCapture";
                var response = client.PostAsync(uri, stringContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                mouseHoverFlag = false;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            random = new Random(120000);
            CheckFolder();
            myTimer.Tick += TimerEventProcessor;
            myTimer.Interval = 5000 * 60;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            mouseHoverFlag = true;
        }

        private static string AddImage()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                long ticks = DateTime.Now.Ticks;
                string filename = $@"{folder}\{ticks}.jpeg";
                bitmap.Save(filename, ImageFormat.Jpeg);
                return filename;
            }
        }

        private void CheckFolder()
        {
            bool exists = Directory.Exists(folder);
            if (!exists)
            {
                var directoryInfo = Directory.CreateDirectory(folder);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            myTimer.Start();
            this.startButton.Enabled = false;
            this.stopButton.Enabled = true;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            myTimer.Stop();
            this.startButton.Enabled = true;
            this.stopButton.Enabled = false;
        }

        private void commentSendButton_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                HttpContent stringContent = new StringContent(this.textBox1.Text, Encoding.UTF8, "application/text");
                string uri = $"{this.ApiBaseUrl}/api/Comment";
                var response = client.PostAsync(uri, stringContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
            }
        }

        private void setTimeInterval(object sender, EventArgs e)
        {
            myTimer.Interval = 1000 * TimeIntervalBar.Value * 60;
        }
    }
}
