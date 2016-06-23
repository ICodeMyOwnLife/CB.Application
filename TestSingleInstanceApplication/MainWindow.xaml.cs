using CB.Application.SingleInstanceApplication;


namespace TestSingleInstanceApplication
{
    public partial class MainWindow: IProcessArgs
    {
        #region  Constructors & Destructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion


        #region Methods
        public void ProcessArgs(string[] args)
        {
            foreach (var arg in args)
            {
                lstArgs.Items.Add(arg);
            }
        }
        #endregion
    }
}