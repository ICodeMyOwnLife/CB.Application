namespace CB.Application.ContextMenuCommands
{
    public class ContextMenuCommandIcon
    {
        #region  Properties & Indexers
        public static ContextMenuCommandIcon None { get; } = new ContextMenuCommandIcon();
        public string Path { get; private set; }
        #endregion


        #region Methods
        public static ContextMenuCommandIcon FromAppPath(string app)
            => new ContextMenuCommandIcon { Path = $"\"{app}\",0" };

        public static ContextMenuCommandIcon FromIconPath(string path) => new ContextMenuCommandIcon { Path = path };
        #endregion
    }

    /*public struct ContextMenuCommandIcon
    {
        #region Fields
        internal bool _useDefaultAppIcon;
        #endregion


        #region  Constructors & Destructor
        public ContextMenuCommandIcon(string iconPath)
        {
            _useDefaultAppIcon = false;
            IconPath = iconPath;
        }
        #endregion


        #region  Properties & Indexers
        public static ContextMenuCommandIcon DefaultAppIcon => new ContextMenuCommandIcon { _useDefaultAppIcon = true };
        public static ContextMenuCommandIcon None => new ContextMenuCommandIcon { IconPath = null };
        public string IconPath { get; set; }

        internal bool UseDefaultAppIcon
        {
            get { return _useDefaultAppIcon; }
            set { _useDefaultAppIcon = value; }
        }
        #endregion


        #region Methods
        public static ContextMenuCommandIcon FromPath(string iconPath) => new ContextMenuCommandIcon(iconPath);
        #endregion


        #region Implementation
        internal string GetDefaultAppIcon(string appPath) => $"\"{0}\",0";
        #endregion
    }*/
}