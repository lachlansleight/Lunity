using System;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayUtils
{
	public static TU[] Map<T, TU>(this T[] arr, Func<T, TU> function)
	{
		var output = new TU[arr.Length];
		for (var i = 0; i < arr.Length; i++) {
			output[i] = function(arr[i]);
		}
		return output;
	}

	public static TU Reduce<T, TU>(this T[] arr, Func<TU, T, TU> function, TU start)
	{
		var output = start;
		for (var i = 0; i < arr.Length; i++) {
			output = function(output, arr[i]);
		}
		return output;
	}

	public static T Find<T>(this T[] arr, T item)
	{
		for (var i = 0; i < arr.Length; i++) {
			if (arr[i].Equals(item)) return item;
		}
		return default;
	}

	public static int FindIndex<T>(this T[] arr, T item)
	{
		for (var i = 0; i < arr.Length; i++) {
			if (arr[i].Equals(item)) return i;
		}
		return -1;
	}

	public static void ForEach<T>(this T[] arr, Action<T> function)
	{
		for (var i = 0; i < arr.Length; i++) {
			function(arr[i]);
		}
	}

	public static T[] Filter<T>(this T[] arr, Func<T, bool> function)
	{
		var output = new List<T>();
		for (var i = 0; i < arr.Length; i++) {
			if (function(arr[i])) output.Add(arr[i]);
		}
		return output.ToArray();
	}

	/// Note - unlike JS, this doesn't mutate the original array, since that behaviour really annoys me :D
	public static T[] Sort<T>(this T[] arr, IComparer<T> function)
	{
		var output = new T[arr.Length];
		Array.Copy(arr, 0, output, 0, arr.Length);
		Array.Sort(output, function);
		return output;
	}

	public static string Join<T>(this T[] arr, string separator = "")
	{
		var output = "";
		for (var i = 0; i < arr.Length; i++) {
			if (i > 0) output += separator;
			output += arr[i].ToString();
		}
		return output;
	}
}