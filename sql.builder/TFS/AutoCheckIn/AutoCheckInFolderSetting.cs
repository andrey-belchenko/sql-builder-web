using sql.builder.TFS.AutoCheckIn.Config;

namespace sql.builder.TFS.AutoCheckIn
{
    internal class AutoCheckInFolderSetting
    {
        private FolderElement config;
        private string name;
        private bool get_changes;
        private int rebuild_scheme;
        private bool queue_build;
        private bool checkin;
        internal AutoCheckInFolderSetting(FolderElement config)
        {
            this.config = config;
            this.name = config.Name.Replace("$/root/", string.Empty).Replace("/all/", string.Empty);
            if (config.GatedCheckIn) {
                this.name = this.name + " (gb)";
            }
        }
        public string Name { get { return this.name; } }
        public bool GetChanges { get { return this.get_changes; } set { this.get_changes = value; } }
        public int RebuildScheme { get { return this.rebuild_scheme; } set { this.rebuild_scheme = value; } }
        public bool CheckIn {
            get {
                return this.checkin;
            }
            set {
                this.checkin = value;
                if (this.checkin && this.config.GatedCheckIn) {
                    this.queue_build = true;
                }
            }
        }
        public bool QueueBuild {
            get {
                return this.queue_build;
            }
            set {
                // всегда true
                if (this.checkin && this.config.GatedCheckIn) {
                    return;
                }
                this.queue_build = value;
            }
        }
        public FolderElement Config { get { return this.config; } }
        internal void Clear()
        {
            this.get_changes = false;
            this.rebuild_scheme = 0;
            this.checkin = false;
            this.queue_build = false;
        }
    }
}