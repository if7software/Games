using TapGreenFramework.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System;
using TapGreenFramework.Utilities;

namespace TapGreenFramework.UI
{
    public class Button : DrawableGameComponent
    {
        private const int BUTTON_PAUSE = 100;

        private bool _isKeyDown;
        private bool _isPressed;
        private SpriteBatch _spriteBatch;
        private InputState _input;

        private ExTexture2D pauseTexture;
        private ExTexture2D pauseTextureButton;

        public Rectangle Bounds { get; set; }

        public event EventHandler Click;

        public Button(InputState input, TapGreenEngine engine)
            : base(engine.Game)
        {
            _input = input;

            _isKeyDown = false;
            _isPressed = false;
        }

        private bool IntersectWith(Vector2 position)
        {
            Rectangle touchTap = new Rectangle((int)position.X - 1, (int)position.Y - 1, 2, 2);
            return Bounds.Intersects(touchTap);
        }

        private void HandleInput(MouseState mouseState)
        {
            bool pressed = false;
            Vector2 position = Vector2.Zero;

            if ((_input.Gestures.Count > 0) && _input.Gestures[0].GestureType == GestureType.Tap)
            {
                pressed = true;
                position = _input.Gestures[0].Position;
            }

            if (pressed)
            {
                if (!_isKeyDown)
                {
                    if (IntersectWith(position))
                    {
                        Click?.Invoke(this, EventArgs.Empty);
                        _isPressed = false;
                    }
                    _isKeyDown = true;
                }
            }
            else
            {
                _isKeyDown = false;
            }
        }

        public override void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.Tap;



            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Rectangle pauseButton = new Rectangle(GraphicsDevice.Viewport.Bounds.Width - (BUTTON_PAUSE + 20), 20, BUTTON_PAUSE, BUTTON_PAUSE);

            var pauseTexture = new ExTexture2D(Game.Content.Load<Texture2D>(General.Textures.Pause), pauseButton);
            var pauseTextureButton = new ExTexture2D(Game.Content.Load<Texture2D>(General.Textures.PauseButton), pauseButton);
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(Mouse.GetState());

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(pauseTextureButton.Texture, pauseTextureButton.Destination, Color.White);
            _spriteBatch.Draw(pauseTexture.Texture, pauseTextureButton.Destination, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
