using System.Linq;
using UnityEngine;

namespace Lunity
{
	public static class LRandom
	{
		/// <summary>
		/// Gets a random number between min (inclusive) and max (non exclusive), unless it's a particular excluded number.
		/// </summary>
		/// <param name="min">Minimum number in range - inclusive</param>
		/// <param name="max">Maximum number in range - exclusive</param>
		/// <param name="except">Function will avoid returning this number if possible</param>
		/// <param name="attempts">How many times to re-roll the number if the 'except' number comes up. Will return the except number if it has tried this many times</param>
		public static int RangeExcept(int min, int max, int except, int attempts = 50)
		{
			var choice = Random.Range(min, max);
			var attempt = 0;
			while (choice == except && attempt < attempts) {
				choice = Random.Range(min, max);
				attempt++;
			}

			if (choice == except) {
				Debug.LogWarning("RangeExcept failed to get a value in the range that wasn't " + except);
			}

			return choice;
		}
		
		/// <summary>
		/// Gets a random number between min (inclusive) and max (non exclusive), unless it's in the list of excluded numbers
		/// </summary>
		/// <param name="min">Minimum number in range - inclusive</param>
		/// <param name="max">Maximum number in range - exclusive</param>
		/// <param name="except">Function will avoid returning any number in this list if possible</param>
		/// <param name="attempts">How many times to re-roll the number rolled number is in the exceptions list. Will return the except number if it has tried this many times</param>
		public static int RangeExcept(int min, int max, int[] except, int attempts = 50)
		{
			var choice = Random.Range(min, max);
			if (except == null || except.Length == 0) return choice;
			
			var attempt = 0;
			while (except.Contains(choice) && attempt < attempts) {
				choice = Random.Range(min, max);
				attempt++;
			}

			if (except.Contains(choice)) {
				Debug.LogWarning("RangeExcept failed to get a value in the range that wasn't " + except);
			}

			return choice;
		}
	}
}