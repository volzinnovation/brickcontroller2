using BrickController2.Helpers;
using BrickController2.PlatformServices.SharedFileStorage;
using System;
using System.IO;

namespace BrickController2.Linux.PlatformServices.SharedFileStorage;

public class SharedFileStorageService : NotifyPropertyChangedSource, ISharedFileStorageService
{
    private bool _isPermissionGranted = true;

    public bool IsSharedStorageAvailable => IsPermissionGranted && SharedStorageDirectory != null;

    public bool IsPermissionGranted
    {
        get => _isPermissionGranted;
        set
        {
            _isPermissionGranted = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(IsSharedStorageAvailable));
        }
    }

    public string? SharedStorageDirectory => SharedStorageBaseDirectory;

    public string? SharedStorageBaseDirectory
    {
        get
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!string.IsNullOrWhiteSpace(documents))
            {
                return documents;
            }

            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return string.IsNullOrWhiteSpace(home)
                ? null
                : Path.Combine(home, ".local", "share", "BrickController2");
        }
    }
}
