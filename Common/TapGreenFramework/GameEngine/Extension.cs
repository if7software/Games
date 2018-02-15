using System;
using TapGreenFramework.GameEngine.Enums;

namespace TapGreenFramework.GameEngine
{
	public class Extension
	{
		public static readonly BlockType[] BLOCKS = { BlockType.BAD, BlockType.BAD, BlockType.BAD,
													  BlockType.MINUS, BlockType.MINUS, BlockType.PLUS, BlockType.PLUS,
													  BlockType.GOOD, BlockType.GOOD, BlockType.GOOD};

		public static BlockType GetRandomBlockType(DateTime dt, int randInt)
		{
			return BLOCKS[randInt % BLOCKS.Length];
		}
	}
}