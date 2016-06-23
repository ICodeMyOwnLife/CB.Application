using System.Windows;


namespace CB.Application.SingleInstanceApplication
{
    public class SingleInstanceViewApplication<TWindow>: SingleInstanceApplication<TWindow>
        where TWindow: Window, IProcessArgs, new()
    {
        #region Override
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _argsProcessor = new TWindow();
            _argsProcessor.ProcessArgs(e.Args);
            _argsProcessor.Show();
        }
        #endregion
    }
}