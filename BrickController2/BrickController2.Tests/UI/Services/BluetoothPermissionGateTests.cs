using BrickController2.PlatformServices.Permission;
using BrickController2.UI.Services.Dialog;
using BrickController2.UI.Services.Permission;
using BrickController2.UI.Services.Preferences;
using BrickController2.UI.Services.Translation;
using FluentAssertions;
using Microsoft.Maui.ApplicationModel;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BrickController2.Tests.UI.Services;

public class BluetoothPermissionGateTests
{
    [Fact]
    public async Task EnsureAccessAsync_FirstDeviceAction_RequestsAndRemembersPermission()
    {
        var decision = BluetoothPermissionDecision.NotRequested;
        var permission = new Mock<IBluetoothPermission>();
        var dialogs = new Mock<IDialogService>();
        var preferences = CreatePreferencesMock(() => decision, value => decision = value);
        var translations = CreateTranslationMock();

        permission.Setup(x => x.CheckStatusAsync()).ReturnsAsync(PermissionStatus.Unknown);
        permission.Setup(x => x.RequestAsync()).ReturnsAsync(PermissionStatus.Granted);
        dialogs.Setup(x => x.ShowQuestionDialogAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var gate = new BluetoothPermissionGate(permission.Object, dialogs.Object, preferences.Object, translations.Object);

        var result = await gate.EnsureAccessAsync(false, CancellationToken.None);

        result.Should().BeTrue();
        decision.Should().Be(BluetoothPermissionDecision.Requested);
        permission.Verify(x => x.RequestAsync(), Times.Once);
    }

    [Fact]
    public async Task EnsureAccessAsync_DeclinedDeviceAction_DoesNotAskAgain()
    {
        var decision = BluetoothPermissionDecision.NotRequested;
        var permission = new Mock<IBluetoothPermission>();
        var dialogs = new Mock<IDialogService>();
        var preferences = CreatePreferencesMock(() => decision, value => decision = value);
        var translations = CreateTranslationMock();

        permission.Setup(x => x.CheckStatusAsync()).ReturnsAsync(PermissionStatus.Unknown);
        dialogs.Setup(x => x.ShowQuestionDialogAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        dialogs.Setup(x => x.ShowMessageBoxAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var gate = new BluetoothPermissionGate(permission.Object, dialogs.Object, preferences.Object, translations.Object);

        (await gate.EnsureAccessAsync(false, CancellationToken.None)).Should().BeFalse();
        (await gate.EnsureAccessAsync(false, CancellationToken.None)).Should().BeFalse();

        decision.Should().Be(BluetoothPermissionDecision.Declined);
        dialogs.Verify(x => x.ShowQuestionDialogAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
        dialogs.Verify(x => x.ShowMessageBoxAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
        permission.Verify(x => x.RequestAsync(), Times.Never);
    }

    private static Mock<IPreferencesService> CreatePreferencesMock(
        System.Func<BluetoothPermissionDecision> getDecision,
        System.Action<BluetoothPermissionDecision> setDecision)
    {
        var preferences = new Mock<IPreferencesService>();
        preferences.Setup(x => x.Get(
                It.IsAny<string>(),
                BluetoothPermissionDecision.NotRequested,
                It.IsAny<string>()))
            .Returns(getDecision);
        preferences.Setup(x => x.Set(
                It.IsAny<string>(),
                It.IsAny<BluetoothPermissionDecision>(),
                It.IsAny<string>()))
            .Callback<string, BluetoothPermissionDecision, string?>((_, value, _) => setDecision(value));
        return preferences;
    }

    private static Mock<ITranslationService> CreateTranslationMock()
    {
        var translations = new Mock<ITranslationService>();
        translations.Setup(x => x.Translate(It.IsAny<string>())).Returns<string>(key => key);
        return translations;
    }
}
