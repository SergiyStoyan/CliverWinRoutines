﻿/********************************************************************************************
        Author: Sergiy Stoyan
        s.y.stoyan@gmail.com, sergiy.stoyan@outlook.com, stoyan@cliversoft.com
        http://www.cliversoft.com
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Management;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;

namespace Cliver.Win
{
    public static class SystemInfo
    {
        public static Size GetCurrentScreenSize(Point windowPosition, bool getWorkingArea)
        {
            Screen s = Screen.FromPoint(windowPosition);
            if (s != null)
                return getWorkingArea ? new Size(s.WorkingArea.Width, s.WorkingArea.Height) : new Size(s.Bounds.Width, s.Bounds.Height);
            return new Size();
        }

        public static Size GetPrimaryScreenSize(bool getWorkingArea)
        {
            Screen s = Screen.AllScreens.Where(x => x.Primary).FirstOrDefault();
            if (s != null)
                return new Size(s.Bounds.Width, s.Bounds.Height);
            s = Screen.AllScreens.FirstOrDefault();
            if (s != null)
                return getWorkingArea ? new Size(s.WorkingArea.Width, s.WorkingArea.Height) : new Size(s.Bounds.Width, s.Bounds.Height);
            return new Size();
        }

        public static List<string> GetScreenshotFiles(string file, System.Drawing.Imaging.ImageFormat format)
        {
            List<string> files = new List<string>();
            foreach (System.Windows.Forms.Screen s in System.Windows.Forms.Screen.AllScreens)
            {
                string f;
                if (s.Primary)
                    f = file;
                else
                    f = Cliver.PathRoutines.InsertSuffixBeforeFileExtension(file, "_" + Cliver.PathRoutines.GetLegalizedFileName(s.DeviceName));
                System.Drawing.Rectangle bounds = s.Bounds;
                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(bounds.Width, bounds.Height))
                {
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
                    }
                    bitmap.Save(f, format);
                }
                files.Add(f);
            }
            return files;
        }

        static public string GetMachineGuid()
        {
            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";

            using (RegistryKey bk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                if (bk != null)
                    using (RegistryKey rk = bk.OpenSubKey(location))
                    {
                        if (rk != null)
                        {
                            object machineGuid = rk.GetValue(name);
                            if (machineGuid != null)
                                return machineGuid.ToString();
                        }
                    }
            }
            using (RegistryKey bk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                if (bk != null)
                    using (RegistryKey rk = bk.OpenSubKey(location))
                    {
                        if (rk != null)
                        {
                            object machineGuid = rk.GetValue(name);
                            if (machineGuid != null)
                                return machineGuid.ToString();
                        }
                    }
            }
            return null;
        }

        public static string GetWindowsVersion()
        {
            List<string> vs = new List<string>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption, Version, CSDVersion FROM Win32_OperatingSystem"))
                foreach (var os in searcher.Get())
                    vs.Add(os["Caption"]?.ToString() + ", " + os["Version"]?.ToString() + ", " + os["CSDVersion"]?.ToString());
            if (vs.Count > 0)
                return vs[0];
            return Environment.OSVersion.ToString();
        }

        static public ulong GetTotalPhysicalMemory()
        {
            return new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        }

        static public TimeSpan GetUpTime()
        {
            using (var uptime = new PerformanceCounter("System", "System Up Time"))
            {
                uptime.NextValue(); //Call this an extra time before reading its value
                return TimeSpan.FromSeconds(uptime.NextValue());
            }
        }

        public static IEnumerable<string> GetMACs()
        {
            return Wmi.GetProperty("Win32_NetworkAdapterConfiguration", "MACAddress");
            //using (ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * From Win32_NetworkAdapterConfiguration"))
            //    foreach (ManagementObject mac in mos.Get())
            //        yield return mac["MACAddress"]?.ToString();
        }

        public static IEnumerable<string> GetMotherboardIds()
        {
            return Wmi.GetProperty("Win32_BaseBoard", "SerialNumber").Concat(Wmi.GetProperty("Win32_BIOS", "SerialNumber"));
            //List<string> ms = new List<string>();
            //using (ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * From Win32_BaseBoard"))
            //    foreach (ManagementObject mac in mos.Get())
            //    {
            //        ms.Add(mac["SerialNumber"]?.ToString());
            //    }
            //return ms;
        }

        public static IEnumerable<ProcessorInfo> GetProcessorInfos()
        {
            using (ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_Processor")
                //win32CompSys = new ManagementObjectSearcher("select * from Win32_ComputerSystem"),
                //win32Memory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory")
                )
            {
                foreach (ManagementObject mo in mos.Get())
                {
                    yield return new ProcessorInfo
                    {
                        Id = mo["ProcessorID"]?.ToString(),
                        ClockSpeed = mo["CurrentClockSpeed"]?.ToString(),
                        ProcName = mo["Name"]?.ToString(),
                        Manufacturer = mo["Manufacturer"]?.ToString(),
                        Version = mo["Version"]?.ToString(),
                    };
                }
            }
        }
        public class ProcessorInfo
        {
            public string Id;
            public string ClockSpeed;
            public string ProcName;
            public string Manufacturer;
            public string Version;
        }

        public static Dictionary<string, DiskInfo> GetDiskInfo()
        {
            Dictionary<string, DiskInfo> dis = new Dictionary<string, DiskInfo>();
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                if (!d.IsReady)
                    continue;
                dis[d.Name] = new DiskInfo { total = d.TotalSize, free = d.TotalFreeSpace };
            }
            return dis;
        }
        public class DiskInfo
        {
            public long total;
            public long free;
        }

        //string disk_size2string(int size)
        //{
        //    foreach(uint m in new uint[] {2^30, 2^20, 2^10, 1})
        //    string s = 
        //}

        public static IPAddress GetLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip;
            }
            return null;
        }
    }
}