﻿using WowTools.Core;

namespace WoWPacketViewer.Parsers.Movement
{
    [Parser(OpCodes.SMSG_PLAYER_MOVE)]
    class MovementPlayerParser : RegularMovementPacket
    {
        protected override MovementStatusElements[] Elements
        {
            get
            {
                return new MovementStatusElements[] {
                    MovementStatusElements.HavePitch,
                    MovementStatusElements.GuidByte6,
                    MovementStatusElements.HaveFallData,
                    MovementStatusElements.HaveFallDirection,
                    MovementStatusElements.GuidByte1,
                    MovementStatusElements.GuidByte2,
                    MovementStatusElements.Flags2,
                    MovementStatusElements.GuidByte0,
                    MovementStatusElements.HaveTransportData,
                    MovementStatusElements.TransportGuidByte1,
                    MovementStatusElements.TransportGuidByte4,
                    MovementStatusElements.TransportGuidByte0,
                    MovementStatusElements.TransportHaveTime2,
                    MovementStatusElements.TransportGuidByte6,
                    MovementStatusElements.TransportGuidByte3,
                    MovementStatusElements.TransportGuidByte2,
                    MovementStatusElements.TransportGuidByte7,
                    MovementStatusElements.TransportHaveTime3,
                    MovementStatusElements.TransportGuidByte5,
                    MovementStatusElements.GuidByte3,
                    MovementStatusElements.GuidByte4,
                    MovementStatusElements.GuidByte5,
                    MovementStatusElements.HaveSpline,
                    MovementStatusElements.GuidByte7,
                    MovementStatusElements.HaveSplineElev,
                    MovementStatusElements.Flags,
                    MovementStatusElements.PositionO,
                    MovementStatusElements.GuidByte0_2,
                    MovementStatusElements.Pitch,
                    MovementStatusElements.GuidByte4_2,
                    MovementStatusElements.FallTime,
                    MovementStatusElements.FallHorizontalSpeed,
                    MovementStatusElements.FallCosAngle,
                    MovementStatusElements.FallSinAngle,
                    MovementStatusElements.FallVerticalSpeed,
                    MovementStatusElements.Timestamp,
                    MovementStatusElements.GuidByte1_2,
                    MovementStatusElements.TransportGuidByte7_2,
                    MovementStatusElements.TransportGuidByte3_2,
                    MovementStatusElements.TransportGuidByte1_2,
                    MovementStatusElements.TransportSeat,
                    MovementStatusElements.TransportTime2,
                    MovementStatusElements.TransportGuidByte0_2,
                    MovementStatusElements.TransportGuidByte6_2,
                    MovementStatusElements.TransportPositionX,
                    MovementStatusElements.TransportPositionY,
                    MovementStatusElements.TransportPositionZ,
                    MovementStatusElements.TransportGuidByte4_2,
                    MovementStatusElements.TransportPositionO,
                    MovementStatusElements.TransportTime3,
                    MovementStatusElements.TransportGuidByte5_2,
                    MovementStatusElements.TransportTime,
                    MovementStatusElements.TransportGuidByte2_2,
                    MovementStatusElements.GuidByte2_2,
                    MovementStatusElements.PositionX,
                    MovementStatusElements.PositionY,
                    MovementStatusElements.PositionZ,
                    MovementStatusElements.GuidByte6_2,
                    MovementStatusElements.SplineElev,
                    MovementStatusElements.GuidByte5_2,
                    MovementStatusElements.GuidByte3_2,
                    MovementStatusElements.GuidByte7_2,
                };
            }
        }
    }
}
