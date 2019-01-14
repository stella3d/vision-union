using Unity.Mathematics;

public static partial class Activation 
{
	public static float RectifiedLinearUnit(float x)
	{
		return math.max(x, 0f);
	}
	
	public static byte RectifiedLinearUnit(byte x)
	{
		return (byte)math.max(x, 0);
	}
}
