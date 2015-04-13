namespace Atrico.Lib.Common.Permutations
{
	// Calculate factorials
	public static class Factorials
	{
		// Factorial
		public static ulong Calculate(uint value)
		{
			if (value < 2)
			{
				return value;
			}
			ulong retVal = 1;
			for (ulong i = 2; i <= value; ++i)
			{
				retVal *= i;
			}
			return retVal;
		}

		// Factorial division
		public static ulong Divide(uint top, uint bottom)
		{
			ulong retVal = 1;
			for (ulong i = bottom + 1; i <= top; ++i)
			{
				retVal *= i;
			}
			return retVal;
		}
	}
}
