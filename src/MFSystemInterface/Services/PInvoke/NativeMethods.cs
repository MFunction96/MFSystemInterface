using MFSystemInterface.Services.PInvoke.Enums;
using System;
using System.Runtime.InteropServices;
using System.Text;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace MFSystemInterface.Services.PInvoke
{
    /// <summary>
    /// 平台调用方法访问非托管代码方法
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// 写入注册表数据。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regsetvalueexw。
        /// </summary>
        /// <param name="hKey">
        /// 已打开注册表句柄。
        /// </param>
        /// <param name="lpValueName">
        /// 注册表键名。
        /// </param>
        /// <param name="lpReserved">
        /// 保留参数，必须为0。
        /// </param>
        /// <param name="dwType">
        /// 注册表键值类型。
        /// </param>
        /// <param name="lpData">
        /// 注册表键值。
        /// </param>
        /// <param name="lpcbData">
        /// 注册表键值占用空间大小。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegSetValueExW")]
        internal static extern int RegSetValueEx(
            IntPtr hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string lpValueName,
            int lpReserved,
            int dwType,
            [MarshalAs(UnmanagedType.LPWStr)] string lpData,
            int lpcbData);

        /// <summary>
        /// 写入注册表数据。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regsetvalueexw。
        /// </summary>
        /// <param name="hKey">
        /// 已打开注册表句柄。
        /// </param>
        /// <param name="lpValueName">
        /// 注册表键名。
        /// </param>
        /// <param name="lpReserved">
        /// 保留参数，必须为0。
        /// </param>
        /// <param name="dwType">
        /// 注册表键值类型。
        /// </param>
        /// <param name="lpData">
        /// 注册表键值。
        /// </param>
        /// <param name="lpcbData">
        /// 注册表键值占用空间大小。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegSetValueExW")]
        internal static extern int RegSetValueEx(
            IntPtr hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string lpValueName,
            int lpReserved,
            int dwType,
            IntPtr lpData,
            int lpcbData);

        /// <summary>
        /// 写入注册表数据。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regsetvalueexw。
        /// </summary>
        /// <param name="hKey">
        /// 已打开注册表句柄。
        /// </param>
        /// <param name="lpValueName">
        /// 注册表键名。
        /// </param>
        /// <param name="lpReserved">
        /// 保留参数，必须为0。
        /// </param>
        /// <param name="dwType">
        /// 注册表键值类型。
        /// </param>
        /// <param name="lpData">
        /// 注册表键值。
        /// </param>
        /// <param name="lpcbData">
        /// 注册表键值占用空间大小。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegSetValueExW")]
        internal static extern int RegSetValueEx(
            IntPtr hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string lpValueName,
            int lpReserved,
            int dwType,
            byte[] lpData,
            int lpcbData);

        /// <summary>
        /// 获取注册表键值信息。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regqueryvalueexw。
        /// </summary>
        /// <param name="hKey">
        /// 已打开的注册表句柄。
        /// </param>
        /// <param name="lpValueName">
        /// 注册表键名。
        /// </param>
        /// <param name="lpReserved">
        /// 保留参数，必须为0。
        /// </param>
        /// <param name="lpType">
        /// 注册表键值类型。
        /// </param>
        /// <param name="lpData">
        /// 注册表键值。
        /// </param>
        /// <param name="lpcbData">
        /// 注册表键值占用空间大小。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW")]
        internal static extern int RegQueryValueEx(
            IntPtr hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string lpValueName,
            IntPtr lpReserved,
            out int lpType,
            IntPtr lpData,
            ref int lpcbData);

        /// <summary>
        /// 获取注册表键值信息。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regqueryvalueexw。
        /// </summary>
        /// <param name="hKey">
        /// 已打开的注册表句柄。
        /// </param>
        /// <param name="lpValueName">
        /// 注册表键名。
        /// </param>
        /// <param name="lpReserved">
        /// 保留参数，必须为0。
        /// </param>
        /// <param name="lpType">
        /// 注册表键值类型。
        /// </param>
        /// <param name="lpData">
        /// 注册表键值。
        /// </param>
        /// <param name="lpcbData">
        /// 注册表键值占用空间大小。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW")]
        internal static extern int RegQueryValueEx(
            IntPtr hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string lpValueName,
            IntPtr lpReserved,
            out int lpType,
            StringBuilder lpData,
            ref int lpcbData);

        /// <summary>
        /// 创建并打开注册表键。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regcreatekeyexw。
        /// </summary>
        /// <param name="hKey">
        /// 注册表根键。
        /// </param>
        /// <param name="lpSubKey">
        /// 注册表子键。
        /// </param>
        /// <param name="lpReserved">
        /// 保留参数，必须为0。
        /// </param>
        /// <param name="lpClass">
        /// 用户定义注册表类。
        /// </param>
        /// <param name="dwOptions">
        /// 注册表创建选项。
        /// </param>
        /// <param name="samDesired">
        /// 注册表访问权限。
        /// </param>
        /// <param name="lpSecurityAttributes">
        /// 注册表安全访问标识符。
        /// </param>
        /// <param name="phkResult">
        /// 注册表键句柄。
        /// </param>
        /// <param name="lpdwDisposition">
        /// 注册表操作类型。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegCreateKeyExW")]
        internal static extern int RegCreateKeyEx(
            IntPtr hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string lpSubKey,
            int lpReserved,
            [MarshalAs(UnmanagedType.LPWStr)] string lpClass,
            int dwOptions,
            int samDesired,
            IntPtr lpSecurityAttributes,
            out IntPtr phkResult,
            out int lpdwDisposition);

        /// <summary>
        /// 打开注册表键。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regopenkeyexw。
        /// </summary>
        /// <param name="hKey">
        /// 注册表根键。
        /// </param>
        /// <param name="lpSubKey">
        /// 注册表子键。
        /// </param>
        /// <param name="ulOptions">
        /// 注册表打开选项。
        /// </param>
        /// <param name="samDesired">
        /// 注册表访问权限。
        /// </param>
        /// <param name="phkResult">
        /// 注册表键句柄。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegOpenKeyExW")]
        internal static extern int RegOpenKeyEx(
            IntPtr hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string lpSubKey,
            int ulOptions,
            int samDesired,
            out IntPtr phkResult);

        /// <summary>
        /// 删除注册表键。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regdeletekeyexw。
        /// </summary>
        /// <param name="hKey">
        /// 注册表根键。
        /// </param>
        /// <param name="lpSubKey">
        /// 注册表子键。
        /// </param>
        /// <param name="samDesired">
        /// 注册表访问权限。
        /// </param>
        /// <param name="lpReserved">
        /// 保留参数，必须为0。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegDeleteKeyExW")]
        internal static extern int RegDeleteKeyEx(
            IntPtr hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string lpSubKey,
            int samDesired,
            int lpReserved);

        /// <summary>
        /// 删除注册表键值。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regdeletevaluew。
        /// </summary>
        /// <param name="hKey">
        /// 已打开的注册表句柄。
        /// </param>
        /// <param name="lpValueName">
        /// 注册表键名。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegDeleteValueW")]
        internal static extern int RegDeleteValue(
            IntPtr hKey,
            [MarshalAs(UnmanagedType.LPWStr)] string lpValueName);

        /// <summary>
        /// 枚举当前子键下所有子键，详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regenumkeyexw。
        /// </summary>
        /// <param name="hKey">
        /// 已打开的注册表句柄。
        /// </param>
        /// <param name="dwIndex">
        /// 注册表子键索引。
        /// </param>
        /// <param name="lpName">
        /// 注册表子键名称。
        /// </param>
        /// <param name="lpcName">
        /// 注册表子键名称占用空间大小。
        /// </param>
        /// <param name="lpReserved">
        /// 保留参数，必须为0。
        /// </param>
        /// <param name="lpClass">
        /// 用户定义注册表类。
        /// </param>
        /// <param name="lpcClass">
        /// 用户定义注册表类占用空间大小。
        /// </param>
        /// <param name="lpftLastWriteTime">
        /// 最后一次修改时间
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegEnumKeyExW")]
        internal static extern int RegEnumKeyEx(
            IntPtr hKey,
            int dwIndex,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpName,
            ref int lpcName,
            IntPtr lpReserved,
            IntPtr lpClass,
            IntPtr lpcClass,
            out FILETIME lpftLastWriteTime);

        /// <summary>
        /// 枚举当前子键下所有键名。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regenumvaluew。
        /// </summary>
        /// <param name="hKey">
        /// 已打开的注册表句柄。
        /// </param>
        /// <param name="dwIndex">
        /// 注册表键名索引。
        /// </param>
        /// <param name="lpValueName">
        /// 注册表键名。
        /// </param>
        /// <param name="lpcchValueName">
        /// 注册表键名占用空间大小。
        /// </param>
        /// <param name="lpReserved">
        /// 保留参数，必须为0。
        /// </param>
        /// <param name="lpType">
        /// 注册表键值类型。
        /// </param>
        /// <param name="lpData">
        /// 注册表枚举键值。
        /// </param>
        /// <param name="lpcbData">
        /// 注册表键值占用空间大小。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegEnumValueW")]
        internal static extern int RegEnumValue(
            IntPtr hKey,
            int dwIndex,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpValueName,
            ref int lpcchValueName,
            IntPtr lpReserved,
            out int lpType,
            IntPtr lpData,
            ref int lpcbData);

        /// <summary>
        /// 关闭注册表句柄。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/winreg/nf-winreg-regclosekey。
        /// </summary>
        /// <param name="hKey">
        /// 已打开的注册表句柄。
        /// </param>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern int RegCloseKey(IntPtr hKey);

        /// <summary>
        /// 返回最后一次错误代码。详情参阅：https://msdn.microsoft.com/en-us/library/windows/desktop/ms679360(v=vs.85).aspx
        /// </summary>
        /// <returns>
        /// Windows错误代码，详情参阅MSDN。
        /// </returns>
        [DllImport("kernel32.dll")]
        internal static extern int GetLastError();

        /// <summary>
        /// 打开进程相关访问权限句柄。详情参阅：https://docs.microsoft.com/en-us/windows/desktop/api/processthreadsapi/nf-processthreadsapi-openprocesstoken
        /// </summary>
        /// <param name="processHandle">
        /// 已打开访问权限的进程句柄。
        /// </param>
        /// <param name="desiredAccess">
        /// 
        /// </param>
        /// <param name="tokenHandle">
        ///
        /// </param>
        /// <returns>
        ///
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenHandle">
        ///
        /// </param>
        /// <param name="tokenInformationClass">
        ///
        /// </param>
        /// <param name="tokenInformation">
        ///
        /// </param>
        /// <param name="tokenInformationLength">
        ///
        /// </param>
        /// <param name="returnLength">
        ///
        /// </param>
        /// <returns>
        ///
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool GetTokenInformation(IntPtr tokenHandle, TOKEN_INFORMATION_CLASS tokenInformationClass, IntPtr tokenInformation, uint tokenInformationLength, out uint returnLength);

    }
}
