using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ApiSerialComm;
using eApi.Commands;
using eApi.Typedef;

namespace eApi
{
    public partial class MainForm : Form
    {
        private void telegramReceivedCallback(SerialComm.Packet_t packet)
        {
            try
            {
                this.BeginInvoke(ApiDataReceived, packet);
            }
            catch { }
        }

        private void telegramSentCallback(SerialComm.Packet_t packet)
        {
        }

        private void ackTimeoutCallback(SerialComm.Packet_t packet)
        {
            try
            {
                errorOccurredEvent(this, new PacketEventArgs(packet));
                // Das andere Gerät antwortet nicht.
            }
            catch { }
        }

        private void nackFailCallback(SerialComm.Packet_t packet)
        {
            try
            {
                errorOccurredEvent(this, new PacketEventArgs(packet));
                // Das andere Gerät erhält inkorrekte Daten
            }
            catch { }
        }

        private void responseTimeoutCallback(SerialComm.Packet_t packet)
        {
            try
            {
                this.BeginInvoke(updateUI, packet, CommStatus.ResponseTimeout);
                errorOccurredEvent(this, new PacketEventArgs(packet));
                // Das andere Gerät schickt keine Antwort
                // Dies kann auch bedeuten das es Verbindungsprobleme gibt aber
                // das Ack wurde empfangen d.h. das warscheinlich ein Problem
                // beim Main Thread des anderen Gerätes vorliegt.
            }
            catch { }
        }

        private void ackNackReceivedHandler(object sender, SerialComm.PacketEventArgs e)
        {
            //try // temp fix
            //{
                if (e.isTimeout == false)
                {
                    switch (e.packet.type)
                    {
                        case SerialComm.PacketType_t.NegAck_e:
                            this.BeginInvoke(updateUI, e.packet, CommStatus.NackReceived);
                            break;
                        case SerialComm.PacketType_t.PosAck_e:
                            this.BeginInvoke(updateUI, e.packet, CommStatus.AckReceived);
                            break;
                        default: break;
                    }
                }
                else
                {
                    this.BeginInvoke(updateUI, e.packet, CommStatus.AckTimeout);
                }
            //}
            //catch { }
        }

        private void API_ReceiveData(SerialComm.Packet_t packet)
        {
            //try // temp fix
            //{
                if (packet.type == SerialComm.PacketType_t.Data_e)
                {
                    // Antwort
                    if (packet.sequenceNumber == serialSession.LastSentPacket.sequenceNumber &&
                        serialSession.LastSentPacket.type == SerialComm.PacketType_t.Request_e)
                    {
                        if (packet.isFail == true)
                        {
                            // Erhaltene Antwort hat einen Fehler
                            serialSession.EnqueuePacket(serialSession.LastSentPacket);
                        }
                        else
                        {
                            showResponse(packet);
                        }
                    }

                    // Kommando
                    switch (packet.message.command)
                    {
                        default:
                            break;
                    }
                }
                else
                {
                    // Request
                    switch (packet.message.command)
                    {
                        default:
                            serialSession.EnqueuePacket(packet.source, (byte)API_Command_t.Undef_e, packet.sequenceNumber);
                            break;
                    }
                }
            //}
            //catch { }
        }
    }
}