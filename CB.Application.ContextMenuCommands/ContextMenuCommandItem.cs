using Microsoft.Win32;


namespace CB.Application.ContextMenuCommands
{
    public class ContextMenuCommandItem: ContextMenuCommandItemBase
    {
        #region  Properties & Indexers
        public string Command { get; set; }
        #endregion


        #region Override
        public override void CreateIn(RegistryKey key)
        {
            base.CreateIn(key);
            UseMainKey(key, mainKey =>
            {
                mainKey.SetValue("", Name);
                using (var commandKey = mainKey.OpenOrCreateSubKey(COMMAND))
                {
                    commandKey.SetValue("", Command);
                }
            });
        }
        #endregion
    }
}