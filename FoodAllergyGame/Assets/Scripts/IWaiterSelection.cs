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

	bool IsQueueable();

	/// <summary>
	/// When the object is pressed, play some sort of animation
	/// </summary>
	void OnPressAnim();

	void AddQueueUI();

	/// <summary>
	/// Update the queue check UI, called from TouchManager
	/// </summary>
	void UpdateQueueUI(int order);

	void DestroyQueueUI();

	void DestroyAllQueueUI();
}
