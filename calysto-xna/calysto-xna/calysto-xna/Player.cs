using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace calysto_xna {
    class Player : NPC {
        private int moveTimer = 0;
        private int moveInterval = 50;
        private ContentManager Content;

        public Player(Texture2D spriteSheet, int xNumberOfFrames, int yNumberOfFrames, int spriteWidth, int spriteHeight, Vector2 objectPosition, ContentManager Content)
            : base(spriteSheet, xNumberOfFrames, yNumberOfFrames, spriteWidth, spriteHeight, objectPosition) {
                this.Content = Content;
        }

        public void Update(GameTime gameTime, Rectangle mapRect, ref Rectangle mapView, ref List<Rectangle> cRectangles, ref List<Transition> transitions) {
            move(gameTime, mapRect, ref mapView, ref cRectangles, ref transitions);
            animate(gameTime);
            hitTransition(transitions, mapView);
        }

        private void move(GameTime gameTime, Rectangle mapRect, ref Rectangle mapView, ref List<Rectangle> cRectangles, ref List<Transition> transitions) {
            int middleX = mapView.Width / 2;
            int middleY = mapView.Height / 2;
            int yConstraint = spriteHeight * 2;
            int xConstraint = spriteWidth * 2;

            KeyboardState kbs = Keyboard.GetState();
            moveTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (moveTimer >= moveInterval) {
                if (kbs.IsKeyDown(Keys.Up) || kbs.IsKeyDown(Keys.Down) || kbs.IsKeyDown(Keys.Left) || kbs.IsKeyDown(Keys.Right)) {
                    isMoving = true;

                    if (kbs.IsKeyDown(Keys.Up)) {
                        movementStatus = MovementStatus.Up;
                        if (!collides(cRectangles, 0, -spriteHeight)) {
                            if (mapInView(mapView, mapRect, 0, spriteHeight, movementStatus)) {
                                if (objectRectangle.Y > middleY)
                                    objectRectangle.Y -= spriteHeight;
                                else {
                                    scroll(movementStatus, ref mapView, ref cRectangles, ref transitions);
                                }
                            } else if (objectRectangle.Y > 0)
                                objectRectangle.Y -= spriteHeight;
                        }
                    }

                    if (kbs.IsKeyDown(Keys.Down)) {
                        movementStatus = MovementStatus.Down;
                        if (!collides(cRectangles, 0, spriteHeight)) {
                            if (mapInView(mapView, mapRect, 0, spriteHeight, movementStatus)) {
                                if (objectRectangle.Y < middleY)
                                    objectRectangle.Y += spriteHeight;
                                else {
                                    scroll(movementStatus, ref mapView, ref cRectangles, ref transitions);
                                }
                            } else if (objectRectangle.Y + spriteHeight < mapView.Height)
                                objectRectangle.Y += spriteHeight;
                        }
                    }

                    if (kbs.IsKeyDown(Keys.Left)) {
                        movementStatus = MovementStatus.Left;
                        if (!collides(cRectangles, -spriteWidth, 0)) {
                            if (mapInView(mapView, mapRect, spriteWidth, 0, movementStatus)) {
                                if (objectRectangle.X > middleX)
                                    objectRectangle.X -= spriteWidth;
                                else {
                                    scroll(movementStatus, ref mapView, ref cRectangles, ref transitions);
                                }
                            } else if (objectRectangle.X > 0)
                                objectRectangle.X -= spriteWidth;
                        }
                    }

                    if (kbs.IsKeyDown(Keys.Right)) {
                        movementStatus = MovementStatus.Right;
                        if (!collides(cRectangles, spriteWidth, 0)) {
                            if (mapInView(mapView, mapRect, spriteWidth, 0, movementStatus)) {
                                if (objectRectangle.X < middleX)
                                    objectRectangle.X += spriteWidth;
                                else {
                                    scroll(movementStatus, ref mapView, ref cRectangles, ref transitions);
                                }
                            } else if (objectRectangle.X + spriteWidth < mapView.Width)
                                objectRectangle.X += spriteWidth;
                        }
                    }
                    moveTimer = 0;
                }
            }

            if (kbs.IsKeyUp(Keys.Up) && kbs.IsKeyUp(Keys.Down) && kbs.IsKeyUp(Keys.Left) && kbs.IsKeyUp(Keys.Right)) {
                isMoving = false;
            }
        }

        private void scroll(MovementStatus movementStatus, ref Rectangle mapView, ref List<Rectangle> cRectangles, ref List<Transition> transitions) {
            switch (movementStatus) {
                case MovementStatus.Up:
                    mapView.Y += -spriteHeight;
                    for (int i = 0; i < cRectangles.Count; i++) {
                        cRectangles[i] = new Rectangle(cRectangles[i].X, cRectangles[i].Y + spriteHeight, cRectangles[i].Width, cRectangles[i].Height);
                    }for(int i = 0; i < transitions.Count; i++){
                        transitions[i].tRect = new Rectangle(transitions[i].tRect.X, transitions[i].tRect.Y + spriteHeight, transitions[i].tRect.Width, transitions[i].tRect.Height);
                    }
                    break;
                case MovementStatus.Down:
                    mapView.Y += spriteHeight;
                    for (int i = 0; i < cRectangles.Count; i++) {
                        cRectangles[i] = new Rectangle(cRectangles[i].X, cRectangles[i].Y - spriteHeight, cRectangles[i].Width, cRectangles[i].Height);
                    }for(int i = 0; i < transitions.Count; i++){
                        transitions[i].tRect = new Rectangle(transitions[i].tRect.X, transitions[i].tRect.Y - spriteHeight, transitions[i].tRect.Width, transitions[i].tRect.Height);
                    }
                    break;
                case MovementStatus.Left:
                    mapView.X += -spriteWidth;
                    for (int i = 0; i < cRectangles.Count; i++) {
                        cRectangles[i] = new Rectangle(cRectangles[i].X + spriteWidth, cRectangles[i].Y, cRectangles[i].Width, cRectangles[i].Height);
                    }for(int i = 0; i < transitions.Count; i++){
                        transitions[i].tRect = new Rectangle(transitions[i].tRect.X + spriteWidth, transitions[i].tRect.Y, transitions[i].tRect.Width, transitions[i].tRect.Height);
                    }
                    break;
                case MovementStatus.Right:
                    mapView.X += spriteWidth;
                    for (int i = 0; i < cRectangles.Count; i++) {
                        cRectangles[i] = new Rectangle(cRectangles[i].X - spriteWidth, cRectangles[i].Y, cRectangles[i].Width, cRectangles[i].Height);
                    }for(int i = 0; i < transitions.Count; i++){
                        transitions[i].tRect = new Rectangle(transitions[i].tRect.X - spriteWidth, transitions[i].tRect.Y, transitions[i].tRect.Width, transitions[i].tRect.Height);
                    }
                    break;
                case MovementStatus.None:
                    break;
            }
        }

        private bool collides(List<Rectangle> cRectangles, int xChange, int yChange) {
            bool collision = false;
            Rectangle tempRect = objectRectangle;
            tempRect.X += xChange;
            tempRect.Y += yChange;

            foreach (Rectangle r in cRectangles) {
                 if (tempRect.Intersects(r)) {
                     collision = true;
                     break;
                 }
            }
            return collision;
        }

        private void hitTransition(List<Transition> transitions, Rectangle mapView){
            foreach (Transition t in transitions) {
                if (objectRectangle.Intersects(t.tRect)) {
                    Game1.currentGameState = new MapState(Content, mapView.Width, mapView.Height, t.nextMapName, this, t);
                    break;
                }
            }
        }

        //Used for map scrolling
        private bool mapInView(Rectangle mapView, Rectangle mapRect, int xChange, int yChange, MovementStatus movementStatus) {

            bool inView = false;
            mapRect.X += xChange;
            mapRect.Y += yChange;

            switch (movementStatus) {
                case MovementStatus.Up:
                    if (mapView.Y > 0)
                        inView = true;
                    break;
                case MovementStatus.Down:
                    if (mapView.Y < (mapRect.Height - mapView.Height))
                        inView = true;
                    break;
                case MovementStatus.Left:
                    if (mapView.X > 0)
                        inView = true;
                    break;
                case MovementStatus.Right:
                    if (mapView.X < (mapRect.Width - mapView.Width))
                        inView = true;
                    break;
                case MovementStatus.None:
                    inView = false;
                    break;
            }

            return inView;
        }

        //public override void Draw(SpriteBatch spriteBatch, Color color) {
        //    base.Draw(spriteBatch, color);
        //}
    }
}
