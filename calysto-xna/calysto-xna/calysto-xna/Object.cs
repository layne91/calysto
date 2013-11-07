using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace calysto_xna {
    abstract class Object {

        protected const int STARTING_FRAME = 0;
        protected Texture2D spriteSheet;
        protected int xNumberOfFrames;
        protected int yNumberOfFrames;
        public int spriteWidth;
        public int spriteHeight;
        protected Rectangle objectRectangle;

        //Testing
        private bool SIZE_16 = false;

        protected Object(Texture2D spriteSheet, int xNumberOfFrames, int yNumberOfFrames, int spriteWidth, int spriteHeight, Vector2 objectPosition) {
            this.spriteSheet = spriteSheet;
            this.xNumberOfFrames = xNumberOfFrames;
            this.yNumberOfFrames = yNumberOfFrames;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.objectRectangle = new Rectangle((int)objectPosition.X, (int)objectPosition.Y, spriteWidth, spriteHeight);

            if (SIZE_16) {
                spriteWidth *= 2;
                spriteHeight *= 2;
                objectRectangle.Width = spriteWidth;
                objectRectangle.Height = spriteHeight;
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch, Color color);
    }
}
