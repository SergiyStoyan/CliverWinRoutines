//********************************************************************************************
//Author: Sergiy Stoyan
//        s.y.stoyan@gmail.com, sergiy.stoyan@outlook.com, stoyan@cliversoft.com
//        http://www.cliversoft.com
//********************************************************************************************


using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Cliver.WinApi
{
    public partial class Advapi32
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CreateProcess(
            string lpApplicationName,                   // LPCTSTR
            string lpCommandLine,                // LPTSTR - note: CreateProcess might insert a null somewhere in this string
            SECURITY_ATTRIBUTES lpProcessAttributes,    // LPSECURITY_ATTRIBUTES
            SECURITY_ATTRIBUTES lpThreadAttributes,     // LPSECURITY_ATTRIBUTES
            bool bInheritHandles,                        // BOOL
            CreationFlags dwCreationFlags,                        // DWORD
            IntPtr lpEnvironment,                       // LPVOID
            string lpCurrentDirectory,                  // LPCTSTR
            STARTUPINFO lpStartupInfo,                  // LPSTARTUPINFO
            PROCESS_INFORMATION lpProcessInformation    // LPPROCESS_INFORMATION
        );

        [DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ConvertSidToStringSid([MarshalAs(UnmanagedType.LPArray)] byte[] pSID, out IntPtr ptrSid);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, OpenServiceDesiredAccess dwDesiredAccess);
        [Flags]
        public enum OpenServiceDesiredAccess : uint
        {
            SERVICE_QUERY_STATUS = 0x0004,
        }

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenSCManager(string machineName, string databaseName, SCM_ACCESS dwAccess);
        [Flags]
        public enum SCM_ACCESS : uint
        {
            STANDARD_RIGHTS_REQUIRED = 0xF0000,
            SC_MANAGER_CONNECT = 0x00001,
            SC_MANAGER_CREATE_SERVICE = 0x00002,
            SC_MANAGER_ENUMERATE_SERVICE = 0x00004,
            SC_MANAGER_LOCK = 0x00008,
            SC_MANAGER_QUERY_LOCK_STATUS = 0x00010,
            SC_MANAGER_MODIFY_BOOT_CONFIG = 0x00020,
            SC_MANAGER_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED |
                             SC_MANAGER_CONNECT |
                             SC_MANAGER_CREATE_SERVICE |
                             SC_MANAGER_ENUMERATE_SERVICE |
                             SC_MANAGER_LOCK |
                             SC_MANAGER_QUERY_LOCK_STATUS |
                             SC_MANAGER_MODIFY_BOOT_CONFIG
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern uint NotifyServiceStatusChange(IntPtr hService, NotifyMask dwNotifyMask, IntPtr pNotifyBuffer);

        public enum NotifyMask : uint
        {
            SERVICE_NOTIFY_CREATED = 0x00000080,
            SERVICE_NOTIFY_CONTINUE_PENDING = 0x00000010,
            SERVICE_NOTIFY_DELETE_PENDING = 0x00000200,
            SERVICE_NOTIFY_DELETED = 0x00000100,
            SERVICE_NOTIFY_PAUSE_PENDING = 0x00000020,
            SERVICE_NOTIFY_PAUSED = 0x00000040,
            SERVICE_NOTIFY_RUNNING = 0x00000008,
            SERVICE_NOTIFY_START_PENDING = 0x00000002,
            SERVICE_NOTIFY_STOP_PENDING = 0x00000004,
            SERVICE_NOTIFY_STOPPED = 0x00000001,
        }

        [DllImportAttribute("kernel32.dll", EntryPoint = "SleepEx")]
        public static extern uint SleepEx(uint dwMilliseconds, [MarshalAsAttribute(UnmanagedType.Bool)] bool bAlertable);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, DesiredAccess DesiredAccess, out IntPtr TokenHandle);
        public enum DesiredAccess : uint
        {
            MAXIMUM_ALLOWED = 0x2000000,
            STANDARD_RIGHTS_READ = 0x00020000,

            TOKEN_QUERY = 0x0008,
            TOKEN_DUPLICATE = 0x0002,
            TOKEN_ASSIGN_PRIMARY = 0x0001,
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);

        public enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        public enum TOKEN_ELEVATION_TYPE
        {
            TokenElevationTypeDefault = 1,
            TokenElevationTypeFull,
            TokenElevationTypeLimited
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LookupPrivilegeValue(IntPtr lpSystemName, string lpname, [MarshalAs(UnmanagedType.Struct)] ref LUID lpLuid);
        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public int LowPart;
            public int HighPart;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATRIBUTES
        {
            public LUID Luid;
            public int Attributes;
        }


        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)]
        [System.Security.SuppressUnmanagedCodeSecurity()]
        public extern static bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            SECURITY_ATTRIBUTES lpProcessAttributes,
            SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            Advapi32.CreationFlags dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            STARTUPINFO lpStartupInfo,
            PROCESS_INFORMATION lpProcessInformation
        );


        public enum CreationFlags : uint
        {
            DEBUG_PROCESS = 0x00000001,
            DEBUG_ONLY_THIS_PROCESS = 0x00000002,
            CREATE_SUSPENDED = 0x00000004,
            DETACHED_PROCESS = 0x00000008,
            CREATE_NEW_CONSOLE = 0x00000010,
            NORMAL_PRIORITY_CLASS = 0x00000020,
            IDLE_PRIORITY_CLASS = 0x00000040,
            HIGH_PRIORITY_CLASS = 0x00000080,
            REALTIME_PRIORITY_CLASS = 0x00000100,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            CREATE_SEPARATE_WOW_VDM = 0x00000800,
            CREATE_SHARED_WOW_VDM = 0x00001000,
            CREATE_FORCEDOS = 0x00002000,
            BELOW_NORMAL_PRIORITY_CLASS = 0x00004000,
            ABOVE_NORMAL_PRIORITY_CLASS = 0x00008000,
            INHERIT_PARENT_AFFINITY = 0x00010000,
            INHERIT_CALLER_PRIORITY = 0x00020000,    // Deprecated
            CREATE_PROTECTED_PROCESS = 0x00040000,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
            PROCESS_MODE_BACKGROUND_BEGIN = 0x00100000,
            PROCESS_MODE_BACKGROUND_END = 0x00200000,
            CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
            CREATE_NO_WINDOW = 0x08000000,
            PROFILE_USER = 0x10000000,
            PROFILE_KERNEL = 0x20000000,
            PROFILE_SERVER = 0x40000000,
            CREATE_IGNORE_SYSTEM_DEFAULT = 0x80000000,
        }
        //[StructLayout(LayoutKind.Sequential)]
        //public struct STARTUPINFO
        //{
        //    public int cb;
        //    public String lpReserved;
        //    public String lpDesktop;
        //    public String lpTitle;
        //    public uint dwX;
        //    public uint dwY;
        //    public uint dwXSize;
        //    public uint dwYSize;
        //    public uint dwXCountChars;
        //    public uint dwYCountChars;
        //    public uint dwFillAttribute;
        //    public dwFlags dwFlags;
        //    public short wShowWindow;
        //    public short cbReserved2;
        //    public IntPtr lpReserved2;
        //    public IntPtr hStdInput;
        //    public IntPtr hStdOutput;
        //    public IntPtr hStdError;
        //}

        [StructLayout(LayoutKind.Sequential)]
        public class STARTUPINFO
        {
            public int cb;
            public IntPtr lpReserved = IntPtr.Zero;
            public IntPtr lpDesktop = IntPtr.Zero;
            public IntPtr lpTitle = IntPtr.Zero;
            public int dwX = 0;
            public int dwY = 0;
            public int dwXSize = 0;
            public int dwYSize = 0;
            public int dwXCountChars = 0;
            public int dwYCountChars = 0;
            public int dwFillAttribute = 0;
            public dwFlags dwFlags = 0;
            public short wShowWindow = 0;
            public short cbReserved2 = 0;
            public IntPtr lpReserved2 = IntPtr.Zero;
            public SafeFileHandle hStdInput = new SafeFileHandle(IntPtr.Zero, false);
            public SafeFileHandle hStdOutput = new SafeFileHandle(IntPtr.Zero, false);
            public SafeFileHandle hStdError = new SafeFileHandle(IntPtr.Zero, false);

            public STARTUPINFO()
            {
                cb = Marshal.SizeOf(this);
            }

            public void Dispose()
            {
                // close the handles created for child process
                if (hStdInput != null && !hStdInput.IsInvalid)
                {
                    hStdInput.Close();
                    hStdInput = null;
                }

                if (hStdOutput != null && !hStdOutput.IsInvalid)
                {
                    hStdOutput.Close();
                    hStdOutput = null;
                }

                if (hStdError != null && !hStdError.IsInvalid)
                {
                    hStdError.Close();
                    hStdError = null;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class PROCESS_INFORMATION
        {
            public IntPtr hProcess = IntPtr.Zero;
            public IntPtr hThread = IntPtr.Zero;
            public int dwProcessId = 0;
            public int dwThreadId = 0;
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DuplicateTokenEx(IntPtr ExistingTokenHandle, DesiredAccess dwDesiredAccess, SECURITY_ATTRIBUTES lpTokenAttributes, SECURITY_IMPERSONATION_LEVEL ImpersonationLevel, TOKEN_TYPE TokenType, out IntPtr DuplicateTokenHandle);
        public enum TOKEN_TYPE
        {
            TokenPrimary = 1,
            TokenImpersonation = 2
        }
        public enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous = 0,
            SecurityIdentification = 1,
            SecurityImpersonation = 2,
            SecurityDelegation = 3,
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, int BufferLength, IntPtr PreviousState, IntPtr ReturnLength);
        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            //LUID_AND_ATRIBUTES
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public int[] Privileges;
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool SetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, ref uint TokenInformation, uint TokenInformationLength);

        [DllImport("advapi32", SetLastError = true)]
        //[SuppressUnmanagedCodeSecurity]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

        public enum dwFlags : uint
        {
            STARTF_USESHOWWINDOW = 0x00000001,
            STARTF_USESIZE = 0x00000002,
            STARTF_USEPOSITION = 0x00000004,
            STARTF_USECOUNTCHARS = 0x00000008,
            STARTF_USEFILLATTRIBUTE = 0x00000010,
            STARTF_RUNFULLSCREEN = 0x00000020,  // ignored for non-x86 platforms
            STARTF_FORCEONFEEDBACK = 0x00000040,
            STARTF_FORCEOFFFEEDBACK = 0x00000080,
            STARTF_USESTDHANDLES = 0x00000100,
        }
    }
}