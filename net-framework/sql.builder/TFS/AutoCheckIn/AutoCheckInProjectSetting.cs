using sql.builder.TFS.AutoCheckIn.Config;

namespace sql.builder.TFS.AutoCheckIn
{
    internal class AutoCheckInProjectSetting
    {
        private ProjectElement config;
        private string name;
        private bool selected;
        public ProjectElement Config { get { return this.config; } }
        public string Name { get { return this.name; } }
        public bool Selected { get { return this.selected; } }
        internal AutoCheckInProjectSetting(ProjectElement config)
        {
            this.config = config;
            this.name = config.Name;
            this.selected = config.Selected;
        }
    }
}