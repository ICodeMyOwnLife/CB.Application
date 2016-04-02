using System;
using System.Threading;
using System.Windows.Forms;
using CB.Win32.Windows;


namespace CB.Application.SingleInstanceApplication
{
    public static class SingleInstance
    {
        #region Methods
        public static void RunSingleInstance(Form windowForm)
        {
            Mutex mutex;
            if (IsNewInstance(windowForm.Text, out mutex))
            {
                System.Windows.Forms.Application.Run(windowForm);
            }
            else
            {
                ActivateCurrentInstance(windowForm.Text);
            }
        }

        public static void SetSingleInstance(System.Windows.Application app, string windowName, out object idObj)
        {
            Mutex mutex;
            if (!IsNewInstance(windowName, out mutex))
            {
                ActivateCurrentInstance(windowName);
                app.Startup += delegate { app.Shutdown(); };
            }
            idObj = mutex;
        }
        #endregion


        #region Implementation
        private static void ActivateCurrentInstance(string windowName)
        {
            var hWnd = Window.FindWindow(null, windowName);
            if (hWnd != IntPtr.Zero)
            {
                Window.SetForegroundWindow(hWnd);
                Window.ShowWindow(hWnd, ShowWindowFlags.Normal);
            }
        }

        private static bool IsNewInstance(string windowName, out Mutex mutex)
        {
            bool isNewInstance;
            mutex = new Mutex(true, windowName + "_singe_instance_", out isNewInstance);
            return isNewInstance;
        }
        #endregion
    }
}