using BrickController2.PlatformServices.Infrared;
using System.Threading.Tasks;

namespace BrickController2.Linux.PlatformServices.Infrared;

public class InfraredService : IInfraredService
{
    public bool IsInfraredSupported => false;

    public bool IsCarrierFrequencySupported(int carrierFrequency) => false;

    public Task SendPacketAsync(int carrierFrequency, int[] packet) => Task.CompletedTask;
}
