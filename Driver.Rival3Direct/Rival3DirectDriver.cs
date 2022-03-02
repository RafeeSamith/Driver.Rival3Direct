using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLed;
using Rival3Support;

namespace Driver.Rival3Direct
{
    public class Rival3DirectDriver : ISimpleLed
    {
        Rival3Controller r3controller;

        public event Events.DeviceChangeEventHandler DeviceAdded;
        public event Events.DeviceChangeEventHandler DeviceRemoved;


        public void Configure(DriverDetails driverDetails)
        {
            var drivers = GetDevices();
            foreach (ControlDevice controlDevice in drivers)
            {
                DeviceAdded?.Invoke(this, new Events.DeviceChangeEventArgs(controlDevice));
            }
        }

        ControlDevice.LedUnit[] leds = new ControlDevice.LedUnit[1];
        public List<ControlDevice> GetDevices()
        {
            return new List<ControlDevice>
            {
                new ControlDevice
                {
                    Name = "Steelseries Rival 3 (Direct)",
                    DeviceType = DeviceTypes.Mouse,
                    Driver = this,
                    LEDs = leds
                }
            };

        }

        public void Dispose()
        {
            throw new NotImplementedException();
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
                Blurb = "My first attempt at making a driver, for direct control of the Steelseries Rival 3",
                CurrentVersion = new ReleaseNumber("1.0.0.6"),
                IsPublicRelease = false,
                GitHubLink = "https://github.com/RafeeSamith/Driver.Rival3Direct"
            };
        }

        public void InterestedUSBChange(int VID, int PID, bool connected)
        {
            throw new NotImplementedException();
        }

        public string Name()
        {
            return "Steelseries Rival 3 Direct";
        }

        public class Rival3LEDData : ControlDevice.LEDData
        {

            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
        }

        public Rival3DirectDriver()
        {
            for (int i = 0; i < 1; i++)
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
            r3controller.packets(255, 0, 0);
            for (int i = 0; i < 1; i++)
            {
                byte r = (byte)controlDevice.LEDs[i].Color.Red;
                byte g = (byte)controlDevice.LEDs[i].Color.Green;
                byte b = (byte)controlDevice.LEDs[i].Color.Blue;
                r3controller.packets(r, g, b);
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
