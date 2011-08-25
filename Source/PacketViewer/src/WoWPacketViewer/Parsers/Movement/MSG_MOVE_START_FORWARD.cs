﻿using WowTools.Core;

namespace WoWPacketViewer.Parsers.Movement
{
    [Parser(OpCodes.MSG_MOVE_START_FORWARD)]
    class MovementStartForwardParser : RegularMovementPacket
    {
            protected override MovementStatusElements[] Elements
            {
                get
                {
                    return new MovementStatusElements[] {
MovementStatusElements.GuidByte0,
MovementStatusElements.HaveSpline,
MovementStatusElements.GuidByte5,
MovementStatusElements.GuidByte3,
MovementStatusElements.GuidByte4,
MovementStatusElements.GuidByte2,
MovementStatusElements.GuidByte7,
MovementStatusElements.Flags2,
MovementStatusElements.Flags,
MovementStatusElements.GuidByte6,
MovementStatusElements.GuidByte1,
MovementStatusElements.HavePitch,
MovementStatusElements.HaveFallData,
MovementStatusElements.HaveFallDirection,
MovementStatusElements.HaveTransportData,
MovementStatusElements.TransportGuidByte6,
MovementStatusElements.TransportGuidByte3,
MovementStatusElements.TransportGuidByte7,
MovementStatusElements.TransportGuidByte4,
MovementStatusElements.TransportGuidByte1,
MovementStatusElements.TransportGuidByte0,
MovementStatusElements.TransportGuidByte2,
MovementStatusElements.TransportGuidByte5,
MovementStatusElements.TransportHaveTime3,
MovementStatusElements.TransportHaveTime2,
MovementStatusElements.HaveSplineElev,
MovementStatusElements.Timestamp,
MovementStatusElements.PositionX,
MovementStatusElements.PositionY,
MovementStatusElements.PositionZ,
MovementStatusElements.PositionO,
MovementStatusElements.GuidByte7_2,
MovementStatusElements.Pitch,
MovementStatusElements.GuidByte1_2,
MovementStatusElements.GuidByte2_2,
MovementStatusElements.FallVerticalSpeed,
MovementStatusElements.FallTime,
MovementStatusElements.FallHorizontalSpeed,
MovementStatusElements.FallCosAngle,
MovementStatusElements.FallSinAngle,
MovementStatusElements.GuidByte3_2,
MovementStatusElements.GuidByte5_2,
MovementStatusElements.GuidByte0_2,
MovementStatusElements.GuidByte6_2,
MovementStatusElements.TransportTime,
MovementStatusElements.TransportPositionX,
MovementStatusElements.TransportPositionY,
MovementStatusElements.TransportPositionZ,
MovementStatusElements.TransportPositionO,
MovementStatusElements.TransportSeat,
MovementStatusElements.TransportGuidByte3_2,
MovementStatusElements.TransportGuidByte1_2,
MovementStatusElements.TransportTime3,
MovementStatusElements.TransportGuidByte6_2,
MovementStatusElements.TransportGuidByte0_2,
MovementStatusElements.TransportGuidByte5_2,
MovementStatusElements.TransportTime2,
MovementStatusElements.TransportGuidByte7_2,
MovementStatusElements.TransportGuidByte4_2,
MovementStatusElements.TransportGuidByte2_2,
MovementStatusElements.GuidByte4_2,
MovementStatusElements.SplineElev,
                };
                }
            }
    }
}
