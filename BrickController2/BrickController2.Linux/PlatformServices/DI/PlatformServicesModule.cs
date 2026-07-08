using Autofac;
using BrickController2.DeviceManagement.CaDA;
using BrickController2.DeviceManagement.JieStar;
using BrickController2.DeviceManagement.MouldKing;
using BrickController2.Linux.PlatformServices.BluetoothLE;
using BrickController2.Linux.PlatformServices.DeviceManagement.CaDA;
using BrickController2.Linux.PlatformServices.DeviceManagement.JieStar;
using BrickController2.Linux.PlatformServices.DeviceManagement.MouldKing;
using BrickController2.Linux.PlatformServices.GameController;
using BrickController2.Linux.PlatformServices.Infrared;
using BrickController2.Linux.PlatformServices.Localization;
using BrickController2.Linux.PlatformServices.Permission;
using BrickController2.Linux.PlatformServices.SharedFileStorage;
using BrickController2.PlatformServices.BluetoothLE;
using BrickController2.PlatformServices.InputDeviceService;
using BrickController2.PlatformServices.Infrared;
using BrickController2.PlatformServices.Localization;
using BrickController2.PlatformServices.Permission;
using BrickController2.PlatformServices.SharedFileStorage;

namespace BrickController2.Linux.PlatformServices.DI;

public class PlatformServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<InfraredService>().As<IInfraredService>().SingleInstance();
        builder.RegisterType<GameControllerService>().As<IInputDeviceService>().As<IStartable>().SingleInstance();
        builder.RegisterType<BluetoothLEService>().As<IBluetoothLEService>().SingleInstance();
        builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();
        builder.RegisterType<SharedFileStorageService>().As<ISharedFileStorageService>().SingleInstance();
        builder.RegisterType<BluetoothPermission>().As<IBluetoothPermission>().InstancePerDependency();
        builder.RegisterType<CameraPermission>().As<ICameraPermission>().InstancePerDependency();
        builder.RegisterType<ReadWriteExternalStoragePermission>().As<IReadWriteExternalStoragePermission>().InstancePerDependency();
        builder.RegisterType<MKPlatformService>().As<IMKPlatformService>().SingleInstance();
        builder.RegisterType<CaDAPlatformService>().As<ICaDAPlatformService>().SingleInstance();
        builder.RegisterType<JieStarPlatformService>().As<IJieStarPlatformService>().SingleInstance();
    }
}
