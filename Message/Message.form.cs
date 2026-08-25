//********************************************************************************************
//Author: Sergiy Stoyan
//        s.y.stoyan@gmail.com, sergiy.stoyan@outlook.com, stoyan@cliversoft.com
//        http://www.cliversoft.com
//********************************************************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Cliver//.Win (!)for backword compatibility and not adding using Cliver.Win;
{
    /// <summary>
    /// Show MessageForm with predefined features
    /// </summary>
    public static partial class Message
    {
        public static void Show(this Control owner, Exception exception)
        {
            Show(exception, owner);
        }

        public static void Inform(this Control owner, string message)
        {
            Inform(message, owner);
        }

        public static void Exclaim(this Control owner, string message)
        {
            Exclaim(message, owner);
        }

        public static void Exclaim0(this Control owner, Exception e)
        {
            Exclaim0(e, owner);
        }

        public static void Exclaim(this Control owner, Exception e)
        {
            Exclaim(e, owner);
        }

        public static void Warning(this Control owner, string message)
        {
            Warning(message, owner);
        }

        public static void Warning(this Control owner, Exception e)
        {
            Warning(e, owner);
        }

        public static void Warning2(this Control owner, Exception e)
        {
            Warning2(e, owner);
        }

        public static void Error(this Control owner, Exception e)
        {
            Error(e, owner);
        }

        public static void Error2(this Control owner, Exception e)
        {
            Error2(e, owner);
        }

        public static void Error(this Control owner, string subtitle, Exception e)
        {
            Error(subtitle, e, owner);
        }

        public static void Error2(this Control owner, string subtitle, Exception e)
        {
            Error2(subtitle, e, owner);
        }

        public static void Error(this Control owner, string message)
        {
            Error(message, owner);
        }

        public static bool YesNo(this Control owner, string question, Icons icon = Icons.Question, bool defaultIsYes = true)
        {
            return YesNo(question, owner, icon, defaultIsYes);
        }

        public static int ShowMessage(this Control owner, string title, Icons icon, string message, string[] buttons, int defaultButton, bool? buttonAutosize = null/*, bool? noDuplicate = null*/, bool? topMost = null)
        {
            return ShowDialog(title, icon, message, buttons, defaultButton, owner, buttonAutosize/*, noDuplicate*/, topMost);
        }

        public static int ShowMessage(this Control owner, string title, Icon icon, string message, string[] buttons, int defaultButton, bool? buttonAutosize = null/*, bool? noDuplicate = null*/, bool? topMost = null)
        {
            return ShowDialog(title, icon, message, buttons, defaultButton, owner, buttonAutosize/*, noDuplicate*/, topMost);
        }
    }
}