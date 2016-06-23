using System;
using System.IO;
using CB.Application.ContextMenuCommands;
using Microsoft.Win32;


namespace TestContextMenuCommands
{
    internal class Program
    {
        #region Fields
        private const string COMMAND = "notepad.exe \"%1\"";
        private const string PROC_ID = ".test";
        private static readonly string _icon = Path.GetFullPath("1.ico");
        private static readonly RegistryKey _key = Registry.ClassesRoot.OpenOrCreateSubKey(PROC_ID);
        #endregion


        #region Implementation
        private static void Main()
        {
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        Register();
                        return;
                    case "2":
                        Unregister();
                        return;
                }
            }
        }

        private static void Register()
        {
            var command = new CascadingContextMenuCommandItem
            {
                Icon = _icon,
                Name = "Test Cascading Context Menu Commands",
                SubCommands = new ContextMenuCommandItemBase[]
                {
                    new ContextMenuCommandItem
                    {
                        Icon = _icon,
                        Name = "Command 1",
                        Command = COMMAND
                    },
                    new ContextMenuCommandItem
                    {
                        Icon = _icon,
                        Name = "Command 2",
                        Command = COMMAND
                    },
                    new CascadingContextMenuCommandItem
                    {
                        Icon = _icon,
                        Name = "Nested command",
                        SubCommands = new ContextMenuCommandItemBase[]
                        {
                            new ContextMenuCommandItem
                            {
                                Icon = _icon,
                                Name = "Command 1",
                                Command = COMMAND
                            },
                            new ContextMenuCommandItem
                            {
                                Icon = _icon,
                                Name = "Command 2",
                                Command = COMMAND
                            }
                        }
                    }
                }
            };
            command.CreateIn(_key);
        }

        private static void Unregister()
        {
            Registry.ClassesRoot.DeleteSubKeyTree(PROC_ID);
        }
        #endregion
    }
}