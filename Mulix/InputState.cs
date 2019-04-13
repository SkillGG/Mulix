using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mulix {

	public enum MouseButton {
		RIGHT,
		LEFT,
		MIDDLE,
		NONE
	}

	public class MouseClick {

		private MouseButton btn;
		private Point pnt;

		public MouseButton button { get { return this.btn; } }
		public Point point { get { return this.pnt; } }

		private MouseClick() { }

		public MouseClick(Point p, MouseButton b) {
			this.pnt = p;
			this.btn = b;
		}

		public MouseClick(int x, int y, MouseButton b) {
			this.pnt = new Point(x, y);
			this.btn = b;
		}

	}

	public class InputState {

		public MouseState mouse;
		public KeyboardState keyboard;
		public GamePadState gamePad1;
		public GamePadState gamePad2;

		public Rectangle MouseRectangle {
			get {
				return new Rectangle(mouse.Position, new Point(1, 1));
			}
		}

		public InputState() {
			this.mouse = Mouse.GetState( );
			this.keyboard = Keyboard.GetState( );
			this.gamePad1 = GamePad.GetState(PlayerIndex.One);
			this.gamePad2 = GamePad.GetState(PlayerIndex.Two);
		}

		public void Update() {
			this.mouse = Mouse.GetState( );
			this.keyboard = Keyboard.GetState( );
			this.gamePad1 = GamePad.GetState(0);
			this.gamePad2 = GamePad.GetState(1);
		}


		// KEYS

		public bool IsKeyDown(Keys k) { return this.keyboard.IsKeyDown(k); }
		public bool IsKeyUp(Keys k) { return this.keyboard.IsKeyUp(k); }

		// KeyChange
		public bool KeyChange(InputState b, Keys k) {
			this.Update( );
			if( b.keyboard.IsKeyDown(k) != this.keyboard.IsKeyDown(k) )
				return true;
			return false;
		}

		// KeyUp
		public bool KeyUp(InputState b, Keys k) {
			if( KeyChange(b, k) )
				if( IsKeyUp(k) )
					return true;
			return false;
		}

		// KeyDown
		public bool KeyDown(InputState b, Keys k) {
			if( KeyChange(b, k) )
				if( IsKeyDown(k) )
					return true;
			return false;
		}


		// BUTTONS

		// IsButtonDown
		public byte IsButtonDown(Buttons k) {
			this.Update( );
			if( gamePad1.IsButtonDown(k) ) {
				if( gamePad2.IsButtonDown(k) )
					return 3;
				return 1;
			} else if( gamePad2.IsButtonDown(k) )
				return 2;
			return 0;
		}
		public bool IsButtonDownAny(Buttons k) {
			return ( IsButtonDown(k) != 0 );
		}

		// IsButtonUp
		public byte IsButtonUp(Buttons k) {
			this.Update( );
			if( gamePad1.IsButtonUp(k) ) {
				if( gamePad2.IsButtonUp(k) )
					return 3;
				return 1;
			} else if( gamePad2.IsButtonUp(k) )
				return 2;
			return 0;
		}
		public bool IsButtonUpAny(Buttons k) {
			return ( IsButtonUp(k) != 0 );
		}

		// Button Change
		public byte ButtonChange(InputState b, Buttons k) {
			this.Update( );
			if( b.gamePad1.IsButtonDown(k) != this.gamePad1.IsButtonDown(k) ) {
				if( b.gamePad2.IsButtonDown(k) != this.gamePad2.IsButtonDown(k) )
					return 3;
				return 1;
			}
			if( b.gamePad2.IsButtonDown(k) != this.gamePad2.IsButtonDown(k) )
				return 2;
			return 0;
		}
		public bool ButtonChangeAny(InputState b, Buttons k) {
			return ( ButtonChange(b, k) != 0 );
		}

		// ButtonDown
		public byte ButtonDown(InputState b, Buttons k) {
			byte bc = ButtonChange(b, k);
			if( bc != 0 && bc == IsButtonDown(k) )
				return bc;
			return 0;
		}
		public bool ButtonDownAny(InputState b, Buttons k) {
			if( ButtonChangeAny(b, k) )
				if( IsButtonDownAny(k) )
					return true;
			return false;
		}

		// ButtonUp
		public byte ButtonUp(InputState b, Buttons k) {
			byte bc = ButtonChange(b, k);
			if( bc != 0 && bc == IsButtonUp(k) )
				return bc;
			return 0;
		}
		public bool ButtonUpAny(InputState b, Buttons k) {
			if( ButtonChangeAny(b, k) )
				if( IsButtonUpAny(k) )
					return true;
			return false;
		}

		/// <summary>
		/// A function that given previous InputState checks if either Right/Left/Middle mouse Button was clicked.
		/// </summary>
		/// <param name="b">Previous InputState (before .Update())</param>
		/// <returns>
		/// MouseClick object that contains every information about given click.
		/// </returns>
		public MouseClick MouseClicked(InputState b) {
			this.Update( );
			if( b.mouse.LeftButton == ButtonState.Released && this.mouse.LeftButton == ButtonState.Pressed )
				return new MouseClick(mouse.X, mouse.Y, MouseButton.LEFT);
			if( b.mouse.RightButton == ButtonState.Released && this.mouse.RightButton == ButtonState.Pressed )
				return new MouseClick(mouse.X, mouse.Y, MouseButton.RIGHT);
			if( b.mouse.MiddleButton == ButtonState.Released && this.mouse.MiddleButton == ButtonState.Pressed )
				return new MouseClick(mouse.X, mouse.Y, MouseButton.MIDDLE);
			return new MouseClick(0, 0, MouseButton.NONE); // No change in state appeared
		}

		/// <summary>
		/// A function that given previous InputState checks if either Right/Left/Middle mouse Button was released.
		/// </summary>
		/// <param name="b">Previous InputState (before .Update())</param>
		/// <returns>
		/// MouseClick object that contains every information about given click.
		/// </returns>
		public MouseClick MouseReleased(InputState b) {
			this.Update( );
			if( b.mouse.LeftButton == ButtonState.Pressed && this.mouse.LeftButton == ButtonState.Released )
				return new MouseClick(mouse.X, mouse.Y, MouseButton.LEFT);
			if( b.mouse.RightButton == ButtonState.Pressed && this.mouse.RightButton == ButtonState.Released )
				return new MouseClick(mouse.X, mouse.Y, MouseButton.RIGHT);
			if( b.mouse.MiddleButton == ButtonState.Pressed && this.mouse.MiddleButton == ButtonState.Released )
				return new MouseClick(mouse.X, mouse.Y, MouseButton.MIDDLE);
			return new MouseClick(0, 0, MouseButton.NONE); // No change in state appeared
		}

	}
}