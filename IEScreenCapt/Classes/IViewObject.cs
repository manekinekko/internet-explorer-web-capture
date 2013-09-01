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
}
