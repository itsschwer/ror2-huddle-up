using BepInEx.Configuration;

namespace HUDdleUP
{
    public sealed class Config
    {
        private readonly ConfigFile file;
        internal void Reload() { Plugin.Logger.LogDebug($"Reloading {file.ConfigFilePath.Substring(file.ConfigFilePath.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1)}"); file.Reload(); }


        // Generic
        private readonly ConfigEntry<bool> fullerItemDescriptions;
        private readonly ConfigEntry<bool> fullerEquipmentDescriptions;
        private readonly ConfigEntry<bool> commandMenuItemTooltips;
        private readonly ConfigEntry<bool> renameEquipmentDrones;
        private readonly ConfigEntry<bool> runDifficultyTooltip;
        private readonly ConfigEntry<bool> scoreboardShowChat;
        // Accessors
        public bool FullerItemDescriptions => fullerItemDescriptions.Value;
        public bool FullerEquipmentDescriptions => fullerEquipmentDescriptions.Value;
        public bool CommandMenuItemTooltips => commandMenuItemTooltips.Value;
        public bool RenameEquipmentDrones => renameEquipmentDrones.Value;
        public bool RunDifficultyTooltip => runDifficultyTooltip.Value;
        public bool ScoreboardShowChat => scoreboardShowChat.Value;

        public Config(ConfigFile config)
        {
            file = config;

            const string Generic = "";
            fullerItemDescriptions = config.Bind<bool>(Generic, nameof(fullerItemDescriptions), true,
                "Replace the default short (pickup) descriptions in item tooltips with a combination of the short and detailed descriptions.\n\nNote that this mod currently does not provide calculated item stack stats.");
            fullerEquipmentDescriptions = config.Bind<bool>(Generic, nameof(fullerEquipmentDescriptions), true,
                "Replace the default short (pickup) descriptions in equipment tooltips with a combination of the short and detailed descriptions, as well as the equipment cooldown.\n\nNote that this mod currently does not provide calculated item stack stats.");
            commandMenuItemTooltips = config.Bind<bool>(Generic, nameof(commandMenuItemTooltips), true,
                "Add tooltips to items and equipment in pickup picker menus (e.g. command cubes, void potentials) that show the (fuller) description of the item.");
            renameEquipmentDrones = config.Bind<bool>(Generic, nameof(renameEquipmentDrones), true,
                "Replace the names of equipment drones in ally cards with the name of its held equipment.");
            runDifficultyTooltip = config.Bind<bool>(Generic, nameof(runDifficultyTooltip), true,
                "Add a tooltip to the difficulty icon in the HUD that shows the description of the run's difficulty.");
            scoreboardShowChat = config.Bind<bool>(Generic, nameof(scoreboardShowChat), true,
                "Show the chat history when the scoreboard is open.");
        }
    }
}
