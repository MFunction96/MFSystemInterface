using MFSystemInterface.Services.PInvoke;
using MFSystemInterface.Services.PInvoke.Enums;
using System;
using System.Runtime.InteropServices;

namespace MFSystemInterface.Models.Registry
{
    /// <inheritdoc cref="RegPath" />
    /// <summary>
    /// 注册表键信息类。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class RegKey : RegPath
    {
        #region Properties

        /// <summary>
        /// 注册表路径信息。
        /// </summary>
        public RegPath RegPath => new RegPath(HKey, LpSubKey, LpValueName, Is64BitRegistry);
        /// <summary>
        /// 注册表键值类型。
        /// </summary>
        public REG_KEY_TYPE LpType { get; set; }
        /// <summary>
        /// 注册表键值。
        /// </summary>
        public object LpValue { get; set; }

        #endregion

        #region Construction

        /// <inheritdoc />
        /// <summary>
        /// 注册表键信息类序列化构造函数。
        /// </summary>
        public RegKey() { }

        /// <inheritdoc />
        /// <summary>
        /// 注册表键信息类构造函数。
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
        /// <param name="is64BitRegistry">
        ///
        /// </param>
        /// <param name="lpType">
        /// 注册表键值类型。
        /// </param>
        /// <param name="lpValue">
        /// 注册表键值。
        /// </param>
        public RegKey(
            REG_ROOT_KEY hKey,
            string lpSubKey,
            string lpValueName = "",
            bool is64BitRegistry = false,
            REG_KEY_TYPE lpType = REG_KEY_TYPE.REG_UNKNOWN,
            object lpValue = null) :
            base(hKey, lpSubKey, lpValueName, is64BitRegistry)
        {
            LpType = lpType;
            LpValue = lpValue;
        }
        /// <inheritdoc />
        /// <summary>
        /// 注册表键信息类构造函数。
        /// </summary>
        /// <param name="regPath">
        /// 注册表键路径信息类。
        /// </param>
        /// <param name="lpType">
        /// 注册表键值类型。
        /// </param>
        /// <param name="lpValue">
        /// 注册表键值。
        /// </param>
        public RegKey(RegPath regPath, REG_KEY_TYPE lpType = REG_KEY_TYPE.REG_UNKNOWN, object lpValue = null) :
            base(regPath)
        {
            LpType = lpType;
            LpValue = lpValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// 注册表键信息类复制构造函数。
        /// </summary>
        /// <param name="regKey">
        /// 注册表键信息类。
        /// </param>
        public RegKey(RegKey regKey) :
            base(regKey.RegPath)
        {
            LpType = regKey.LpType;
            LpValue = regKey.LpValue;
        }

        #endregion

        #region Methods

        #region Implement

        /// <summary>
        /// 注册表路径信息类默认排序规则。
        /// </summary>
        /// <param name="obj">
        /// 待比较的对象。
        /// </param>
        /// <returns>
        /// 大小比较结果。
        /// </returns>
        public new int CompareTo(object obj)
        {
            if (!(obj is RegKey reg_key)) throw new NullReferenceException();
            var flag = base.CompareTo(obj);
            if (flag != 0) return flag;
            if (LpType < reg_key.LpType) return 1;
            if (LpType > reg_key.LpType) return -1;
            return string.CompareOrdinal(LpValue.ToString(), reg_key.LpValue.ToString());
        }

        /// <inheritdoc />
        /// <summary>
        /// 主动析构内部析构逻辑。
        /// </summary>
        /// <param name="disposing">
        /// 是否主动析构。
        /// true是主动析构。
        /// false是被动析构。
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            base.Dispose(true);
        }

        /// <inheritdoc />
        /// <summary>
        /// 由GC被动析构时使用的析构函数。
        /// </summary>
        ~RegKey()
        {
            Dispose(false);
        }

        /// <inheritdoc />
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
            if (!(obj is RegKey reg_key)) return false;
            return Equals(reg_key);
        }

        /// <inheritdoc />
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
                var hash_code = base.GetHashCode();
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hash_code = (hash_code * 397) ^ (int)LpType;
                // ReSharper disable twice NonReadonlyMemberInGetHashCode
                hash_code = (hash_code * 397) ^ (LpValue != null ? LpValue.GetHashCode() : 0);
                return hash_code;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// 检查注册表项是否与系统一致。
        /// </summary>
        /// <returns>
        /// true表示注册表项与系统一致。
        /// false表示注册表项与系统不一致。
        /// </returns>
        public bool Check()
        {
            return Equals(Get());
        }

        /// <summary>
        /// 设置注册表键。
        /// </summary>
        public void Set()
        {
            int reg_set_value, exists;
            IntPtr phk_result;
            if (Judge64BitRegistry())
            {
                reg_set_value = NativeMethods.RegCreateKeyEx(new IntPtr((int)HKey), LpSubKey, 0, null,
                    (int)OPERATE_OPTION.REG_OPTION_NON_VOLATILE,
                    (int)KEY_SAM_FLAGS.KEY_WOW64_64KEY | (int)KEY_ACCESS_TYPE.KEY_READ |
                    (int)KEY_ACCESS_TYPE.KEY_WRITE, IntPtr.Zero, out phk_result, out exists);
            }
            else
            {
                reg_set_value = NativeMethods.RegCreateKeyEx(new IntPtr((int)HKey), LpSubKey, 0, null,
                    (int)OPERATE_OPTION.REG_OPTION_NON_VOLATILE,
                    (int)KEY_ACCESS_TYPE.KEY_READ |
                    (int)KEY_ACCESS_TYPE.KEY_WRITE, IntPtr.Zero, out phk_result, out exists);
            }
            if (reg_set_value != (int)ERROR_CODE.ERROR_SUCCESS && exists != (int)REG_CREATE_DISPOSITION.REG_OPENED_EXISTING_KEY)
            {
                throw new Exception($"注册表访问失败\n错误代码：{reg_set_value}\n{nameof(Set)}");
            }
            IntPtr lp_data;
            int lpcb_data;
            if (LpType == REG_KEY_TYPE.REG_SZ ||
                LpType == REG_KEY_TYPE.REG_EXPAND_SZ ||
                LpType == REG_KEY_TYPE.REG_MULTI_SZ)
            {
                if (!(LpValue is string lp_data_str)) throw new NullReferenceException();
                lpcb_data = lp_data_str.Length + 1 << 1;
                lp_data = Marshal.StringToHGlobalUni(lp_data_str);
            }
            else if (LpType == REG_KEY_TYPE.REG_DWORD)
            {
                lpcb_data = Marshal.SizeOf(typeof(int));
                lp_data = Marshal.AllocHGlobal(lpcb_data);
                Marshal.WriteInt32(lp_data, (int)LpValue);
            }
            else if (LpType == REG_KEY_TYPE.REG_QWORD)
            {
                lpcb_data = Marshal.SizeOf(typeof(long));
                lp_data = Marshal.AllocHGlobal(lpcb_data);
                Marshal.WriteInt64(lp_data, (long)LpValue);
            }
            else if (LpType == REG_KEY_TYPE.REG_BINARY)
            {
                if (!(LpValue is byte[] lp_data_bin)) throw new NullReferenceException();
                lpcb_data = lp_data_bin.Length;
                lp_data = Marshal.AllocHGlobal(lpcb_data);
                Marshal.Copy(lp_data_bin, 0, lp_data, lpcb_data);
            }
            else
            {
                throw new Exception($"注册表访问失败\n错误代码：{reg_set_value}\n{nameof(Set)}");
            }
            reg_set_value =
                NativeMethods.RegSetValueEx(phk_result, LpValueName, 0, (int)LpType, lp_data, lpcb_data);
            NativeMethods.RegCloseKey(phk_result);
            if (reg_set_value != (int)ERROR_CODE.ERROR_SUCCESS)
            {
                throw new Exception($"注册表访问失败\n错误代码：{reg_set_value}\n{nameof(Set)}");
            }
        }

        #endregion

        #endregion
    }
}
