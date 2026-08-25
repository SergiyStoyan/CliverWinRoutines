//********************************************************************************************
//Author: Sergiy Stoyan
//        s.y.stoyan@gmail.com, sergiy.stoyan@outlook.com, stoyan@cliversoft.com
//        http://www.cliversoft.com
//********************************************************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliver//.Win (!)for backword compatibility and not adding using Cliver.Win;
{
    /// <summary>
    /// Show MessageForm with predefined features
    /// </summary>
    public partial class Message
    {
        public static async Task InformAsync(this Control owner, string message)
        {
            await Task.Run(() => { Message.Inform(message, owner); });
        }

        public static async Task ExclaimAsync(this Control owner, string message)
        {
            await Task.Run(() => { Message.Exclaim(message, owner); });
        }

        public static async Task Exclaim0Async(this Control owner, Exception e)
        {
            await Task.Run(() => { Message.Exclaim0(e, owner); });
        }

        public static async Task ExclaimAsync(this Control owner, Exception e)
        {
            await Task.Run(() => { Message.Exclaim(e, owner); });
        }

        public static async Task WarningAsync(this Control owner, string message)
        {
            await Task.Run(() => { Message.Warning(message, owner); });
        }

        public static async Task WarningAsync(this Control owner, Exception e)
        {
            await Task.Run(() => { Message.Warning(e, owner); });
        }

        public static async Task Warning2Async(this Control owner, Exception e)
        {
            await Task.Run(() => { Message.Warning2(e, owner); });
        }

        public static async Task ErrorAsync(this Control owner, Exception e)
        {
            await Task.Run(() => { Message.Error(e, owner); });
        }

        public static async Task Error2Async(this Control owner, Exception e)
        {
            await Task.Run(() => { Message.Error2(e, owner); });
        }

        public static async Task ErrorAsync(this Control owner, string subtitle, Exception e)
        {
            await Task.Run(() => { Message.Error(subtitle, e, owner); });
        }

        public static async Task Error2Async(this Control owner, string subtitle, Exception e)
        {
            await Task.Run(() => { Message.Error2(subtitle, e, owner); });
        }

        public static async Task ErrorAsync(this Control owner, string message)
        {
            await Task.Run(() => { Message.Error(message, owner); });
        }

        public static async Task<bool> YesNoAsync(this Control owner, string question, Icons icon = Icons.Question, bool defaultIsYes = true)
        {
            return await Task.Run(() => { return Message.YesNo(question, owner, icon, defaultIsYes); });
        }

        public static async Task<int> ShowDialogAsync(this Control owner, string title, Icons icon, string message, string[] buttons, int defaultButton, bool? buttonAutosize = null/*, bool? noDuplicate = null*/, bool? topMost = null)
        {
            return await Task.Run(() => { return Message.ShowDialog(title, icon, message, buttons, defaultButton, owner, buttonAutosize/*, noDuplicate*/, topMost); });

        }

        public static async Task<int> ShowDialogAsync(this Control owner, string title, Icon icon, string message, string[] buttons, int defaultButton, bool? buttonAutosize = null/*, bool? noDuplicate = null*/, bool? topMost = null)
        {
            return await Task.Run(() => { return Message.ShowDialog(title, icon, message, buttons, defaultButton, owner, buttonAutosize/*, noDuplicate*/, topMost); });
        }
    }
}