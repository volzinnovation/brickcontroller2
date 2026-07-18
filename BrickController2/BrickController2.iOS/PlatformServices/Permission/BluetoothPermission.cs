using BrickController2.PlatformServices.Permission;
using CoreBluetooth;
using CoreFoundation;
using Microsoft.Maui.ApplicationModel;
using System.Threading.Tasks;

namespace BrickController2.iOS.PlatformServices.Permission
{
    internal sealed class BluetoothPermission : CBCentralManagerDelegate, IBluetoothPermission
    {
        private readonly object _lock = new();
        private CBCentralManager? _permissionManager;
        private TaskCompletionSource<PermissionStatus>? _requestCompletionSource;

        public Task<PermissionStatus> CheckStatusAsync() => Task.FromResult(GetCurrentStatus());

        public Task<PermissionStatus> RequestAsync()
        {
            var currentStatus = GetCurrentStatus();
            if (currentStatus != PermissionStatus.Unknown)
            {
                return Task.FromResult(currentStatus);
            }

            lock (_lock)
            {
                if (_requestCompletionSource is not null)
                {
                    return _requestCompletionSource.Task;
                }

                _requestCompletionSource = new TaskCompletionSource<PermissionStatus>(TaskCreationOptions.RunContinuationsAsynchronously);
#pragma warning disable CA1422 // Validate platform compatibility
                _permissionManager = new CBCentralManager(this, DispatchQueue.MainQueue);
#pragma warning restore CA1422 // Validate platform compatibility
                return _requestCompletionSource.Task;
            }
        }

        public override void UpdatedState(CBCentralManager central)
        {
            var status = GetCurrentStatus();
            if (status != PermissionStatus.Unknown)
            {
                _requestCompletionSource?.TrySetResult(status);
            }
        }

        private static PermissionStatus GetCurrentStatus()
        {
            return CBManager.Authorization switch
            {
                CBManagerAuthorization.AllowedAlways => PermissionStatus.Granted,
                CBManagerAuthorization.Denied => PermissionStatus.Denied,
                CBManagerAuthorization.Restricted => PermissionStatus.Restricted,
                _ => PermissionStatus.Unknown
            };
        }
    }
}
