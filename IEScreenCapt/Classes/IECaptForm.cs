////////////////////////////////////////////////////////////////////
//
// IECapt# - A Internet Explorer Web Page Rendering Capture Utility
//
// Copyright (C) 2007 Bjoern Hoehrmann <bjoern@hoehrmann.de>
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
using AxSHDocVw;
using IECaptComImports;

namespace IEScreenCapt.Classes
{
    class IECaptForm : System.Windows.Forms.Form
    {
        private string mURL;
        private string mFile;
        private int mMinWidth;
        private AxWebBrowser mWb;
        public System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();
        private ImageDiff mImageDiff;

        public IECaptForm(string url, string file, int minWidth, int delay, AxWebBrowser wb, ImageDiff imageDiff)
        {
            mURL = url;
            mFile = file;
            mMinWidth = minWidth;
            mTimer.Interval = delay;
            mTimer.Tick += new EventHandler(mTimer_Tick);
            mWb = wb;
            mImageDiff = imageDiff;
        }

        private void mTimer_Tick(object sender, EventArgs e)
        {
            mTimer.Stop();

            try
            {
                DoCapture();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            mWb.Dispose();
            System.Windows.Forms.Application.Exit();
        }

        public void DoCapture()
        {
            IHTMLDocument2 doc2 = (IHTMLDocument2)mWb.Document;
            IHTMLDocument3 doc3 = (IHTMLDocument3)mWb.Document;
            IHTMLElement2 body2 = (IHTMLElement2)doc2.body;
            IHTMLElement2 root2 = (IHTMLElement2)doc3.documentElement;

            // Determine dimensions for the image; we could add minWidth here
            // to ensure that we get closer to the minimal width (the width
            // computed might be a few pixels less than what we want).
            int width = Math.Max(body2.scrollWidth, root2.scrollWidth);
            int height = Math.Max(root2.scrollHeight, body2.scrollHeight);

            // Resize the web browser control
            mWb.SetBounds(0, 0, width, height);

            // Do it a second time; in some cases the initial values are
            // off by quite a lot, for as yet unknown reasons. We could
            // also do this in a loop until the values stop changing with
            // some additional terminating condition like n attempts.
            width = Math.Max(body2.scrollWidth, root2.scrollWidth);
            height = Math.Max(root2.scrollHeight, body2.scrollHeight);

            //@todo override with fixed values
            Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            width = resolution.Width;
            height = resolution.Height;
            mWb.SetBounds(0, 0, width, height);

            Bitmap image = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(image);

            _RECTL bounds;
            bounds.left = 0;
            bounds.top = 0;
            bounds.right = width;
            bounds.bottom = height;

            IntPtr hdc = g.GetHdc();
            IViewObject iv = doc2 as IViewObject;

            // TODO: Write to Metafile instead if requested.

            iv.Draw(1, -1, (IntPtr)0, (IntPtr)0, (IntPtr)0,
              (IntPtr)hdc, ref bounds, (IntPtr)0, (IntPtr)0, 0);

            g.ReleaseHdc(hdc);
            image.Save(mFile);
            image.Dispose();

            if (mImageDiff != null)
            {
                mImageDiff.run();
            }

        }
    }
}
