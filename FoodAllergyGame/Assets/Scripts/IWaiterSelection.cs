/// <summary>
/// Interface for an object that is clickable and is a waiter destination
/// </summary>
interface IWaiterSelection{

	/// <summary>
	/// Raises the waiter arrived event.
	/// This is called when the waiter actually arrives at its destination
	/// </summary>
	void OnWaiterArrived();

	/// <summary>
	/// Raises the clicked event.
	/// Most of the time it will tell the waiter to move to that position
	/// </summary>
	void OnClicked();
}
