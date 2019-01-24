using MFSystemInterface.Services.PInvoke;
using MFSystemInterface.Services.PInvoke.Structures;
using System;
using System.Runtime.InteropServices;
using MFSystemInterface.Utils;

namespace MFSystemInterface.Models.Process
{
    /// <inheritdoc />
    /// <summary>
    /// Windows API方式控制的进程类。
    /// </summary>
    public class ProcessEx : IDisposable
    {
        #region Properties

        /// <summary>
        /// 应用程序所在详细位置。
        /// </summary>
        public string AppPath { get; set; }

        /// <summary>
        /// 应用程序运行参数。
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// 进程运行环境路径。
        /// </summary>
        public string LaunchDirectory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public STARTUPINFO StartupInfo;

        /// <summary>
        /// 
        /// </summary>
        public PROCESS_INFORMATION ProcessInformation;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ProcessEx()
        {
            StartupInfo = new STARTUPINFO();
            ProcessInformation = new PROCESS_INFORMATION();
            LaunchDirectory = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start(int milliseconds = 0)
        {
            if (string.IsNullOrEmpty(AppPath)) throw new InvalidOperationException();
            var cmd = $"{AppPath} {Arguments}";
            var sap = new SECURITY_ATTRIBUTES
            {
                lpSecurityDescriptor = IntPtr.Zero,
                bInheritHandle = true
            };
            sap.nLength = Marshal.SizeOf(sap);
            var ptr_sap = SystemUtil.CopyMemoryEx(sap);
            var sat = new SECURITY_ATTRIBUTES
            {
                lpSecurityDescriptor = IntPtr.Zero,
                bInheritHandle = true
            };
            sat.nLength = Marshal.SizeOf(sat);
            var ptr_sat = SystemUtil.CopyMemoryEx(sat);
            if (!NativeMethods.CreateProcess(null, cmd, ptr_sap, ptr_sat, true, 0, IntPtr.Zero, LaunchDirectory,
                ref StartupInfo, out ProcessInformation))
                throw new Exception($"Launch Process Failed!\nError Code: {NativeMethods.GetLastError()}");

            SystemUtil.FreeMemoryEx(ptr_sat);
            SystemUtil.FreeMemoryEx(ptr_sap);

            if (milliseconds > 0) WaitForExit(milliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds"></param>
        protected void WaitForExit(int milliseconds)
        {
            NativeMethods.WaitForSingleObjectEx(ProcessInformation.hProcess, 0, false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
        }
        /// <summary>
        /// 
        /// </summary>
        ~ProcessEx()
        {
            Dispose(false);
        }
    }
}
