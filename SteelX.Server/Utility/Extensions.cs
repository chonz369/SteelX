using System.Collections.Generic;

namespace SteelX.Server.Utility
{
	/// <summary>
	/// This extension is used to assist in auto generating ids for jobs and rooms
	/// </summary>
	public static class SortedListExtensions
	{
		///Add item to sortedList (numeric key) to next available key item, and return key
		public static int AddNext<T>(this SortedList<int, T> sortedList, T item)
		{
			int key = 1; // Make it 0 to start from Zero based index
			int count = sortedList.Count;

			int counter=0;
			do
			{
				if (count == 0) break;
				int nextKeyInList = sortedList.Keys[counter++];

				if (key != nextKeyInList) break;

				key = nextKeyInList +1;

				if (count == 1 || counter == count  ) break;


				if (key != sortedList.Keys[counter])
					break;

			} while (true);

			sortedList.Add(key, item);
			return key;
		}
	}
}