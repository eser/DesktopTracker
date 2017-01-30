using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DesktopTracker
{
    public class AppFolderManager
    {
        // fields

        public const string AppFolderName = "DesktopTracker";
        public const string ItemsFilename = "items.json";

        // constructors

        public AppFolderManager()
        {

        }

        // methods

        public string GetAppFile(string filename)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (filename == null)
            {
                return Path.Combine(appData, AppFolderManager.AppFolderName);
            }

            return Path.Combine(appData, AppFolderManager.AppFolderName, filename);
        }

        public string GetAppDirectory()
        {
            return this.GetAppFile(null);
        }

        public void EnsureAppDirectoryIsCreated()
        {
            var appDirectory = this.GetAppDirectory();

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }
        }

        public void SaveItems(IEnumerable<CounterItem> counterItems)
        {
            var serialized = JsonConvert.SerializeObject(counterItems, Formatting.Indented);

            this.EnsureAppDirectoryIsCreated();

            var path = this.GetAppFile(AppFolderManager.ItemsFilename);

            File.WriteAllText(path, serialized, Encoding.UTF8);
        }

        public IEnumerable<CounterItem> LoadItems()
        {
            var path = this.GetAppFile(AppFolderManager.ItemsFilename);

            if (!File.Exists(path)) {
                return new CounterItem[0];
            }

            var serialized = File.ReadAllText(path, Encoding.UTF8);

            return JsonConvert.DeserializeObject<IEnumerable<CounterItem>>(serialized);
        }
    }
}
