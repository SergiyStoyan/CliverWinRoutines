//********************************************************************************************
//Author: Sergiy Stoyan
//        s.y.stoyan@gmail.com, sergiy.stoyan@outlook.com, stoyan@cliversoft.com
//        http://www.cliversoft.com
//********************************************************************************************



using System;
using System.Drawing;
using System.Threading.Tasks;
using static Cliver.Message;

namespace Cliver//.Win (!)for backword compatibility and not adding using Cliver.Win;
{
    public partial class MessageBox
    {
        public async Task InformAsync(string message, string title = null)
        {
            await Task.Run(() => { Inform(message, title); });
        }

        public  async Task ExclaimAsync(string message, string title = null)
        {
            await Task.Run(() => { Exclaim(message, title); });
        }

        public  async Task Exclaim0Async(Exception e, string title = null)
        {
            await Task.Run(() => { Exclaim0(e, title); });
        }

        public  async Task ExclaimAsync(Exception e, string title = null)
        {
            await Task.Run(() => { Exclaim(e, title); });
        }

        public  async Task WarningAsync(string message, string title = null)
        {
            await Task.Run(() => { Warning(message, title); });
        }

        public  async Task WarningAsync(Exception e, string title = null)
        {
            await Task.Run(() => { Warning(e, title); });
        }

        public  async Task Warning2Async(Exception e, string title = null)
        {
            await Task.Run(() => { Warning2(e, title); });
        }

        public  async Task ErrorAsync(Exception e, string title = null)
        {
            await Task.Run(() => { Error(e, title); });
        }

        public  async Task Error2Async(Exception e, string title = null)
        {
            await Task.Run(() => { Error2(e, title); });
        }

        public  async Task ErrorAsync(string subtitle, Exception e, string title = null)
        {
            await Task.Run(() => { Error(subtitle, e, title); });
        }

        public  async Task Error2Async(string subtitle, Exception e, string title = null)
        {
            await Task.Run(() => { Error2(subtitle, e, title); });
        }

        public  async Task ErrorAsync(string message, string title = null)
        {
            await Task.Run(() => { Error(message, title); });
        }

        public  async Task<bool> YesNoAsync(string question, Icons icon = Icons.Question, bool defaultIsYes = true, string title = null)
        {
            return await Task.Run(() => { return YesNo(question, icon, defaultIsYes, title); });
        }

        public  async Task<int> ShowDialogAsync(Icons icon, string message, string[] buttons, int defaultButton, string title = null)
        {
            return await Task.Run(() => { return ShowDialog(icon, message, buttons, defaultButton, title); });

        }

        public  async Task<int> ShowDialogAsync(Icon icon, string message, string[] buttons, int defaultButton, string title = null)
        {
            return await Task.Run(() => { return ShowDialog(icon, message, buttons, defaultButton, title); });
        }
    }
}

