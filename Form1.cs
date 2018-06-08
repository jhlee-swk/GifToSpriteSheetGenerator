using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public double Row = 5;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = null;
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "gif files (*.gif)|*.gif";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = false;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                }
            }
            Bitmap tempImage = new Bitmap(fileName);
            FrameDimension dimension = new FrameDimension(tempImage.FrameDimensionsList[0]);
            int numOfFrames = tempImage.GetFrameCount(dimension);
            int width = tempImage.Width;
            int height = tempImage.Height;

            var tempFrames = new List<byte[]>();
            for(int i = 0; i < numOfFrames; i++)
            {
                tempImage.SelectActiveFrame(dimension,i);
                using (MemoryStream ms = new MemoryStream())
                {
                    tempImage.Save(ms,ImageFormat.Png);
                    tempFrames.Add(ms.ToArray());
                }
            }

            List<Bitmap> images = new List<Bitmap>();
            foreach(byte[] arr in tempFrames)
            {
                using (var ms = new MemoryStream(arr))
                {
                    images.Add((Bitmap)Image.FromStream(ms));
                }
            }
            //get sprite sheet size
            int sheetWidth = width * 5;
            int sheetHeight = height*(numOfFrames / 5);
            Bitmap spriteSheet = new Bitmap(sheetWidth,sheetHeight);
            int y = 0;
            int x = 0;
            for (int i =0;i<images.Count;i++)
            {
                var image = images[i];

                
                using (Graphics g = Graphics.FromImage(spriteSheet))
                {
                    g.DrawImage(image,x,y);
                }
                x += width;
                if (i == 4)
                {
                    y += height;
                    x = 0;
                }
            }

            string newFileName = fileName.Split('.')[0];

            spriteSheet.Save(newFileName+"_spriteSheet.png",ImageFormat.Png);

                Console.WriteLine("check");



        }

    }
}
