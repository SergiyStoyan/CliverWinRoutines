//********************************************************************************************
//Author: Sergiy Stoyan
//        s.y.stoyan@gmail.com, sergiy.stoyan@outlook.com, stoyan@cliversoft.com
//        http://www.cliversoft.com
//********************************************************************************************

using Cliver.Win;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Cliver//.Win (!)for backword compatibility and not adding using Cliver.Win;
{
    /// <summary>
    /// Show MessageForm with predefined features
    /// </summary>
    public partial class Message
    {
        public class Config
        {
            public Control Owner = null;
            public string Title = null;// Cliver.Message.AppName;
            public Icon Icon = null;
            public string Message = null;
            public string[] Buttons = [];
            public int DefaultButton = 0;
            public bool ButtonAutosize = false;
            //public bool NoDuplicate = false;
            public bool TopMost = false;
            public bool ShowInTaskbar = true;
            public SizeF ScreenMaxPart = new SizeF(0.75f, 0.75f);
            public bool MaximizeBox = true;
            public Icon FormIcon = GetAppIcon();
        }

        /// <summary>
        /// A handler that places the message box on the top of either the app form or the desktop.
        /// </summary>
        /// <param name="c"></param>
        static public void SetOnTop(Message.Config c)
        {
            c.Owner = c.Owner?.FindForm();
            if (c.Owner?.Visible == true)
                return;
            var fs = GetAppOpenForms();
            for (int i = fs.Count - 1; i >= 0; i--)
            {
                c.Owner = fs[i];
                if (c.Owner?.Visible == true)
                    return;
            }
            c.TopMost = true;
        }

        public static FormCollection GetAppOpenForms()
        {
            return Application.OpenForms;
        }

        public static string GetAppName()
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            string n = a.GetProduct();
            if (n != null)
                return n;
            n = a.GetTitle();
            if (n != null)
                return n;
            n = ProgramRoutines.GetAppName();
            return n;
        }

        readonly public static string AppName = GetAppName();

        public static Icon GetAppIcon()
        {
            lock (AppName)//just any static as a lock
            {
                return Icon.FromHandle(ImageRoutines.GetCopy(appIcon.ToBitmap()).GetHicon());
            }
        }
        readonly static Icon appIcon = Win.AssemblyRoutines.GetAppIcon();

        //public static string GetExceptionDetails(Exception e)
        //{
        //    List<string> ms = new List<string>();
        //    bool stack_trace_added = false;
        //    for (; e != null; e = e.InnerException)
        //    {
        //        string s = e.Message;
        //        if (!stack_trace_added && e.StackTrace != null)
        //        {
        //            stack_trace_added = true;
        //            s += "\r\n" + e.StackTrace;
        //        }
        //        ms.Add(s);
        //    }
        //    return string.Join("\r\n=>\r\n", ms);
        //}

        public enum Icons
        {
            Information,
            Warning,
            Error,
            Question,
            Exclamation,
        }
        ///// <summary>
        ///// (!)Looking ugly.
        ///// </summary>
        ///// <param name="icon"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //static public Icon GetIcon1(Icons icon)
        //{
        //    switch (icon)
        //    {
        //        case Icons.Information:
        //            return SystemIcons.Information;
        //        case Icons.Warning:
        //            return SystemIcons.Warning;
        //        case Icons.Error:
        //            return SystemIcons.Error;
        //        case Icons.Question:
        //            return SystemIcons.Question;
        //        case Icons.Exclamation:
        //            return SystemIcons.Exclamation;
        //        default: throw new Exception("No option: " + icon);
        //    }
        //}
        static public Icon GetIcon(Icons icon)
        {
            lock (AppName)//just any static as a lock. (!)When calling from async's it may go to wrong pointer!
            {
                WinApi.Shell32.SHSTOCKICONID iId;
                switch (icon)
                {
                    case Icons.Information:
                        iId = WinApi.Shell32.SHSTOCKICONID.SIID_INFO;
                        break;
                    case Icons.Warning:
                        iId = WinApi.Shell32.SHSTOCKICONID.SIID_WARNING;
                        break;
                    case Icons.Error:
                        iId = WinApi.Shell32.SHSTOCKICONID.SIID_ERROR;
                        break;
                    case Icons.Question:
                        iId = WinApi.Shell32.SHSTOCKICONID.SIID_HELP;
                        break;
                    case Icons.Exclamation:
                        iId = WinApi.Shell32.SHSTOCKICONID.SIID_WARNING;
                        break;
                    default: throw new Exception("No option: " + icon);
                }
                var sii = new WinApi.Shell32.SHSTOCKICONINFO();
                sii.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(WinApi.Shell32.SHSTOCKICONINFO));
                WinApi.Shell32.SHGetStockIconInfo(iId, WinApi.Shell32.SHGSI.SHGSI_ICON, ref sii);
                return System.Drawing.Icon.FromHandle(sii.hIcon);
            }
        }
    }
}

