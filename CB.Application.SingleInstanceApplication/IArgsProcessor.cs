namespace CB.Application.SingleInstanceApplication
{
    public interface IArgsProcessor
    {
        #region Abstract
        IProcessArgs ArgsProcessor { get; }
        #endregion
    }
}