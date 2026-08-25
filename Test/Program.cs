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
            {
                throw new Exception("test");

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