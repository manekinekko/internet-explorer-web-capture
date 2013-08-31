Internet Explorer Screen Capture
================================

This little command-line utility is used to capture a web page into various image file formats. It uses different Internet Explorer's rendering modes (those installed on this computer). It is also able to compute a diff image output between two rendering modes.

Example
=======

<span style="display: inline-block;">
 <img src="https://raw.github.com/manekinekko/internet-explorer-web-capture/master/IEScreenCapt/bin/Debug/tmp/ref.png" title="The image used as the reference" alt="The image used as the reference" width="250px"/>
 <br/>
 <b>The ref image using the most recent IE installed (using the edge mode).</b>
</span>

<span style="display: inline-block;">
 <img src="https://raw.github.com/manekinekko/internet-explorer-web-capture/master/IEScreenCapt/bin/Debug/tmp/ie-tester-ie7.png" title="The output of the web page using the IE7 rendering engine" alt="The output of the web page using the IE7 rendering engine" width="250px"/>
 <br/>
 <b>Web capture using IE7 rendering</b>
</span>

<span style="display: inline-block;">
 <img src="https://raw.github.com/manekinekko/internet-explorer-web-capture/master/IEScreenCapt/bin/Debug/tmp/diff.png" title="The diff image" alt="The diff image" width="250px"/>
 <br/>
 <b>Diff output. Only different pixels are highlighted.</b>
</span>

Usage
=====
Open a command prompt as an administrator:
<pre>
C:\> IEScreenCapt
 -----------------------------------------------------------------------------
 Usage: IEScreenCapt -u=http://google.com
 -----------------------------------------------------------------------------
 Options:
  --url | -u             The URL to capture
  --out | -o             The target file (.png|jpeg|bmp|emf|tiff)
  --min-width | -mw      Minimal width for the image (default: 800)
  --delay | -dy          Capturing delay in ms (default: 1)
  --diff | -df           Generate and show the diff image (default: no)
 -----------------------------------------------------------------------------
</pre>

Dependencies
============
In order to run this program, you must have the <a href="http://www.microsoft.com/en-us/download/details.aspx?id=30653">.NET 4.5 framework</a> installed on your computer.

Important Notes
===============
This "up-to-date" command line utility is based on the work of Björn Höhrmann. He is the author of <a href="http://iecapt.sourceforge.net/">IECapt</a>. All credits go to him for his original work ^^.
