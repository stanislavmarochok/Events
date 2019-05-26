using System;
using System.Collections.Generic;
using System.Text;

namespace GoBuhat.Common
{
    public interface IMessage
    {
        void Longtime(string message);
        void Shorttime(string message);
    }
}
