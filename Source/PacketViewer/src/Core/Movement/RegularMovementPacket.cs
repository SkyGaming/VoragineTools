using System;
using System.Collections.Generic;
using System.ComponentModel;

/*[Parser(OpCodes.MSG_MOVE_START_FORWARD)]
[Parser(OpCodes.MSG_MOVE_START_BACKWARD)]
[Parser(OpCodes.MSG_MOVE_STOP)]
[Parser(OpCodes.MSG_MOVE_START_STRAFE_LEFT)]
[Parser(OpCodes.MSG_MOVE_START_STRAFE_RIGHT)]
[Parser(OpCodes.MSG_MOVE_STOP_STRAFE)]
[Parser(OpCodes.MSG_MOVE_JUMP)]
[Parser(OpCodes.MSG_MOVE_START_TURN_LEFT)]
[Parser(OpCodes.MSG_MOVE_START_TURN_RIGHT)]
[Parser(OpCodes.MSG_MOVE_STOP_TURN)]
[Parser(OpCodes.MSG_MOVE_START_PITCH_UP)]
[Parser(OpCodes.MSG_MOVE_START_PITCH_DOWN)]
[Parser(OpCodes.MSG_MOVE_STOP_PITCH)]
[Parser(OpCodes.MSG_MOVE_SET_RUN_MODE)]
[Parser(OpCodes.MSG_MOVE_SET_WALK_MODE)]
[Parser(OpCodes.MSG_MOVE_FALL_LAND)]
[Parser(OpCodes.MSG_MOVE_START_SWIM)]
[Parser(OpCodes.MSG_MOVE_STOP_SWIM)]
[Parser(OpCodes.MSG_MOVE_SET_FACING)]
[Parser(OpCodes.MSG_MOVE_SET_PITCH)]
[Parser(OpCodes.MSG_MOVE_HEARTBEAT)]
[Parser(OpCodes.CMSG_MOVE_FALL_RESET)]
[Parser(OpCodes.CMSG_MOVE_SET_FLY)]
[Parser(OpCodes.MSG_MOVE_START_ASCEND)]
[Parser(OpCodes.MSG_MOVE_STOP_ASCEND)]
[Parser(OpCodes.CMSG_MOVE_CHNG_TRANSPORT)]
[Parser(OpCodes.MSG_MOVE_START_DESCEND)]*/

namespace WowTools.Core
{

    // Original code by LordJZ
    public abstract class RegularMovementPacket : Parser
    {
        /// <summary>
        /// Defines all transmitted elements of MovementStatus.
        /// </summary>
        protected enum MovementStatusElements
        {
            Flags,
            Flags2,
            Timestamp,
            HavePitch,
            #region Guid
            GuidByte0, // 6DB645
            GuidByte1, // 6DB5D7
            GuidByte2, // 6DB605
            GuidByte3, // 6DB8C3
            GuidByte4, // 6DB8E8
            GuidByte5, // 6DB90D
            GuidByte6, // 6DB581
            GuidByte7, // 6DB960
            #endregion
            HaveFallData,
            HaveFallDirection,
            HaveTransportData,
            TransportHaveTime2,
            TransportHaveTime3,
            #region TransportGuid
            TransportGuidByte0, // 6DB70C
            TransportGuidByte1, // 6DB6A8
            TransportGuidByte2, // 6DB7EC
            TransportGuidByte3, // 6DB7BA
            TransportGuidByte4, // 6DB6DA
            TransportGuidByte5, // 6DB89B
            TransportGuidByte6, // 6DB788
            TransportGuidByte7, // 6DB81E
            #endregion
            HaveSpline,
            HaveSplineElev,
            PositionX,
            PositionY,
            PositionZ,
            PositionO, // 6DB9B7
            #region Guid Seq
            GuidByte0_2, // 6DB9BF
            GuidByte1_2, // 6DBAC6
            GuidByte2_2, // 6DBD6F
            GuidByte3_2, // 6DBE0E
            GuidByte4_2, // 6DB9F8
            GuidByte5_2, // 6DBDF5
            GuidByte6_2, // 6DBDB0
            GuidByte7_2, // 6DBE27
            #endregion
            Pitch, // 6DB9F0
            FallTime, // 6DBA31
            #region TransportGuid Seq
            TransportGuidByte0_2, // 6DBD35
            TransportGuidByte1_2, // 6DBB63
            TransportGuidByte2_2, // 6DBD49
            TransportGuidByte3_2, // 6DBB31
            TransportGuidByte4_2, // 6DBC79
            TransportGuidByte5_2, // 6DBCFD
            TransportGuidByte6_2, // 6DBC17
            TransportGuidByte7_2, // 6DBAFF
            #endregion
            SplineElev, // 6DBDED
            FallHorizontalSpeed,
            FallVerticalSpeed,
            FallCosAngle,
            FallSinAngle,
            TransportSeat,
            TransportPositionO,
            TransportPositionX,
            TransportPositionY,
            TransportPositionZ,
            TransportTime,
            TransportTime2,
            TransportTime3,
            Speed0,
        }

        /// <summary>
        /// Gets the <see cref="System.Array"/> of
        /// <see cref="MovementStatusElements"/>
        /// in the sequence they are transmitted.
        /// </summary>
        protected abstract MovementStatusElements[] Elements { get; }

        void VerifySequence(MovementStatusElements[] sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException("Elements", "Elements must not be null.");

            var already = new List<MovementStatusElements>(sequence.Length);

            for (int i = 0; i < sequence.Length; i++)
            {
                var element = sequence[i];

                var idx = already.IndexOf(element);
                if (idx != -1)
                    throw new InvalidOperationException(
                        string.Format("Sequence has at least two entries of {0}: at {1} and {2}", element, idx, i)
                        );

                already.Add(element);
            }

            Console.WriteLine("Debug: sequence.Length = {0}", sequence.Length);
        }

        public override void Parse()
        {
            var sequence = this.Elements;
            VerifySequence(sequence);

            bool HaveTransportData = false,
                HaveTransportTime2 = false,
                HaveTransportTime3 = false,
                HavePitch = false,
                HaveFallData = false,
                HaveFallDirection = false,
                HaveSplineElevation = false,
                HaveSpline = false;

            var guid = new byte[8];
            var tguid = new byte[8];

            for (int i = 0; i < sequence.Length; ++i)
            {
                var element = sequence[i];

                if (element >= MovementStatusElements.GuidByte0 && element <= MovementStatusElements.GuidByte7)
                {
                    ReadByteMask(ref guid[element - MovementStatusElements.GuidByte0]);
                    continue;
                }

                if (element >= MovementStatusElements.TransportGuidByte0 &&
                    element <= MovementStatusElements.TransportGuidByte7)
                {
                    if (HaveTransportData)
                        ReadByteMask(ref tguid[element - MovementStatusElements.TransportGuidByte0]);
                    continue;
                }

                if (element >= MovementStatusElements.GuidByte0_2 && element <= MovementStatusElements.GuidByte7_2)
                {
                    ReadByteSeq(ref guid[element - MovementStatusElements.GuidByte0_2]);
                    continue;
                }

                if (element >= MovementStatusElements.TransportGuidByte0_2 &&
                    element <= MovementStatusElements.TransportGuidByte7_2)
                {
                    if (HaveTransportData)
                        ReadByteSeq(ref tguid[element - MovementStatusElements.TransportGuidByte0_2]);
                    continue;
                }

                switch (element)
                {
                    case MovementStatusElements.Flags:
                        ReadEnum<MovementFlags>("Movement Flags", 30);
                        break;
                    case MovementStatusElements.Flags2:
                        ReadEnum<MovementFlags2>("Extra Movement Flags", 12);
                        break;
                    case MovementStatusElements.Timestamp:
                        ReadUInt32(element.ToString());
                        break;
                    case MovementStatusElements.HavePitch:
                        HavePitch = ReadBit();
                        break;
                    case MovementStatusElements.HaveFallData:
                        HaveFallData = ReadBit();
                        break;
                    case MovementStatusElements.HaveFallDirection:
                        if (HaveFallData)
                            HaveFallDirection = ReadBit();
                        break;
                    case MovementStatusElements.HaveTransportData:
                        HaveTransportData = ReadBit();
                        break;
                    case MovementStatusElements.TransportHaveTime2:
                        if (HaveTransportData)
                            HaveTransportTime2 = ReadBit();
                        break;
                    case MovementStatusElements.TransportHaveTime3:
                        if (HaveTransportData)
                            HaveTransportTime3 = ReadBit();
                        break;
                    case MovementStatusElements.HaveSpline:
                        HaveSpline = ReadBit();
                        break;
                    case MovementStatusElements.HaveSplineElev:
                        HaveSplineElevation = ReadBit();
                        break;
                    case MovementStatusElements.PositionX:
                        ReadCoords3("Position");
                        break;
                    case MovementStatusElements.PositionY:
                    case MovementStatusElements.PositionZ:
                        break;  // assume they always go as vector of 3
                    case MovementStatusElements.PositionO:
                        ReadSingle(element.ToString());
                        break;
                    case MovementStatusElements.Pitch:
                        if (HavePitch)
                            ReadSingle(element.ToString());
                        break;
                    case MovementStatusElements.FallTime:
                        if (HaveFallData)
                            ReadUInt32(element.ToString());
                        break;
                    case MovementStatusElements.SplineElev:
                        if (HaveSplineElevation)
                            ReadSingle(element.ToString());
                        break;
                    case MovementStatusElements.FallHorizontalSpeed:
                        if (HaveFallDirection)
                            ReadSingle(element.ToString());
                        break;
                    case MovementStatusElements.FallVerticalSpeed:
                        if (HaveFallData)
                            ReadSingle(element.ToString());
                        break;
                    case MovementStatusElements.FallCosAngle:
                        if (HaveFallDirection)
                            ReadSingle(element.ToString());
                        break;
                    case MovementStatusElements.FallSinAngle:
                        if (HaveFallDirection)
                            ReadSingle(element.ToString());
                        break;
                    case MovementStatusElements.TransportSeat:
                        if (HaveTransportData)
                            ReadUInt8(element.ToString());
                        break;
                    case MovementStatusElements.TransportPositionO:
                        if (HaveTransportData)
                            ReadSingle("Transport Facing");
                        break;
                    case MovementStatusElements.TransportPositionX:
                        if (HaveTransportData)
                            ReadCoords3("Transport Position");
                        break;
                    case MovementStatusElements.TransportPositionY:
                    case MovementStatusElements.TransportPositionZ:
                        break;  // assume they always go as vector of 3
                    case MovementStatusElements.TransportTime:
                        if (HaveTransportData)
                            ReadUInt32(element.ToString());
                        break;
                    case MovementStatusElements.TransportTime2:
                        if (HaveTransportTime2)
                            ReadUInt32(element.ToString());
                        break;
                    case MovementStatusElements.TransportTime3:
                        if (HaveTransportTime3)
                            ReadUInt32(element.ToString());
                        break;
                    case MovementStatusElements.Speed0: // ?
                        ReadUInt32(element.ToString());
                        break;
                    default:
                        throw new InvalidOperationException("Unknown element: " + element);
                }
            }

            AppendFormatLine("GUID: {0:X16}", BitConverter.ToUInt64(guid, 0));
            UInt64 transportGUID = BitConverter.ToUInt64(tguid, 0);
            if(transportGUID != 0)
                AppendFormatLine("Transport GUID: {0:X16}", transportGUID);
        }

        protected void ReadByteMask(ref byte b)
        {
            b = ReadBit() ? (byte)1 : (byte)0;
        }

        protected void ReadByteSeq(ref byte b)
        {
            if (b != 0)
                b ^= Reader.ReadByte();
        }
    }
}