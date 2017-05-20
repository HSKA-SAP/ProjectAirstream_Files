using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace ProjectAirstream
{
    [Serializable]
    public struct Telegram
    {
        //need a struct because data is in different types
        public byte SOH, PIP, PIE, PN, SA, DA, MI;
        public ushort MP, DL;
        public byte[] data;
        public ushort CRC;
        public byte EOT;




    }
}
