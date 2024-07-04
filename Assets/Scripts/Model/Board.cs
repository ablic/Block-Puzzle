using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaspberryGames.BlockPuzzle
{
	public class Board
	{
		public struct Step
		{
			public int X { get; private set; }
			public int Y { get; private set; }
			public Figure Figure { get; set; }
			public int[,] BoardState { get; private set; }
			
			public Step(int x, int y, Figure figure, int[,] boardContent)
			{
				X = x;
				Y = y;
				Figure = figure;
				
				BoardState = new int[Size, Size];
				
				for (int x1 = 0; x1 < Size; x1++)
				{
					for (int y1 = 0; y1 < Size; y1++)
					{
						BoardState[x1, y1] = boardContent[x1, y1];
					}
				}
			}
		}
		
		public const int Size = 10;
		public const int NullBlock = -1;

		public int[,] Content { get; private set; } = new int[Size, Size];
		
		private Stack<Step> steps = new Stack<Step>();
		
		public Board()
		{
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					Content[x, y] = NullBlock;
				}
			}
		}

		public bool CanPlaceFigure(Figure figure)
		{
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					if (CanPlaceFigureTo(x, y, figure))
					{
						return true;
					}
				}
			}

			return false;
		}

		public bool HasOnlyOneMatchingPosition(Figure figure, out Vector2Int lastPosition)
		{
			lastPosition = -Vector2Int.one;

			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					if (!CanPlaceFigureTo(x, y, figure))
						continue;

					if (lastPosition != -Vector2Int.one)
						return false;

					lastPosition = new Vector2Int(x, y);
				}
			}

			return lastPosition != -Vector2Int.one;
		}

		public bool CanPlaceFigureTo(int x, int y, Figure figure)
		{
			for (int x1 = 0; x1 < figure.Size; x1++)
			{
				for (int y1 = 0; y1 < figure.Size; y1++)
				{
					if (IsFigurePartEmpty(figure, x1, y1))
						continue;
					
					int blockX = x + x1;
					int blockY = y + y1;

					if (IsOutOfBounds(blockX) ||
						IsOutOfBounds(blockY) ||
						Content[blockX, blockY] != NullBlock)
					{
						return false;
					}
				}
			}

			return true;
		}

		public void PlaceFigureTo(int x, int y, Figure figure)
		{
			steps.Push(new Step(x, y, figure, Content));

			for (int x1 = 0; x1 < figure.Size; x1++)
			{
				for (int y1 = 0; y1 < figure.Size; y1++)
				{
					if (IsFigurePartEmpty(figure, x1, y1))
						continue;
					
					int blockX = x + x1;
					int blockY = y + y1;

					Content[blockX, blockY] = figure.BlockId;
				}
			}
		}
		
		public Step StepBack()
		{
			if (steps.Count == 0)
				return new Step(-1, -1, null, new int[0, 0]);
				
			Step lastStep = steps.Pop();
						
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					Content[x, y] = lastStep.BoardState[x, y];
				}
			}
			return lastStep;
		}

		public bool HorizontalLineIsFilled(int y)
		{
			for (int x = 0; x < Size; x++)
			{
				if (Content[x, y] == NullBlock)
				{
					return false;
				}
			}

			return true;
		}

		public bool VerticalLineIsFilled(int x)
		{
			for (int y = 0; y < Size; y++)
			{
				if (Content[x, y] == NullBlock)
				{
					return false;
				}
			}

			return true;
		}
		
		public void ClearFilledLines(out List<int> horizontalLines, out List<int> verticalLines)
		{
			horizontalLines = new List<int>(Size);
			verticalLines = new List<int>(Size);
			
			for (int y = 0; y < Size; y++)
			{
				if (HorizontalLineIsFilled(y))
				{
					horizontalLines.Add(y);
				}
			}
			for (int x = 0; x < Size; x++)
			{
				if (VerticalLineIsFilled(x))
				{
					verticalLines.Add(x);
				}
			}
			
			foreach (var y in horizontalLines)
			{
				ClearHorizontalLine(y);
			}
			foreach (var x in verticalLines)
			{
				ClearVerticalLine(x);
			}
		}
		
		public void ClearHorizontalLine(int y)
		{
			for (int x = 0; x < Size; x++)
			{
				Content[x, y] = NullBlock;
			}
		}
		
		public void ClearVerticalLine(int x)
		{
			for (int y = 0; y < Size; y++)
			{
				Content[x, y] = NullBlock;
			}
		}
		
		public void Clear()
		{
			for (int x = 0; x < Size; x++)
			{
				for (int y = 0; y < Size; y++)
				{
					Content[x, y] = NullBlock;
				}
			}
		}
		
		public bool IsOutOfBounds(int c)
		{
			return c < 0 || c >= Size;
		}
		
		private bool IsFigurePartEmpty(Figure figure, int x, int y)
		{
			return figure.Content[x, y] == 0;
		}
	}
}