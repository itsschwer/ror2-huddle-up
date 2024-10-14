using BepInEx.Configuration;

namespace HUDdleUP
{
    public sealed class Config
    {
        private readonly ConfigFile file;
        internal void Reload() { Plugin.Logger.LogDebug($"Reloading {file.ConfigFilePath.Substring(file.ConfigFilePath.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1)}"); file.Reload(); }


        private readonly ConfigEntry<bool> scoreboardShowChat;
        public bool ScoreboardShowChat => scoreboardShowChat.Value;

        public Config(ConfigFile config)
        {
            file = config;

            scoreboardShowChat = config.Bind<bool>("", nameof(scoreboardShowChat), true,
                "Show the chat history when the scoreboard is open.");
        }
    }
}
