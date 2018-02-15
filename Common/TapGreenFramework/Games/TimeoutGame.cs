using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TapGreenFramework.GameEngine;
using TapGreenFramework.GameEngine.Enums;

namespace TapGreenFramework.Games
{
    public class TimeoutGame : TapGreenEngine
    {
        private const int TEXT_SIZE = 104;

        private int _goodTap;
        private int _badTap;
        private int _emptyTap;
        private int _totalGoodTap;
        private int _totalBadTap;
        private double _percent;

        private TimeSpan _allGameTime;
        private TimeSpan _restGameTime;

        private Vector2 _timeTextVector;
        private Vector2 _scoreTextVector;

        public event EventHandler GameClosed;

        public TimeoutGame(ScreenManager screenManager)
            : base(screenManager)
        {
            _goodTap = 0;
            _badTap = 0;
            _emptyTap = 0;
            _totalGoodTap = 0;
            _totalBadTap = 0;

            _restGameTime = new TimeSpan();
            _allGameTime = new TimeSpan(0, 1, 1);
        }

        public void HandleInput(Point point)
        {
            HandleInputEngine(point);
        }

        public void LoadContent()
        {
            LoadContentEngine();

            float y = (Game.GraphicsDevice.Viewport.Height - Game.GraphicsDevice.Viewport.Width) / 2;

            _timeTextVector = new Vector2(separator, y - TEXT_SIZE);
            _scoreTextVector = new Vector2(separator, (separator * ScreenManager.BlockInLine * 2) + (blockWidth * ScreenManager.BlockInLine) + y);
        }

        public void Update(GameTime gameTime)
        {
            if (!GameStartTime.HasValue)
                GameStartTime = gameTime.TotalGameTime;

            if (!IsPaused)
            {
                TimeSpan totalGameTime = gameTime.TotalGameTime - GameStartTime.Value;
                _restGameTime = _allGameTime - totalGameTime;

                if (_allGameTime < (gameTime.TotalGameTime - GameStartTime))
                {
                    _restGameTime = new TimeSpan(0, 0, 0);

                    GameClosed.Invoke(_percent, new System.EventArgs());
                }
                else if (NextStepTime < (gameTime.TotalGameTime - GameStartTime))
                {
                    Random rand = new Random();
                    long offset = 0;

                    var offsetBound = General.GetOffsetBound(ScreenManager.BlockInLine);

                    if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_0 && totalGameTime.TotalSeconds <= General.TimePeriod.PERIOD_1)
                        offset = rand.Next(offsetBound[0].Item1, offsetBound[0].Item2) * 10000;
                    else if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_1 && totalGameTime.TotalSeconds <= General.TimePeriod.PERIOD_2)
                        offset = rand.Next(offsetBound[1].Item1, offsetBound[1].Item2) * 10000;
                    else if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_2 && totalGameTime.TotalSeconds <= General.TimePeriod.PERIOD_3)
                        offset = rand.Next(offsetBound[2].Item1, offsetBound[2].Item2) * 10000;
                    else if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_3 && totalGameTime.TotalSeconds <= General.TimePeriod.PERIOD_4)
                        offset = rand.Next(offsetBound[3].Item1, offsetBound[3].Item2) * 10000;
                    else if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_4 && totalGameTime.TotalSeconds <= General.TimePeriod.PERIOD_5)
                        offset = rand.Next(offsetBound[4].Item1, offsetBound[4].Item2) * 10000;
                    else if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_5 && totalGameTime.TotalSeconds <= General.TimePeriod.PERIOD_6)
                        offset = rand.Next(offsetBound[5].Item1, offsetBound[5].Item2) * 10000;
                    else if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_6 && totalGameTime.TotalSeconds <= General.TimePeriod.PERIOD_7)
                        offset = rand.Next(offsetBound[6].Item1, offsetBound[6].Item2) * 10000;
                    else if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_7 && totalGameTime.TotalSeconds <= General.TimePeriod.PERIOD_8)
                        offset = rand.Next(offsetBound[7].Item1, offsetBound[7].Item2) * 10000;
                    else if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_8 && totalGameTime.TotalSeconds <= General.TimePeriod.PERIOD_9)
                        offset = rand.Next(offsetBound[8].Item1, offsetBound[8].Item2) * 10000;
                    else if (totalGameTime.TotalSeconds > General.TimePeriod.PERIOD_9)
                        offset = rand.Next(offsetBound[9].Item1, offsetBound[9].Item2) * 10000;

                    UpdateEngine(gameTime, offset);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            DrawEngine(gameTime);

            if (_restGameTime < new TimeSpan(0, 0, 5) && _restGameTime.Seconds % 2 == 0)
                SpriteBatch.DrawString(ScreenManager.GetSpriteFont(General.Fonts.SegoePrint48), string.Format("Time: {0:mm\\:ss\\.f}", _restGameTime), _timeTextVector, Color.DarkRed);
            else
                SpriteBatch.DrawString(ScreenManager.GetSpriteFont(General.Fonts.SegoePrint48), string.Format("Time: {0:mm\\:ss\\.f}", _restGameTime), _timeTextVector, General.Colors.SCORE_FONT);

            CalculateCurrentScore();

            SpriteBatch.DrawString(ScreenManager.GetSpriteFont(General.Fonts.SegoePrint48), string.Format("Percent: {0:N1}%", _percent), _scoreTextVector, General.Colors.SCORE_FONT);

            SpriteBatch.End();
        }

        protected override void TapButton(BlockType type)
        {
            switch (type)
            {
                case BlockType.PLUS:
                    _allGameTime = _allGameTime.Add(new TimeSpan(0, 0, 1));
                    _totalGoodTap++;
                    _goodTap++;
                    break;
                case BlockType.MINUS:
                    _allGameTime = _allGameTime.Add(new TimeSpan(0, 0, -1));
                    _totalBadTap++;
                    _badTap++;
                    break;
                case BlockType.GOOD:
                    _totalGoodTap++;
                    _goodTap++;
                    break;
                case BlockType.BAD:
                    _totalBadTap++;
                    _badTap++;
                    break;
                case BlockType.EMPTY:
                    _emptyTap++;
                    break;
            }
        }

        protected override void SetScore(int points, bool? plusOrMinus)
        {
            if (points != 0)
            {
                if (points > 0)
                    _totalGoodTap++;
                else
                    _totalBadTap++;
            }
            else if (plusOrMinus.HasValue)
            {
                if (plusOrMinus.Value)
                    _totalGoodTap++;
                else
                    _totalBadTap++;
            }
        }

        protected override void AfterPause(TimeSpan time)
        {
            _allGameTime = _allGameTime.Add(time);
        }

        private void CalculateCurrentScore()
        {
            _percent = 0.0;

            if (_totalGoodTap == 0 || _totalBadTap == 0)
                return;

            double goodPercent = (_goodTap * 100) / _totalGoodTap;
            double badPercent = (_badTap * 100) / (_totalBadTap * 2.5);
            //double emptyPercent = (_socreGame.EmptyTap * 100) / (_socreGame.TotalGoodTap + _socreGame.TotalBadTap);

            if (goodPercent >= (badPercent /*+ emptyPercent*/))
                _percent = goodPercent - (badPercent /*+ emptyPercent*/);
        }
    }
}
