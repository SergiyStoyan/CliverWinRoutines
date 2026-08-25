//********************************************************************************************
//Author: Sergiy Stoyan
//        s.y.stoyan@gmail.com, sergiy.stoyan@outlook.com, stoyan@cliversoft.com
//        http://www.cliversoft.com
//********************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Cliver.Win;

namespace Cliver//.Win (!)for backword compatibility and not adding using Cliver.Win;
{
    /// <summary>
    /// Show MessageForm with predefined features
    /// </summary>
    public partial class Message
    {
        ///// <summary>
        ///// Display only one message box for all same messages thrown. When the first one is being displayed, the rest are ignored.
        ///// </summary>
        //public static bool NoDuplicate = true;

        public static string Title = null;

        /// <summary>
        /// Whether the message box must be displayed in the Windows taskbar.
        /// </summary>
        public static bool ShowInTaskbar = true;

        /// <summary>
        /// Whether the message box must be displayed topMost.
        /// </summary>
        public static bool TopMost = false;

        /// <summary>
        /// Owner that is used by default
        /// </summary>
        public static Control Owner;

        /// <summary>
        /// Autosize buttons by text
        /// </summary>
        public static bool ButtonAutosize = false;

        public static void Show(Exception exception, Control owner = null)
        {
            if (exception is MessageException me)
            {
                switch (me.MessageType)
                {
                    case Log.MessageType.TRACE:
                    case Log.MessageType.DEBUG:
                    case Log.MessageType.INFORM:
                        ShowDialog(null, Icons.Information, Log.GetExceptionMessage(me, me.PrintDetails), ["OK"], 0, owner);
                        break;
                    case Log.MessageType.ERROR:
                    case Log.MessageType.EXIT:
                        ShowDialog(null, Icons.Error, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(me, me.PrintDetails), ["OK"], 0, owner);
                        break;
                    case Log.MessageType.WARNING:
                        ShowDialog(null, Icons.Warning, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(me, me.PrintDetails), ["OK"], 0, owner);
                        break;
                    default:
                        throw new Exception("Unknown option: " + me.MessageType);
                }
                return;
            }
            Error(exception, owner);
        }

        public static void Inform(string message, Control owner = null)
        {
            ShowDialog(null, Icons.Information, message, ["OK"], 0, owner);
        }

        public static void Exclaim(string message, Control owner = null)
        {
            ShowDialog(null, Icons.Exclamation, message, ["OK"], 0, owner);
        }

        public static void Exclaim0(Exception e, Control owner = null)
        {
            ShowDialog(null, Icons.Exclamation, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(e, !(e is Exception2)), ["OK"], 0, owner);
        }

        public static void Exclaim(Exception e, Control owner = null)
        {
            ShowDialog(null, Icons.Exclamation, Log.GetExceptionMessage2(e), ["OK"], 0, owner);
        }

        public static void Warning(string message, Control owner = null)
        {
            ShowDialog(null, Icons.Warning, message, ["OK"], 0, owner);
        }

        public static void Warning(Exception e, Control owner = null)
        {
            ShowDialog(null, Icons.Warning, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(e, !(e is Exception2)), ["OK"], 0, owner);
        }

        public static void Warning2(Exception e, Control owner = null)
        {
            ShowDialog(null, Icons.Warning, Log.GetExceptionMessage2(e), ["OK"], 0, owner);
        }

        public static void Error(Exception e, Control owner = null)
        {
            ShowDialog(null, Icons.Error, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(e, !(e is Exception2)), ["OK"], 0, owner);
        }

        public static void Error2(Exception e, Control owner = null)
        {
            Error(Log.GetExceptionMessage2(e), owner);
        }

        public static void Error(string subtitle, Exception e, Control owner = null)
        {
            Error(subtitle + "\r\n\r\n" + Log.GetExceptionMessage(e, !(e is Exception2)), owner);
        }

        public static void Error2(string subtitle, Exception e, Control owner = null)
        {
            Error(subtitle + "\r\n\r\n" + Log.GetExceptionMessage2(e), owner);
        }

        public static void Error(string message, Control owner = null)
        {
            ShowDialog(null, Icons.Error, message, ["OK"], 0, owner);
        }

        public static bool YesNo(string question, Control owner = null, Icons icon = Icons.Question, bool defaultIsYes = true)
        {
            return ShowDialog(null, icon, question, ["Yes", "No"], defaultIsYes ? 0 : 1, owner) == 0;
        }

        public static int ShowDialog(string title, Icons icon, string message, string[] buttons, int defaultButton, Control owner = null, bool? buttonAutosize = null/*, bool? noDuplicate = null*/, bool? topMost = null)
        {
            return ShowDialog(title, GetIcon(icon), message, buttons, defaultButton, owner, buttonAutosize/*, noDuplicate*/, topMost);
        }

        public static int ShowDialog(string title, Icon icon, string message, string[] buttons, int defaultButton, Control owner, bool? buttonAutosize = null/*, bool? noDuplicate = null*/, bool? topMost = null)
        {
            Message.Config c = new Message.Config { Title = title ?? Title ?? AppName, Icon = icon, Message = message, Buttons = buttons, DefaultButton = defaultButton, Owner = owner ?? Owner, ButtonAutosize = buttonAutosize ?? ButtonAutosize, /*NoDuplicate = noDuplicate ?? NoDuplicate,*/ TopMost = topMost ?? TopMost };
            return MessageBox.ShowDialog(c);
        }

        public static MessageBox MessageBox = new MessageBox();

        public static Action<Message.Config> OnShowing
        {
            set { MessageBox.OnShowing = value; }
            get { return MessageBox.OnShowing; }
        }
    }
}
