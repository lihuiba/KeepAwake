using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeepAwake
{
    class Program
    {
        public static void Main()
        {
            var program = new Program();
            program.run();
        }

        NotifyIcon notifyicon;
        MenuItem mtNoScreenSaver, mtNoScreenShutdown, mtNoSystemSleep;
        MenuItem mtForever, mtTiming, mtStart, mtExit;
        public void run()
        {
            var menu = new ContextMenu();
            mtNoScreenSaver = menu.MenuItems.Add("阻止屏保", NoScreenSaver);
            mtNoScreenShutdown = menu.MenuItems.Add("阻止关闭屏幕", NoScreenShutdown);
            mtNoSystemSleep = menu.MenuItems.Add("阻止待机（睡眠）", NoSystemSleep);
            mtNoScreenSaver.Enabled = false;
            mtNoScreenShutdown.Enabled = false;
            mtNoSystemSleep.Checked = true;
            menu.MenuItems.Add("-");

            mtForever = menu.MenuItems.Add("长期", Forever);
            mtTiming = menu.MenuItems.Add("计时", Timing);
            mtForever.Checked = true;
            mtTiming.Enabled = false;
            menu.MenuItems.Add("-");

            foreach (var item in menu.MenuItems)
                (item as MenuItem).RadioCheck = true;

            mtStart = menu.MenuItems.Add("开始", OnStartStop);
            mtExit = menu.MenuItems.Add("退出", OnExit);
            mtStart.Tag = "停止";

            notifyicon = new NotifyIcon();
            notifyicon.ContextMenu = menu;
            notifyicon.Visible = true;
            notifyicon.Icon = SystemIcons.Shield;
            Application.Run();
        }

        private void NoScreenSaver(object sender, EventArgs e)
        {
            mtNoScreenSaver.Checked = true;
            mtNoScreenShutdown.Checked = false;
            mtNoSystemSleep.Checked = false;
        }
        private void NoScreenShutdown(object sender, EventArgs e)
        {
            mtNoScreenSaver.Checked = false;
            mtNoScreenShutdown.Checked = true;
            mtNoSystemSleep.Checked = false;
        }
        private void NoSystemSleep(object sender, EventArgs e)
        {
            mtNoScreenSaver.Checked = false;
            mtNoScreenShutdown.Checked = false;
            mtNoSystemSleep.Checked = true;
        }
        private void Forever(object sender, EventArgs e)
        {
            mtForever.Checked = true;
            mtTiming.Checked = false;
        }
        private void Timing(object sender, EventArgs e)
        {
            mtForever.Checked = false;
            mtTiming.Checked = true;
        }

        Awaker awaker = new Awaker();
        private void OnStartStop(object sender, EventArgs e)
        {
            var s = mtStart.Tag as string;
            mtStart.Tag = mtStart.Text;
            mtStart.Text = s;
            if (awaker.isStarted) awaker.Stop();
            else awaker.Start();
        }
        private void OnExit(object sender, EventArgs e)
        {
            awaker.Stop();
            notifyicon.Visible = false;
            Application.Exit();
        }
    }
}
