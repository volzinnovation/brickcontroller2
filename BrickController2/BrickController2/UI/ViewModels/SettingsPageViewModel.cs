using BrickController2.InputDeviceManagement.HttpControl;
using BrickController2.InputDeviceManagement.Sensors;
using BrickController2.PlatformServices.InputDeviceService;
using BrickController2.UI.Commands;
using BrickController2.UI.Services.Dialog;
using BrickController2.UI.Services.Localization;
using BrickController2.UI.Services.Navigation;
using BrickController2.UI.Services.Permission;
using BrickController2.UI.Services.Theme;
using BrickController2.UI.Services.Translation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BrickController2.UI.ViewModels
{
    public class SettingsPageViewModel : PageViewModelBase
    {
        private const int ProgressDialogDelayMs = 500;

        private readonly IThemeService _themeService;
        private readonly ILocalizationService _localizationService;
        private readonly IInputDeviceService<OrientationSensorController> _orientationSensorService;
        private readonly IHttpControlService _httpControlService;
        private readonly IBluetoothPermissionGate _bluetoothPermissionGate;
        private readonly CreationListPageViewModel _parentViewModel;
        private readonly IDialogService _dialogService;
        private string _bluetoothPermissionStatus = string.Empty;

        public SettingsPageViewModel(
            INavigationService navigationService,
            ITranslationService translationService,
            IDialogService dialogService,
            IThemeService themeService,
            ILocalizationService localizationService,
            IInputDeviceService<OrientationSensorController> orientationSensorService,
            IHttpControlService httpControlService,
            IBluetoothPermissionGate bluetoothPermissionGate,
            NavigationParameters parameters) : 
            base(navigationService, translationService)
        {
            _themeService = themeService;
            _dialogService = dialogService;
            _localizationService = localizationService;
            _orientationSensorService = orientationSensorService;
            _httpControlService = httpControlService;
            _bluetoothPermissionGate = bluetoothPermissionGate;
            _parentViewModel = parameters.Get<CreationListPageViewModel>("parent");
            SelectThemeCommand = new SafeCommand(SelectThemeAsync);
            SelectLanguageCommand = new SafeCommand(SelectAppLanguageAsync);
            SelectHttpControlPortCommand = new SafeCommand(SelectHttpControlPortAsync);
            SelectHttpControlListenModeCommand = new SafeCommand(SelectHttpControlListenModeAsync);
            RegenerateHttpControlTokenCommand = new SafeCommand(RegenerateHttpControlToken);
            CopyHttpControlUrlCommand = new SafeCommand(CopyHttpControlUrlAsync);
            CopyHttpControlTokenCommand = new SafeCommand(CopyHttpControlTokenAsync);
            ConfigureBluetoothCommand = new SafeCommand(ConfigureBluetoothAsync);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            _httpControlService.StatusChanged += HttpControlServiceStatusChanged;
            RaiseHttpControlPropertiesChanged();
            await RefreshBluetoothPermissionStatusAsync();
        }

        public override void OnDisappearing()
        {
            _httpControlService.StatusChanged -= HttpControlServiceStatusChanged;
            base.OnDisappearing();
        }

        public ThemeType CurrentTheme
        {
            get => _themeService.CurrentTheme;
            set
            {
                if (CurrentTheme != value)
                {
                    _themeService.CurrentTheme = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Language CurrentLanguage
        {
            get => _localizationService.CurrentLanguage;
            set
            {
                if (_localizationService.CurrentLanguage != value)
                {
                    _localizationService.CurrentLanguage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand SelectThemeCommand { get; }
        public ICommand SelectLanguageCommand { get; }
        public ICommand SelectHttpControlPortCommand { get; }
        public ICommand SelectHttpControlListenModeCommand { get; }
        public ICommand RegenerateHttpControlTokenCommand { get; }
        public ICommand CopyHttpControlUrlCommand { get; }
        public ICommand CopyHttpControlTokenCommand { get; }
        public ICommand ConfigureBluetoothCommand { get; }

        public string BluetoothPermissionStatus
        {
            get => _bluetoothPermissionStatus;
            private set
            {
                if (_bluetoothPermissionStatus != value)
                {
                    _bluetoothPermissionStatus = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsOrientationSensorSupported => _orientationSensorService.IsSupported;

        public bool IsOrientationSensorEnabled
        {
            get => _orientationSensorService.IsEnabled;
            set
            {
                if (IsOrientationSensorEnabled != value)
                {
                    _orientationSensorService.IsEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsHttpControlSupported => _httpControlService.IsSupported;

        public bool IsHttpControlEnabled
        {
            get => _httpControlService.Options.Enabled;
            set
            {
                if (IsHttpControlEnabled != value)
                {
                    _httpControlService.ApplyOptions(_httpControlService.Options with { Enabled = value });
                    RaiseHttpControlPropertiesChanged();
                }
            }
        }

        public string HttpControlPort => _httpControlService.Options.Port.ToString();

        public string HttpControlListenMode => _httpControlService.Options.ListenMode.ToString();

        public string HttpControlStatus
        {
            get
            {
                var status = _httpControlService.RuntimeStatus.ToString();
                return string.IsNullOrWhiteSpace(_httpControlService.ErrorMessage)
                    ? status
                    : $"{status}: {_httpControlService.ErrorMessage}";
            }
        }

        public string HttpControlUrls
            => _httpControlService.ReachableUrls.Count == 0
                ? Translate("HttpControlNotRunning")
                : string.Join(Environment.NewLine, _httpControlService.ReachableUrls);

        public string HttpControlAccessToken
            => string.IsNullOrWhiteSpace(_httpControlService.Options.AccessToken)
                ? Translate("HttpControlNoToken")
                : _httpControlService.Options.AccessToken;

        private async Task SelectThemeAsync()
        {
            var result = await _dialogService.ShowSelectionDialogAsync(
                Enum.GetNames<ThemeType>(),
                Translate("Theme"),
                Translate("Cancel"),
                DisappearingToken);

            if (result.IsOk)
            {
                CurrentTheme = Enum.Parse<ThemeType>(result.SelectedItem);
            }
        }

        private async Task SelectAppLanguageAsync()
        {
            var result = await _dialogService.ShowSelectionDialogAsync(
                Enum.GetNames<Language>(),
                Translate("Language"),
                Translate("Cancel"),
                DisappearingToken);

            if (result.IsOk && Enum.TryParse<Language>(result.SelectedItem, out var currentLanguage))
            {
                // apply the change
                CurrentLanguage = currentLanguage;

                // use some notification via progress dialog
                await _dialogService.ShowProgressDialogAsync(
                    false,
                    (progressDialog, token) =>
                    {
                        // recreate the root page to apply the change
                        if (Application.Current is App myApp)
                        {
                            myApp.ReloadRootPage();
                        }
                        // some delay to show the progress dialog
                        return Task.Delay(ProgressDialogDelayMs, token);
                    },
                    Translate("Applying"),
                    token: DisappearingToken);

                // back to the previous page
                await NavigationService.NavigateBackAsync();
                // workaround for settings cmd available
                _parentViewModel.OpenSettingsPageCommand.RaiseCanExecuteChanged();
            }
        }

        private async Task SelectHttpControlPortAsync()
        {
            var result = await _dialogService.ShowInputDialogAsync(
                HttpControlPort,
                Translate("Port"),
                Translate("Ok"),
                Translate("Cancel"),
                KeyboardType.Numeric,
                IsValidPort,
                DisappearingToken);

            if (result.IsOk && int.TryParse(result.Result, out var port))
            {
                _httpControlService.ApplyOptions(_httpControlService.Options with { Port = port });
                RaiseHttpControlPropertiesChanged();
            }
        }

        private async Task SelectHttpControlListenModeAsync()
        {
            var result = await _dialogService.ShowSelectionDialogAsync(
                Enum.GetNames<HttpControlListenMode>(),
                Translate("HttpControlListenMode"),
                Translate("Cancel"),
                DisappearingToken);

            if (result.IsOk && Enum.TryParse<HttpControlListenMode>(result.SelectedItem, out var listenMode))
            {
                _httpControlService.ApplyOptions(_httpControlService.Options with { ListenMode = listenMode });
                RaiseHttpControlPropertiesChanged();
            }
        }

        private void RegenerateHttpControlToken()
        {
            _httpControlService.RegenerateAccessToken();
            RaiseHttpControlPropertiesChanged();
        }

        private Task CopyHttpControlUrlAsync()
        {
            var url = _httpControlService.ReachableUrls.FirstOrDefault();
            return string.IsNullOrWhiteSpace(url) ? Task.CompletedTask : Clipboard.Default.SetTextAsync(url);
        }

        private Task CopyHttpControlTokenAsync()
        {
            var token = _httpControlService.Options.AccessToken;
            return string.IsNullOrWhiteSpace(token) ? Task.CompletedTask : Clipboard.Default.SetTextAsync(token);
        }

        private async Task ConfigureBluetoothAsync()
        {
            await _bluetoothPermissionGate.EnsureAccessAsync(true, DisappearingToken);
            await RefreshBluetoothPermissionStatusAsync();
        }

        private async Task RefreshBluetoothPermissionStatusAsync()
        {
            if (_bluetoothPermissionGate.Decision == BluetoothPermissionDecision.NotRequested)
            {
                BluetoothPermissionStatus = Translate("BluetoothNotRequested");
                return;
            }

            if (_bluetoothPermissionGate.Decision == BluetoothPermissionDecision.Declined)
            {
                BluetoothPermissionStatus = Translate("BluetoothNotAllowed");
                return;
            }

            var status = await _bluetoothPermissionGate.CheckStatusAsync();
            BluetoothPermissionStatus = status switch
            {
                Microsoft.Maui.ApplicationModel.PermissionStatus.Granted => Translate("BluetoothAllowed"),
                Microsoft.Maui.ApplicationModel.PermissionStatus.Denied => Translate("BluetoothDenied"),
                Microsoft.Maui.ApplicationModel.PermissionStatus.Restricted => Translate("BluetoothRestricted"),
                _ => Translate("BluetoothNotRequested")
            };
        }

        private void HttpControlServiceStatusChanged(object? sender, EventArgs e)
        {
            RaiseHttpControlPropertiesChanged();
        }

        private void RaiseHttpControlPropertiesChanged()
        {
            RaisePropertyChanged(nameof(IsHttpControlEnabled));
            RaisePropertyChanged(nameof(HttpControlPort));
            RaisePropertyChanged(nameof(HttpControlListenMode));
            RaisePropertyChanged(nameof(HttpControlStatus));
            RaisePropertyChanged(nameof(HttpControlUrls));
            RaisePropertyChanged(nameof(HttpControlAccessToken));
        }

        private static bool IsValidPort(string value)
            => int.TryParse(value, out var port) && port is >= 1 and <= 65535;
    }
}
