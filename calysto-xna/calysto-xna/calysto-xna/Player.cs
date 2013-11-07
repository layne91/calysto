using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace calysto_xna {
    class Player : NPC {

        private const int PLAYER_SPEED = 0;
        //private int moveTimer = 0;
        //private int moveInterval = 100;

        public Player(Texture2D spriteSheet, int xNumberOfFrames, int yNumberOfFrames, int spriteWidth, int spriteHeight, Vector2 objectPosition)
            : base(spriteSheet, xNumberOfFrames, yNumberOfFrames, spriteWidth, spriteHeight, objectPosition) {

        }

        public void Update(GameTime gameTime) {
            move(gameTime);
            animate(gameTime);
        }

        private void move(GameTime gameTime) {
            KeyboardState kbs = Keyboard.GetState();
            //moveTimer += gameTime.ElapsedGameTime.Milliseconds;
            //if (moveTimer >= moveInterval) {
                if (kbs.IsKeyDown(Keys.Up) || kbs.IsKeyDown(Keys.Down) || kbs.IsKeyDown(Keys.Left) || kbs.IsKeyDown(Keys.Right)) {
                    isMoving = true;

                    if (kbs.IsKeyDown(Keys.Up)) {
                        movementStatus = MovementStatus.Up;
                        //objectRectangle.Y -= spriteHeight;
                    } if (kbs.IsKeyDown(Keys.Down)) {
                        movementStatus = MovementStatus.Down;
                        //objectRectangle.Y += spriteHeight;
                    } if (kbs.IsKeyDown(Keys.Left)) {
                        movementStatus = MovementStatus.Left;
                        //objectRectangle.X -= spriteWidth;
                    } if (kbs.IsKeyDown(Keys.Right)) {
                        movementStatus = MovementStatus.Right;
                        //objectRectangle.X += spriteWidth;
                    }
                    //moveTimer = 0;
                //}
            }

            if (kbs.IsKeyUp(Keys.Up) && kbs.IsKeyUp(Keys.Down) && kbs.IsKeyUp(Keys.Left) && kbs.IsKeyUp(Keys.Right)) {
                isMoving = false;
            }
        }

        //public override void Draw(SpriteBatch spriteBatch, Color color) {
        //    base.Draw(spriteBatch, color);
        //}
    }
}
