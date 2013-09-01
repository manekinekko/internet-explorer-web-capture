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
////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using IECaptComImports;

namespace IEScreenCapt.Classes
{
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
}
