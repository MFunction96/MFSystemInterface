﻿using System;
using System.Runtime.InteropServices;
// ReSharper disable IdentifierTypo

// ReSharper disable InconsistentNaming

namespace MFSystemInterface.Services.PInvoke.Structures
{
    /// <summary>
    /// 进程启动选项结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct STARTUPINFO
    {
        public int cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }
}
