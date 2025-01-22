using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace WPFGallery.Models
{
    public class ControlInfoDataItem
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconGlyph { get; set; }
        public string ImagePath { get; set; }
        public string PageName { get; set; }
        public bool IsGroup { get; set; } = false;

        public Type PageType
        {
            get
            {
                return _assembly.GetType($"WPFGallery.Views.{PageName}");
            }
        }

        public Uri ImageSource 
        {
            get
            {
                return new Uri($"pack://application:,,,/{ImagePath}");
            }
        }

        public ObservableCollection<ControlInfoDataItem> Items { get; set; } = new ObservableCollection<ControlInfoDataItem>();

        public override string ToString()
        {
            return Title;
        }

        private static Assembly _assembly = typeof(ControlsInfoDataSource).Assembly;
    }

    public sealed class ControlsInfoDataSource
    {
        private static readonly object _lock = new();

        #region Singleton

        private static readonly ControlsInfoDataSource _instance;

        public static ControlsInfoDataSource Instance
        {
            get
            {
                return _instance;
            }
        }

        static ControlsInfoDataSource()
        {
            _instance = new ControlsInfoDataSource();
        }

        private ControlsInfoDataSource()
        {
            var jsonText = ReadControlsData();
            ControlsInfo = JsonSerializer.Deserialize<List<ControlInfoDataItem>>(jsonText);
        }

        #endregion

        public ICollection<ControlInfoDataItem> ControlsInfo { get; }

        private string ReadControlsData()
        {
            var assembly = typeof(ControlsInfoDataSource).Assembly;
            var resourceName = "WPFGallery.Models.ControlsInfoData.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new System.IO.StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public ICollection<ControlInfoDataItem> GetControlsInfo(string groupName)
        {
            return ControlsInfo.Where(x => x.UniqueId == groupName).FirstOrDefault()?.Items;
        }

        public ICollection<ControlInfoDataItem> GetAllControlsInfo()
        {
            ICollection<ControlInfoDataItem> allControls = new ObservableCollection<ControlInfoDataItem>();
            foreach (ControlInfoDataItem ci in ControlsInfo)
            {
                if(ci.UniqueId != "Samples")
                {
                    var items = ci.Items;
                    foreach (ControlInfoDataItem item in items)
                    {
                        allControls.Add(item);
                    }
                }
            }

            return allControls;
        }

        public ICollection<ControlInfoDataItem> GetGroupedControlsInfo()
        {
            return ControlsInfo.Where(x => x.IsGroup == true && x.UniqueId != "Design Guidance" && x.UniqueId != "Samples").ToList();
        }
    }
}
