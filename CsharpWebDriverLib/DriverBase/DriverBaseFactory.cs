

namespace CsharpWebDriverLib
{
    public abstract class DriverBaseFactory
    {
        public static IDriverBase CreateDriverBase(DriverBaseParams driverBaseParams)
        {
            return new DriverBase(driverBaseParams);
        }
    }
}
