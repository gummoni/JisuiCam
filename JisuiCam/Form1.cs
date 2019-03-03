using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace JisuiCam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        VideoCapture video = null;
        int counter = 1;

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //開始
            if (buttonStart.Text == "開始")
            {
                try
                {
                    video = new VideoCapture();
                    video.Open(0);
                    video.FrameWidth = 1920;
                    video.FrameHeight = 1080;

                    if (!video.IsOpened())
                    {
                        MessageBox.Show("カメラがありません");
                        return;
                    }

                    buttonStart.Text = "停止";
                    timer1.Start();
                }
                catch
                {
                }
            }
            else
            {
                timer1.Stop();
                buttonStart.Text = "開始";
                video?.Dispose();
                video = null;
            }
        }

        object _lock = new object();
        private void ButtonCapture_Click(object sender, EventArgs e)
        {
            using (var bmp = GetCapture())
            {
                var cnt = counter++;
                var filename = $"{textBox1.Text}_{cnt.ToString("D4")}.jpg";
                bmp.Save(filename, ImageFormat.Jpeg);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (null == video) return;
            var bmp = GetCapture();
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = bmp;
            timer1.Start();
        }

        Bitmap GetCapture()
        {
            lock (_lock)
            {
                using (var mat = new Mat())
                {
                    video.Read(mat);
                    return mat.ToBitmap();
                }
            }
        }
    }
}
