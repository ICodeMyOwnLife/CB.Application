namespace CB.Application.SingleInstanceApplication
{
    public interface IProcessArgs
    {
        #region Abstract
        void ProcessArgs(string[] args);
        #endregion
    }
}