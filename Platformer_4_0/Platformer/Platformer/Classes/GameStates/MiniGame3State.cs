using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Platformer
{
    class MiniGame3State : GameState
    {
        static Texture2D heartTexture;
        static Texture2D bugTexture;
        static SoundEffect music;

        Vector2 heartPosition = new Vector2(240, 240);
        List<Vector2> bugPositions = new List<Vector2>();
        List<Vector2> bugTargetPositions = new List<Vector2>();

        bool attackedByBugs;
        float timer = 0F;
        bool stopUpdate = false;

        int countdown;
        int frameCounter;
        int quitTimer = 40;

        // constants
        private const int BUG_SPEED = 5;
        private const int MAX_NUMBER_BUGS = 3;
        private const float TIMER = 4.0F;

        public MiniGame3State()
        {
            PlatformerGame.minigameMusic = music;
            if (PlatformerGame.musicLoop != null) PlatformerGame.musicLoop.Stop();

            attackedByBugs = false;

            // generate bugs
            Random random = new Random();
            int numberBugs = random.Next(2, MAX_NUMBER_BUGS + 1);
            for (int i = 0; i < numberBugs; i++)
            {
                bugPositions.Add(new Vector2(random.Next(0, 1280), random.Next(0, 720)));
                bugTargetPositions.Add(new Vector2(random.Next(0, 1280), random.Next(0, 720)));

            }
            countdown = 4;
            frameCounter = 30;

        }

        public static void LoadContent(ContentManager manager)
        {
            bugTexture = manager.Load<Texture2D>("Minigames/Roach");
            heartTexture = manager.Load<Texture2D>("Minigames/heart");

        }

        void GameState.Update(PlatformerGame game, GameTime gameTime)
        {
            if (!stopUpdate)
            {
                if (game.keyboard.IsKeyDown(Keys.Escape))
                {
                    game.currentState = new MenuState();
                }
                if (countdown != 0)
                {
                    frameCounter--;
                    if (frameCounter == 0) { countdown--; frameCounter = 40; }
                }
                else
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // Update heart position according to mouse position
                    MouseState currentMouseState = new MouseState();
                    currentMouseState = Mouse.GetState();
                    float posDiff = currentMouseState.X - heartPosition.X;
                    heartPosition.X += posDiff * 0.1f;
                    if (heartPosition.X < 0) heartPosition.X = 0;
                    if (heartPosition.X > 1280 - heartTexture.Width) heartPosition.X = 1280 - heartTexture.Width;
                    posDiff = currentMouseState.Y - heartPosition.Y;
                    heartPosition.Y += posDiff * 0.1f;
                    if (heartPosition.Y < 0) heartPosition.Y = 0;
                    if (heartPosition.Y > 720 - heartTexture.Height) heartPosition.Y = 720 - heartTexture.Height;

                    // bug moving randomly towards heart
                    for (int i = 0; i < bugPositions.Count; i++)
                    {
                        Vector2 bug = new Vector2(bugPositions[i].X, bugPositions[i].Y);
                        Vector2 bugTarget = new Vector2(bugTargetPositions[i].X, bugTargetPositions[i].Y);
                        Random random = new Random();

                        if (bug.X < (bugTargetPositions[i].X + BUG_SPEED) && bug.X > (bugTargetPositions[i].X - BUG_SPEED))
                            bugTarget.X = random.Next(-bugTexture.Width, (1280 - bugTexture.Width));
                        if (bug.Y < (bugTargetPositions[i].Y + BUG_SPEED) && bug.Y > (bugTargetPositions[i].Y - BUG_SPEED))
                            bugTarget.Y = random.Next(-bugTexture.Height, (720 - bugTexture.Height));
                        if (bug.X < bugTargetPositions[i].X) bug.X += BUG_SPEED;
                        if (bug.X > bugTargetPositions[i].X) bug.X -= BUG_SPEED;
                        if (bug.Y < bugTargetPositions[i].Y) bug.Y += BUG_SPEED;
                        if (bug.Y > bugTargetPositions[i].Y) bug.Y -= BUG_SPEED;

                        bugPositions[i] = bug;
                        bugTargetPositions[i] = bugTarget;
                    }

                    // check if heart touches any bug
                    foreach (Vector2 bug in bugPositions)
                    {
                        float x = heartPosition.X + 70;
                        float y = heartPosition.Y + 120;

                        if (x < (bug.X + 30) && x > (bug.X - 30) && y < (bug.Y + 50) && y > (bug.Y - 50))
                            attackedByBugs = true;
                    }
                }
            }
        }

        void GameState.Draw(PlatformerGame game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.font, "Mini Game 3 State", new Vector2(10, 10), Color.White);
            if (countdown == 4)
            {
                spriteBatch.DrawString(game.font, "AVOID", new Vector2(480, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            }
            else if (countdown > 0)
            {
                spriteBatch.DrawString(game.font, "" + countdown, new Vector2(630, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            }

            if (attackedByBugs)
            {
                spriteBatch.DrawString(game.font, "FAIL!", new Vector2(450, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            }
            else if (timer >= TIMER)
            {
                spriteBatch.DrawString(game.font, "GOOD!", new Vector2(450, 100), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
                stopUpdate = true;
                quitTimer--;
                if (quitTimer == 0)
                {
                    Util.gamesWon++;
                    if (Util.gamesWon < Util.GAMES_TO_WIN)
                    {
                        Util.GotoRandomMinigame(game);
                    }
                    else
                    {
                        game.currentState = new CutsceneState();
                    }
                }
            }
            else
            {
                foreach (Vector2 bug in bugPositions)
                {
                    spriteBatch.Draw(bugTexture, bug, Color.White);
                }
                spriteBatch.Draw(heartTexture, heartPosition, Color.White);
            }
        }

    }
}
