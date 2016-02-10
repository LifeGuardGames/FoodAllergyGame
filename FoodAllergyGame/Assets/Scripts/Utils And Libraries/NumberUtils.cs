using UnityEngine;
using System.Collections.Generic;

public static class NumberUtils {
	/// <summary>
	/// Generates a non-repeating random list, guaranteeing all ranges be traversed at least once before it repeats again
	/// </summary>
	/// <param name="listLength">Length of your list</param>
	/// <param name="minInclusive">Starting index</param>
	/// <param name="maxInclusive">Ending index</param>
	/// <returns>Non-repeating random list, repeats of overflow</returns>
	public static List<int> UniqueRandomList(int listLength, int minInclusive, int maxInclusive) {
		List<int> returnList = new List<int>();			// Initialize list for recursion
		return UniqueRandomListRecurseOneSet(ref returnList, listLength, minInclusive, maxInclusive);
    }

	/// <summary>
	/// Recursive helper for UniqueRandomList
	/// </summary>
	private static List<int> UniqueRandomListRecurseOneSet(ref List<int> randomList, int listLength, int minInclusive, int maxInclusive) {
		if(randomList.Count >= listLength) {			// Base case
			if(randomList.Count == listLength) {
				return randomList;
			}
			else {
				Debug.LogError("Incorrect size for recursion " + randomList.Count + " " + listLength);
				return new List<int>();					// Return empty list
            }
		}

		int deltaRange = maxInclusive - minInclusive + 1;			// Account for inclusive max
		int loopAmount;
		bool isIncompleteRecursion = listLength - randomList.Count > deltaRange;
		if(isIncompleteRecursion) {
			loopAmount = deltaRange;
		}
		else {
			loopAmount = listLength - randomList.Count;
		}

		List<int> candidates = new List<int>();
		for(int i = minInclusive; i <= maxInclusive; i++) {			// Populate list to traverse
			candidates.Add(i);
		}
		for(int i = 0; i < loopAmount; i++) {						// Pick from list random indexes
			int randomIndex = UnityEngine.Random.Range(0, candidates.Count);	
			randomList.Add(candidates[randomIndex]);
			candidates.RemoveAt(randomIndex);
		}
		return UniqueRandomListRecurseOneSet(ref randomList, listLength, minInclusive, maxInclusive);	// Recurse
	}
}
