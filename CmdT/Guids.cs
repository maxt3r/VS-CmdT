// Guids.cs
// MUST match guids.h
using System;

namespace Jitbit.CmdT
{
    static class GuidList
    {
        public const string guidCmdTPkgString = "a945d6fc-c52d-4cb8-97a8-ae4a75f4012c";
        public const string guidCmdTCmdSetString = "49809e29-3ca3-43e5-8f92-6574f2cdc21d";
        public const string guidToolWindowPersistanceString = "86c9d038-84a5-45e3-b78b-8766a46c3dee";

        public static readonly Guid guidCmdTCmdSet = new Guid(guidCmdTCmdSetString);
    };
}