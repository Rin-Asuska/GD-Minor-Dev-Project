using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project
{
    enum GM1State
    {
        Start,
        Playing,
        Paused,
        GameOver,
        Reset
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Main variables
        GM1State gameState;
        Boolean debugOn;
        Vector2 debugPos;

        //RNG
        public static readonly Random RNG = new Random();

        //DATA
        DATA gameData;

        //Player
        Player player;

        //Items
        List<LinearObject> Mines;
        List<PointItem> ScoreItems;
        List<Explosion> GOverBombs;

        float ScoreItemSpawnTime;
        const int ItemBASESPAWNTIME = 2;

        //Fonts
        SpriteFont InfoFont, MainFont, ScoreFont, MassiveFont;

        //TEXT
        Text TxtTitle, TxtStart, TxtPause, TxtGOver, TxtStartTutorial;
        TextScore TxtScore, TxtHighScore;

        //START SCREEN CONTROLLS
        StaticImage imgKeyboard, imgGamePad;

        //Controllers
        GamePadState pad1, oldpad1;
        KeyboardState key, oldkey;
        
        //MainMethod
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //=== SYSTEM INITIALIZE ===
        protected override void Initialize()
        {
            //Program Init
            gameState = GM1State.Start;

            Mines = new List<LinearObject>();
            ScoreItems = new List<PointItem>();
            GOverBombs = new List<Explosion>();

            //INIT STATE
            gameState = GM1State.Start;

            //Setup Game Data
            gameData = new DATA();

            //REMOVE THIS
            AddMine(10);

            //debugInfo
            debugOn = false;
            debugPos = new Vector2(0, 0);

            base.Initialize();
        }

        //=== LOAD CONTENT ===
        protected override void LoadContent()
        {
            //============================================================================//

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Player
            player = new Player(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, Color.DarkTurquoise, Content.Load<Texture2D>("Textures\\Ally\\Circle-Hollow-10px(PL)"));
            player.SetMoveBorders(0, GraphicsDevice.Viewport.Width, 0, GraphicsDevice.Viewport.Height);

            //Load Fonts
            InfoFont = Content.Load<SpriteFont>("Fonts\\InfoFont");
            MainFont = Content.Load<SpriteFont>("Fonts\\MainFont");
            ScoreFont = Content.Load<SpriteFont>("Fonts\\ScoreFont");
            MassiveFont = Content.Load<SpriteFont>("Fonts\\MassiveFont");

            //Setup All Text
            TxtScore = new TextScore((GraphicsDevice.Viewport.Width / 2) - (int)InfoFont.MeasureString("Score: 0000").X, 0, Color.Aqua, "Score: ", ScoreFont);
            TxtHighScore = new TextScore(-999, -999, Color.Aqua, "HighScore: ", ScoreFont);
            TxtHighScore.setAlignment(Alignment.Top, 0, GraphicsDevice);
            TxtGOver = new Text(0, 0, Color.Sienna, "       GAME OVER\n Press Any Key to Retry", MainFont);
            TxtGOver.setAlignment(Alignment.Center, 0, GraphicsDevice);

            TxtPause = new Text(-999, -999, Color.White, "         Game Paused\nPress Start/Space to Continue.", MainFont);
            TxtPause.setAlignment(Alignment.Center, 0, GraphicsDevice);

            TxtStart = new Text(-999, -999, Color.White, "Press Start/Space to to Start", MainFont);
            TxtStart.setAlignment(Alignment.Center, 0, GraphicsDevice);
            TxtStart.modifyPosition(0, 10);

            TxtStartTutorial = new Text(-999, -999, Color.White, "Collect Squares and Evade Red Things", InfoFont);
            TxtStartTutorial.setAlignment(Alignment.Center, 0, GraphicsDevice);
            TxtStartTutorial.modifyPosition(0, 35);
            
            TxtTitle = new Text(-999, -999, Color.LightGoldenrodYellow, "Diamond Collector", MassiveFont);
            TxtTitle.setAlignment(Alignment.Center, 0, GraphicsDevice);
            TxtTitle.modifyPosition(0, -40);

            //Load & Setup Start Screen Help
            imgKeyboard = new StaticImage(0, 0, Color.White, Content.Load<Texture2D>("Textures\\Tutorial\\Arrow-keys145"));
            imgKeyboard.setAlignment(Alignment.BottomRight, GraphicsDevice);
            imgGamePad = new StaticImage(0, 0, Color.White, Content.Load<Texture2D>("Textures\\Tutorial\\GamePad150"));
            imgGamePad.setAlignment(Alignment.BottomRight, GraphicsDevice);
            imgGamePad.modifyPosition(-imgKeyboard.getTexture().Width, 0);

            //============================================================================//
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //METHOD AddMine
        public void AddMine(int no2Create)
        {
            //============================================================================//
            float scale = (float)RNG.NextDouble(); //Gen Random Scale
            float scal = (float)RNG.NextDouble();
            if (scale < 0.5f)
                scale = 0.5f;
            if (scal > 0.5f)
                scal = 0.5f;
            scale = scale + scal;

            for (int i = 0; i < no2Create; i++ )
            {
                 Mines.Add(
                    new LinearObject(
                        0, 0,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height,
                        RNG, scale, Color.OrangeRed * 0.8f,
                        Content.Load<Texture2D>("Textures\\Enemy\\Circle-Hollow-Cross-10(Mi)")
                        )
                );
            }
            //============================================================================//
        }

        //METHOD AddScoreItem
        public void AddScoreItem(int no2Create, int pts)
        {
            //============================================================================//
            int rwPoint = pts; //Points Rewarded for player

            float scale = (float)RNG.NextDouble();
            float scal = (float)RNG.NextDouble();
            if (scale < 0.5f)
                scale = 0.5f;
            if (scal > 0.5f)
                scal = 0.5f;
            scale = scale + scal;

            for (int i = 0; i < no2Create; i++) //Create New Point Item
            {
                ScoreItems.Add(
                   new PointItem(
                        0, 0,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height,
                        rwPoint,
                        RNG, scale, Color.LightGoldenrodYellow * 0.8f,
                        Content.Load<Texture2D>("Textures\\Items\\Square-10px(pt)"),
                        Content.Load<Texture2D>("Textures\\Effects\\Circle-Hollow-60(Ef)")
                        )
               );
            }
            //============================================================================//
        }

        //METHOD GameOver Explosion
        public void NewExplosion(int no2Create, Color clr)
        {
            //============================================================================//
            float scale = (float)RNG.NextDouble();
            float scal = (float)RNG.NextDouble();
            if (scale < 0.5f)
                scale = 0.5f;
            if (scal > 0.5f)
                scal = 0.5f;
            scale = scale + scal;

            for (int i = 0; i < no2Create; i++)
            {
                GOverBombs.Add(
                   new Explosion(
                        (int)player.pos.X + (player.texture.Width / 2), (int)player.pos.Y + (player.texture.Height / 2), 0.10,
                       RNG, scale, clr * 0.6f,
                       Content.Load<Texture2D>("Textures\\Effects\\wongstock-8-bit-pixel-art-explosion-15")
                       )
               );
            }
            //============================================================================//
        }

        //METHOD General Explosion
        public void NewExplosion(int no2Create, int x, int y, int offsetRNG, Color clr)
        {
            //============================================================================//
            float scale = (float)RNG.NextDouble();
            float scal = (float)RNG.NextDouble();
            int posOffsetX, posOffsetY;

            if (scale < 0.5f)
                scale = 0.5f;
            if (scal > 0.5f)
                scal = 0.5f;
            scale = scale + scal;

            for (int i = 0; i < no2Create; i++)
            {
                posOffsetX = RNG.Next(-offsetRNG, offsetRNG);
                posOffsetY = RNG.Next(-offsetRNG, offsetRNG);

                GOverBombs.Add(
                   new Explosion(
                        x + posOffsetX, y + posOffsetY, 0.10,
                       RNG, scale, clr * 0.6f,
                       Content.Load<Texture2D>("Textures\\Effects\\wongstock-8-bit-pixel-art-explosion-15")
                       )
               );
            }
            //============================================================================//
        }

        //METHOD Return to Start Menu
        public void toStartScreen()
        {
            //============================================================================//
            Mines.Clear();
            ScoreItems.Clear();

            TxtHighScore.setAlignment(Alignment.Top, 0, GraphicsDevice);

            gameState = GM1State.Start;
            //============================================================================//
        }

        //CHECK ANY BUTTON GamePad
        public bool GamePadAnyKeyPressed(GamePadState gp)
        {
            if (gp.Buttons.A == ButtonState.Pressed
                || gp.Buttons.B == ButtonState.Pressed
                || gp.Buttons.X == ButtonState.Pressed
                || gp.Buttons.Y == ButtonState.Pressed
                || gp.Buttons.Back == ButtonState.Pressed
                || gp.Buttons.LeftShoulder == ButtonState.Pressed
                || gp.Buttons.LeftStick == ButtonState.Pressed
                || gp.Buttons.RightShoulder == ButtonState.Pressed
                || gp.Buttons.RightStick == ButtonState.Pressed
                || gp.Buttons.Start == ButtonState.Pressed
                )
            {
                return true;
            }
            else return false;
        }

        //MAIN >>>UPDATE<<< METHOD
        protected override void Update(GameTime gameTime)
        {
            //============================================================================//

            //Control Settings
            pad1 = GamePad.GetState(PlayerIndex.One);
            key = Keyboard.GetState();

            //----------------------------------------------------------------------------//

            #region Debug Controls
            //DebugOverlay Settings
            if (key.IsKeyDown(Keys.P) && !oldkey.IsKeyDown(Keys.P))
            {
                if (debugOn)
                    debugOn = false;
                else debugOn = true;
            }

            #endregion

            //----------------------------------------------------------------------------//

            #region Game Reset Fnc
            //Game RESET - StateReset
            if (gameState == GM1State.Reset)
            {
                player.ResetObj((GraphicsDevice.Viewport.Width / 2) - (player.texture.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (player.texture.Height / 2));
                player.score = 0;
                player.scoreMultiplier = 1;
                Mines.Clear();
                ScoreItems.Clear();

                TxtHighScore.setAlignment(Alignment.TopRight, 0, GraphicsDevice);
                TxtScore.setAlignment(Alignment.Top, 0, GraphicsDevice);

                //Finalised RESET
                gameState = GM1State.Playing;

                AddMine(10);
            }
            #endregion

            //----------------------------------------------------------------------------//

            switch (gameState) //=====// PARMARY GAME LOOP <<==============<<
            {
                //----------------------------------------------------------------------------//
                case GM1State.Start: //Primary Game - StateStart
                    #region Player Controls
                    //Keyboard
                    if (key.IsKeyDown(Keys.Space) && !oldkey.IsKeyDown(Keys.Space) /*key.GetPressedKeys().Length > 0 && key != oldkey*/)
                        gameState = GM1State.Reset;
                    //GAME EXIT
                    if (key.IsKeyDown(Keys.Escape) && !oldkey.IsKeyDown(Keys.Escape))
                        this.Exit();

                    //GamePad
                    if (pad1.IsConnected)
                    {
                        if (pad1.Buttons.Start == ButtonState.Pressed)
                            gameState = GM1State.Reset;

                        //GAME EXIT
                        if (pad1.Buttons.Back == ButtonState.Pressed && oldpad1.Buttons.Back != ButtonState.Pressed)
                            this.Exit();
                    }
                    #endregion Player Controls
                    break;
                //----------------------------------------------------------------------------//
                case GM1State.Playing: //PRIMARY GAME - StatePlaying
                    #region Player Controls
                    //Controls of Player
                    //Keyboard
                    #region NEW MOVE
                    bool hasMoved = false;

                    if (key.IsKeyDown(Keys.Up) || pad1.DPad.Up == ButtonState.Pressed)
                    {
                        player.UpdateObject(Move.Up);
                        hasMoved = true;
                    }
                    if (key.IsKeyDown(Keys.Down) || pad1.DPad.Down == ButtonState.Pressed)
                    {
                        player.UpdateObject(Move.Down);
                        hasMoved = true;
                    }
                    if (key.IsKeyDown(Keys.Left) || pad1.DPad.Left == ButtonState.Pressed)
                    {
                        player.UpdateObject(Move.Left);
                        hasMoved = true;
                    }
                    if (key.IsKeyDown(Keys.Right) || pad1.DPad.Right == ButtonState.Pressed)
                    {
                        player.UpdateObject(Move.Right);
                        hasMoved = true;
                    }
                    //GAME Return To Start (CONTROLLER BACK MOVED TO PAUSED ONLY)
                    if (key.IsKeyDown(Keys.Escape))
                        toStartScreen();
                    //Pause Game
                    if ((key.IsKeyDown(Keys.Space) && !oldkey.IsKeyDown(Keys.Space)) || (pad1.Buttons.Start == ButtonState.Pressed && oldpad1.Buttons.Start != ButtonState.Pressed))
                        gameState = GM1State.Paused;

                    //Move Thumbstick
                    if (((pad1.ThumbSticks.Left.X != 0.0f) || (pad1.ThumbSticks.Left.Y != 0.0f)) && (!hasMoved))
                    {
                            player.UpdateObject(pad1.ThumbSticks.Left.X, pad1.ThumbSticks.Left.Y);
                    }
                    #endregion NEW MOVE

                    #endregion

                    #region debug Controls
                    if (debugOn)
                    {
                        if (key.IsKeyDown(Keys.W))
                            AddMine(RNG.Next(1, 5));
                        if (key.IsKeyDown(Keys.A))
                            player.ResetObj(-99999, -99999);
                        if (key.IsKeyDown(Keys.R))
                            AddMine(100);
                        if (key.IsKeyDown(Keys.T))
                            NewExplosion(1, Color.White);
                        if (key.IsKeyDown(Keys.Q))
                            AddScoreItem(1, RNG.Next(10, 41));
                        if (key.IsKeyDown(Keys.F))
                            for (int i = 0; i < Mines.Count; i++) { Mines.RemoveAt(0); }
                        if (key.IsKeyDown(Keys.G))
                            for (int i = 0; i < ScoreItems.Count; i++) { ScoreItems.RemoveAt(0); }
                        if (key.IsKeyDown(Keys.Y) && !oldkey.IsKeyDown(Keys.Y))
                            gameState = GM1State.Reset;
                    }
                    #endregion

                    #region Update Mines
                    //Update Mines
                    for (int i = 0; i < Mines.Count; i++)
                    {
                        Mines[i].UpdateObject();

                        if (Mines[i].colSph.Intersects(player.colSph))
                            Mines[i].objState = FloatObjState.Hit;

                        #region newCode
                        //newCode
                        switch (Mines[i].objState)
                        {
                            case FloatObjState.Hit: //Object State Hit
                                Mines[i].objColor = Color.Beige;
                                Mines.RemoveAt(i);
                                gameState = GM1State.GameOver;
                                NewExplosion(RNG.Next(2, 6), Color.White);
                                break;
                            case FloatObjState.HitRemove: //Object State Hit By AOE Passive Effect
                                NewExplosion(10, (int)Mines[i].getPos().X, (int)Mines[i].getPos().Y, 3, Color.Azure);
                                Mines.RemoveAt(i);
                                break;
                            case FloatObjState.Standby: //Object State Standby
                                Mines.RemoveAt(i);
                                AddMine(RNG.Next(1, 5));
                                break;
                        }
                        #endregion

                    }
                    #endregion

                    #region Spawn Items
                    //Spawn Items
                    if (ScoreItemSpawnTime < 0) //Timed Item Spawn
                    {
                        int spawnAm = RNG.Next(0, RNG.Next(5, 10));

                        for (int i = 0; i < spawnAm; i++)
                        {
                            AddScoreItem(1, RNG.Next(10, 41));
                        }

                        spawnAm = RNG.Next(0, RNG.Next(1, 5));
                        AddMine(spawnAm);

                        ScoreItemSpawnTime = ItemBASESPAWNTIME + RNG.Next(0, RNG.Next(1, 6));
                    }
                    else
                    {
                        ScoreItemSpawnTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    #endregion

                    #region UpdateScoreItems
                    //Update ScoreItems
                    for (int i = 0; i < ScoreItems.Count; i++)
                    {
                        ScoreItems[i].UpdateObject();

                        if (ScoreItems[i].objState != FloatObjState.Hit && ScoreItems[i].objState != FloatObjState.FadeOut)
                        {
                            if (ScoreItems[i].colSph.Intersects(player.colSph))
                                ScoreItems[i].objState = FloatObjState.Hit;
                        }

                        #region newCode
                        //NEW CODE
                        switch (ScoreItems[i].objState)
                        {
                            case FloatObjState.Hit:

                                player.score += (int)(ScoreItems[i].rewardPTs * player.scoreMultiplier);
                                player.scoreMultiplier += 0.1f;

                                ScoreItems[i].objState = FloatObjState.FadeOut;
                                ScoreItems[i].isHitRemove();

                                //Set ALL Mines within new hitbox to STATE HitRemove
                                for (int j = 0; j < Mines.Count; j++)
                                {
                                    if (Mines[j].colSph.Intersects(ScoreItems[i].colSph))
                                        Mines[j].objState = FloatObjState.HitRemove;
                                }
                                //ScoreItems.RemoveAt(i);
                                break;
                            case FloatObjState.FadeOut:
                                ScoreItems[i].FadeUpdate(gameTime);
                                if (ScoreItems[i].objAlpha < 0)
                                    ScoreItems.RemoveAt(i);
                                break;
                            case FloatObjState.Standby:
                                ScoreItems.RemoveAt(i);
                                break;
                        }

                        #endregion newCode


                    }
                    #endregion

                    #region Score Update
                    if (player.score > gameData.HighScore)
                        gameData.HighScore = player.score;
                    #endregion

                    break;
                //END PRIMARY GAME - StatePlaying END
                //----------------------------------------------------------------------------//
                case GM1State.Paused: //State Paused
                    #region Primary Game - StatePaused
                        if (key.IsKeyDown(Keys.Space) && !oldkey.IsKeyDown(Keys.Space))
                            gameState = GM1State.Playing;
                        if (pad1.Buttons.Start == ButtonState.Pressed && oldpad1.Buttons.Start != ButtonState.Pressed)
                            gameState = GM1State.Playing;

                        if (pad1.Buttons.Back == ButtonState.Pressed)
                            toStartScreen();
                    #endregion

                    break;
                //----------------------------------------------------------------------------//
                case GM1State.GameOver: //State Game Over
                    #region Primary Game - StateGameOver
                    if (key.GetPressedKeys().Length > 0 && key != oldkey)
                        gameState = GM1State.Reset;

                    if (pad1.IsConnected)
                    {
                        if (GamePadAnyKeyPressed(pad1))
                            gameState = GM1State.Reset;
                        if (pad1.Buttons.Back == ButtonState.Pressed)
                            toStartScreen();
                    }
                    #endregion

                    break;
                //----------------------------------------------------------------------------//
            }

            //----------------------------------------------------------------------------//
            for (int i = 0; i < GOverBombs.Count; i++)
                GOverBombs[i].FadeUpdate(gameTime);

            //----------------------------------------------------------------------------//

            base.Update(gameTime);

            oldpad1 = pad1;
            oldkey = key;

            //END UPDATE METHOD
            //============================================================================//
        }

        //MAIN >>>DRAW<<< METHOD
        protected override void Draw(GameTime gameTime)
        {
            //============================================================================//

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //----------------------------------------------------------------------------//

            switch (gameState) //DRAW STUFF!!!
            {
                //----------------------------------------------------------------------------//
                case GM1State.Start: //STATE Start
                    //Draw Text Attract Screen
                    TxtStart.DrawObject(spriteBatch);
                    TxtTitle.DrawObject(spriteBatch);
                    TxtStartTutorial.DrawObject(spriteBatch);

                    //Draw Tutorial
                    imgKeyboard.DrawObject(spriteBatch);
                    imgGamePad.DrawObject(spriteBatch);
                    break;
                //----------------------------------------------------------------------------//
                case GM1State.Playing: //STATE Playing
                    //Draw Player(s)
                    player.DrawObject(spriteBatch);

                    //Draw Text Score Data
                    TxtScore.DrawObject(spriteBatch, player.score);
                    TxtHighScore.setAlignment(Alignment.TopRight, 0, gameData.HighScore, GraphicsDevice);
                    break;
                //----------------------------------------------------------------------------//
                case GM1State.Paused:
                    //Draw Player(s)
                    player.DrawObject(spriteBatch);

                    //Draw Text Score Data
                    TxtScore.DrawObject(spriteBatch, player.score);
                    TxtHighScore.setAlignment(Alignment.TopRight, 0, gameData.HighScore, GraphicsDevice);

                    //Draw Paused Text
                    TxtPause.DrawObject(spriteBatch);

                    break;
                //----------------------------------------------------------------------------//
                case GM1State.GameOver: //STATE GameOver
                    //GAMEOVER Text
                    TxtGOver.DrawObject(spriteBatch);

                    //Draw Text Score Data
                    TxtScore.DrawObject(spriteBatch, player.score);
                    TxtHighScore.setAlignment(Alignment.TopRight, 0, gameData.HighScore, GraphicsDevice);
                    break;
                //----------------------------------------------------------------------------//
            }

            //GLOBAL//\\ - Draw Mines
            for (int i = 0; i < Mines.Count; i++)
                Mines[i].DrawObject(spriteBatch);

            //GLOBAL//\\ - Draw ScoreItems
            for (int i = 0; i < ScoreItems.Count; i++)
                ScoreItems[i].DrawObject(spriteBatch);

            //GLOBAL//\\ - Draw Explosions
            for (int i = 0; i < GOverBombs.Count; i++)
                GOverBombs[i].DrawObject(spriteBatch);

            //GLOBAL//\\ - Draw HighScore
            TxtHighScore.DrawObject(spriteBatch, gameData.HighScore);

            //GLOBAL//\\ - DebugOverlay
            if (debugOn)
                spriteBatch.DrawString(ScoreFont, "DEBUG MODE ACTIVE" + "\nNo.RedObj  = " + Mines.Count + "\nNo.BlueObj = " + ScoreItems.Count + "\nRunTime    = " + gameTime.TotalGameTime.TotalMilliseconds /* + "\nFPS        = " + (1/ (float)gameTime.ElapsedGameTime.TotalSeconds).ToString("N0")*/, debugPos, Color.PaleGoldenrod);

            //----------------------------------------------------------------------------//
            spriteBatch.End();

            //----------------------------------------------------------------------------//

            base.Draw(gameTime);

            //============================================================================//
        }
    }
}
