using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLed;

namespace Driver.Rival3Direct
{
    public class Rival3DirectDriver : ISimpleLed
    {
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

        ControlDevice.LedUnit[] leds = new ControlDevice.LedUnit[4];
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
            throw new NotImplementedException();
        }

        public void InterestedUSBChange(int VID, int PID, bool connected)
        {
            throw new NotImplementedException();
        }

        public string Name()
        {
            return "Steelseries Rival 3 Direct";
        }

        public void Pull(ControlDevice controlDevice)
        {
            throw new NotImplementedException();
        }

        public void Push(ControlDevice controlDevice)
        {
            throw new NotImplementedException();
        }

        public void PutConfig<T>(T config) where T : SLSConfigData
        {
            throw new NotImplementedException();
        }
    }
}
