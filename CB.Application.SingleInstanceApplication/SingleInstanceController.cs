using System.Linq;
using Microsoft.VisualBasic.ApplicationServices;


namespace CB.Application.SingleInstanceApplication
{
    public class SingleInstanceController<TApplication>: WindowsFormsApplicationBase
        where TApplication: SingleInstanceApplicationBase, new()
    {
        #region Fields
        private TApplication _app;
        #endregion


        #region  Constructors & Destructor
        public SingleInstanceController()
        {
            IsSingleInstance = true;
        }
        #endregion


        #region Override
        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            _app = new TApplication();
            _app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
            => _app?.ProcessArgs(eventArgs.CommandLine.ToArray());
        #endregion
    }
}