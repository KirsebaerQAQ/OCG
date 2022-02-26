using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal interface Interface1
    {
        static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        static readonly int CR_SUCCESS = 0x00000000;
        static readonly int DN_HAS_PROBLEM = 0x00000400;
        static readonly Guid DisplayAdapter = new Guid("{5B45201D-F2F2-4F3B-85BB-30FF1F953599}");

        [Flags]
        enum DiGetClassFlags : uint
        {
            DIGCF_DEFAULT = 0x00000001, // only valid with DIGCF_DEVICEINTERFACE
            DIGCF_PRESENT = 0x00000002,
            DIGCF_ALLCLASSES = 0x00000004,
            DIGCF_PROFILE = 0x00000008,
            DIGCF_DEVICEINTERFACE = 0x00000010,
        }

        [Flags]
        enum DEVPROPTYPE : ulong
        {
            DEVPROP_TYPEMOD_ARRAY = 0x00001000,
            DEVPROP_TYPEMOD_LIST = 0x00002000,

            DEVPROP_TYPE_EMPTY = 0x00000000,  // nothing, no property data
            DEVPROP_TYPE_NULL = 0x00000001,  // null property data
            DEVPROP_TYPE_SBYTE = 0x00000002,  // 8-bit signed int (SBYTE)
            DEVPROP_TYPE_BYTE = 0x00000003,  // 8-bit unsigned int (BYTE)
            DEVPROP_TYPE_INT16 = 0x00000004,  // 16-bit signed int (SHORT)
            DEVPROP_TYPE_UINT16 = 0x00000005,  // 16-bit unsigned int (USHORT)
            DEVPROP_TYPE_INT32 = 0x00000006,  // 32-bit signed int (LONG)
            DEVPROP_TYPE_UINT32 = 0x00000007,  // 32-bit unsigned int (ULONG)
            DEVPROP_TYPE_INT64 = 0x00000008,  // 64-bit signed int (LONG64)
            DEVPROP_TYPE_UINT64 = 0x00000009,  // 64-bit unsigned int (ULONG64)
            DEVPROP_TYPE_FLOAT = 0x0000000A,  // 32-bit floating-point (FLOAT)
            DEVPROP_TYPE_DOUBLE = 0x0000000B,  // 64-bit floating-point (DOUBLE)
            DEVPROP_TYPE_DECIMAL = 0x0000000C,  // 128-bit data (DECIMAL)
            DEVPROP_TYPE_GUID = 0x0000000D,  // 128-bit unique identifier (GUID)
            DEVPROP_TYPE_CURRENCY = 0x0000000E,  // 64 bit signed int currency value (CURRENCY)
            DEVPROP_TYPE_DATE = 0x0000000F,  // date (DATE)
            DEVPROP_TYPE_FILETIME = 0x00000010,  // filetime (FILETIME)
            DEVPROP_TYPE_BOOLEAN = 0x00000011,  // 8-bit boolean (DEVPROP_BOOLEAN)
            DEVPROP_TYPE_STRING = 0x00000012,  // null-terminated string
            DEVPROP_TYPE_STRING_LIST = (DEVPROP_TYPE_STRING | DEVPROP_TYPEMOD_LIST), // multi-sz string list
            DEVPROP_TYPE_SECURITY_DESCRIPTOR = 0x00000013,  // self-relative binary SECURITY_DESCRIPTOR
            DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING = 0x00000014,  // security descriptor string (SDDL format)
            DEVPROP_TYPE_DEVPROPKEY = 0x00000015,  // device property key (DEVPROPKEY)
            DEVPROP_TYPE_DEVPROPTYPE = 0x00000016,  // device property type (DEVPROPTYPE)
            DEVPROP_TYPE_BINARY = (DEVPROP_TYPE_BYTE | DEVPROP_TYPEMOD_ARRAY),  // custom binary data
            DEVPROP_TYPE_ERROR = 0x00000017,  // 32-bit Win32 system error code
            DEVPROP_TYPE_NTSTATUS = 0x00000018, // 32-bit NTSTATUS code
            DEVPROP_TYPE_STRING_INDIRECT = 0x00000019, // string resource (@[path\]<dllname>,-<strId>)

            MAX_DEVPROP_TYPE = 0x00000019,
            MAX_DEVPROP_TYPEMOD = 0x00002000,

            DEVPROP_MASK_TYPE = 0x00000FFF,
            DEVPROP_MASK_TYPEMOD = 0x0000F000


        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid classGuid;
            public uint devInst;
            public IntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DEVPROPKEY
        {
            public Guid fmtid;
            public UInt32 pid;
        }

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, [MarshalAs(UnmanagedType.LPTStr)] string enumerator, IntPtr hwndParent, DiGetClassFlags flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetupDiEnumDeviceInfo([In] IntPtr hDevInfo, [In] uint memberIndex, [In, Out] ref SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceProperty([In] IntPtr hDevInfo, [In] ref SP_DEVINFO_DATA deviceInfoData, [In] ref DEVPROPKEY propertyKey, [In, Out] ref DEVPROPTYPE propertyType, [In, Out] byte[] propertyBuffer, [In] uint propertyBufferSize, [In, Out] ref uint requiredSize, [In] uint flags = 0);

        [DllImport("cfgmgr32.dll", SetLastError = true)]
        static extern int CM_Get_DevNode_Status(out UInt32 status, out UInt32 probNum, UInt32 devInst, int flags);

        [DllImport("shell32.dll")]
        public extern static int ShellAbout(IntPtr hWnd, string szApp, string szOtherStuff, IntPtr hIcon);
    }
}
