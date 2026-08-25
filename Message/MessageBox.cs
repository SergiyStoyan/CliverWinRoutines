//********************************************************************************************
//Author: Sergiy Stoyan
//        s.y.stoyan@gmail.com, sergiy.stoyan@outlook.com, stoyan@cliversoft.com
//        http://www.cliversoft.com
//********************************************************************************************


using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static Cliver.Message;
using Cliver.Win;

namespace Cliver//.Win (!)for backword compatibility and not adding using Cliver.Win;
{
    public class MessageBoxOnTop : MessageBox
    {
        public MessageBoxOnTop()
        {
            OnShowing = SetOnTop;
        }
    }

    /// <summary>
    /// Show MessageForm with properties managed on fly
    /// </summary>
    public partial class MessageBox
    {
        public Action<Message.Config> OnShowing = null;

        virtual public int ShowDialog(Message.Config c)
        {
            OnShowing?.Invoke(c);
            c.FormIcon = c.FormIcon ?? Cliver.Message.GetAppIcon();
            c.Owner = c.Owner?.FindForm();
            if (c.Owner?.Visible != true)
                c.Owner = null;
            if (c.Owner?.InvokeRequired == true)
                return (int)c.Owner.Invoke(showDialog, c);
            return showDialog(c);
        }

        virtual protected int showDialog(Message.Config c)
        {
            //string caller = null;
            //if (c.NoDuplicate ?? NoDuplicate)
            //{
            //    StackTrace st = new StackTrace(true);
            //    StackFrame sf = null;
            //    for (int i = 1; i < st.FrameCount; i++)
            //    {
            //        sf = st.GetFrame(i);
            //        string file_name = sf.GetFileName();
            //        if (file_name == null || !Regex.IsMatch(file_name, @"\\Message\.cs$"))
            //            break;
            //    }
            //    caller = sf.GetMethod().Name + "," + sf.GetNativeOffset().ToString();

            //    lock (callers2message)
            //    {
            //        if (callers2message.TryGetValue(caller, out string m) && m == c.Message)
            //            return -1;
            //        callers2message[caller] = c.Message;
            //    }
            //}

            MessageForm mf = new MessageForm(c);
            int result = mf.ShowDialog();

            //if (c.NoDuplicate ?? NoDuplicate)
            //    lock (callers2message)
            //    {
            //        callers2message.Remove(caller);
            //    }

            return result;
        }
        //static Dictionary<string, string> callers2message = new Dictionary<string, string>();

        virtual public int ShowDialog(Icons icon, string message, string[] buttons, int defaultButton, string title = null)
        {
            return ShowDialog(GetIcon(icon), message, buttons, defaultButton, title);
        }

        virtual public int ShowDialog(Icon icon, string message, string[] buttons, int defaultButton, string title = null)
        {
            Message.Config c = new Message.Config { Title = title ?? AppName, Icon = icon, Message = message, Buttons = buttons, DefaultButton = defaultButton };
            return ShowDialog(c);
        }

        public void Show(Exception exception, string title = null)
        {
            if (exception is MessageException me)
            {
                switch (me.MessageType)
                {
                    case Log.MessageType.TRACE:
                    case Log.MessageType.DEBUG:
                    case Log.MessageType.INFORM:
                        ShowDialog(Icons.Information, Log.GetExceptionMessage(me, me.PrintDetails), ["OK"], 0, title);
                        break;
                    case Log.MessageType.ERROR:
                    case Log.MessageType.EXIT:
                        ShowDialog(Icons.Error, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(me, me.PrintDetails), ["OK"], 0, title);
                        break;
                    case Log.MessageType.WARNING:
                        ShowDialog(Icons.Warning, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(me, me.PrintDetails), ["OK"], 0, title);
                        break;
                    default:
                        throw new Exception("Unknown option: " + me.MessageType);
                }
                return;
            }
            Error(exception, title);
        }

        public void Inform(string message, string title = null)
        {
            ShowDialog(Icons.Information, message, ["OK"], 0, title);
        }

        public void Exclaim(string message, string title = null)
        {
            ShowDialog(Icons.Exclamation, message, ["OK"], 0, title);
        }

        public void Exclaim0(Exception e, string title = null)
        {
            ShowDialog(Icons.Exclamation, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(e, !(e is Exception2)), ["OK"], 0, title);
        }

        public void Exclaim(Exception e, string title = null)
        {
            ShowDialog(Icons.Exclamation, Log.GetExceptionMessage2(e), ["OK"], 0, title);
        }

        public void Warning(string message, string title = null)
        {
            ShowDialog(Icons.Warning, message, ["OK"], 0, title);
        }

        public void Warning(Exception e, string title = null)
        {
            ShowDialog(Icons.Warning, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(e, !(e is Exception2)), ["OK"], 0, title);
        }

        public void Warning2(Exception e, string title = null)
        {
            ShowDialog(Icons.Warning, Log.GetExceptionMessage2(e), ["OK"], 0, title);
        }

        public void Error(Exception e, string title = null)
        {
            ShowDialog(Icons.Error, /*GetExceptionDetails(e)*/Log.GetExceptionMessage(e, !(e is Exception2)), ["OK"], 0, title);
        }

        public void Error2(Exception e, string title = null)
        {
            Error(Log.GetExceptionMessage2(e), title);
        }

        public void Error(string subtitle, Exception e, string title = null)
        {
            Error(subtitle + "\r\n\r\n" + Log.GetExceptionMessage(e, !(e is Exception2)), title);
        }

        public void Error2(string subtitle, Exception e, string title = null)
        {
            Error(subtitle + "\r\n\r\n" + Log.GetExceptionMessage2(e), title);
        }

        public void Error(string message, string title = null)
        {
            ShowDialog(Icons.Error, message, ["OK"], 0, title);
        }

        public bool YesNo(string question, Icons icon = Icons.Question, bool defaultIsYes = true, string title = null)
        {
            return ShowDialog(icon, question, ["Yes", "No"], defaultIsYes ? 0 : 1, title) == 0;
        }
    }
}

