using System;

namespace Domo.Modules
{
    [Flags]
    public enum ReadWriteMode
    {
        Read = 1,
        Write = 2,
        Both = Read | Write,
    }
}