using System;

/**
 * Random permutation algorithm called the Fisher-Yates algorithm
 * https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
 */
static class RandomExtensions
{
	public static void Shuffle<T> (this Random rng, T[] array)
	{
		int n = array.Length;
		while (n > 1) 
		{
			int k = rng.Next(n--);
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}
}