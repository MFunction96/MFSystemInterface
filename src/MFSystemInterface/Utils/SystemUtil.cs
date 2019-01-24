using System;
using System.Data.SqlTypes;
using System.Management;
using System.Runtime.InteropServices;
using MFSystemInterface.Services.PInvoke;
using MFSystemInterface.Services.PInvoke.Enums;

namespace MFSystemInterface.Utils
{
    public static class SystemUtil
    {
        #region Memory

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IntPtr CopyMemoryEx<TType>(TType source)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(source));
            Marshal.StructureToPtr(source, ptr, false);
            return ptr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IntPtr CopyMemoryEx(IntPtr source)
        {
            if (source == IntPtr.Zero) return IntPtr.Zero;
            var ptr = Marshal.AllocHGlobal(source);
            NativeMethods.CopyMemory(ptr, source, Marshal.SizeOf(source));
            return ptr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intPtr"></param>
        /// <returns></returns>
        public static bool FreeMemoryEx(IntPtr intPtr)
        {
            if (intPtr == IntPtr.Zero) return false;
            Marshal.FreeHGlobal(intPtr);
            return true;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        public static void KeepScreenOn(bool flag)
        {
            if (flag)
            {
                NativeMethods.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS |
                                                      EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                                                      EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
            }
            else
            {
                NativeMethods.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        public static void BlockInput(bool flag)
        {
            NativeMethods.BlockInput(flag);
            //var result = NativeMethods.GetLastError();
            //if (result != (int) ERROR_CODE.ERROR_SUCCESS) throw new SystemException($"Error Code : {result}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetBiosSerial()
        {
            var com_serial = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            var result = string.Empty;
            foreach (var o in com_serial.Get())
            {
                try
                {
                    if (!(o is ManagementObject wmi)) continue;
                    result = wmi.GetPropertyValue("SerialNumber").ToString();
                    if (result != string.Empty) break;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            if (result == string.Empty) throw new SqlNullValueException(nameof(GetBiosSerial));
            return result;
        }

    }
}
