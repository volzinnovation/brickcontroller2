using BrickController2.PlatformServices.Permission;
using BrickController2.UI.Services.Dialog;
using BrickController2.UI.Services.Preferences;
using BrickController2.UI.Services.Translation;
using Microsoft.Maui.ApplicationModel;
using System.Threading;
using System.Threading.Tasks;

namespace BrickController2.UI.Services.Permission;

public enum BluetoothPermissionDecision
{
    NotRequested,
    Requested,
    Declined
}

public interface IBluetoothPermissionGate
{
    BluetoothPermissionDecision Decision { get; }

    Task<PermissionStatus> CheckStatusAsync();

    Task<bool> EnsureAccessAsync(bool initiatedFromSettings, CancellationToken token);
}

public sealed class BluetoothPermissionGate : IBluetoothPermissionGate
{
    private const string PreferenceKey = "BluetoothPermissionDecision";
    private const string PreferenceSection = "com.scn.brickcontroller2.Permissions";

    private readonly IBluetoothPermission _bluetoothPermission;
    private readonly IDialogService _dialogService;
    private readonly IPreferencesService _preferencesService;
    private readonly ITranslationService _translationService;

    public BluetoothPermissionGate(
        IBluetoothPermission bluetoothPermission,
        IDialogService dialogService,
        IPreferencesService preferencesService,
        ITranslationService translationService)
    {
        _bluetoothPermission = bluetoothPermission;
        _dialogService = dialogService;
        _preferencesService = preferencesService;
        _translationService = translationService;
    }

    public BluetoothPermissionDecision Decision => _preferencesService.Get(
        PreferenceKey,
        BluetoothPermissionDecision.NotRequested,
        PreferenceSection);

    public Task<PermissionStatus> CheckStatusAsync() => _bluetoothPermission.CheckStatusAsync();

    public async Task<bool> EnsureAccessAsync(bool initiatedFromSettings, CancellationToken token)
    {
        var decision = Decision;
        var status = await CheckStatusAsync();

        if (decision == BluetoothPermissionDecision.Requested)
        {
            if (status == PermissionStatus.Granted)
            {
                return true;
            }

            if (status == PermissionStatus.Unknown)
            {
                status = await _bluetoothPermission.RequestAsync();
                return status == PermissionStatus.Granted;
            }

            if (initiatedFromSettings)
            {
                var openSettings = await _dialogService.ShowQuestionDialogAsync(
                    Translate("BluetoothPermissionTitle"),
                    Translate("BluetoothPermissionDenied"),
                    Translate("OpenSettings"),
                    Translate("Cancel"),
                    token);

                if (openSettings)
                {
                    AppInfo.ShowSettingsUI();
                }
            }
            else
            {
                await _dialogService.ShowMessageBoxAsync(
                    Translate("BluetoothPermissionTitle"),
                    Translate("BluetoothPermissionDenied"),
                    Translate("Ok"),
                    token);
            }

            return false;
        }

        if (decision == BluetoothPermissionDecision.Declined && !initiatedFromSettings)
        {
            await _dialogService.ShowMessageBoxAsync(
                Translate("BluetoothPermissionTitle"),
                Translate("BluetoothPermissionDeclined"),
                Translate("Ok"),
                token);
            return false;
        }

        var shouldRequestBluetooth = await _dialogService.ShowQuestionDialogAsync(
            Translate("BluetoothPermissionTitle"),
            Translate("BluetoothPermissionRequired"),
            Translate("Allow"),
            Translate("NotNow"),
            token);

        if (!shouldRequestBluetooth)
        {
            SaveDecision(BluetoothPermissionDecision.Declined);
            return false;
        }

        SaveDecision(BluetoothPermissionDecision.Requested);
        status = await _bluetoothPermission.RequestAsync();
        return status == PermissionStatus.Granted;
    }

    private string Translate(string key) => _translationService.Translate(key);

    private void SaveDecision(BluetoothPermissionDecision decision)
        => _preferencesService.Set(PreferenceKey, decision, PreferenceSection);
}
