using System.Linq;
using Microsoft.VisualBasic.ApplicationServices;


namespace TestSingleInstanceApplication
{
    public class SingleInstanceAppController: WindowsFormsApplicationBase
    {
        #region Fields
        private App _app;
        #endregion


        #region  Constructors & Destructor
        public SingleInstanceAppController()
        {
            IsSingleInstance = true;
        }
        #endregion


        #region Override
        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            _app = new App();
            _app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            _app.ProcessArgs(eventArgs.CommandLine.ToArray());
        }
        #endregion
    }
}