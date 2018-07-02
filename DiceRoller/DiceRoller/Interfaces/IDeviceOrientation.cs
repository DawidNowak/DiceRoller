namespace DiceRoller.Interfaces
{
	public enum DeviceOrientations
	{
		Undefined,
		Landscape,
		Portrait
	}

	public interface IDeviceOrientation
	{
		DeviceOrientations GetOrientation();
	}
}
