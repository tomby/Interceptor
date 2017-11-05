using Micsun.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Interceptor
{
    public class Input
    {
        private const String TAG = "Input";
        private IntPtr context;
        private Thread callbackThread;

        /// <summary>
        /// Determines whether the driver traps no keyboard events, all events, or a range of events in-between (down only, up only...etc). Set this before loading otherwise the driver will not filter any events and no keypresses can be sent.
        /// </summary>
        public KeyboardFilterMode KeyboardFilterMode { get; set; }
        
        /// <summary>
        /// Determines whether the driver traps no events, all events, or a range of events in-between. Set this before loading otherwise the driver will not filter any events and no mouse clicks can be sent.
        /// </summary>
        public MouseFilterMode MouseFilterMode { get; set; }

        public bool IsLoaded { get; set; }

        /// <summary>
        /// Gets or sets the delay in milliseconds after each key stroke down and up. Pressing a key requires both a key stroke down and up. A delay of 0 (inadvisable) may result in no keys being apparently pressed. A delay of 20 - 40 milliseconds makes the key presses visible.
        /// </summary>
        public int KeyPressDelay { get; set; }

        /// <summary>
        /// Gets or sets the delay in milliseconds after each mouse event down and up. 'Clicking' the cursor (whether left or right) requires both a mouse event down and up. A delay of 0 (inadvisable) may result in no apparent click. A delay of 20 - 40 milliseconds makes the clicks apparent.
        /// </summary>
        public int ClickDelay { get; set; }

        public int ScrollDelay { get; set; }

        public event EventHandler<KeyPressedEventArgs> OnKeyPressed;
        public event EventHandler<MousePressedEventArgs> OnMousePressed;

        private int keyboardDeviceId=-1; /* Very important; which device the driver sends events to */
        private int mouseDeviceId=-1;

        public Input()
        {
            context = IntPtr.Zero;

            KeyboardFilterMode = KeyboardFilterMode.None;
            MouseFilterMode = MouseFilterMode.None;

            KeyPressDelay = 1;
            ClickDelay = 1;
            ScrollDelay = 15;
        }

        /*
         * Attempts to load the driver. You may get an error if the C++ library 'interception.dll' is not in the same folder as the executable and other DLLs. MouseFilterMode and KeyboardFilterMode must be set before Load() is called. Calling Load() twice has no effect if already loaded.
         */
        public bool Load()
        {
            if (IsLoaded) return false;

            context = InterceptionDriver.CreateContext();

            if (context != IntPtr.Zero)
            {
                callbackThread = new Thread(new ThreadStart(DriverCallback));
                callbackThread.Priority = ThreadPriority.Highest;
                callbackThread.IsBackground = true;
                callbackThread.Start();

                IsLoaded = true;

                return true;
            }
            else
            {
                IsLoaded = false;

                return false;
            }
        }

        /*
         * Safely unloads the driver. Calling Unload() twice has no effect.
         */
        public void Unload()
        {
            if (!IsLoaded) return;

            if (context != IntPtr.Zero)
            {
                callbackThread.Abort();
                InterceptionDriver.DestroyContext(context);
                IsLoaded = false;
            }
        }

        private void DriverCallback()
        {
            InterceptionDriver.SetFilter(context, InterceptionDriver.IsKeyboard, (Int32) KeyboardFilterMode);
            InterceptionDriver.SetFilter(context, InterceptionDriver.IsMouse, (Int32) MouseFilterMode);

            Stroke stroke = new Stroke();
            int deviceId = 0;
            while (InterceptionDriver.Receive(context, deviceId = InterceptionDriver.Wait(context), ref stroke, 1) > 0)
            {
                if (InterceptionDriver.IsMouse(deviceId) > 0)
                {
                    mouseDeviceId = deviceId;
                    if (OnMousePressed != null)
                    {
                        Log.Debug(TAG, "Mouse Device Detected:[" + deviceId + "] x:" + stroke.Mouse.X + ",y:" + stroke.Mouse.Y + " tag:" + stroke.Mouse.Flags + " state:" + stroke.Mouse.State);
                        var args = new MousePressedEventArgs() { X = stroke.Mouse.X, Y = stroke.Mouse.Y, State = stroke.Mouse.State, Rolling = stroke.Mouse.Rolling };
                        OnMousePressed(this, args);

                        if (args.Handled)
                        {
                            continue;
                        }
                        stroke.Mouse.X = args.X;
                        stroke.Mouse.Y = args.Y;
                        stroke.Mouse.State = args.State;
                        stroke.Mouse.Rolling = args.Rolling;

                    }
                }

                if (InterceptionDriver.IsKeyboard(deviceId) > 0)
                {
                    keyboardDeviceId = deviceId;
                    if (OnKeyPressed != null)
                    {
                        Log.Debug(TAG, "Keyboard Device Detected:[" + deviceId + "]");
                        var args = new KeyPressedEventArgs() { Key = stroke.Key.Code, State = stroke.Key.State};
                        OnKeyPressed(this, args);

                        if (args.Handled)
                        {
                            continue;
                        }
                        stroke.Key.Code = args.Key;
                        stroke.Key.State = args.State;
                    }
                }

                InterceptionDriver.Send(context, deviceId, ref stroke, 1);
            }

            Unload();
            throw new Exception("Interception.Receive() failed for an unknown reason. The driver has been unloaded.");
        }

        public void SendKey(Keys key, KeyState state)
        {
            Stroke stroke = new Stroke();
            KeyStroke keyStroke = new KeyStroke();

            keyStroke.Code = key;
            keyStroke.State = state;

            stroke.Key = keyStroke;

            InterceptionDriver.Send(context, keyboardDeviceId, ref stroke, 1);

            if (KeyPressDelay > 0)
                Thread.Sleep(KeyPressDelay);
        }

        /// <summary>
        /// Warning: Do not use this overload of SendKey() for non-letter, non-number, or non-ENTER keys. It may require a special KeyState of not KeyState.Down or KeyState.Up, but instead KeyState.E0 and KeyState.E1.
        /// </summary>
        public void SendKey(Keys key)
        {
            SendKey(key, KeyState.Down);

            if (KeyPressDelay > 0)
                Thread.Sleep(KeyPressDelay);

            SendKey(key, KeyState.Up);
        }

        public void SendKeys(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                SendKey(key);
            }
        }

        /// <summary>
        /// Warning: Only use this overload for sending letters, numbers, and symbols (those to the right of the letters on a U.S. keyboard and those obtained by pressing shift-#). Do not send special keys like Tab or Control or Enter.
        /// </summary>
        /// <param name="text"></param>
        public void SendText(string text)
        {
            foreach (char letter in text)
            {
                var tuple = KeyConvert.CharacterToKeysEnum(letter);

                if (tuple.Item2 == true) // We need to press shift to get the next character
                    SendKey(Keys.LeftShift, KeyState.Down);

                SendKey(tuple.Item1);

                if (tuple.Item2 == true)
                    SendKey(Keys.LeftShift, KeyState.Up);
            }
        }

 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void SendMouseEvent(int x,int y,MouseState state)
        {
            Stroke stroke = new Stroke();
            MouseStroke mouseStroke = new MouseStroke();

            mouseStroke.State = state;

            if (state == MouseState.ScrollUp)
            {
                mouseStroke.Rolling = 120;
            }
            else if (state == MouseState.ScrollDown)
            {
                mouseStroke.Rolling = -120;
            }

            stroke.Mouse = mouseStroke;

            Point point = ConvertDevicePoint(x, y);

            stroke.Mouse.X = point.X;
            stroke.Mouse.Y = point.Y;
            stroke.Mouse.Flags = MouseFlags.MoveAbsolute;

            InterceptionDriver.Send(context, mouseDeviceId, ref stroke, 1);
        }

        /// <summary>
        /// Change Screen location to Device location
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static Point ConvertDevicePoint(int x,int y)
        {
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            return new Point(0xFFFF * x / SW, 0xFFFF * y / SH);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void SendMouseEvent( MouseState state)
        {
            Stroke stroke = new Stroke();
            MouseStroke mouseStroke = new MouseStroke();

            mouseStroke.State = state;

            if (state == MouseState.ScrollUp)
            {
                mouseStroke.Rolling = 120;
            }
            else if (state == MouseState.ScrollDown)
            {
                mouseStroke.Rolling = -120;
            }

            stroke.Mouse = mouseStroke;
            InterceptionDriver.Send(context, mouseDeviceId, ref stroke, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendLeftClick()
        {
            SendMouseEvent(MouseState.LeftDown);
            Thread.Sleep(ClickDelay);
            SendMouseEvent(MouseState.LeftUp);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendRightClick()
        {
            SendMouseEvent(MouseState.RightDown);
            Thread.Sleep(ClickDelay);
            SendMouseEvent(MouseState.RightUp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        public void ScrollMouse(ScrollDirection direction)
        {
            switch (direction)
            { 
                case ScrollDirection.Down:
                    SendMouseEvent(MouseState.ScrollDown);
                    break;
                case ScrollDirection.Up:
                    SendMouseEvent(MouseState.ScrollUp);
                    break;
            }
        }

        /// <summary>
        /// Warning: This function, if using the driver, does not function reliably and often moves the mouse in unpredictable vectors. An alternate version uses the standard Win32 API to get the current cursor's position, calculates the desired destination's offset, and uses the Win32 API to set the cursor to the new position.
        /// </summary>
        public void MoveMouseBy(int deltaX, int deltaY, bool useDriver = false)
        {
            if (useDriver)
            {
                Stroke stroke = new Stroke();
                MouseStroke mouseStroke = new MouseStroke();

                Point point = ConvertDevicePoint(deltaX, deltaY);

                mouseStroke.X = point.X;
                mouseStroke.Y = point.Y;

                stroke.Mouse = mouseStroke;
                stroke.Mouse.Flags = MouseFlags.MoveRelative;

                InterceptionDriver.Send(context, mouseDeviceId, ref stroke, 1);
            }
            else
            {
                var currentPos = Cursor.Position;
                Cursor.Position = new Point(currentPos.X + deltaX, currentPos.Y - deltaY); // Coordinate system for y: 0 begins at top, and bottom of screen has the largest number
            }
        }

        /// <summary>
        /// Warning: This function, if using the driver, does not function reliably and often moves the mouse in unpredictable vectors. An alternate version uses the standard Win32 API to set the cursor's position and does not use the driver.
        /// </summary>
        public void MoveMouseTo(int x, int y, bool useDriver = false)
        {
            if (useDriver)
            {
                Stroke stroke = new Stroke();
                MouseStroke mouseStroke = new MouseStroke();

                Point point = ConvertDevicePoint(x, y);

                mouseStroke.X = point.X;
                mouseStroke.Y = point.Y;
                mouseStroke.State = 0;

                stroke.Mouse = mouseStroke;
                stroke.Mouse.Flags = MouseFlags.MoveAbsolute;
                InterceptionDriver.Send(context, mouseDeviceId, ref stroke, 1);
            }
            else
            {
                Cursor.Position = new Point(x, y);
            }
        }
    }
}
 
