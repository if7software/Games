using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapGreenFramework.Utilities;
using TapGreenFramework.GameEngine.Enums;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TapGreenFramework.GameEngine
{
	public abstract class TapGreenEngine
	{
		protected float blockWidth;
		protected float separator;

        private Dictionary<BlockType, Texture2D> blockItems;
		private List<Block> layoutItems;

        protected SpriteBatch SpriteBatch
		{
			get { return _screenManager.SpriteBatch; }
		}

		protected Game Game
		{
			get { return _screenManager.Game; }
		}

        private TimeSpan? _gameStartTime;
        protected TimeSpan? GameStartTime
        {
            get { return _gameStartTime; }
            set { _gameStartTime = value; }
        }

        private TimeSpan _nextStepTime;
        protected TimeSpan NextStepTime
        {
            get { return _nextStepTime; }
        }

        private ScreenManager _screenManager;
        protected ScreenManager ScreenManager
        {
            get { return _screenManager; }
            set { _screenManager = value; }
        }

        private bool _isPaused;
		public bool IsPaused
		{
			get { return _isPaused; }
			set { _isPaused = value; }
		}

		public TapGreenEngine(ScreenManager screenManager)
		{
			_isPaused = false;
			_gameStartTime = null;
			
			_nextStepTime = new TimeSpan(0, 0, 1);
			_screenManager = screenManager;

            layoutItems = new List<Block>();
			blockItems = new Dictionary<BlockType, Texture2D>();
		}

		public void AddTimeAfterPause(TimeSpan time)
		{
            _nextStepTime = _nextStepTime.Add(time);

            AfterPause(time);
		}

		protected void LoadContentEngine()
		{
			separator = (Game.GraphicsDevice.Viewport.Width * 0.1f) / (_screenManager.BlockInLine * 2);
			blockWidth = (Game.GraphicsDevice.Viewport.Width / _screenManager.BlockInLine) - (2 * separator);

			float y = (Game.GraphicsDevice.Viewport.Height - Game.GraphicsDevice.Viewport.Width) / 2;

			for (int i = 0; i < _screenManager.BlockInLine; i++)
			{
				float x = 0;
				float top = 0;
				float bottom = 0;

				y += separator;
				top = y;

				y += blockWidth;
				bottom = y;

				y += separator;

				for (int j = 0; j < _screenManager.BlockInLine; j++)
				{
					x += separator;

					Vector2 start = new Vector2(x, top);

					x += blockWidth;

					Vector2 stop = new Vector2(x, bottom);

					x += separator;

					Block layoutItem = new Block(i * _screenManager.BlockInLine + j, start, stop);
					layoutItems.Add(layoutItem);
				}
			}

			foreach (BlockType item in Enum.GetValues(typeof(BlockType)))
			{
				Color colorBackgroundBlock = Color.White;

				switch (item)
				{
					case BlockType.EMPTY:
						colorBackgroundBlock = General.Colors.BLOCK_EMPTY;
						break;
					case BlockType.MINUS:
					case BlockType.BAD:
						colorBackgroundBlock = General.Colors.BLOCK_RED;
						break;
					case BlockType.PLUS:
					case BlockType.GOOD:
						colorBackgroundBlock = General.Colors.BLOCK_GREEN;
						break;
				}

				Texture2D blockBackgroundTexture = new Texture2D(Game.GraphicsDevice, (int)blockWidth, (int)blockWidth, false, SurfaceFormat.Color);
				StaticTexture2D.FillTexture2DWithRoundCorners(ref blockBackgroundTexture, (int)blockWidth, (int)blockWidth, colorBackgroundBlock, 0, 45, 0);

				blockItems[item] = blockBackgroundTexture;
			}
		}

		protected void HandleInputEngine(Point point)
		{
			foreach (var layout in layoutItems)
			{
				BlockType blockType = layout.Type;

				if (layout.BlockRect.Contains(point))
				{
					layout.SetScaleBlock();

                    TapButton(blockType);
				}
			}
		}

		protected void UpdateEngine(GameTime gameTime, long offset)
		{
            Random rand = new Random();
            DateTime dt = DateTime.Now;
            _nextStepTime += new TimeSpan(offset);
            var layoutList = layoutItems.Where(a => a.IsEmpty).Select(a => a.Id).ToArray();

            if (layoutList.Any())
            {
                BlockType type = Extension.GetRandomBlockType(dt, rand.Next());

                int number = (dt.Second + dt.Minute) * dt.Millisecond % layoutList.Count();
                int blockId = layoutList[number];

                layoutItems[blockId].SetBlockType(gameTime, type);
            }
        }

		protected void DrawEngine(GameTime gameTime)
		{
			foreach (var block in layoutItems)
			{
				int points = 0;
				bool? plusOrMinus = null;

				block.SyncRestLockTime(gameTime, _isPaused);
				block.CheckBlock(gameTime, out points, out plusOrMinus);
                SetScore(points, plusOrMinus);

				switch (block.Type)
				{
					case BlockType.EMPTY:

						if (block.IsScaleBlock())
							SpriteBatch.Draw(blockItems[BlockType.EMPTY], destinationRectangle: block.ImageRect, scale: new Vector2(10, 10), color: Color.White);
						else
							SpriteBatch.Draw(blockItems[BlockType.EMPTY], block.BlockRect, Color.White);

						break;
					case BlockType.PLUS:

						if (block.IsScaleBlock())
							SpriteBatch.Draw(blockItems[BlockType.PLUS], destinationRectangle: block.ImageRect, scale: new Vector2(10, 10), color: Color.White);
						else
							SpriteBatch.Draw(blockItems[BlockType.PLUS], block.BlockRect, Color.White);

						SpriteBatch.Draw(_screenManager.GetTexture2D(General.Textures.Plus).Texture, block.ImageRect, Color.White);

						break;
					case BlockType.GOOD:

						if (block.IsScaleBlock())
							SpriteBatch.Draw(blockItems[BlockType.GOOD], destinationRectangle: block.ImageRect, scale: new Vector2(10, 10), color: Color.White);
						else
							SpriteBatch.Draw(blockItems[BlockType.GOOD], block.BlockRect, Color.White);

						SpriteBatch.Draw(_screenManager.GetTexture2D(General.Textures.Symbol).Texture, block.ImageRect, Color.White);

						break;
					case BlockType.MINUS:

						if (block.IsScaleBlock())
							SpriteBatch.Draw(blockItems[BlockType.MINUS], destinationRectangle: block.ImageRect, scale: new Vector2(10, 10), color: Color.White);
						else
							SpriteBatch.Draw(blockItems[BlockType.MINUS], block.BlockRect, Color.White);

						SpriteBatch.Draw(_screenManager.GetTexture2D(General.Textures.Minus).Texture, block.ImageRect, Color.White);

						break;
					case BlockType.BAD:

						if (block.IsScaleBlock())
							SpriteBatch.Draw(blockItems[BlockType.BAD], destinationRectangle: block.ImageRect, scale: new Vector2(10, 10), color: Color.White);
						else
							SpriteBatch.Draw(blockItems[BlockType.BAD], block.BlockRect, Color.White);

						SpriteBatch.Draw(_screenManager.GetTexture2D(General.Textures.Cross).Texture, block.ImageRect, Color.White);

						break;
				}
			}
		}

        #region Abstract
        protected abstract void SetScore(int points, bool? plusOrMinus);
        protected abstract void TapButton(BlockType type);
        protected abstract void AfterPause(TimeSpan time);
        #endregion
    }
}