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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Security;
using System.Security.AccessControl;
using AxSHDocVw;
using IECaptComImports;
using IEScreenCapt;
using IEScreenCapt.Classes;

class IECapt
{
    static void PrintUsage()
    {
        Console.WriteLine("Usage: IEScreenCapt -u=http://... [options]");
        Console.WriteLine();
        Console.WriteLine("Options:");

        Console.WriteLine("  --url | -u             The URL to capture");
        Console.WriteLine("  --out | -o             The target file (default: %tmp%/<url>.png)");
        Console.WriteLine("  --reference | -rf      The reference file (default: <empty>)");
        Console.WriteLine("  --min-width | -mw      Minimal width for the image (default: 800)");
        Console.WriteLine("  --max-height | -mh     Maximal height to capture (default: 0)");
        Console.WriteLine("  --diff | -df           Generate and show the diff image (default: no)");
        Console.WriteLine("  --delay | -dy          Capturing delay in ms (default: 1)");
        Console.WriteLine("  --silent | -s          Launch IE in silent (default: yes)");
        Console.WriteLine("  --debug | -dg          Set the debug output (default: no)");
    }

    [STAThread]
    static void Main(string[] args)
    {
        string URL = null;
        string file = null;
        int minWidth = 800;
        int minHeight = 600;
        int delay = 1;
        string mode = "edge";
        bool diff = false;
        string reference = null;
        string tmpPath = System.IO.Path.GetTempPath()+@"IECapt\";
        Directory.CreateDirectory(tmpPath);
        bool debug = false;
        bool silent = false;
        string IEversion = "";

        string debugMessage = "";

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
            else if (tmp[0].Equals("-rf") || tmp[0].Equals("--reference"))
            {
                reference = tmp[1]; ;
            }
            else if (tmp[0].Equals("-mw") || tmp[0].Equals("--min-width"))
            {
                minWidth = int.Parse(tmp[1]);
            }
            else if (tmp[0].Equals("-mh") || tmp[0].Equals("--min-height"))
            {
                minHeight = int.Parse(tmp[1]);
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
                IEversion = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\Version Vector", "IE", o).ToString();

                Int32 value = 0;
                mode = tmp[1];

                if (mode == "edge") {
                    value = dic[IEversion.Split('.')[0]];
                }
                else if (dic.ContainsKey(mode)) {
                    value = dic[mode];
                    IEversion = value.ToString();
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
            else if (tmp[0].Equals("-dg") || tmp[0].Equals("--debug"))
            {
                debug = (tmp[1].ToString() == "yes");
            }
            else if (tmp[0].Equals("-s") || tmp[0].Equals("--silent"))
            {
                silent = (tmp[1].ToString() == "yes");
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
            file = tmpPath + URL.ToLower().Replace("http://", "").Replace("/", "-") + "-" + mode + ".png";
        }

        Debug dbg = new Debug(debug);
        debugMessage += "URL: " + URL + "\n";
        debugMessage += "Output: " + file + "{"+(File.Exists(reference))+"}\n";
        debugMessage += "Reference: " + reference + "{"+(File.Exists(reference))+"}\n";
        debugMessage += "Min-width: " + minWidth + "px\n";
        debugMessage += "Min-height: " + minHeight + "px\n";
        debugMessage += "Delay: " + delay + "ms\n";
        debugMessage += "Diff: " + diff + "\n";
        debugMessage += "IE Mode: " + IEversion + "\n";
        debugMessage += "IE Silent Mode: " + silent + "\n";
        dbg.log(debugMessage);

        ImageDiff imageDiff = null;
        if (reference != null) {
            imageDiff = new ImageDiff(reference, file);        
        }
        AxWebBrowser wb = new AxWebBrowser();

        System.Windows.Forms.Form main = new IECaptForm(URL, file, minWidth, delay, wb, imageDiff);

        wb.BeginInit();
        wb.Parent = main;
        wb.EndInit();

        // Set the initial dimensions of the browser's client area.
        wb.SetBounds(0, 0, minWidth, minHeight);

        object oBlank = "about:blank";
        object oURL = URL;
        object oNull = String.Empty;

        // Internet Explorer should show no dialog boxes; this does not dis-
        // able script debugging however, I am not aware of a method to dis-
        // able that, other than manual configuration in he Internet Settings
        // or perhaps the registry.
        wb.Silent = silent;

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