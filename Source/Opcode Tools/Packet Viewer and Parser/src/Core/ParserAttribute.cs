using System;

namespace WowTools.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ParserAttribute : Attribute
    {
        public ParserAttribute(OpCodes code)
        {
            Code = code;
        }

        public OpCodes Code { get; private set; }
    }
}
