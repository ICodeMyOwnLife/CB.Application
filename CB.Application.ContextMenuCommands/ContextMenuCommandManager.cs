using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;


namespace CB.Application.ContextMenuCommands
{
    public class ContextMenuCommandManager
    {
        #region Fields
        private const string ALL_DIRECTORIES = "Directory";
        private const string ALL_DRIVES = "Drive";
        private const string ALL_FILES = "*";
        private const string BACKGROUND = @"Directory\Background";
        private const string CMD = "Command";
        private const string ICON = "Icon";
        private const string REG_PATH = @"\{0}\shell";
        #endregion


        #region Methods
        public static void AddCommand(CommandCategories categories, ContextMenuCommandItem commandItem)
        {
            foreach (var subKeyName in GetCategorySubKeys(categories))
            {
                using (var key = Registry.ClassesRoot.OpenOrCreateSubKey(subKeyName))
                {
                    commandItem.CreateIn(key);
                }
            }
        }

        public static bool AddCommand(CommandCategories categories, string cmdName, string appPath,
            bool passFilePathToApp, string iconPath = null, params string[] additionalArgs)
            => AddCommand(categories, cmdName, iconPath, CreateCmdArgArray(appPath, passFilePathToApp, additionalArgs));

        public static bool AddCommand(CommandCategories categories, string cmdName, string appPath,
            bool passFilePathToApp, bool useDefaultIcon = false, params string[] additionalArgs)
            => AddCommand(categories, cmdName, GetIconPath(appPath, useDefaultIcon),
                CreateCmdArgArray(appPath, passFilePathToApp, additionalArgs));

        public static bool AddCommand(string fileType, string cmdName, string appPath, bool passFilePathToApp,
            string iconPath = null, params string[] additionalArgs)
            => AddCommand(fileType, cmdName, iconPath, CreateCmdArgArray(appPath, passFilePathToApp, additionalArgs));

        public static bool AddCommand(string fileType, string cmdName, string appPath, bool passFilePathToApp,
            bool useDefaultIcon, params string[] additionalArgs)
            => AddCommand(fileType, cmdName, appPath, passFilePathToApp, GetIconPath(appPath, useDefaultIcon),
                additionalArgs);

        public static bool AddCommand(CommandCategories categories, string cmdName, string iconPath = null,
            params string[] cmdArgs)
        {
            var result = true;

            if (categories.HasFlag(CommandCategories.AllDirectories))
            {
                result &= AddCommandHelper(ALL_DIRECTORIES, cmdName, cmdArgs, iconPath);
            }

            if (categories.HasFlag(CommandCategories.AllDrives))
            {
                result &= AddCommandHelper(ALL_DRIVES, cmdName, cmdArgs, iconPath);
            }

            if (categories.HasFlag(CommandCategories.AllFiles))
            {
                result &= AddCommandHelper(ALL_FILES, cmdName, cmdArgs, iconPath);
            }

            if (categories.HasFlag(CommandCategories.Background))
            {
                result &= AddCommandHelper(BACKGROUND, cmdName, cmdArgs, iconPath);
            }
            return result;
        }

        public static bool AddCommand(string fileType, string cmdName, string iconPath = null, params string[] cmdArgs)
            => AddCommandHelper(GetSubKeyFromFileType(fileType), cmdName, cmdArgs, iconPath);

        public static bool RemoveCommand(CommandCategories categories, string cmdName)
        {
            var result = true;

            if (categories.HasFlag(CommandCategories.AllDirectories))
            {
                result &= RemoveCommandHelper(ALL_DIRECTORIES, cmdName);
            }
            if (categories.HasFlag(CommandCategories.AllDrives))
            {
                result &= RemoveCommandHelper(ALL_DRIVES, cmdName);
            }
            if (categories.HasFlag(CommandCategories.AllFiles))
            {
                result &= RemoveCommandHelper(ALL_FILES, cmdName);
            }
            if (categories.HasFlag(CommandCategories.Background))
            {
                result &= RemoveCommandHelper(BACKGROUND, cmdName);
            }

            return result;
        }

        public static bool RemoveCommand(string fileType, string cmdName)
        {
            return RemoveCommandHelper(GetSubKeyFromFileType(fileType), cmdName);
        }
        #endregion


        #region Implementation
        private static bool AddCommandHelper(string subKey, string cmdName, IEnumerable<string> cmdArgs,
            string iconPath = null)
        {
            using (var shellKey = GetShellKey(subKey))
            {
                if (shellKey == null) return false;

                using (var nameKey = shellKey.OpenSubKey(cmdName, true) ?? shellKey.CreateSubKey(cmdName))
                {
                    if (nameKey == null) return false;
                    if (!string.IsNullOrEmpty(iconPath))
                    {
                        nameKey.SetValue(ICON, iconPath);
                    }

                    using (var cmdKey = nameKey.OpenSubKey(CMD, true) ?? nameKey.CreateSubKey(CMD))
                    {
                        if (cmdKey == null) return false;

                        cmdKey.SetValue("", string.Join(" ", cmdArgs.Select(s => '"' + s + '"')));
                        return true;
                    }
                }
            }
        }

        private static string[] CreateCmdArgArray(string appPath, bool passFilePathToApp,
            IEnumerable<string> additionalArgs)
        {
            var cmdArgList = new List<string> { appPath };
            if (passFilePathToApp)
            {
                cmdArgList.Add("%1");
            }
            cmdArgList.AddRange(additionalArgs);
            var cmdArgs = cmdArgList.ToArray();
            return cmdArgs;
        }

        private static IEnumerable<string> GetCategorySubKeys(CommandCategories categories)
        {
            var subKeys = new List<string>();
            if (categories.HasFlag(CommandCategories.AllDirectories))
            {
                subKeys.Add(ALL_DIRECTORIES);
            }
            if (categories.HasFlag(CommandCategories.AllDrives))
            {
                subKeys.Add(ALL_DRIVES);
            }
            if (categories.HasFlag(CommandCategories.AllFiles))
            {
                subKeys.Add(ALL_FILES);
            }
            if (categories.HasFlag(CommandCategories.Background))
            {
                subKeys.Add(BACKGROUND);
            }
            return subKeys;
        }

        private static string GetIconPath(string appPath, bool useDefaultIcon)
            => useDefaultIcon ? $"\"{appPath}\",0" : null;

        private static RegistryKey GetShellKey(string subKey)
        {
            var shellKeyPath = string.Format(REG_PATH, subKey);
            return Registry.ClassesRoot.OpenSubKey(shellKeyPath, true) ??
                   Registry.ClassesRoot.CreateSubKey(shellKeyPath);
        }

        private static string GetSubKeyFromFileType(string fileType)
        {
            return fileType.StartsWith(".") ? fileType : "." + fileType;
        }

        private static bool RemoveCommandHelper(string subKey, string cmdName)
        {
            using (var shellKey = GetShellKey(subKey))
            {
                if (shellKey != null)
                {
                    shellKey.DeleteSubKeyTree(cmdName);
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}


// TODO: HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\Play
// TODO: HKEY_CLASSES_ROOT\*\shell\File Manager COOL