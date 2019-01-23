using MFSystemInterface.Services.PInvoke;
using MFSystemInterface.Services.PInvoke.Enums;
using MFSystemInterface.Services.Utils;
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
        /// 字符串与注册表内部类型转换字典。
        /// </summary>
        public static Dictionary<string, REG_ROOT_KEY> RegRootKeys { get; }

        /// <summary>
        /// 注册表内部类型与字符串转换字典。
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
        /// 64位注册表项。
        /// 注：64位操作系统会重定向32位应用程序注册表访问，此属性仅用于修正32位应用程序的64位注册表项访问。
        /// </summary>
        public bool Is64BitRegistry { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// 全局初始化字符串、字符串转换字典。
        /// </summary>
        static RegPath()
        {
            RegRootKeys = new Dictionary<string, REG_ROOT_KEY>
            {
                // ReSharper disable seventh StringLiteralTypo
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
            Is64BitRegistry = is64BitRegistry;
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
            Is64BitRegistry = regPath.Is64BitRegistry;
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
            if (!(obj is RegPath reg_path)) throw new NullReferenceException();
            if (HKey < reg_path.HKey) return 1;
            if (HKey > reg_path.HKey) return -1;
            var flag = string.CompareOrdinal(LpSubKey, reg_path.LpSubKey);
            return flag != 0 ? flag : string.CompareOrdinal(LpValueName, LpValueName);
        }

        /// <inheritdoc />
        /// <summary>
        /// 主动析构的析构函数。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 主动析构内部析构逻辑。
        /// </summary>
        /// <param name="disposing">
        /// 是否主动析构。
        /// true是主动析构。
        /// false是被动析构。
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            //if (!disposing) return;

        }
        /// <summary>
        /// 由GC被动析构时使用的析构函数。
        /// </summary>
        ~RegPath()
        {
            Dispose(false);
        }
        /// <summary>
        /// 注册表路径信息。
        /// </summary>
        /// <returns>
        /// 字符串表现的注册表路径信息。
        /// </returns>
        public override string ToString()
        {
            var str = ResRegRootKeys[HKey];
            if (string.IsNullOrEmpty(LpSubKey)) return str;
            str += $"\\{LpSubKey}";
            if (!string.IsNullOrEmpty(LpValueName)) str += $"\\{LpValueName}";
            return str;
        }
        /// <summary>
        /// 判断对象是否相等。
        /// </summary>
        /// <param name="obj">
        /// 待判断的对象。
        /// </param>
        /// <returns>
        /// 对象是否与本对象相同。
        /// true表示相同。
        /// false表示不同。
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is RegPath reg_path)) return false;
            return Equals(reg_path);
        }
        /// <summary>
        /// 判断对象是否相等。
        /// </summary>
        /// <param name="regPath">
        /// 待判断的对象。
        /// </param>
        /// <returns>
        /// 对象是否与本对象相同。
        /// true表示相同。
        /// false表示不同。
        /// </returns>
        protected bool Equals(RegPath regPath)
        {
            /*return HKey == regPath.HKey && 
                   string.Equals(LpSubKey, regPath.LpSubKey) && 
                   string.Equals(LpValueName, regPath.LpValueName) && 
                   Is64BitRegistry == regPath.Is64BitRegistry;*/
            return BinaryUtil.ComputeSHA1(this) == BinaryUtil.ComputeSHA1(regPath);
        }
        /// <summary>
        /// 获取对象哈希值。
        /// </summary>
        /// <returns>
        /// 对象哈希值。
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable twice NonReadonlyMemberInGetHashCode
                var hash_code = (int) HKey;
                hash_code = (hash_code * 397) ^ (LpSubKey != null ? LpSubKey.GetHashCode() : 0);
                hash_code = (hash_code * 397) ^ (LpValueName != null ? LpValueName.GetHashCode() : 0);
                hash_code = (hash_code * 397) ^ Is64BitRegistry.GetHashCode();
                return hash_code;
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
        protected (IntPtr, bool) RegOpenKey(KEY_ACCESS_TYPE keyAccessType)
        {
            int reg_open_key;
            IntPtr phk_result;
            var is64_bit_registry = false;
            if (Judge64BitRegistry())
            {
                reg_open_key = NativeMethods.RegOpenKeyEx(new IntPtr((int)HKey), LpSubKey, 0,
                    (int)KEY_SAM_FLAGS.KEY_WOW64_64KEY |
                    (int)keyAccessType, out phk_result);
                is64_bit_registry = true;
            }
            else
            {
                reg_open_key = NativeMethods.RegOpenKeyEx(new IntPtr((int)HKey), LpSubKey, 0,
                    (int)keyAccessType, out phk_result);
            }

            if (reg_open_key == (int)ERROR_CODE.ERROR_FILE_NOT_FOUND)
            {
                throw new NullReferenceException($"注册表项不存在\n错误代码：{reg_open_key}\n{nameof(RegOpenKey)}");
            }

            if (reg_open_key != (int)ERROR_CODE.ERROR_SUCCESS)
            {
                throw new SystemException($"注册表访问失败\n错误代码：{reg_open_key}\n{nameof(RegOpenKey)}");
            }

            return (phk_result, is64_bit_registry);
        }

        /// <summary>
        /// 转换注册表所需数据。
        /// </summary>
        /// <param name="is64BitRegistry">
        /// </param>
        /// <param name="lpType">
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
        protected RegKey ConvertData(bool is64BitRegistry ,REG_KEY_TYPE lpType, IntPtr lpData, int lpcbData)
        {
            RegKey reg_key;
            if (lpType == REG_KEY_TYPE.REG_DWORD)
            {
                var lp_data_int = Marshal.ReadInt32(lpData);
                reg_key = new RegKey(this, lpType, lp_data_int);
            }
            else if (lpType == REG_KEY_TYPE.REG_QWORD)
            {
                var lp_data_long = Marshal.ReadInt64(lpData);
                reg_key = new RegKey(this, lpType, lp_data_long);
            }
            else if (lpType == REG_KEY_TYPE.REG_SZ ||
                     lpType == REG_KEY_TYPE.REG_EXPAND_SZ ||
                     lpType == REG_KEY_TYPE.REG_MULTI_SZ)
            {
                var lp_data_str = Marshal.PtrToStringUni(lpData);
                lp_data_str = lp_data_str?.Trim();
                reg_key = new RegKey(this, lpType, lp_data_str);
            }
            else if (lpType == REG_KEY_TYPE.REG_BINARY)
            {
                var lp_data_bin = new byte[lpcbData];
                Marshal.Copy(lpData, lp_data_bin, 0, lpcbData);
                reg_key = new RegKey(this, lpType, lp_data_bin);
            }
            else
            {
                throw new DataException($"注册表访问失败\n注册表数据类型异常\n{nameof(ConvertData)}");
            }

            return reg_key;
        }

        #endregion

        #region Public

        /// <summary>
        /// 判断当前注册表项是否是64位注册表项。
        /// </summary>
        /// <param name="fixProperty">
        /// 判断时是否同时修正当前注册表项属性。
        /// </param>
        /// <returns>
        /// 当前注册表项是否是64位注册表项。
        /// true是64位注册表项。
        /// false不是64位注册表项。
        /// </returns>
        public bool Judge64BitRegistry(bool fixProperty = true)
        {
            if (fixProperty) return Is64BitRegistry = Environment.Is64BitOperatingSystem && Is64BitRegistry;
            return Environment.Is64BitOperatingSystem && Is64BitRegistry;
        }

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
            RegKey reg_key;
            try
            {
                var (phk_result, is_64_bit_registry) = RegOpenKey(KEY_ACCESS_TYPE.KEY_READ);
                var lp_cb_data = 0;
                NativeMethods.RegQueryValueEx(phk_result, LpValueName, IntPtr.Zero, out var lp_type, IntPtr.Zero, ref lp_cb_data);
                if (lp_cb_data == 0)
                {
                    NativeMethods.RegCloseKey(phk_result);
                    throw new InternalBufferOverflowException($"注册表访问失败\n无法获取缓冲区大小\n{nameof(Get)}");
                }
                var lp_data = Marshal.AllocHGlobal(lp_cb_data);
                var reg_get_value_temp = NativeMethods.RegQueryValueEx(phk_result, LpValueName, IntPtr.Zero, out lp_type, lp_data, ref lp_cb_data);
                if (reg_get_value_temp != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{reg_get_value_temp}\n{nameof(Get)}");
                }
                NativeMethods.RegCloseKey(phk_result);
                if (reg_get_value_temp != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{reg_get_value_temp}\n{nameof(Get)}");
                }

                reg_key = ConvertData(is_64_bit_registry, (REG_KEY_TYPE)lp_type, lp_data, lp_cb_data);
                Marshal.FreeHGlobal(lp_data);
            }
            catch (Exception)
            {
                reg_key = new RegKey(this);
            }
            return reg_key;
        }

        /// <summary>
        /// 枚举当前子键下所有子键信息。
        /// </summary>
        /// <returns>
        /// 枚举得到的注册表键名信息。
        /// </returns>
        public ICollection<RegPath> EnumKeys()
        {
            var (phk_result, is_64_bit_registry) = RegOpenKey(KEY_ACCESS_TYPE.KEY_READ);
            var list = new List<RegPath>();
            for (var index = 0; ; index++)
            {
                var sb = new StringBuilder(0x7FFF);
                var size = 0x7FFF;
                var reg_enum_key = NativeMethods.RegEnumKeyEx(phk_result, index, sb, ref size, IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero, out _);
                if (reg_enum_key == (int)ERROR_CODE.ERROR_NO_MORE_ITEMS)
                {
                    break;
                }

                if (reg_enum_key != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表键值枚举失败\n错误代码：{reg_enum_key}\n{nameof(EnumKeys)}");
                }

                list.Add(new RegPath(HKey, string.IsNullOrEmpty(LpSubKey) ? sb.ToString() : $"{LpSubKey}\\{sb}",
                    string.Empty, is_64_bit_registry));
            }

            NativeMethods.RegCloseKey(phk_result);
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
            var (phk_result, is_64_bit_registry) = RegOpenKey(KEY_ACCESS_TYPE.KEY_READ);
            var list = new List<RegKey>();
            for (var index = 0; ; index++)
            {
                var sb = new StringBuilder(0x7FFF);
                var size = 0x7FFF;
                var lpcb_data = 0;
                var reg_enum_value = NativeMethods.RegEnumValue(phk_result, index, sb, ref size, IntPtr.Zero,
                    out var lp_type,
                    IntPtr.Zero, ref lpcb_data);
                size += 2;
                if (reg_enum_value == (int)ERROR_CODE.ERROR_NO_MORE_ITEMS) break;
                if (reg_enum_value == (int)ERROR_CODE.ERROR_FILE_NOT_FOUND)
                    throw new NullReferenceException($"注册表键值枚举失败\n错误代码：{reg_enum_value}\n{nameof(EnumValues)}");
                if (reg_enum_value != (int)ERROR_CODE.ERROR_SUCCESS)
                    throw new Exception($"注册表键值枚举失败\n错误代码：{reg_enum_value}\n{nameof(EnumValues)}");
                var lp_data = Marshal.AllocHGlobal(lpcb_data);
                reg_enum_value = NativeMethods.RegEnumValue(phk_result, index, sb, ref size, IntPtr.Zero,
                    out lp_type,
                    lp_data, ref lpcb_data);
                if (reg_enum_value != (int)ERROR_CODE.ERROR_SUCCESS)
                    throw new Exception($"注册表键值枚举失败\n错误代码：{reg_enum_value}\n{nameof(EnumValues)}");
                var str = sb.ToString().Trim();
                if (!defaultReg && str == string.Empty) continue;
                var reg_key = ConvertData(is_64_bit_registry, (REG_KEY_TYPE)lp_type, lp_data, lpcb_data);
                Marshal.FreeHGlobal(lp_data);
                list.Add(reg_key);

            }

            NativeMethods.RegCloseKey(phk_result);
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
            int reg_delete_key;
            if (string.IsNullOrEmpty(LpValueName))
            {
                if (Judge64BitRegistry())
                {
                    reg_delete_key = NativeMethods.RegDeleteKeyEx(new IntPtr((int)HKey), LpSubKey,
                        (int)KEY_SAM_FLAGS.KEY_WOW64_64KEY | (int)KEY_ACCESS_TYPE.KEY_SET_VALUE, 0);
                }
                else
                {
                    reg_delete_key = NativeMethods.RegDeleteKeyEx(new IntPtr((int)HKey), LpSubKey,
                        (int)KEY_ACCESS_TYPE.KEY_SET_VALUE, 0);
                }
                if (reg_delete_key != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{reg_delete_key}\n{nameof(Delete)}");
                }
            }
            else
            {
                IntPtr phk_result;
                if (Judge64BitRegistry())
                {
                    reg_delete_key = NativeMethods.RegOpenKeyEx(new IntPtr((int)HKey), LpSubKey, 0,
                        (int)KEY_SAM_FLAGS.KEY_WOW64_64KEY | (int)KEY_ACCESS_TYPE.KEY_SET_VALUE, out phk_result);
                }
                else
                {
                    reg_delete_key = NativeMethods.RegOpenKeyEx(new IntPtr((int)HKey), LpSubKey, 0,
                        (int)KEY_ACCESS_TYPE.KEY_SET_VALUE, out phk_result);
                }
                
                if (reg_delete_key != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{reg_delete_key}\n{nameof(Delete)}");
                }

                reg_delete_key = NativeMethods.RegDeleteValue(phk_result, LpValueName);
                if (reg_delete_key != (int)ERROR_CODE.ERROR_SUCCESS)
                {
                    throw new Exception($"注册表访问失败\n错误代码：{reg_delete_key}\n{nameof(Delete)}");
                }

                NativeMethods.RegCloseKey(phk_result);
            }
        }

        #endregion

        #endregion
    }
}
