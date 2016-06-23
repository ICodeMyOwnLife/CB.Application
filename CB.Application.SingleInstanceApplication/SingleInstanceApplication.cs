namespace CB.Application.SingleInstanceApplication
{
    public class SingleInstanceApplication<TProcessArgs>: SingleInstanceApplicationBase where TProcessArgs: IProcessArgs
    {
        #region Fields
        protected TProcessArgs _argsProcessor;
        #endregion


        #region Override
        public override void ProcessArgs(string[] args)
            => _argsProcessor.ProcessArgs(args);
        #endregion
    }
}