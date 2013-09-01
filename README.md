Internet Explorer Screen Capture
================================

This little command-line utility is used to capture a web page into various image file formats. It uses different Internet Explorer's rendering modes (those installed on this computer). It is also able to compute a diff image output between two rendering modes.

Example
=======

<span style="display: inline-block;">
 <img src="https://raw.github.com/manekinekko/internet-explorer-web-capture/master/Samples/yahoo.com-edge.png" title="The image used as the reference" alt="The image used as the reference" width="250px"/>
 <br/>
 <b>The reference image captured using the most recent IE installed.</b>
</span>

<span style="display: inline-block;">
 <img src="https://raw.github.com/manekinekko/internet-explorer-web-capture/master/Samples/yahoo.com-7-diff.png" title="The output of the web page using the IE7 rendering engine" alt="The output of the web page using the IE7 rendering engine" width="250px"/>
 <br/>
 <b>The diff capture using the IE7 rendering. Only the different pixels are highlighted</b>
</span>

<span style="display: inline-block;">
 <img src="https://raw.github.com/manekinekko/internet-explorer-web-capture/master/Samples/yahoo.com-8-diff.png" title="The output of the web page using the IE8 rendering engine" alt="The output of the web page using the IE8 rendering engine" width="250px"/>
 <br/>
 <b>The diff capture using the IE8 rendering. Only the different pixels are highlighted.</b>
</span>

Usage
=====
Open a command prompt as an administrator:
<pre>
C:\> IEScreenCapt.exe
Usage: IEScreenCapt -u=http://... [options]

Options:
  --url | -u             The URL to capture
  --out | -o             The target file (default: %tmp%/<url>.png)
  --reference | -rf      The reference file (default: <empty>)
  --min-width | -mw      Minimal width for the image (default: 800)
  --max-height | -mh     Maximal height to capture (default: 0)
  --diff | -df           Generate and show the diff image (default: no)
  --delay | -dy          Capturing delay in ms (default: 1)
  --silent | -s          Launch IE in silent (default: yes)
  --debug | -dg          Set the debug output (default: no)
</pre>

Here is a simple BAT file:
```bat
@echo off
set url=%1
set ref="%tmp%/IECapt/%url%-edge.png"

rem you need to generate a first image wich will be used as a reference (generated only once)
rem because this will be the ref image, no need the run a diff operation
if not exist %ref% IEScreenCapt.exe -u=%url% -ie="edge" -df="no" -dg="no" -s="yes"

rem capture the url in IE7 mode, and generate a diff output using the ref image
IEScreenCapt.exe -u=%url% -ie="7" -df="yes" -rf=%ref% -dg="yes" -s="yes"
```

The BAT file is used as follow:
<pre>
C:\> capture.bat yahoo.com
</pre>

And this is the output of the debug log:
<pre>
URL: yahoo.com
Output: <your temp dir>/IECapt/yahoo.com-7.png {True}
Reference: <your temp dir>/IECapt/yahoo.com-edge.png {True}
Min-width: 800px
Min-height: 600px
Delay: 1ms
Diff: True
IE Mode: 7000
IE Silent Mode: True
</pre>

Dependencies
============
In order to run this program, you must have the <a href="http://www.microsoft.com/en-us/download/details.aspx?id=30653">.NET 4.5 framework</a> installed on your computer.

Important Notes
===============
This "up-to-date" command line utility is based on the work of Björn Höhrmann. He is the author of <a href="http://iecapt.sourceforge.net/">IECapt</a>. All credits go to him for his original work ^^.
