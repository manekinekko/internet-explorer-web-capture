////////////////////////////////////////////////////////////////////
//
// IECapt# - A Internet Explorer Web Page Rendering Capture Utility
//
// Copyright (C) 2013 Wassim Chegham <wassim.chegham@gmail.com>
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace IEScreenCapt.Classes
{

    class ImageDiff
    {
        private string reference;
        private string file;
 
        public ImageDiff(string reference, string file)
        {
            this.reference = reference;
            this.file = file;
        }
        public Bitmap Diff(Bitmap img1, Bitmap img2)
        {
            // lock img1
            Rectangle rect1 = new Rectangle(0, 0, img1.Width, img1.Height);
            BitmapData bitmapDataImg1 = img1.LockBits(rect1, ImageLockMode.ReadWrite, img1.PixelFormat);
            IntPtr IptrImg1 = bitmapDataImg1.Scan0;

            // copy bitmap1 to pixels array
            int bytes1 = Math.Abs(bitmapDataImg1.Stride) * img1.Height;
            byte[] PixelsImg1 = new byte[bytes1];
            Marshal.Copy(IptrImg1, PixelsImg1, 0, bytes1);
            img1.UnlockBits(bitmapDataImg1);


            // lock img2
            Rectangle rect2 = new Rectangle(0, 0, img2.Width, img2.Height);
            BitmapData bitmapDataImg2 = img2.LockBits(rect2, ImageLockMode.ReadWrite, img2.PixelFormat);
            IntPtr IptrImg2 = bitmapDataImg2.Scan0;

            // copy bitmap2 to pixels array
            int bytes2 = Math.Abs(bitmapDataImg2.Stride) * img2.Height;
            byte[] PixelsImg2 = new byte[bytes2];
            Marshal.Copy(IptrImg2, PixelsImg2, 0, bytes2);

            int length = PixelsImg2.Length;

            for (int i = 0; i < length; i += 4)
            {
                if (PixelsImg1[i] == PixelsImg2[i] && PixelsImg1[i + 1] == PixelsImg2[i + 1] && PixelsImg1[i + 2] == PixelsImg2[i + 2])
                {
                    PixelsImg2[i] = 255;
                    PixelsImg2[i + 1] = 255;
                    PixelsImg2[i + 2] = 255;
                    PixelsImg2[i + 3] = 255;
                }
                else
                {
                    //PixelsImg2[i + 1] = (byte)(PixelsImg1[i + 1] << 100);
                    PixelsImg2[i] = 0;
                    PixelsImg2[i + 1] = 0;
                    PixelsImg2[i + 2] = PixelsImg1[i];
                    PixelsImg2[i + 3] = 255;
                }
            }

            // copy pixels array to bitmap
            Marshal.Copy(PixelsImg2, 0, IptrImg2, bytes2);

            // unlock
            img2.UnlockBits(bitmapDataImg2);

            return img2;
        }

        public void run()
        {

            //Console.WriteLine(File.Exists(fname1).ToString()+":"+File.Exists(fname2).ToString());

            Bitmap img1 = new Bitmap(this.reference); // image de reference
            Bitmap img2 = new Bitmap(this.file);
            Bitmap bm = this.Diff(img1, img2);
            string diffFile = this.file.Replace(".png", "-diff.png");
            bm.Save(diffFile);
            bm.Dispose();
            try
            {
                // open image
                System.Diagnostics.Process.Start(diffFile);
            }
            catch (Exception e) { }
        }
    }
}
