using System.Collections.Generic;
using System.Linq;
using System;
using HidSharp;
using System.Diagnostics;
using SimpleLed;

namespace Rival3Support
{
    public class Rival3Controller
    {
        HidDevice device = null;
        HidStream stream = null;

        //Steelseries Rival 3 (Updated Firmware) IDs
        public int vendorID = 0x1038;
        public int productID = 0x184C;

        public Rival3Controller()
        {

        }

        //Function to send a given packet to the device
        public byte[] send_packet(byte[] packet)
        {
            this.stream.Write(packet);
            return packet;
        }

        //Functions that processes the SimpleLED data into a packet
        public void packets(byte led, byte r, byte g, byte b)
        {
            var terp = new OpenConfiguration();
            terp.SetOption(OpenOption.Exclusive, true);
            var list = DeviceList.Local;
            var devices = list.GetHidDevices(vendorID, productID);

            byte[] pkt = new byte[16];
            //Prefix packets for individual RGB control
            //pkt[0] = 0x00;
            //pkt[1] = 0x0a;
            //pkt[2] = 0x00;
            //pkt[3] = 0x0f;

            //Convert R, G, B values to Rival 3 protocol format
            //for (int i = 1; i <= 4; i++)
            //{
            //    pkt[3*i+1] = r;
            //    pkt[3*i+2] = g;
            //    pkt[3*i+3] = b;
            //}

            //Header packets for direct control with LED selection
            pkt[1] = 0x05;
            pkt[2] = 0x00;
            //LED number packet
            pkt[3] = led;
            //RGB packets
            pkt[4] = r;
            pkt[5] = g;
            pkt[6] = b;
            //Brightness packet (100%)
            pkt[7] = 0x64;

            //Pick correct interface and send packet
            foreach (Device device in devices)
            {
                //Interface 3, please let me know if there is a better way to do this
                if (device.DevicePath.IndexOf("mi_03") != -1)
                {
                    stream = (HidStream)device.Open();
                    stream.Write(pkt);
                }
            }
        }

    }
   
}
