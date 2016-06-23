using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;


namespace CB.Application.ContextMenuCommands
{
    public abstract class ContextMenuCommandItem
    {
        #region Fields
        protected const string COMMAND = "command";
        protected const string ICON = "icon";
        protected const string SHELL = "shell";
        #endregion


        #region  Properties & Indexers
        public ContextMenuCommandIcon Icon { get; set; }
        public string Name { get; set; }
        #endregion


        #region Methods
        public static SingleContextMenuCommandItem FromAppPath(string name, string appPath,
            bool passFilePathToApp, string iconPath, params string[] args)
            => FromAppPath(name, appPath, passFilePathToApp, ContextMenuCommandIcon.FromIconPath(iconPath), args);

        public static SingleContextMenuCommandItem FromAppPath(string name, string appPath,
            bool passFilePathToApp, ContextMenuCommandIcon icon, params string[] args)
            => new SingleContextMenuCommandItem
            {
                Name = name,
                Icon = icon,
                Command = CreateCommand(appPath, passFilePathToApp, args)
            };

        public static SingleContextMenuCommandItem FromAppPath(string name, string appPath,
            bool passFilePathToApp, bool useDefaultIcon, params string[] args)
            =>
                FromAppPath(name, appPath, passFilePathToApp,
                    useDefaultIcon ? ContextMenuCommandIcon.FromAppPath(appPath) : ContextMenuCommandIcon.None, args);

        public virtual void CreateIn(RegistryKey key)
            => UseMainKey(key, mainKey =>
            {
                if (!string.IsNullOrEmpty(Icon.Path))
                {
                    mainKey.SetValue(ICON, Icon.Path);
                }
            });
        #endregion


        #region Implementation
        private static string CreateCommand(string appPath, bool passFilePathToApp, IEnumerable<string> args)
        {
            var list = new List<string> { appPath };
            if (passFilePathToApp)
            {
                list.Add("%1");
            }
            list.AddRange(args);
            return string.Join(" ", list.Select(s => $"\"{s}\""));
        }

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