﻿using System;
using System.Reflection;

namespace SpellWork
{
    public enum CompareType
    {
        NotEqual,
        Equal,
        AndStrict,
        And,
        NotAnd,

        StartsWith,
        EndsWith,
        Contains,
    }

    public static class LinqExtensions
    {
        /// <summary>
        /// Compares two values object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="T_entry"></param>
        /// <param name="field">Value Type is MemberInfo</param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool CreateFilter<T>(this T T_entry, object field, object val, CompareType compareType)
        {
            object basicValue = GetValue<T>(T_entry, (MemberInfo)field);
            
            switch (basicValue.GetType().Name)
            {
                case "UInt32":
                    return Compare(basicValue.ToUInt32(), val.ToUInt32(), compareType);
                case "Int32":
                    return Compare(basicValue.ToInt32(), val.ToInt32(), compareType);
                case "Single":
                    return Compare(basicValue.ToFloat(), val.ToFloat(), compareType);
                case "UInt64":
                    return Compare(basicValue.ToUlong(), val.ToUlong(), compareType);
                case "String":
                    return Compare(basicValue.ToString(), val.ToString(), compareType);
                case @"UInt32[]":
                {
                    uint val_uint = val.ToUInt32();
                    foreach (uint el in (uint[])basicValue)
                    {
                        if (Compare(el, val_uint, compareType))
                            return true;
                    }
                    return false;
                }
                case @"Int32[]":
                {
                    int val_int = val.ToInt32();
                    foreach (int el in (int[])basicValue)
                    {
                        if (Compare(el, val_int, compareType))
                            return true;
                    }
                    return false;
                }
                case @"Single[]":
                {
                    float val_float = val.ToFloat();
                    foreach (float el in (float[])basicValue)
                    {
                        if (Compare(el, val_float, compareType))
                            return true;
                    }
                    return false;
                }
                case @"UInt64[]":
                {
                    ulong val_ulong = val.ToUlong();
                    foreach (ulong el in (ulong[])basicValue)
                    {
                        if (Compare(el, val_ulong, compareType))
                            return true;
                    }
                    return false;
                }
                default:
                    return false;
            }
        }

        #region Specific Compares

        private static Boolean Compare(String baseValue, String value, CompareType compareType)
        {
            switch (compareType)
            {
                case CompareType.StartsWith:
                    return baseValue.StartsWith(value);
                case CompareType.EndsWith:
                    return baseValue.EndsWith(value);

                case CompareType.Contains:
                    return baseValue.ContainsText(value);

                case CompareType.NotEqual:
                    return !baseValue.Equals(value, StringComparison.CurrentCultureIgnoreCase);
                case CompareType.Equal:
                default:
                    return baseValue.Equals(value, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        private static Boolean Compare(float baseValue, float value, CompareType compareType)
        {
            switch (compareType)
            {
                case CompareType.NotEqual:
                    return baseValue != value;
                case CompareType.Equal:
                default:
                    return baseValue == value;
            }
        }

        private static Boolean Compare(UInt64 baseValue, UInt64 value, CompareType compareType)
        {
            switch (compareType)
            {
                case CompareType.AndStrict:
                    return (baseValue & value) == value;
                case CompareType.And:
                    return (baseValue & value) != 0;
                case CompareType.NotAnd:
                    return (baseValue & value) == 0;

                case CompareType.NotEqual:
                    return baseValue != value;
                case CompareType.Equal:
                default:
                    return baseValue == value;
            }
        }

        private static Boolean Compare(Int32 baseValue, Int32 value, CompareType compareType)
        {
            switch (compareType)
            {
                case CompareType.AndStrict:
                    return (baseValue & value) == value;
                case CompareType.And:
                    return (baseValue & value) != 0;
                case CompareType.NotAnd:
                    return (baseValue & value) == 0;

                case CompareType.NotEqual:
                    return baseValue != value;
                case CompareType.Equal:
                default:
                    return baseValue == value;
            }
        }

        private static Boolean Compare(UInt32 baseValue, UInt32 value, CompareType compareType)
        {
            switch (compareType)
            {
                case CompareType.AndStrict:
                    return (baseValue & value) == value;
                case CompareType.And:
                    return (baseValue & value) != 0;
                case CompareType.NotAnd:
                    return (baseValue & value) == 0;

                case CompareType.NotEqual:
                    return baseValue != value;
                case CompareType.Equal:
                default:
                    return baseValue == value;
            }
        }

        #endregion

        private static Object GetValue<T>(T T_entry, MemberInfo field)
        {
            if (field is FieldInfo)
                return typeof(T).GetField(field.Name).GetValue(T_entry);
            else if (field is PropertyInfo)
                return typeof(T).GetProperty(field.Name).GetValue(T_entry, null);
            else
                return null;
        }
    }
}
