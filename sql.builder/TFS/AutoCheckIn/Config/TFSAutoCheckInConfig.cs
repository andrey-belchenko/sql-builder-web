using System.Configuration;

namespace sql.builder.TFS.AutoCheckIn.Config
{
    /// <summary>
    /// Объектная модель, описывающая настройки, которые хранятся в секции tfsAutoCheckInConfig  файла app.config
    /// </summary>
    internal class TFSAutoCheckInConfig: ConfigurationSection
    {
        [ConfigurationProperty("folders")]
        public FoldersCollection Folders
        {
            get { return ((FoldersCollection)(base["folders"])); }
        }


        [ConfigurationProperty("projects")]
        public ProjectsCollection Projects
        {
            get { return ((ProjectsCollection)(base["projects"])); }
        }
    }
}