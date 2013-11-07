using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FuncWorks.XNA.XTiled;

namespace calysto_xna {
    class MapState : GameState {

        private Map map;
        private Rectangle mapView;
        private string mapName;
        private Player player;

        //TESTING
        private Texture2D grid;
        private Vector2 gridPosition;

        public MapState(ContentManager Content, int PreferredBackBufferWidth, int PreferredBackBufferHeight, string mapName, Player player)
            : base(Content) {
            this.mapView = new Rectangle(0, 0, PreferredBackBufferWidth, PreferredBackBufferWidth);
            this.mapName = mapName;
            this.player = player;
        }

        public override void LoadContent() {
            map = Content.Load<Map>("maps/" + mapName);
            grid = Content.Load<Texture2D>("1024x1024_grid");
            gridPosition = new Vector2();
        }

        public override void UnloadContent() {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime) {
            player.Update(gameTime);
            scrollMap(player.movementStatus);
        }

        private void scrollMap(MovementStatus movementStatus) {
            Rectangle delta = mapView;
            //for each, check collision on player
            if (player.isMoving) {
                switch (movementStatus) {
                    case MovementStatus.Up:
                        //gridPosition.Y -= player.spriteHeight;
                        //delta.Y -= player.spriteHeight;
                        //yChange -= player.spriteHeight;
                        break;
                    case MovementStatus.Down:
                        //gridPosition.Y += player.spriteHeight;
                        //delta.Y += player.spriteHeight;
                        //yChange += player.spriteHeight;
                        break;
                    case MovementStatus.Left:
                        //gridPosition.X -= player.spriteWidth;
                        //delta.X -= player.spriteWidth;
                        //xChange -= player.spriteHeight;
                        break;
                    case MovementStatus.Right:
                        //gridPosition.X += player.spriteWidth;
                        //delta.X += player.spriteWidth;
                        //xChange += player.spriteHeight;
                        break;
                }
            }
            //if (map.Bounds.Contains(delta)) {
            //mapView = delta;
            //}
            //mapView.X += xChange;
            //mapView.Y += yChange;
            //gridPosition.X += xChange;
            //gridPosition.Y += yChange;
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch) {
            batch.Begin();
            map.Draw(batch, mapView, false);
            //batch.Draw(grid, gridPosition, Color.White);
            player.Draw(batch, Color.White);
            batch.End();
        }
    }
}
