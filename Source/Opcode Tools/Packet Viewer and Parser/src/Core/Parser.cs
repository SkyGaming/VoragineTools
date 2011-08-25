using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WowTools.Core
{
    public abstract class Parser
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        public void Append(string format, params object[] args)
        {
            stringBuilder.AppendFormat(format, args);
        }

        public void AppendLine()
        {
            stringBuilder.AppendLine();
        }

        public void AppendLine(string format, params object[] args)
        {
            stringBuilder.AppendFormat(format, args).AppendLine();
        }

        public void WriteLine(string format, params object[] args) { AppendLine(format, args); }

        public void AppendFormat(string format, params object[] args)
        {
            stringBuilder.AppendFormat(format, args);
        }

        public void AppendFormatLine(string format, params object[] args)
        {
            stringBuilder.AppendFormat(format, args).AppendLine();
        }

        public void CheckPacket()
        {
            if (Reader.BaseStream.Position != Reader.BaseStream.Length)
            {
                string msg = String.Format("{0}: Packet size changed, should be {1} instead of {2}", Packet.Code, Reader.BaseStream.Position, Reader.BaseStream.Length);
                MessageBox.Show(msg);
            }
        }

        protected Packet Packet { get; private set; }

        protected BinaryReader Reader { get; private set; }

        protected Parser packet;

        private byte _currentByte;
        // position of the next bit in _currentByte to be read
        private sbyte _bitPos = -1;

        protected Parser()
        {
            packet = this;  // an alias for parser compatibility with SilinoronParser
        }

        public virtual void Initialize(Packet packet)
        {
            Packet = packet;

            if (packet != null)
            {
                Reader = Packet.CreateReader();
            }
        }

        public virtual void Parse() { }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        public ulong ReadPackedGuid(string format = null, params object[] args)
        {
            var ret = Reader.ReadPackedGuid();
            if (format != null)
            {
                if (args.Length == 0 && !format.Contains("{0"))
                    format += ": {0}";
                AppendFormatLine(format, MergeArguments(args, ret));
            }
            return ret;
        }

        public T Read<T>(string format, params object[] args) where T : struct
        {
            var ret = Reader.ReadStruct<T>();
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        private object[] MergeArguments(object[] args, object arg)
        {
            var newArgs = new object[args.Length + 1];
            Array.Copy(args, newArgs, args.Length);
            newArgs[args.Length] = arg;
            return newArgs;
        }

        public void For(int count, Action func)
        {
            for (var i = 0; i < count; ++i)
                func();
        }

        public void For(int count, Action<int> func)
        {
            for (var i = 0; i < count; ++i)
                func(i);
        }

        private KeyValuePair<long, T> ReadEnum<T>(TypeCode code, byte bitsCount = (byte)0)
        {
            var type = typeof(T);
            object value = null;
            long rawVal = 0;

            if (code == TypeCode.Empty)
                code = Type.GetTypeCode(type.GetEnumUnderlyingType());

            switch (code)
            {
                case TypeCode.SByte:
                    rawVal = Reader.ReadSByte();
                    break;
                case TypeCode.Byte:
                    rawVal = Reader.ReadByte();
                    break;
                case TypeCode.Int16:
                    rawVal = Reader.ReadInt16();
                    break;
                case TypeCode.UInt16:
                    rawVal = Reader.ReadUInt16();
                    break;
                case TypeCode.Int32:
                    rawVal = Reader.ReadInt32();
                    break;
                case TypeCode.UInt32:
                    rawVal = Reader.ReadUInt32();
                    break;
                case TypeCode.Int64:
                    rawVal = Reader.ReadInt64();
                    break;
                case TypeCode.UInt64:
                    rawVal = (long)Reader.ReadUInt64();
                    break;
                case TypeCode.DBNull:
                    rawVal = ReadBits(bitsCount);
                    break;
            }
            value = System.Enum.ToObject(type, rawVal);

            return new KeyValuePair<long, T>(rawVal, (T)value);
        }

        public T ReadEnum<T>(string name = null, TypeCode code = TypeCode.Empty)
        {
            KeyValuePair<long, T> val = ReadEnum<T>(code);
            if(name != null)
                AppendFormatLine("{0}: {1} ({2})", name, val.Value, val.Key);
            return val.Value;
        }

        public T ReadEnum<T>(string name, byte bitsCount)
        {
            KeyValuePair<long, T> val = ReadEnum<T>(TypeCode.DBNull, bitsCount);
            AppendFormatLine("{0}: {1} ({2})", name, val.Value, val.Key);
            return val.Value;
        }

        public Coords3 ReadCoords3(string name = null)
        {
            Coords3 val;
            val.X = Reader.ReadSingle();
            val.Y = Reader.ReadSingle();
            val.Z = Reader.ReadSingle();
            if (name != null)
                AppendFormatLine("{0}: {1}", name, val);
            return val;
        }

        /* TODO: port GUID stuff
        public Guid ReadGuid()
        {
            return new Guid(Reader.ReadUInt64());
        }*/
        public bool Bit() { return ReadBit(); }
        public bool ReadBit()
        {
            if (_bitPos < 0)
            {
                _currentByte = Reader.ReadByte();
                _bitPos = 7;
            }
            return ((_currentByte >> _bitPos--) & 1) != 0;
        }

        /// <summary>
        /// Reads an integer stored in bitsCount bits inside the bit stream.
        /// </summary>
        public uint ReadBits(byte bitsCount)
        {
            uint value = 0;
            for (int i = bitsCount - 1; i >= 0; i--)
            {
                if (ReadBit())
                    value |= (uint)1 << i;
            }
            return value;
        }
        public uint Bits(byte bitsCount) { return ReadBits(bitsCount); }

        public bool IsRead()
        {
            return Reader.BaseStream.Position == Reader.BaseStream.Length;
        }

        private T Print<T>(T value, string format = null, params object[] args)
        {
            if (format != null)
            {
                if (args.Length == 0 && !format.Contains("{0"))
                    format += ": {0}";
                AppendFormatLine(format, MergeArguments(args, value));
            }
            return value;
        }

        public Byte[] ReadBytes(int count) { return Reader.ReadBytes(count); }
        public DateTime ReadPackedTime(string fmt = null, params object[] args) { return Print(Reader.ReadInt32().AsGameTime(), fmt, args); }
        public DateTime ReadTime(string fmt = null, params object[] args) { return Print(Reader.ReadUInt32().AsUnixTime(), fmt, args); }
        public string ReadString(string fmt = null, params object[] args) { return Print(Reader.ReadCString(), fmt, args); }
        public string ReadCString(string fmt = null, params object[] args) { return Print(Reader.ReadCString(), fmt, args); }
        public float ReadSingle(string fmt = null, params object[] args) { return Print(Reader.ReadSingle(), fmt, args); }
        public float ReadFloat(string fmt = null, params object[] args) { return Print(Reader.ReadSingle(), fmt, args); }
        public long ReadInt64(string fmt = null, params object[] args) { return Print(Reader.ReadInt64(), fmt, args); }
        public int ReadInt32(string fmt = null, params object[] args) { return Print(Reader.ReadInt32(), fmt, args); }
        public short ReadInt16(string fmt = null, params object[] args) { return Print(Reader.ReadInt16(), fmt, args); }
        public sbyte ReadInt8(string fmt = null, params object[] args) { return Print(Reader.ReadSByte(), fmt, args); }
        public ulong ReadUInt64(string fmt = null, params object[] args) { return Print(Reader.ReadUInt64(), fmt, args); }
        public uint ReadUInt32(string fmt = null, params object[] args) { return Print(Reader.ReadUInt32(), fmt, args); }
        public ushort ReadUInt16(string fmt = null, params object[] args) { return Print(Reader.ReadUInt16(), fmt, args); }
        public byte ReadUInt8(string fmt = null, params object[] args) { return Print(Reader.ReadByte(), fmt, args); }
        public byte ReadByte(string fmt = null, params object[] args) { return Print(Reader.ReadByte(), fmt, args); }

        public Byte[] Bytes(int count) { return Reader.ReadBytes(count); }
        public DateTime PackedTime(string fmt = null, params object[] args) { return ReadPackedTime(fmt, args); }
        public DateTime Time(string fmt = null, params object[] args) { return ReadTime(fmt, args); }
        public string CString(string fmt = null, params object[] args) { return ReadCString(fmt, args); }
        public float Float(string fmt = null, params object[] args) { return ReadFloat(fmt, args); }
        public long Int64(string fmt = null, params object[] args) { return ReadInt64(fmt, args); }
        public int Int32(string fmt = null, params object[] args) { return ReadInt32(fmt, args); }
        public short Int16(string fmt = null, params object[] args) { return ReadInt16(fmt, args); }
        public sbyte Int8(string fmt = null, params object[] args) { return ReadInt8(fmt, args); }
        public ulong UInt64(string fmt = null, params object[] args) { return ReadUInt64(fmt, args); }
        public uint UInt32(string fmt = null, params object[] args) { return ReadUInt32(fmt, args); }
        public ushort UInt16(string fmt = null, params object[] args) { return ReadUInt16(fmt, args); }
        public byte UInt8(string fmt = null, params object[] args) { return ReadByte(fmt, args); }
        public byte Byte(string fmt = null, params object[] args) { return ReadByte(fmt, args); }
    }
}
