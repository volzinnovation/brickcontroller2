using BrickController2.DeviceManagement.CaDA;
using BrickController2.Protocols;

namespace BrickController2.Linux.PlatformServices.DeviceManagement.CaDA;

public class CaDAPlatformService : ICaDAPlatformService
{
    private const int HeaderOffset = 15;
    private const int PayloadOffset = 3;
    private const int PayloadLength = 24 + PayloadOffset;

    public bool TryGetRfPayload(byte[] rawData, out byte[] rfPayload)
    {
        rfPayload = new byte[PayloadLength];
        CryptTools.GetRfPayload(CaDAProtocol.SeedArray, CaDAProtocol.HeaderArray, rawData, HeaderOffset, CaDAProtocol.CTXValue1, CaDAProtocol.CTXValue2, rfPayload, PayloadOffset);

        return true;
    }
}
