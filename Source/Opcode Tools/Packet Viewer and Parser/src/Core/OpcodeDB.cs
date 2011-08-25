using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace WowTools.Core
{
    public static class OpcodeDB
    {
        /// <summary>
        /// If set to false the DB is not used and OpCodes enum values are assumed to equal real opcode values
        /// </summary>
        public static bool Enabled = true;
        //static Dictionary<uint, OpCodes> NumberToEnum = new Dictionary<uint, OpCodes>();
        public static OpCodes[] NumberToEnum = new OpCodes[0xFFFF];
        public static Dictionary<OpCodes, uint> EnumToNumber = new Dictionary<OpCodes, uint>();

        public static uint BuildLoaded { private set; get; }
        private static string ConnectionString;

        public static void Load(uint build, String connectionString)
        {
            Enabled = (uint) OpCodes.SMSG_UPDATE_OBJECT > 0xFFFF;   // if DB is used all the enum values have to be > 0xFFFF
            if(build == 0)
            {
                build = 14333;  // default to latest
            }
            ConnectionString = connectionString;
            if (!Enabled || build == BuildLoaded)
            {
                return;
            }

            using (var conn = new MySqlConnection(ConnectionString))
            {
                var query = String.Format("select name, number from emuopcodes where version = {0};", build);
                var command = new MySqlCommand(query, conn);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        uint number = reader.GetUInt16(1);
                        OpCodes enumVal;
                        if (!Enum.TryParse(name, out enumVal))
                        {
                            Console.WriteLine("No OpCodes enum value for DB entry {0}", name);
                            continue;
                        }
                        NumberToEnum[number] = enumVal;
                        EnumToNumber[enumVal] = number;
                    }
                }
            }

            BuildLoaded = build;
        }

        public static void Reload()
        {
            var curBuild = BuildLoaded;
            BuildLoaded = 0;
            
            Array.Clear(NumberToEnum, 0, NumberToEnum.Length);
            EnumToNumber.Clear();
            Load(curBuild, ConnectionString);
        }

        static public OpCodes GetOpcode(uint number)
        {
            if(!Enabled)
            {
                return (OpCodes)number; // cast it to an appropriate OpCodes member
            }
            if(number > 0xFFFF || NumberToEnum[number] == 0)//!NumberToEnum.ContainsKey(number))
            {
                return (OpCodes)number; // it will be left as a number to display
            }
            return NumberToEnum[number];    // DB mapping found
        }

        static public uint GetOpcode(OpCodes enumValue)
        {
            if(!Enabled)
            {
                return (uint) enumValue;
            }
            if (!EnumToNumber.ContainsKey(enumValue))
            {
                return 0;
            }
            return EnumToNumber[enumValue];
        }

        public static string UpdateOpcode(string name, uint number)
        {
            OpCodes enumVal;
            if(!OpCodes.TryParse(name, true, out enumVal))
                return "";

            if (EnumToNumber[enumVal] == number)
                return "";  // value not changed

            NumberToEnum[number] = enumVal;
            EnumToNumber[enumVal] = number;
            string query = String.Format("UPDATE emuopcodes SET number = {0} WHERE name = \"{1}\" and version = @ver;",
                                         number, name);
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var command = new MySqlCommand(query.Replace("@ver", BuildLoaded.ToString()), conn);
                command.ExecuteNonQuery();
            }
            return query;
        }
    }
}
