using MFSystemInterface.Services.PInvoke;
using MFSystemInterface.Services.PInvoke.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MFSystemInterface.Models.Registry
{
    /// <inheritdoc cref="IComparable"/>
    /// <inheritdoc cref="ICloneable" />
    /// <summary>
    /// 注册表路径信息类。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class RegPath : ICloneable, IComparable, IDisposable
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, REG_ROOT_KEY> RegRootKeys { get; }
        /// <summary>
        /// 
        /// </summary>
        protected static Dictionary<REG_ROOT_KEY, string> ResRegRootKeys { get; }

        /// <summary>
        /// 注册表根键。
        /// </summary>
        public REG_ROOT_KEY HKey { get; set; }
        /// <summary>
        /// 注册表子键。
        /// </summary>
        public string LpSubKey { get; set; }
        /// <summary>
        /// 注册表键名。
        /// </summary>
        public string LpValueName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Is64BitRegistry { get; set; }
        #endregion

        #region Construction
        /// <summary>
        /// 
        /// </summary>
        static RegPath()
        {
            RegRootKeys = new Dictionary<string, REG_ROOT_KEY>
            {
                ["HKEY_CLASSES_ROOT"] = REG_ROOT_KEY.HKEY_CLASSES_ROOT,
                ["HKEY_CURRENT_USER"] = REG_ROOT_KEY.HKEY_CURRENT_USER,
                ["HKEY_LOCAL_MACHINE"] = REG_ROOT_KEY.HKEY_LOCAL_MACHINE,
                ["HKEY_USERS"] = REG_ROOT_KEY.HKEY_USERS,
                ["HKEY_PERFORMANCE_DATA"] = REG_ROOT_KEY.HKEY_PERFORMANCE_DATA,
                ["HKEY_CURRENT_CONFIG"] = REG_ROOT_KEY.HKEY_CURRENT_CONFIG,
                ["HKEY_DYN_DATA"] = REG_ROOT_KEY.HKEY_DYN_DATA
            };
            ResRegRootKeys = new Dictionary<REG_ROOT_KEY, string>
            {
                [REG_ROOT_KEY.HKEY_CLASSES_ROOT] = "HKEY_CLASSES_ROOT",
                [REG_ROOT_KEY.HKEY_CURRENT_USER] = "HKEY_CURRENT_USER",
                [REG_ROOT_KEY.HKEY_LOCAL_MACHINE] = "HKEY_LOCAL_MACHINE",
                [REG_ROOT_KEY.HKEY_USERS] = "HKEY_USERS",
                [REG_ROOT_KEY.HKEY_PERFORMANCE_DATA] = "HKEY_PERFORMANCE_DATA",
                [REG_ROOT_KEY.HKEY_CURRENT_CONFIG] = "HKEY_CURRENT_CONFIG",
                [REG_ROOT_KEY.HKEY_DYN_DATA] = "HKEY_DYN_DATA"
            };
        }
        /// <summary>
        /// 注册表路径信息类序列化构造函数。
        /// </summary>
        public RegPath()
        {

        }

        /// <summary>
        /// 注册表路径信息类构造函数。
        /// </summary>
        /// <param name="path">
        /// 注册表路径信息。
        /// </param>
        /// <param name="is64BitRegistry"></param>
        /// <param name="refMark">
        /// 是否为字符串引用，即包含双引号、括号等。
        /// </param>
        public RegPath(string path, bool is64BitRegistry = false, bool refMark = false)
        {
            if (refMark) path = path.Substring(1, path.Length - 2);
            Is64BitRegistry = is64BitRegistry;
            var index1 = path.IndexOf('\\');
            var index2 = path.LastIndexOf('\\');
            HKey = RegRootKeys[path.Substring(0, index1)];
            if (index1 == index2)
            {
                LpSubKey = index1 == -1 ? string.Empty : path.Substring(index1 + 1, index2 - index1 - 1);
                LpValueName = string.Empty;
            }
            else
            {
                LpSubKey = path.Substring(index1 + 1, index2 - index1 - 1);
                LpValueName = path.Substring(index2 + 1, path.Length - index2 - 1);
            }
        }

        /// <summary>
        /// 注册表路径信息类构造函数。
        /// </summary>
        /// <param name="hKey">
        /// 注册表根键。
        /// </param>
        /// <param name="lpSubKey">
        /// 注册表子键。
        /// </param>
        /// <param name="lpValueName">
        /// 注册表键名。
        /// </param>
        /// <param name="is64BitRegistry"></param>
        public RegPath(REG_ROOT_KEY hKey, string lpSubKey, string lpValueName = "", bool is64BitRegistry = false)
        {
            HKey = hKey;
            LpSubKey = lpSubKey;
            LpValueName = lpValueName;
        }

        /// <summary>
        /// 注册表路径信息类复制构造函数。
        /// </summary>
        /// <param name="regPath">
        /// 注册表路径信息类。
        /// </param>
        public RegPath(RegPath regPath)
        {
            HKey = regPath.HKey;
            LpSubKey = regPath.LpSubKey;
            LpValueName = regPath.LpValueName;
        }
        #endregion

        #region Methods

        #region Implement

        /// <inheritdoc />
        /// <summary>
        /// 获取当前对象的深表副本。
        /// </summary>
        /// <returns>
        /// 当前对象的深表副本。
        /// </returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <inheritdoc />
        /// <summary>
        /// 注册表路径信息类默认排序规则。
        /// </summary>
        /// <param name="obj">
        /// 待比较的对象。
        /// </param>
        /// <returns>
        /// 大小比较结果。
        /// </returns>
        public int CompareTo(object obj)
        {
            if (!(obj is RegPath regpath)) throw new NullReferenceException();
            if (HKey < regpath.HKey) return 1;
            if (HKey > regpath.HKey) return -1;
            var flag = string.CompareOrdinal(LpSubKey, regpath.LpSubKey);
            return flag != 0 ? flag : string.CompareOrdinal(LpValueName, LpValueName);
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
            //if (!disposing) return;

        }
        /// <summary>
        /// 
        /// </summary>
        ~RegPath()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = ResRegRootKeys[HKey];
            if (string.IsNullOrEmpty(LpSubKey)) return str;
            str += $"\\{LpSubKey}";
            if (!string.IsNullOrEmpty(LpValueName)) str += $"\\{LpValueName}";
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is RegPath regpath)) throw new NullReferenceException();
            return Equals(regpath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regPath"></param>
        /// <returns></returns>
        protected bool Equals(RegPath regPath)
        {
            return HKey == regPath.HKey && 
                   string.Equals(LpSubKey, regPath.LpSubKey) && 
                   string.Equals(LpValueName, regPath.LpValueName) && 
                   Is64BitRegistry == regPath.Is64BitRegistry;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) HKey;
                hashCode = (hashCode * 397) ^ (LpSubKey != null ? LpSubKey.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LpValueName != null ? LpValueName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Is64BitRegistry.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        #region Protected
        
        /// <summary>
        /// 打开注册表子键句柄
        /// </summary>
        /// <returns>
        /// 注册表子键句柄
        /// </returns>
        protected IntPtr RegOpenKey()
        {
            int regopenkeytmp;
            IntPtr phkresult;
            if (Environment.Is64BitOperatingSystem && Is64BitRegistry)
            {
                regopenkeytmp = NativeMethods.RegOpenKeyEx(new IntPtr((int)HKey), LpSubKey, 0,
                    (int)KEY_SAM_FLAGS.KEY_WOW64_64KEY |
                    (int)KEY_ACCESS_TYPE.KEY_READ, out phkresult);
            }
            else
            {
                regopenkeytmp = NativeMethods.RegOpenKeyEx(new IntPtr((int)HKey), LpSubKey, 0,
                    (int)KEY_ACCESS_TYPE.KEY_READ, out phkresult);
            }

            if (regopenkeytmp == (int)ERROR_CODE.ERROR_FILE_NOT_FOUND)
            {
                throw new NullReferenceException($"注册表访问失败\n错误代码：{regopenkeytmp}\n{nameof(RegOpenKey)}");
            }

            if (regopenkeytmp != (int)ERROR_CODE.ERROR_SUCCESS)
            {
                throw new SystemException($"注册表访问失败\n错误代码：{regopenkeytmp}\n{nameof(RegOpenKey)}");
            }

            return phkresult;
        }

        /// <summary>
        /// 转换注册表所需数据。
        /// </summary>
        /// <param name="lpKind">
        /// 注册表键值类型。
        /// </param>
        /// <param name="lpData">
        /// 注册表键值。
        /// </param>
        /// <param name="lpcbData">
        /// 注册表键值所需内存。
        /// </param>
        /// <returns>
        /// 转换为已封装数据。
        /// </returns>
        protected RegKey ConvertData(REG_KEY_TYPE lpKind, IntPtr lpData, int lpcbData)
        {
            RegKey regkey;
            if (lpKind == REG_KEY_TYPE.REG_DWORD)
            {
                var lpdataint = Marshal.ReadInt32(lpData);
                regkey = new RegKey(this, lpKind, lpdataint);
            }
            else if (lpKind == REG_KEY_TYPE.REG_QWORD)
            {
                var lpdataint = Marshal.ReadInt64(lpData);
                regkey = new RegKey(this, lpKind, lpdataint);
            }
            else if (lpKind == REG_KEY_TYPE.REG_SZ ||
                     lpKind == REG_KEY_TYPE.REG_EXPAND_SZ ||
                     lpKind == REG_KEY_TYPE.REG_MULTI_SZ)
            {
                var lpdatastr = Marshal.PtrToStringUni(lpData);
                lpdatastr = lpdatastr?.Trim();
                regkey = new RegKey(this, lpKind, lpdatastr);
            }
            else if (lpKind == REG_KEY_TYPE.REG_BINARY)
            {
                var lpdatabin = new byte[lpcbData];
                Marshal.Copy(lpData, lpdatabin, 0, lpcbData);
                regkey = new RegKey(this, lpKind, lpdatabin);
            }
            else
            {
                throw new DataException($"注册表访问失败\n注册表数据类型异常\n{nameof(ConvertData)}");
            }

            return regkey;
        }

        #endregion

        #region Public

        /// <summary>
        /// 获取注册表键信息。
        /// </summary>
        /// <exception cref="Exception">
        /// 非托管代码获取注册表时产生的异常，详情请参阅MSDN。
        /// </exception>
        /// <returns>
        /// 注册表键信息。
        /// </returns>
        public RegKey Get()
        {
            RegKey regkey;
            try
            {
                var phkresult = RegOpenKey();
                var lpcbData = 0;
                NativeMethods.RegQueryValueEx(phkresult, LpValueName, IntPtr.Zero, out var lpkind, IntPtr.Zero, ref lpcbData);
                if (lpcbData == 0)
                {
                    NativeMethods.RegCloseKey(phkresult);
                    throw new InternalBufferOverflowException($"注册表访问失败\n无法获取缓冲区大小\n{nameof(Get)}");
                }
                var lpdata = Marshal.AllocHGlobal(lpcbData);
                var reggetvaluetemp = NativeMethods.RegQueryValueEx(phkresult, LpValueName, IntPtr.Zero, out lpkind, lpdata, ref lpcbData);
                if (reggetvaluetemp != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{reggetvaluetemp}\n{nameof(Get)}");
                }
                NativeMethods.RegCloseKey(phkresult);
                if (reggetvaluetemp != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{reggetvaluetemp}\n{nameof(Get)}");
                }

                regkey = ConvertData((REG_KEY_TYPE)lpkind, lpdata, lpcbData);
                Marshal.FreeHGlobal(lpdata);
            }
            catch (Exception)
            {
                regkey = new RegKey(this);
            }
            return regkey;
        }

        /// <summary>
        /// 枚举当前子键下所有子键信息。
        /// </summary>
        /// <returns>
        /// 枚举得到的注册表键名信息。
        /// </returns>
        public ICollection<RegPath> EnumKeys()
        {
            var phkresult = RegOpenKey();
            var list = new List<RegPath>();
            for (var index = 0; ; index++)
            {
                var sb = new StringBuilder(0x7FFF);
                var size = 0x7FFF;
                var regenumkeytmp = NativeMethods.RegEnumKeyEx(phkresult, index, sb, ref size, IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero, out _);
                if (regenumkeytmp == (int)ERROR_CODE.ERROR_NO_MORE_ITEMS)
                {
                    break;
                }

                if (regenumkeytmp != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表键值枚举失败\n错误代码：{regenumkeytmp}\n{nameof(EnumKeys)}");
                }

                list.Add(new RegPath(HKey, string.IsNullOrEmpty(LpSubKey) ? sb.ToString() : $"{LpSubKey}\\{sb}"));
            }

            NativeMethods.RegCloseKey(phkresult);
            list.Sort();
            return list;
        }

        /// <summary>
        /// 枚举当前子键下所有键名信息。
        /// </summary>
        /// <param name="defaultReg">
        /// 是否包含默认注册表项。
        /// </param>
        /// <returns>
        /// 枚举得到的注册表键名信息。
        /// </returns>
        public ICollection<RegKey> EnumValues(bool defaultReg = false)
        {
            var phkresult = RegOpenKey();
            var list = new List<RegKey>();
            for (var index = 0; ; index++)
            {
                var sb = new StringBuilder(0x7FFF);
                var size = 0x7FFF;
                var lpcbdata = 0;
                var regenumvaluetmp = NativeMethods.RegEnumValue(phkresult, index, sb, ref size, IntPtr.Zero,
                    out var lpkind,
                    IntPtr.Zero, ref lpcbdata);
                size += 2;
                if (regenumvaluetmp == (int)ERROR_CODE.ERROR_NO_MORE_ITEMS) break;
                if (regenumvaluetmp == (int)ERROR_CODE.ERROR_FILE_NOT_FOUND)
                    throw new NullReferenceException($"注册表键值枚举失败\n错误代码：{regenumvaluetmp}\n{nameof(EnumValues)}");
                if (regenumvaluetmp != (int)ERROR_CODE.ERROR_SUCCESS)
                    throw new Exception($"注册表键值枚举失败\n错误代码：{regenumvaluetmp}\n{nameof(EnumValues)}");
                var lpdata = Marshal.AllocHGlobal(lpcbdata);
                regenumvaluetmp = NativeMethods.RegEnumValue(phkresult, index, sb, ref size, IntPtr.Zero,
                    out lpkind,
                    lpdata, ref lpcbdata);
                if (regenumvaluetmp != (int)ERROR_CODE.ERROR_SUCCESS)
                    throw new Exception($"注册表键值枚举失败\n错误代码：{regenumvaluetmp}\n{nameof(EnumValues)}");
                var str = sb.ToString().Trim();
                if (!defaultReg && str == string.Empty) continue;
                var regkey = ConvertData((REG_KEY_TYPE)lpkind, lpdata, lpcbdata);
                Marshal.FreeHGlobal(lpdata);
                list.Add(regkey);

            }

            NativeMethods.RegCloseKey(phkresult);
            list.Sort();
            return list;
        }

        /// <summary>
        /// 删除指定注册表键。
        /// </summary>
        /// <returns>
        /// 异步方法运行状态。
        /// </returns>
        public void Delete()
        {
            int regdelkeytmp;
            if (string.IsNullOrEmpty(LpValueName))
            {
                regdelkeytmp = NativeMethods.RegDeleteKeyEx(new IntPtr((int)HKey), LpSubKey,
                    (int)KEY_SAM_FLAGS.KEY_WOW64_64KEY | (int)KEY_ACCESS_TYPE.KEY_SET_VALUE, 0);
                if (regdelkeytmp != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{regdelkeytmp}\n{nameof(Delete)}");
                }
            }
            else
            {
                regdelkeytmp = NativeMethods.RegOpenKeyEx(new IntPtr((int)HKey), LpSubKey, 0,
                    (int)KEY_SAM_FLAGS.KEY_WOW64_64KEY |
                    (int)KEY_ACCESS_TYPE.KEY_SET_VALUE, out var phkresult);
                if (regdelkeytmp != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{regdelkeytmp}\n{nameof(Delete)}");
                }

                regdelkeytmp = NativeMethods.RegDeleteValue(phkresult, LpValueName);
                if (regdelkeytmp != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{regdelkeytmp}\n{nameof(Delete)}");
                }

                NativeMethods.RegCloseKey(phkresult);
            }
        }

        #endregion

        #endregion
    }
}
