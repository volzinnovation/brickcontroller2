using Autofac;
using BrickController2.InputDeviceManagement;
using BrickController2.PlatformServices.InputDeviceService;
using Microsoft.Extensions.Logging;

namespace BrickController2.Linux.PlatformServices.GameController;

internal class GameControllerService : IInputDeviceService, IStartable
{
    private readonly ILogger<GameControllerService> _logger;

    public GameControllerService(IInputDeviceManagerService inputDeviceManagerService, ILogger<GameControllerService> logger)
    {
        _logger = logger;
        inputDeviceManagerService.RegisterInputDeviceService(this);
    }

    public void Initialize()
    {
        _logger.LogWarning("Game controller input is not implemented for the Linux GTK4 desktop head.");
    }

    public void Stop()
    {
    }

    public void Start()
    {
    }
}
