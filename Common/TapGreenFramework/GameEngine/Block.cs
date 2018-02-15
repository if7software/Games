using System;
using TapGreenFramework.GameEngine.Enums;
using Microsoft.Xna.Framework;

namespace TapGreenFramework.GameEngine
{
	public class Block
	{
		private const double IMAGE_PROCENT = 0.75;
		private static object sync = new object();

		private int _scaleCount;

		private TimeSpan _restLockTime;
		private TimeSpan? _lastLockTime;

		private int _id;
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		private BlockType _type;
		public BlockType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public Rectangle BlockRect { get; set; }
		public Rectangle ImageRect { get; set; }

		public bool IsEmpty
		{
			get { return _type == BlockType.EMPTY; }
		}

		public Block(int id, Vector2 start, Vector2 stop)
		{
			Initialize();

			_id = id;
			int blockWidth = (int)(stop.X - start.X);
			int blockHeight = (int)(stop.Y - start.Y);

			int imageWidth = (int)(blockWidth * IMAGE_PROCENT);
			int imageHeight = (int)(blockHeight * IMAGE_PROCENT);

			BlockRect = new Rectangle((int)start.X, (int)start.Y, blockWidth, blockHeight);
			ImageRect = new Rectangle((int)(start.X + ((blockWidth - imageWidth) / 2)), (int)(start.Y + ((blockHeight - imageHeight) / 2)), imageWidth, imageHeight);
		}

		public void SetBlockType(GameTime gameTime, BlockType type)
		{
			_type = type;
			_lastLockTime = gameTime.TotalGameTime;
			_restLockTime = new TimeSpan(0, 0, 0, 0, 750);
		}

		public void CheckBlock(GameTime gameTime, out int points, out bool? plusOrMinus)
		{
			plusOrMinus = null;
			points = 0;

			if (!IsEmpty)
			{
				if (_restLockTime < TimeSpan.Zero && _scaleCount == 0)
				{
					if (_type == BlockType.BAD)
						points = 50;
					else if (_type == BlockType.GOOD)
						points = -50;
					else if (_type == BlockType.PLUS)
						plusOrMinus = true;
					else if (_type == BlockType.MINUS)
						plusOrMinus = false;

					ClearBlock();
				}
			}
		}

		public void SyncRestLockTime(GameTime gameTime, bool isPaused)
		{
			lock (sync)
			{
				if (!isPaused && _lastLockTime.HasValue)
				{
					TimeSpan time = gameTime.TotalGameTime - _lastLockTime.Value;
					_lastLockTime = gameTime.TotalGameTime;
					_restLockTime -= time;
				}
			}
		}

		public bool IsScaleBlock()
		{
			if (_scaleCount > 0)
			{
				if (_scaleCount == 1)
					ClearBlock();

				_scaleCount--;
				return true;
			}

			return false;
		}

		public void SetScaleBlock()
		{
			_scaleCount = 3;
		}

		public void Initialize()
		{
			_scaleCount = 0;
			ClearBlock();
		}

		private void ClearBlock()
		{
			_type = BlockType.EMPTY;
			_restLockTime = new TimeSpan();
			_lastLockTime = null;
		}
	}
}