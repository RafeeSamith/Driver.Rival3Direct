using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLed;
using Rival3Support;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Driver.Rival3Direct
{
    public class Rival3DirectDriver : ISimpleLed
    {
        Rival3Controller r3controller;

        public event EventHandler DeviceRescanRequired;
        public event Events.DeviceChangeEventHandler DeviceAdded;
        public event Events.DeviceChangeEventHandler DeviceRemoved;

        public static Assembly assembly = Assembly.GetExecutingAssembly();
        public static Stream imageStream = assembly.GetManifestResourceStream("Driver.Rival3Direct.rival.png");


        public void Configure(DriverDetails driverDetails)
        {
            var drivers = GetDevices();
            foreach (ControlDevice controlDevice in drivers)
            {
                DeviceAdded?.Invoke(this, new Events.DeviceChangeEventArgs(controlDevice));
            }
        }

        public static List<USBDevice> Rival3USB = new List<USBDevice>
        {
            new USBDevice{VID = 0x1038, HID = 0x184C, DeviceName = "Rival 3", DevicePrettyName = "Rival 3", DeviceType = DeviceTypes.Mouse, ManufacturerName = "Steelseries"},
            new USBDevice{VID = 0x1038, HID = 0x1824, DeviceName = "Rival 3", DevicePrettyName = "Rival 3", DeviceType = DeviceTypes.Mouse, ManufacturerName = "Steelseries"}
        };

        ControlDevice.LedUnit[] leds = new ControlDevice.LedUnit[4];
        public List<ControlDevice> GetDevices()
        {
            List<ControlDevice> devices = new List<ControlDevice>();

            var connectedMice = SLSManager.GetSupportedDevices(Rival3USB);

            ControlDevice mouse = new ControlDevice
            {
                //Name = "Steelseries Rival 3",
                DeviceType = SimpleLed.DeviceTypes.Mouse,
                Driver = this,
                LEDs = leds,
                ProductImage = (Bitmap)System.Drawing.Image.FromStream(imageStream),
            };
            

            if (connectedMice.Any())
            {

                if (connectedMice.Any())
                {
                    USBDevice first = connectedMice.First();

                    mouse.Name = first.DevicePrettyName;
                }

                devices.Add(mouse);
            }
            return devices;
            //return new List<ControlDevice>
            //{
            //    new ControlDevice
            //    {
            //        Name = "Steelseries Rival 3",
            //        DeviceType = SimpleLed.DeviceTypes.Mouse,
            //        Driver = this,
            //        LEDs = leds,
            //        ProductImage = (Bitmap)System.Drawing.Image.FromStream(imageStream),
            //    }
            //};

        }

        public void Dispose()
        {
            for (byte i = 1; i <= 4; i++)
            {
                r3controller.packets(i, 0, 0, 0);
            }
        }

        public T GetConfig<T>() where T : SLSConfigData
        {
            throw new NotImplementedException();
        }

        public DriverProperties GetProperties()
        {
            return new DriverProperties
            {
                SupportsPush = true,
                SupportsPull = false,
                IsSource = false,
                SupportsCustomConfig = false,
                Id = Guid.Parse("afeeea5d-49b9-43dd-bb66-dfd20c1c3751"),
                Author = "Rafee",
                Blurb = "Driver for direct control of the Steelseries Rival 3",
                CurrentVersion = new ReleaseNumber("1.3.0.10"),
                IsPublicRelease = true,
                GitHubLink = "https://github.com/RafeeSamith/Driver.Rival3Direct"
            };
        }

        public void InterestedUSBChange(int VID, int PID, bool connected)
        {
            throw new NotImplementedException();
        }

        public string Name()
        {
            return "Steelseries Rival 3 (Direct)";
        }

        public class Rival3LEDData : ControlDevice.LEDData
        {
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
        }

        public Rival3DirectDriver()
        {
            for (int i = 0; i < 4; i++)
            {
                leds[i] = new ControlDevice.LedUnit
                {
                    LEDName = "LED " + i,
                    Data = new Rival3LEDData
                    {
                        LEDNumber = i,
                    },
                };
            }
        }

        public void Pull(ControlDevice controlDevice)
        {
            throw new NotImplementedException();
        }

        public void Push(ControlDevice controlDevice)
        {
            r3controller = new Rival3Controller();
            for (byte i = 0; i < 4; i++)
            {
                byte r = (byte)controlDevice.LEDs[i].Color.Red;
                byte g = (byte)controlDevice.LEDs[i].Color.Green;
                byte b = (byte)controlDevice.LEDs[i].Color.Blue;
                r3controller.packets(i, r, g, b);
            }
            //r3controller = new Rival3Controller();
            //r3controller.packets(255, 25, 0);
        }

        public void PutConfig<T>(T config) where T : SLSConfigData
        {
            throw new NotImplementedException();
        }
    }
}
