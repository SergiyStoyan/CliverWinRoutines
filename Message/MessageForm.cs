//********************************************************************************************
//Author: Sergiy Stoyan
//        s.y.stoyan@gmail.com, sergiy.stoyan@outlook.com, stoyan@cliversoft.com
//        http://www.cliversoft.com
//********************************************************************************************


using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Cliver.Win
{
    /// <summary>
    /// Confgurable dialog box
    /// </summary>
    public partial class MessageForm : Form
    {
        public MessageForm(Message.Config config)
        {
            InitializeComponent();

            //Load += delegate!!!closes the form immediately
            //{
            set(config);
            //};

            FormClosing += delegate (object sender, FormClosingEventArgs e)
            {
            };
        }

        public MessageForm(string title, Icon icon, string message, string[] buttons, int defaultButton, Form owner, bool buttonAutosize = false)
            : this(new Message.Config { Title = title, Icon = icon, Message = message, Buttons = buttons, DefaultButton = defaultButton, ButtonAutosize = buttonAutosize })
        { }

        void set(Message.Config c)
        {
            if (!IsHandleCreated)
                CreateHandle();

            config = c;

            this.Icon = c.FormIcon;

            this.MaximizeBox = c.MaximizeBox;

            Owner = c.Owner?.FindForm();

            this.Text = c.Title;

            if (c.Icon != null)
            {
                int w = c.Icon.Width - imageBox.Width;
                imageBox.Image = (Image)c.Icon.ToBitmap();
                if (w > 0)
                {
                    this.Width += w;
                    this.message.Width -= w;
                    this.message.Left = this.message.Left + w;
                }
            }

            this.message.Text = c.Message;

            if (c.Buttons != null)
            {
                for (int i = c.Buttons.Length - 1; i >= 0; i--)
                {
                    Button b = new Button();
                    b.Tag = i;
                    b.Text = c.Buttons[i];
                    b.AutoSize = true;
                    b.Click += button_Click;
                    flowLayoutPanel1.Controls.Add(b);
                    if (i == c.DefaultButton)
                        b.Select();
                }

                if (!c.ButtonAutosize)
                {
                    Size max_size = new Size(0, 0);
                    foreach (Button b in flowLayoutPanel1.Controls)
                    {
                        if (b.Width > max_size.Width)
                            max_size.Width = b.Width;
                        if (b.Height > max_size.Height)
                            max_size.Height = b.Height;
                    }
                    foreach (Button b in flowLayoutPanel1.Controls)
                    {
                        b.AutoSize = false;
                        b.Size = max_size;
                    }
                }
            }

            this.TopMost = c.TopMost;

            this.ShowInTaskbar = c.ShowInTaskbar;

            //Size s = this.message.GetPreferredSize(new Size(Screen.PrimaryScreen.WorkingArea.Width * 3 / 4, Screen.PrimaryScreen.WorkingArea.Height * 3 / 4));
            //this.Width = this.Width + s.Width - this.message.Width;
            //this.Height = this.Height + s.Height - this.message.Height;
        }

        Message.Config config;

        private void button_Click(object sender, EventArgs e)
        {
            ClickedButton = (int)((Button)sender).Tag;
            this.Close();
        }

        public int ClickedButton { get; private set; } = -1;

        new public int ShowDialog()
        {
            base.ShowDialog();
            return ClickedButton;
        }

        private void message_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            var rtb = (RichTextBox)sender;
            Size s = this.Size;
            {
                int h = e.NewRectangle.Height - rtb.Height;
                if (h > 0)
                {
                    int h2 = (int)(Screen.PrimaryScreen.WorkingArea.Height * config.ScreenMaxPart.Height) - this.Height;
                    s.Height += h2 < h ? h2 : h;
                }
            }
            {
                int w = e.NewRectangle.Width - rtb.Width;
                if (w > 0)
                {
                    int w2 = (int)(Screen.PrimaryScreen.WorkingArea.Width * config.ScreenMaxPart.Width) - this.Width;
                    s.Width += w2 < w ? w2 : w;
                }
            }
            {
                int d = s.Height - s.Width;
                if (d > 0)
                {
                    s.Height -= d;
                    s.Width += d;
                }
            }
            this.Size = s;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x0112) // WM_SYSCOMMAND
            {
                switch ((Int32)m.WParam)
                {
                    case 0xF030: // Maximize event - SC_MAXIMIZE from Winuser.h
                        restoredSize = this.Size;
                        break;
                    case 0xF120: // Restore event - SC_RESTORE from Winuser.h
                        this.Size = restoredSize;
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private Size restoredSize;

        public new void Close()
        {
            try
            {
                ControlRoutines.Invoke(this, () =>
                {
                    try
                    {
                        base.Close();
                    }
                    catch { }//if closed already
                });
            }
            catch { }//if closed already
        }

        private void message_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Cliver.ProcessRoutines.Open(e.LinkText);
            }
            catch (Exception ex)
            {
                this.Error2(ex);
            }
        }
    }
}