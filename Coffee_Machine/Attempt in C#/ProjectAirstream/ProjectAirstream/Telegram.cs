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
        public byte SOH, PIP, PIE, PN, SA, DA, MI;
        public ushort MP, DL;
        public byte[] data;
        public ushort CRC;
        public byte EOT;

        public Telegram(byte SOH_e, byte PIP_e,
                        byte PIE_e, byte PN_e,
                        byte SA_e, byte DA_e,
                        byte MI_e, ushort MP_e, ushort DL_e,
                        byte[] data_e, ushort CRC_e, byte EOT_e)
                        {
                            SOH = SOH_e;
                            PIP = PIE_e;
                            PIE = PIE_e;
                            PN = PN_e;
                            SA = SA_e;
                            DA = DA_e;
                            MI = MI_e;
                            MP = MP_e;
                            DL = DL_e;
                            data = data_e;
                            CRC = CRC_e;
                            EOT = EOT_e;
        }




    }
}
