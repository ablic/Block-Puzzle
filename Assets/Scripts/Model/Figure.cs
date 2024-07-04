namespace RaspberryGames.BlockPuzzle
{
	public class Figure
	{
		public int Size { get; private set; }
		public int BlockId { get; private set; }
		public int[,] Content { get; private set; }
		public int NumberBlocks { get; private set; }
		
		public Figure(int[,] content, int blockId)
		{
			Content = content;
			Size = Content.GetLength(0);
			BlockId = blockId;
			
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					if (Content[x, y] != 0)
						NumberBlocks++;
				}
			}
		}
	}
}