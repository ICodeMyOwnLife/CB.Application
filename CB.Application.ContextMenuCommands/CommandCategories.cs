using System;


namespace CB.Application.ContextMenuCommands
{
    [Flags]
    public enum CommandCategories
    {
        AllDrives = 0x01,
        AllDirectories = 0x02,
        AllFiles = 0x04,
        Background = 0x08
    }
}