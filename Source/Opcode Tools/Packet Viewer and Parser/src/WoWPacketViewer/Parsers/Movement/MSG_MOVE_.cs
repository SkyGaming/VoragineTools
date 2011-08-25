using WowTools.Core;

namespace WoWPacketViewer.Parsers.Movement
{
    [Parser((OpCodes))]
    class Movement : RegularMovementPacket
    {
            protected override MovementStatusElements[] Elements
            {
                get
                {
                    return new MovementStatusElements[] {

                };
                }
            }
    }
}
