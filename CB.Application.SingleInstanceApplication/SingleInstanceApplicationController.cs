using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.ApplicationServices;


namespace CB.Application.SingleInstanceApplication
{
    public class SingleInstanceApplicationController<TApplication>: WindowsFormsApplicationBase
        where TApplication: IProcessArgsApplication, new()
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
            ProcessCommandLine(eventArgs.CommandLine);
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
            => ProcessCommandLine(eventArgs.CommandLine);
        #endregion


        #region Implementation
        private void ProcessCommandLine(IReadOnlyCollection<string> commandLine)
        {
            if (commandLine.Count > 0)
            {
                _app?.ProcessArgs(commandLine.ToArray());
            }
        }
        #endregion
    }
}