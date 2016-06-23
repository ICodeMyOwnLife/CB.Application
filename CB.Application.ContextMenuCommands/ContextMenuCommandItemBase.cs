using System;
using Microsoft.Win32;


namespace CB.Application.ContextMenuCommands
{
    public abstract class ContextMenuCommandItemBase
    {
        #region Fields
        protected const string COMMAND = "command";
        protected const string ICON = "icon";
        protected const string SHELL = "shell";
        #endregion


        #region  Properties & Indexers
        public string Icon { get; set; }
        public string Name { get; set; }
        #endregion


        #region Methods
        public virtual void CreateIn(RegistryKey key)
            => UseMainKey(key, mainKey =>
            {
                if (!string.IsNullOrEmpty(Icon))
                {
                    mainKey.SetValue(ICON, Icon);
                }
            });
        #endregion


        #region Implementation
        protected virtual void UseMainKey(RegistryKey key, Action<RegistryKey> action)
        {
            using (var shellKey = key.OpenOrCreateSubKey(SHELL))
            {
                using (var mainKey = shellKey.OpenOrCreateSubKey(Name))
                {
                    action(mainKey);
                }
            }
        }
        #endregion
    }
}