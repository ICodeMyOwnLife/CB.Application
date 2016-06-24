using System.Linq;
using Microsoft.VisualBasic.ApplicationServices;


namespace CB.Application.SingleInstanceApplication
{
    public class SingleInstanceApplicationController<TApplication>: WindowsFormsApplicationBase
        where TApplication: IRun, IArgsProcessor, IInitializeComponent, new()
    {
        #region Fields
        private TApplication _app;
        #endregion


        #region  Constructors & Destructor
        public SingleInstanceApplicationController()
        {
            IsSingleInstance = true;
        }
        #endregion


        #region Override
        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            _app = new TApplication();
            _app.InitializeComponent();
            _app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            if (eventArgs.CommandLine.Count > 0)
            {
                _app.ArgsProcessor?.ProcessArgs(eventArgs.CommandLine.ToArray());
            }
        }
        #endregion
    }
}