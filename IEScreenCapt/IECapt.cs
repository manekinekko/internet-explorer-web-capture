////////////////////////////////////////////////////////////////////
//
// IECapt# - A Internet Explorer Web Page Rendering Capture Utility
//
// Copyright (C) 2007 Bjoern Hoehrmann <bjoern@hoehrmann.de>
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
// $Id: IECapt.cs,v 1.4 2008/06/23 10:22:50 hoehrmann Exp $
//
////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Security;
using System.Security.AccessControl;
using AxSHDocVw; // Use `aximp %SystemRoot%\system32\shdocvw.dll`
using IECaptComImports;

[ComImport, Guid("0000010D-0000-0000-C000-000000000046"), InterfaceType((short)1), ComConversionLoss]
public interface IViewObject
{
    void Draw([MarshalAs(UnmanagedType.U4)] UInt32 dwDrawAspect,
              int lindex,
              IntPtr pvAspect,
              [In] IntPtr ptd,
              IntPtr hdcTargetDev,
              IntPtr hdcDraw,
              [MarshalAs(UnmanagedType.Struct)] ref _RECTL lprcBounds,
              [In] IntPtr lprcWBounds,
              IntPtr pfnContinue,
              [MarshalAs(UnmanagedType.U4)] UInt32 dwContinue);

    void RemoteGetColorSet([In] uint dwDrawAspect, [In] int lindex, [In] uint pvAspect, [In] ref tagDVTARGETDEVICE ptd, [In] uint hicTargetDev, [Out] IntPtr ppColorSet);
    void RemoteFreeze([In] uint dwDrawAspect, [In] int lindex, [In] uint pvAspect, out uint pdwFreeze);
    void Unfreeze([In] uint dwFreeze);
    void SetAdvise([In] uint aspects, [In] uint advf, [In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);
    void RemoteGetAdvise(out uint pAspects, out uint pAdvf, [MarshalAs(UnmanagedType.Interface)] out IAdviseSink ppAdvSink);
}


class IECaptUIHandler : IDocHostUIHandler
{

    public void ShowContextMenu(uint dwID, ref tagPOINT ppt, object pcmdtReserved, object pdispReserved)
    {
        // TODO: is this okay?
        throw new NotImplementedException();
    }

    public void GetHostInfo(ref _DOCHOSTUIINFO pInfo)
    {
        pInfo.cbSize = (uint)Marshal.SizeOf(pInfo);
        pInfo.dwDoubleClick = 0;
        pInfo.pchHostCss = (IntPtr)0;
        pInfo.pchHostNS = (IntPtr)0;
        pInfo.dwFlags = (uint)(0
          | tagDOCHOSTUIFLAG.DOCHOSTUIFLAG_SCROLL_NO
          | tagDOCHOSTUIFLAG.DOCHOSTUIFLAG_NO3DBORDER
          | tagDOCHOSTUIFLAG.DOCHOSTUIFLAG_NO3DOUTERBORDER
        );
    }

    public void ShowUI(uint dwID, IOleInPlaceActiveObject pActiveObject, IOleCommandTarget pCommandTarget, IOleInPlaceFrame pFrame, IOleInPlaceUIWindow pDoc)
    {
        // TODO: is this okay?
        throw new NotImplementedException();
    }

    public void HideUI()
    {
        throw new NotImplementedException();
    }

    public void UpdateUI()
    {
        throw new NotImplementedException();
    }

    public void EnableModeless(int fEnable)
    {
        throw new NotImplementedException();
    }

    public void OnDocWindowActivate(int fActivate)
    {
        throw new NotImplementedException();
    }

    public void OnFrameWindowActivate(int fActivate)
    {
        throw new NotImplementedException();
    }

    public void ResizeBorder(ref tagRECT prcBorder, IOleInPlaceUIWindow pUIWindow, int fRameWindow)
    {
        throw new NotImplementedException();
    }

    public void TranslateAccelerator(ref tagMSG lpmsg, ref Guid pguidCmdGroup, uint nCmdID)
    {
        throw new NotImplementedException();
    }

    public void GetOptionKeyPath(out string pchKey, uint dw)
    {
        pchKey = null;
        throw new NotImplementedException();
    }

    public void GetDropTarget(IDropTarget pDropTarget, out IDropTarget ppDropTarget)
    {
        ppDropTarget = null;
        throw new NotImplementedException();
    }

    public void GetExternal(out object ppDispatch)
    {
        ppDispatch = null;
        throw new NotImplementedException();
    }


    public void TranslateUrl(uint dwTranslate, ref ushort pchURLIn, IntPtr ppchURLOut)
    {
        // TODO: is this okay?
        throw new NotImplementedException();
    }

    public void FilterDataObject(IDataObject pDO, out IDataObject ppDORet)
    {
        ppDORet = null;
        // TODO: is this okay?
        throw new NotImplementedException();
    }

}

class ImageDiff
{
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

    public void run(string fname1, string fname2)
    {

        //Console.WriteLine(File.Exists(fname1).ToString()+":"+File.Exists(fname2).ToString());

        Bitmap img1 = new Bitmap(fname1); // image de reference
        Bitmap img2 = new Bitmap(fname2);
        Bitmap bm = this.Diff(img1, img2);
        string file = System.Windows.Forms.Application.StartupPath + @"\\tmp\\diff.png";
        bm.Save(file);
        bm.Dispose();
        try
        {
            // open image
            System.Diagnostics.Process.Start(file);
        }
        catch (Exception e) { }
    }

    //private Color GetPixel(int x, int y)
    //{
    //    Color clr = Color.Empty;
    //    int cCount = 24 / 8; // 24 bpp
    //    int i = ((y * this.Width) + x) * cCount;

    //    if (i > this.Pixels.Length - cCount)
    //    {
    //        throw new IndexOutOfRangeException();
    //    }

    //    byte b = this.Pixels[i];
    //    byte g = this.Pixels[i + 1];
    //    byte r = this.Pixels[i + 2];
    //    clr = Color.FromArgb(r, g, b);

    //    return clr;
    //}

    //private void SetPixel(int x, int y, Color color)
    //{
    //    int cCount = 24 / 8; // 24 bpp
    //    int i = ((y * this.Width) + x) * cCount;

    //    this.Pixels[i] = color.B;
    //    this.Pixels[i + 1] = color.G;
    //    this.Pixels[i + 2] = color.R;
    //}

}

class IECaptForm : System.Windows.Forms.Form
{
    private string mURL;
    private string mFile;
    private int mMinWidth;
    private AxWebBrowser mWb;
    public System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();
    private bool mDiff;

    public IECaptForm(string url, string file, int minWidth, int delay, AxWebBrowser wb, bool diff)
    {
        mURL = url;
        mFile = file;
        mMinWidth = minWidth;
        mTimer.Interval = delay;
        mTimer.Tick += new EventHandler(mTimer_Tick);
        mWb = wb;
        mDiff = diff;
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

        if (mDiff)
        {
            //System.IO.Path.GetTempPath()
            string reference = System.Windows.Forms.Application.StartupPath + @"\tmp\ref.png";
            (new ImageDiff()).run(reference, mFile);
        }

    }

}

class IECapt
{
    static void PrintUsage()
    {
        Console.WriteLine("Usage: IEScreenCapt -u=http://... [options]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --url | -u             The URL to capture");
        Console.WriteLine("  --out | -o             The target file (.png|jpeg|bmp|emf|tiff)");
        Console.WriteLine("  --min-width | -mw      Minimal width for the image (default: 800)");
        Console.WriteLine("  --delay | -dy           Capturing delay in ms (default: 1)");
        Console.WriteLine("  --diff | -df           Generate and show the diff image (default: no)");
        // Console.WriteLine("  --max-height        Maximal height to capture (default: 0)");
        // Console.WriteLine("" --user-style        Path to user style sheet (.css) file");

    }

    [STAThread]
    static void Main(string[] args)
    {
        string URL = null;
        string file = null;
        int minWidth = 800;
        int delay = 1;
        string mode = "edge";
        bool diff = false;

        if (args.Length == 0)
        {
            PrintUsage();
            return;
        }

        // Parse command line parameters
        foreach (string arg in args)
        {
            string[] tmp = arg.Split(new char[] { '=' }, 2);

            if (tmp.Length < 2)
            {
                PrintUsage();
                return;
            }
            else if (tmp[0].Equals("-u") || tmp[0].Equals("--url"))
            {
                URL = tmp[1];
            }
            else if (tmp[0].Equals("-o") || tmp[0].Equals("--out"))
            {
                file = tmp[1]; ;
            }
            else if (tmp[0].Equals("-mw") || tmp[0].Equals("--min-width"))
            {
                minWidth = int.Parse(tmp[1]);
            }
            else if (tmp[0].Equals("-dy") || tmp[0].Equals("--delay"))
            {
                delay = int.Parse(tmp[1]);
            }
            else if (tmp[0].Equals("-df") || tmp[0].Equals("--diff"))
            {
                diff = (tmp[1].ToString() == "yes");
            }
            else if (tmp[0].Equals("-ie") || tmp[0].Equals("--internet-explorer"))
            {
                Dictionary<string, int> dic = new Dictionary<string, int>();
                dic.Add("quirks", 0x1388);
                dic.Add("7", 0x1b58);
                dic.Add("8", 0x1f40);
                dic.Add("8+", 0x22b8);
                dic.Add("9", 0x2328);
                dic.Add("9+", 0x270f);
                dic.Add("10", 0x3e8);

                object o = "edge";
                string IEversion = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\Version Vector", "IE", o).ToString();

                Int32 value = 0;
                mode = tmp[1];

                if (mode == "edge") {
                    value = dic[IEversion.Split('.')[0]];
                }
                else if (dic.ContainsKey(mode)) {
                    value = dic[mode];
                }

                try
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", Path.GetFileName(System.Windows.Forms.Application.ExecutablePath), value, RegistryValueKind.DWord);
                }
                catch (System.UnauthorizedAccessException e)
                {
                    Console.Error.WriteLine(e.Message.ToString());
                    return;
                }
            }
            else
            {
                Console.WriteLine("Warning: unknown parameter {0}", tmp[0]);
            }
        }

        if (delay < 1 || URL == null)
        {
            PrintUsage();
            return;
        }

        if (String.IsNullOrEmpty(file))
        {
            file = "./" + URL.ToLower().Replace("http://", "").Replace("/", "-") + "-" + mode + ".png";
        }


        AxWebBrowser wb = new AxWebBrowser();
        //System.Windows.Forms.WebBrowser wb = new System.Windows.Forms.WebBrowser();
        System.Windows.Forms.Form main = new IECaptForm(URL, file, minWidth, delay, wb, diff);

        wb.BeginInit();
        wb.Parent = main;
        wb.EndInit();

        // Set the initial dimensions of the browser's client area.
        wb.SetBounds(0, 0, minWidth, 600);

        object oBlank = "about:blank";
        object oURL = URL;
        object oNull = String.Empty;

        // Internet Explorer should show no dialog boxes; this does not dis-
        // able script debugging however, I am not aware of a method to dis-
        // able that, other than manual configuration in he Internet Settings
        // or perhaps the registry.
        wb.Silent = true;

        // The custom UI handler can only be registered on a document, so we
        // navigate to about:blank as a first step, then register the handler.
        wb.Navigate2(ref oBlank, ref oNull, ref oNull, ref oNull, ref oNull);

        ICustomDoc cdoc = wb.Document as ICustomDoc;
        cdoc.SetUIHandler(new IECaptUIHandler());

        // Register a document complete handler. It will be called whenever a
        // document completes loading, including embedded documents and the
        // initial about:blank document.
        wb.DocumentComplete +=
          new DWebBrowserEvents2_DocumentCompleteEventHandler(IE_DocumentComplete);

        // Register an error handler. If the main document cannot be loaded,
        // the document complete event will not fire, so we have to listen to
        // this and shut the application down in case of a fatal error.
        wb.NavigateError +=
          new DWebBrowserEvents2_NavigateErrorEventHandler(IE_NavigateError);

        // Now navigate to the final destination.
        wb.Navigate2(ref oURL, ref oNull, ref oNull, ref oNull, ref oNull);

        System.Windows.Forms.Application.Run();

    }

    private static void IE_DocumentComplete(object sender,
      DWebBrowserEvents2_DocumentCompleteEvent e)
    {

        AxWebBrowser wb = (AxWebBrowser)sender;
        IECaptForm main = (IECaptForm)wb.Parent;

        // Skip document complete event for embedded frames.
        if (wb.Application != e.pDisp)
            return;

        // Skip the initial about:blank document; this is not necessarily
        // the best thing to do, e.g. if the requested page is about:blank
        // or redirects to it, we might never exit. This could be avoided
        // by remembering whether we saw the first document complete event.
        if (e.uRL.Equals("about:blank"))
            return;

        main.mTimer.Start();
    }

    private static void IE_NavigateError(object sender, DWebBrowserEvents2_NavigateErrorEvent e)
    {
        AxWebBrowser wb = (AxWebBrowser)sender;
        IECaptForm main = (IECaptForm)wb.Parent;

        // Ignore errors for embedded documents
        if (wb.Application != e.pDisp)
            return;

        // If we get here, the main document cannot be navigated 
        // to meaning there is nothing to draw, so we just croak.
        Console.Error.WriteLine("Failed to navigate to {0} (0x{1:X08})",
          e.uRL, e.statusCode);

        wb.Dispose();
        System.Windows.Forms.Application.Exit();
    }
}