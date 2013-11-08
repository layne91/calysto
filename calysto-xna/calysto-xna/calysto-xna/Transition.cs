using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace calysto_xna {
    class Transition {

        public string nextMapName;
        public Rectangle tRect;
        public int direction;
        public int xChange;
        public int yChange;
        public int xPlayer;
        public int yPlayer;

        public Transition(string nextMapName, Rectangle tRect, int direction, int xChange, int yChange, int xPlayer, int yPlayer) {
            this.nextMapName = nextMapName;
            this.tRect = tRect;
            this.direction = direction;
            this.xChange = xChange;
            this.yChange = yChange;
            this.xPlayer = xPlayer;
            this.yPlayer = yPlayer;
        }
    }
}
