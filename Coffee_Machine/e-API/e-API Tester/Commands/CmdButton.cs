using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ApiSerialComm;

namespace eApi.Commands
{
    abstract public class CmdButton : System.Windows.Forms.Button
    {
        public class PacketEventArgs : EventArgs
        {
            /// <summary>
            /// Packet p will be shown by default.
            /// </summary>
            public PacketEventArgs(SerialComm.Packet_t p)
            {
                packet = p;
                showPacket = true;
            }

            /// <summary>
            /// Packet p will be shown if show is set to true.
            /// </summary>
            public PacketEventArgs(SerialComm.Packet_t p, bool show)
            {
                packet = p;
                showPacket = show;
            }
            public SerialComm.Packet_t packet;
            public bool showPacket;
        }

        public delegate void PacketEventHandler(object sender, PacketEventArgs e);
        /// <summary>
        /// Every packet which should be sent is given to this event.
        /// It will call the PacketToSendEventHandler method in MainForm.Commands.cs
        /// </summary>
        private event PacketEventHandler PacketToSendEvent;

        /// <summary>
        /// Adds a Packet to the PacketToSendEvent.
        /// </summary>
        /// <param name="sender">original sender of the packet (mostly "this")</param>
        public void addPacketEvent(object sender, PacketEventArgs e)
        {
            PacketToSendEvent(sender, e);
        }

        /// <summary>
        /// Adds a handler to the PacketToSendEvent.
        /// </summary>
        /// <param name="handler">Handler for PacketToSendEvent</param>
        public void registerPacketEventHandler(PacketEventHandler handler)
        {
            this.PacketToSendEvent += handler;
        }

        /// <summary>
        /// Tool-tip of the button. Describe the function of the button as short as possible.
        /// </summary>
        public string toolTip = "command";
        /// <summary>
        /// For future use. (implementation of filters and a better search)
        /// </summary>
        public List<string> TagList = new List<string>();
        /// <summary>
        /// The default packet of this command. Use it as a template for other packets.
        /// </summary>
        public SerialComm.Packet_t packet;
        /// <summary>
        /// True = when a response is received the responseReceived method will be called. 
        /// </summary>
        public bool processResponse = false;
        /// <summary>
        /// implement this in the responseReceived method. If false, no form should be opened.
        /// If true, you can show the processed data in a new form (not necessary).
        /// </summary>
        public bool showResponseForm = false;

        /// <summary>
        /// Show an editor to change the packet data. If true, an editor for the data of this command should be opened.
        /// If false, the default packet should be sent. (implement in OnButtonClick method)
        /// </summary>
        public bool showCommandForm = true;

        /// <summary>
        /// This method is called when the button is pressed. At the end, the method should call
        /// the addPacketEvent method. It should check the variable showCommandForm.
        /// </summary>
        abstract public void OnButtonClick(object sender, EventArgs e);

        /// <summary>
        /// This method is called when a response is received. It will only be called when processResponse is true.
        /// It should check the variable showResponseForm.
        /// </summary>
        /// <param name="p"></param>
        abstract public void responseReceived(SerialComm.Packet_t p);
    }
}
