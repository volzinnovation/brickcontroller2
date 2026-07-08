using BrickController2.PlatformServices.Permission;
using Microsoft.Maui.ApplicationModel;
using System.Threading.Tasks;

namespace BrickController2.Linux.PlatformServices.Permission;

internal class BluetoothPermission : IBluetoothPermission
{
    public Task<PermissionStatus> CheckStatusAsync() => Task.FromResult(PermissionStatus.Denied);

    public Task<PermissionStatus> RequestAsync() => Task.FromResult(PermissionStatus.Denied);
}
