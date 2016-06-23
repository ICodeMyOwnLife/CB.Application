using System.Collections.Generic;
using Microsoft.Win32;


namespace CB.Application.ContextMenuCommands
{
    public class CascadingContextMenuCommandItem: ContextMenuCommandItem
    {
        #region Fields
        protected const string SUB_COMMANDS = "subcommands";
        #endregion


        #region  Commands
        public IEnumerable<ContextMenuCommandItem> SubCommands { get; set; }
        #endregion


        #region Override
        public override void CreateIn(RegistryKey key)
        {
            base.CreateIn(key);
            UseMainKey(key, mainKey =>
            {
                mainKey.SetValue(SUB_COMMANDS, "");

                foreach (var command in SubCommands)
                {
                    command.CreateIn(mainKey);
                }
            });
        }
        #endregion
    }
}