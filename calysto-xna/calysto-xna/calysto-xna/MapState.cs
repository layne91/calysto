﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FuncWorks.XNA.XTiled;

namespace calysto_xna {
    class MapState : GameState {

        private bool DEBUG = true;

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

        //Constructor for loading a starting map (Map with a Load object layer)
        public MapState(ContentManager Content, int PreferredBackBufferWidth, int PreferredBackBufferHeight, string mapName, Player player)
            : base(Content) {
            this.mapView = new Rectangle(0, 0, PreferredBackBufferWidth, PreferredBackBufferHeight);
            this.mapName = mapName;
            this.player = player;
            LoadContent();
        }

        //Constructor when loading from a transition collision
        public MapState(ContentManager Content, int PreferredBackBufferWidth, int PreferredBackBufferHeight, string mapName, Player player, Transition transition) 
            : this(Content, PreferredBackBufferWidth, PreferredBackBufferHeight, mapName, player) {
            int xChange = transition.xChange;
            int yChange = transition.yChange;

            //Move camera
            mapView.X += xChange;
            mapView.Y += yChange;

            //Place player location
            player.objectRectangle.X = transition.xPlayer;
            player.objectRectangle.Y = transition.yPlayer;

            //Move collision rectangles
            for (int i = 0; i < cRectangles.Count; i++) {
                cRectangles[i] = new Rectangle(cRectangles[i].X - xChange, cRectangles[i].Y - yChange, cRectangles[i].Width, cRectangles[i].Height);
            } 

            //Move transition rectangles
            for (int i = 0; i < transitions.Count; i++) {
                transitions[i].tRect = new Rectangle(transitions[i].tRect.X - xChange, transitions[i].tRect.Y - yChange, transitions[i].tRect.Width, transitions[i].tRect.Height);
            }

            //Set player direction
            player.movementStatus = (MovementStatus)transition.direction;
        }

        //NOTE: this actually gets loaded twice when a the first map get's called. 
        public override void LoadContent() {
            map = Content.Load<Map>("maps/" + mapName);
            mapRect = new Rectangle(0, 0, map.Width * map.TileWidth, map.Height * map.TileHeight);
            loadCollisionRectangles();
            loadTransitionRectangles();
            loadMapInfo();

            //Debug purposes
            collision = Content.Load<Texture2D>("collision");
            transition = Content.Load<Texture2D>("transition");
        }

        private void loadMapInfo() {
            if (map.ObjectLayers["Load"] == null)
                return;
            foreach (MapObject mo in map.ObjectLayers["Load"].MapObjects) {
                if (mo.Name == "Spawn") {
                    int xChange = (int)mo.Properties["xChange"].AsInt32 * 2;
                    int yChange = (int)mo.Properties["yChange"].AsInt32 * 2;

                    //Move camera first
                    mapView.X += xChange / 2;   //Divide by 2 to account for one side difference only
                    mapView.Y += yChange / 2;
                    
                    //Place player location
                    player.objectRectangle.X = mo.Bounds.X - xChange;
                    player.objectRectangle.Y = mo.Bounds.Y - yChange;

                    //Move collision rectangles
                    for (int i = 0; i < cRectangles.Count; i++) {
                        cRectangles[i] = new Rectangle(cRectangles[i].X - xChange, cRectangles[i].Y - yChange, cRectangles[i].Width, cRectangles[i].Height);
                    } 
                    
                    //Move transition rectangles
                    for (int i = 0; i < transitions.Count; i++) {
                        transitions[i].tRect = new Rectangle(transitions[i].tRect.X - xChange, transitions[i].tRect.Y - yChange, transitions[i].tRect.Width, transitions[i].tRect.Height);
                    }

                    player.movementStatus = (MovementStatus)mo.Properties["direction"].AsInt32;
                }
            }
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
                int direction = (int)mo.Properties["d"].AsInt32;
                int xChange = (int)mo.Properties["cx"].AsInt32;
                int yChange = (int)mo.Properties["cy"].AsInt32;
                int xPlayer = (int)mo.Properties["x"].AsInt32;
                int yPlayer = (int)mo.Properties["y"].AsInt32;
                Transition t = new Transition(mo.Name, tRect, direction, xChange, yChange, xPlayer, yPlayer);
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

            if (DEBUG) {
                foreach (Rectangle r in cRectangles) {
                    batch.Draw(collision, r, Color.White);
                }
                foreach (Transition t in transitions) {
                    batch.Draw(transition, t.tRect, Color.White);
                }
            }
            batch.End();
        }
    }
}
