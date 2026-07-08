using Autofac;
using Autofac.Extensions.DependencyInjection;
using BrickController2.BusinessLogic.DI;
using BrickController2.CreationManagement.DI;
using BrickController2.Database.DI;
using BrickController2.DeviceManagement.DI;
using BrickController2.Extensions;
using BrickController2.InputDeviceManagement.DI;
using BrickController2.Linux.PlatformServices.DI;
using BrickController2.UI.DI;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Platforms.Linux.Gtk4.Essentials.Hosting;
using Microsoft.Maui.Platforms.Linux.Gtk4.Hosting;
using ZXing.Net.Maui.Controls;

namespace BrickController2.Linux;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiAppLinuxGtk4<BrickController2.App>();

        builder
            .ConfigureSymbolFonts()
            .UseBarcodeReader()
            .ConfigureContainer(new AutofacServiceProviderFactory(), autofacBuilder =>
            {
                autofacBuilder.RegisterModule(new PlatformServicesModule());

                autofacBuilder.RegisterModule(new BusinessLogicModule());
                autofacBuilder.RegisterModule(new DatabaseModule());
                autofacBuilder.RegisterModule(new CreationManagementModule());
                autofacBuilder.RegisterModule(new DeviceManagementModule());
                autofacBuilder.RegisterModule(new InputDeviceManagementModule());
                autofacBuilder.RegisterModule(new UiModule());
            });

        builder.AddLinuxGtk4Essentials();

        return builder.Build();
    }
}
