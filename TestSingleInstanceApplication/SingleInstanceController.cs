using System;
using CB.Application.SingleInstanceApplication;


namespace TestSingleInstanceApplication
{
    public class SingleInstanceController: SingleInstanceController<SingleInstanceViewApplication>
    {
        #region Implementation
        [STAThread]
        private static void Main(string[] args)
        {
            new SingleInstanceController().Run(args);
        }
        #endregion
    }
}