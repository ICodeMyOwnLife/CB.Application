using Microsoft.Win32;


namespace CB.Application.ContextMenuCommands
{
    public static class RegistryHelpers
    {
        #region Methods
        public static RegistryKey OpenOrCreateSubKey(this RegistryKey key, string name)
            => key.OpenSubKey(name, true) ?? key.CreateSubKey(name);
        #endregion
    }
}