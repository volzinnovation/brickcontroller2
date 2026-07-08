using BrickController2.PlatformServices.Permission;
using Microsoft.Maui.ApplicationModel;
using System.Threading.Tasks;

namespace BrickController2.Linux.PlatformServices.Permission;

public class ReadWriteExternalStoragePermission : IReadWriteExternalStoragePermission
{
    public Task<PermissionStatus> CheckStatusAsync() => Task.FromResult(PermissionStatus.Granted);

    public Task<PermissionStatus> RequestAsync() => Task.FromResult(PermissionStatus.Granted);
}
