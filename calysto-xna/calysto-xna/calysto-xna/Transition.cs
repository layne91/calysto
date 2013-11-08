using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace calysto_xna {
    class Transition {

        public string nextMapName;
        public Rectangle tRect;

        public Transition(string nextMapName, Rectangle tRect) {
            this.nextMapName = nextMapName;
            this.tRect = tRect;
        }
    }
}
