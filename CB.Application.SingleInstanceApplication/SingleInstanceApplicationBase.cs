namespace CB.Application.SingleInstanceApplication
{
    public abstract class SingleInstanceApplicationBase: System.Windows.Application, IProcessArgs
    {
        #region Abstract
        public abstract void ProcessArgs(string[] args);
        #endregion
    }
}