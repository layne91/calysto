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

        private const int BACKGROUND_LAYER = 0;
        private const int FOREGROUND_LAYER = 1;
        private const int TOP_LAYER = 2;

        private Map map;
        private Rectangle mapView;
        private Rectangle mapRect;
        private string mapName;
        private Player player;
        private List<Rectangle> cRectangles;
        private List<Transition> transitions;

        //Debug purposes
        private Texture2D collision;
        private Texture2D transition;

        public MapState(ContentManager Content, int PreferredBackBufferWidth, int PreferredBackBufferHeight, string mapName, Player player)
            : base(Content) {
            this.mapView = new Rectangle(0, 0, PreferredBackBufferWidth, PreferredBackBufferHeight);
            this.mapName = mapName;
            this.player = player;
        }

        public override void LoadContent() {
            map = Content.Load<Map>("maps/" + mapName);
            mapRect = new Rectangle(0, 0, map.Width * map.TileWidth, map.Height * map.TileHeight);
            loadCollisionRectangles();
            loadTransitionRectangles();

            //Debug purposes
            collision = Content.Load<Texture2D>("collision");
            transition = Content.Load<Texture2D>("transition");
        }

        private void loadCollisionRectangles() {
            cRectangles = new List<Rectangle>();
            int tileSize = map.TileWidth / 2;   //Not sure why dividing by 2
            foreach (TileData[] td in map.TileLayers[FOREGROUND_LAYER].Tiles) {
                foreach (TileData t in td) {
                    if (t != null) {
                        //Console.WriteLine(t.Target);
                        Rectangle cRect = t.Target;
                        cRect.X -= tileSize;
                        cRect.Y -= tileSize;
                        cRectangles.Add(cRect);
                    }
                }
            }
        }

        private void loadTransitionRectangles() {
            transitions = new List<Transition>();
            int tileSize = map.TileWidth / 2;   //Not sure why dividing by 2
            foreach(MapObject mo in map.ObjectLayers["Transition"].MapObjects){
                Rectangle tRect = mo.Bounds;
                Transition t = new Transition(mo.Name, tRect);
                transitions.Add(t);
            }
        }

        public override void UnloadContent() {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime) {
            player.Update(gameTime, mapRect, ref mapView, ref cRectangles, ref transitions);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch) {
            batch.Begin();
            map.DrawLayer(batch, BACKGROUND_LAYER, mapView, 0);
            map.DrawLayer(batch, FOREGROUND_LAYER, mapView, 0);
            player.Draw(batch, Color.White);
            map.DrawLayer(batch, TOP_LAYER, mapView, 0);
            foreach (Rectangle r in cRectangles) {
                batch.Draw(collision, r, Color.White);
            }
            foreach (Transition t in transitions) {
                batch.Draw(transition, t.tRect, Color.White);
            }
            batch.End();
        }
    }
}
