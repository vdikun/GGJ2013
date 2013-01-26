using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IAT236P.UI
{
    public class UIElement
    {
        private Vector2 mPosition;
        private Texture2D mTexture;
        private string mText;

        public UIElement(Texture2D texture, string text)
        {
            this.mTexture = texture;
            this.mText = text;
        }

        public Vector2 Position { get { return this.mPosition; } set { this.mPosition = value; } }
        public Texture2D Texture { get { return this.mTexture; } set { this.mTexture = value; } }
        public string Text { get { return this.mText; } set { this.mText = value; } }
    }
}
