namespace Sprint5
{
    public interface ICollectable
	{
		/*
		 * 0 = Not Collected, 1 = Being Collected, 2 = Collected 
		 */
		int CollectionState { get; set; }

		/*
		 * completes an action based on what is collected
		 * /
		void Collect(IPlayer player, GameState gameState);

		/* 
		 * returns true if CollectionState = 2
		 */
		bool HasBeenCollected();

		/*
		 * returns true if CollectionState = 1
		 */
		bool IsBeingCollected();

	}
}
