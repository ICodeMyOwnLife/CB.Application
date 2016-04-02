using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;


namespace CB.Application.ContextMenuCommands
{
    public class ContextMenuCommand
    {
        #region Fields
        private const string ALL_DIRECTORIES = "Directory";
        private const string ALL_DRIVES = "Drive";
        private const string ALL_FILES = "*";
        private const string BACKGROUND = @"Directory\Background";
        private const string CMD = "Command";
        private const string REG_PATH = @"\{0}\shell";
        #endregion


        #region Methods
        public static bool AddCommand(CommandCategories categories, string cmdName, string appPath,
            bool passFilePathToApp, params string[] additionalArgs)
        {
            var cmdArgs = CreateCmdArgArray(appPath, passFilePathToApp, additionalArgs);
            return AddCommand(categories, cmdName, cmdArgs);
        }

        public static bool AddCommand(string fileType, string cmdName, string appPath, bool passFilePathToApp,
            params string[] additionalArgs)
        {
            var cmdArgs = CreateCmdArgArray(appPath, passFilePathToApp, additionalArgs);
            return AddCommand(fileType, cmdName, cmdArgs);
        }

        public static bool AddCommand(CommandCategories categories, string cmdName, params string[] cmdArgs)
        {
            var result = true;

            if (categories.HasFlag(CommandCategories.AllDirectories))
            {
                result &= AddCommandHelper(ALL_DIRECTORIES, cmdName, cmdArgs);
            }

            if (categories.HasFlag(CommandCategories.AllDrives))
            {
                result &= AddCommandHelper(ALL_DRIVES, cmdName, cmdArgs);
            }

            if (categories.HasFlag(CommandCategories.AllFiles))
            {
                result &= AddCommandHelper(ALL_FILES, cmdName, cmdArgs);
            }

            if (categories.HasFlag(CommandCategories.Background))
            {
                result &= AddCommandHelper(BACKGROUND, cmdName, cmdArgs);
            }
            return result;
        }

        public static bool AddCommand(string fileType, string cmdName, params string[] cmdArgs)
        {
            return AddCommandHelper(GetSubKeyFromFileType(fileType), cmdName, cmdArgs);
        }

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
        private static bool AddCommandHelper(string subKey, string cmdName, string[] cmdArgs)
        {
            using (var shellKey = GetShellKey(subKey))
            {
                if (shellKey != null)
                {
                    using (var nameKey = shellKey.OpenSubKey(cmdName, true) ?? shellKey.CreateSubKey(cmdName))
                    {
                        if (nameKey != null)
                        {
                            using (var cmdKey = nameKey.OpenSubKey(CMD, true) ?? nameKey.CreateSubKey(CMD))
                            {
                                if (cmdKey != null)
                                {
                                    cmdKey.SetValue("", string.Join(" ", cmdArgs.Select(s => '"' + s + '"')));
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static string[] CreateCmdArgArray(string appPath, bool passFilePathToApp, string[] additionalArgs)
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