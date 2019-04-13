using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mulix {

	public class Debug {
		private Debug() { }
		public enum Importance {
			INIT_INFO = 0,
			DRAW_INFO = 1,
			VALUE_INFO = 2,
			NOTIFICATION = 3,
			IMPORTANT_INFO = 4,
			WARN = 5,
			hid = 6,
			ERROR = 7
		}
		private static Importance Minimp = Importance.INIT_INFO;
		public static Importance MinImp { get => Minimp; set => Minimp = value; }

		public Debug(string invoker, object msg, Importance importance = (Importance) 0) {
			uint imp = (uint) importance;
			if( imp >= (uint) MinImp )
				Console.WriteLine($"[{importance.ToString( )}]( " + invoker + " ) " + msg.ToString( ));
		}
		public Debug(string invoker, string msg, Importance importance = (Importance) 0) {
			uint imp = (uint) importance;
			if( imp >= (uint) MinImp )
				Console.WriteLine($"[{importance.ToString( )}]( " + invoker + " ) " + msg);
		}
	}

	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Mulix : Game {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		private InputState inputs;

		// SCREEN
		private const int DEFRES = 800;
		private int[] useRes = new int[2] { DEFRES, DEFRES / 12 * 9 };
		private bool fullScreen = false;
		private bool borderLess = true;
		private bool changeRes = false;
		public int WIDTH { get { return this.useRes[0]; } set { if( changeRes ) this.useRes[0] = value; } }
		public int HEIGHT { get { return this.useRes[1]; } set { if( changeRes ) this.useRes[1] = value; } }
		public bool FULLSCREEN { get { return this.fullScreen; } set { if( changeRes ) this.fullScreen = value; } }
		public bool BORDERLESS { get { return this.borderLess; } set { if( changeRes ) this.borderLess = value; } }
		// SCREEN



		public Mulix() {
			Debug.MinImp = Debug.Importance.IMPORTANT_INFO;
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			// Show the mouse
			this.IsMouseVisible = true;

			// Setting default resolution settings
			this.graphics.PreferredBackBufferHeight = HEIGHT;
			this.graphics.PreferredBackBufferWidth = WIDTH;
			this.graphics.IsFullScreen = FULLSCREEN;
			Window.IsBorderless = BORDERLESS;
			Window.Position = new Point(ScreenCentre(WIDTH, 0).X, 0);

			// Setting default FPS settings
			this.IsFixedTimeStep = true;
			this.graphics.SynchronizeWithVerticalRetrace = true;
			this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 33); // Play at ~30.3FPS

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			// TODO: Add your initialization logic here

			this.inputs = new InputState( );

			base.Initialize( );
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			if( GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState( ).IsKeyDown(Keys.Escape) )
				Exit( );

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}

		public void ChangeGameResolution(int? w, int? h, bool? FS, bool? BL) {
			string pl = "Multris#ChangeGameResolution";
			new Debug(pl, "Changing Resolution with: {w:" + w + ",h:" + h + ",FS:" + FS + ",BL:" + BL + "}", Debug.Importance.IMPORTANT_INFO);
			this.changeRes = true;
			this.WIDTH = w ?? this.WIDTH;
			this.HEIGHT = h ?? this.HEIGHT;
			this.FULLSCREEN = FS ?? this.FULLSCREEN;
			this.BORDERLESS = BL ?? this.BORDERLESS;
			this.changeRes = false;
			new Debug("Multris#ChangeGameResolution", "Successfully changed resolution", Debug.Importance.NOTIFICATION);

			new Debug(pl, "Saving changes", Debug.Importance.INIT_INFO);
			// Save game res
			this.graphics.PreferredBackBufferHeight = HEIGHT;
			this.graphics.PreferredBackBufferWidth = WIDTH;
			this.graphics.IsFullScreen = FULLSCREEN;
			Window.IsBorderless = BORDERLESS;
			this.graphics.ApplyChanges( );
			new Debug(pl, "Successfully saved new resolution", Debug.Importance.NOTIFICATION);
		}

		public static Point ScreenCentre(int W, int H) {
			return new Point(
				( GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - W ) / 2,
				( GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - H ) / 2
			);
		}

		public Point WindowCentre() {
			return new Rectangle(0, 0, WIDTH, HEIGHT).Center;
		}

	}
}
