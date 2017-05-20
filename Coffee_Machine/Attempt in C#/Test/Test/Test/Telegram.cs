using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [Serializable]
    public struct Telegram
    {
        public byte SOH, PIP, PIE, PN, SA, DA, MI;
        public ushort MP, DL;
        public byte[] data;
        public ushort CRC;
        public byte EOT;

    }
}
