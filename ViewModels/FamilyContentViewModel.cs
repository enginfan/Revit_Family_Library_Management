
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TripleKill.Commands;
using TripleKill.Interfaces;
using TripleKill.RevitFileUtility;
using TripleKill.RevitFileUtility.Utility;

namespace TripleKill.ViewModels
{
    public delegate void OperateFamily(string familyName);
    public class FamilyContentViewModel : SelectableObject, IModelCore<IContent>
    {
        private readonly Size _thumbnailSize = new Size(100.0, 100.0);

        public IContent Model { get; }

        public FamilyContentViewModel(IContent content)
        {
            Model = content;
            Name = Model.Name;
            if (string.IsNullOrEmpty(Model.ContentLocalPath))
                Status = "下载";
            else Status = "载入";
            DownloadCommand = new DelegateCommand(delegate
            {
                if (LoadFamily != null)
                {
                    if (Status == "载入" && _canLoad)
                    {
                        LoadFamily(Model.ContentLocalPath);
                        _canLoad = false;
                        _hasLoaded = true;
                    }
                }
            }, (x) => { if (Status == "载入") return _hasLoaded || _canLoad; return false; });
            PlaceCommand = new DelegateCommand(delegate { if (PlaceFamily != null) PlaceFamily(Name); }, (x) => true);

        }

        private static bool _canLoad = true;
        private static bool _hasLoaded = false;

        public OperateFamily LoadFamily;

        public OperateFamily PlaceFamily;


        private string displayName;
        public string DisplayName
        {
            get => displayName;
            set
            {
                displayName = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                Model.Name = value;
                RefreshDisplayName();
                OnPropertyChanged();
            }
        }

        private ImageSource thumbnail;
        public ImageSource Thumbnail
        {
            get
            {
                if (thumbnail == null) RefreshThumbnail();
                return thumbnail;
            }
            set
            {
                thumbnail = value;
                OnPropertyChanged();
            }
        }
        private void RefreshThumbnail()
        {
            var imageSource = GetImageSourceFromLocal(Model, _thumbnailSize);
            if (imageSource != null) Thumbnail = imageSource;
        }

        private ImageSource GetImageSourceFromLocal(IContent model, Size thumbnailSize)
        {
            var revitFile = new RevitFile(model.ContentLocalPath);
            var image = revitFile.Thumbnail;

            return image == null ? null : ImageUtils.ImageToImageSource(image);
        }

        private void RefreshDisplayName()
        {
            if (Name == null)
            {
                DisplayName = null;
                return;
            }

            var text = Name.ToLower();
            var length = text.Length;
            foreach (var text2 in Extensions)
                if (text.EndsWith(text2))
                {
                    DisplayName = Name.Substring(0, length - text2.Length);
                    return;
                }

            DisplayName = Name;
        }

        private string status;

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlaceCommand { get; }

        public ICommand DownloadCommand { get; }

        private static string[] Extensions { get; } = { ".rfa" };

    }
}
