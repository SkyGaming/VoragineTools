﻿using WowTools.Core;

namespace WoWPacketViewer.Parsers.Movement
{
    [Parser(OpCodes.MSG_MOVE_START_STRAFE_LEFT)]
    class MovementStartStrafeLeftParser : RegularMovementPacket
    {
            protected override MovementStatusElements[] Elements
            {
                get
                {
                    return new MovementStatusElements[] {
MovementStatusElements.GuidByte5,
MovementStatusElements.GuidByte0,
MovementStatusElements.GuidByte3,
MovementStatusElements.Flags,
MovementStatusElements.GuidByte6,
MovementStatusElements.GuidByte1,
MovementStatusElements.GuidByte4,
MovementStatusElements.Flags2,
MovementStatusElements.HaveSpline,
MovementStatusElements.GuidByte7,
MovementStatusElements.GuidByte2,
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
MovementStatusElements.HaveFallData,
MovementStatusElements.HaveFallDirection,
MovementStatusElements.HavePitch,
MovementStatusElements.PositionO,
MovementStatusElements.Timestamp,
MovementStatusElements.PositionX,
MovementStatusElements.PositionY,
MovementStatusElements.PositionZ,
MovementStatusElements.GuidByte3_2,
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
MovementStatusElements.GuidByte5_2,
MovementStatusElements.GuidByte1_2,
MovementStatusElements.GuidByte4_2,
MovementStatusElements.GuidByte2_2,
MovementStatusElements.GuidByte0_2,
MovementStatusElements.SplineElev,
MovementStatusElements.GuidByte6_2,
MovementStatusElements.FallVerticalSpeed,
MovementStatusElements.FallTime,
MovementStatusElements.FallHorizontalSpeed,
MovementStatusElements.FallCosAngle,
MovementStatusElements.FallSinAngle,
MovementStatusElements.GuidByte7_2,
MovementStatusElements.Pitch,
                };
                }
            }
    }
}