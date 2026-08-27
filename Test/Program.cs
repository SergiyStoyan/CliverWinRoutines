using Cliver;

namespace Test
{
    internal static class Program
    {
        public static MessageBoxOnTop MessageOnTop = new MessageBoxOnTop();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {//DateTime.Now.GetSecondsSinceUnixEpoch
                string f = Log.GetReferencedAssembliesInfo(nameof(Cliver));
                                                throw new Exception(f);

                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();

                MessageOnTop.Inform("test");

                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageOnTop.Error(ex);
            }
        }
    }
}