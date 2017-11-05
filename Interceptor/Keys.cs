using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interceptor
{
    /// <summary>
    /// A list of scancodes.
    /// </summary>
    /// <remarks>Scancodes change according to keyboard layout...so this may be inaccurate.</remarks>
    public enum Keys : ushort
    {
        Escape = 1,
        One = 2,
        Two = 3,
        Three = 4,
        Four = 5,
        Five = 6,
        Six = 7,
        Seven = 8,
        Eight = 9,
        Nine = 10,
        Zero = 11,
        DashUnderscore = 12,
        PlusEquals = 13,
        Backspace = 14,
        Tab = 15,
        Q = 16,
        W = 17,
        E = 18,
        R = 19,
        T = 20,
        Y = 21,
        U = 22,
        I = 23,
        O = 24,
        P = 25,
        OpenBracketBrace = 26,
        CloseBracketBrace = 27,
        Enter = 28,
        Control = 29,
        A = 30,
        S = 31,
        D = 32,
        F = 33,
        G = 34,
        H = 35,
        J = 36,
        K = 37,
        L = 38,
        SemicolonColon = 39,
        SingleDoubleQuote = 40,
        Tilde = 41,
        LeftShift = 42,
        BackslashPipe = 43,
        Z = 44,
        X = 45,
        C = 46,
        V = 47,
        B = 48,
        N = 49,
        M = 50,
        CommaLeftArrow = 51,
        PeriodRightArrow = 52,
        ForwardSlashQuestionMark = 53,
        RightShift = 54,
        RightAlt = 56,
        Space = 57,
        CapsLock = 58,
        F1 = 59,
        F2 = 60,
        F3 = 61,
        F4 = 62,
        F5 = 63,
        F6 = 64,
        F7 = 65,
        F8 = 66,
        F9 = 67,
        F10 = 68,
        F11 = 87,
        F12 = 88,
        Up = 72,
        Down = 80,
        Right = 77,
        Left = 75,
        Home = 71,
        End = 79,
        Delete = 83,
        PageUp = 73,
        PageDown = 81,
        Insert = 82,
        PrintScreen = 55, // And break key is 42
        NumLock = 69,
        ScrollLock = 70,
        Menu = 93,
        WindowsKey = 91,
        NumpadDivide = 53,
        NumpadAsterisk = 55,
        Numpad7 = 71,
        Numpad8 = 72,
        Numpad9 = 73,
        Numpad4 = 75,
        Numpad5 = 76,
        Numpad6 = 77,
        Numpad1 = 79,
        Numpad2 = 80,
        Numpad3 = 81,
        Numpad0 = 82,
        NumpadDelete = 83,
        NumpadEnter = 28,
        NumpadPlus = 78,
        NumpadMinus = 74,
    }

    public class KeyConvert{
        /// <summary>
        /// Converts a character to a Keys enum and a 'do we need to press shift'.
        /// </summary>
        public static Tuple<Keys, bool> CharacterToKeysEnum(char c)
        {
            switch (Char.ToLower(c))
            {
                case 'a':
                    return new Tuple<Keys, bool>(Keys.A, false);
                case 'b':
                    return new Tuple<Keys, bool>(Keys.B, false);
                case 'c':
                    return new Tuple<Keys, bool>(Keys.C, false);
                case 'd':
                    return new Tuple<Keys, bool>(Keys.D, false);
                case 'e':
                    return new Tuple<Keys, bool>(Keys.E, false);
                case 'f':
                    return new Tuple<Keys, bool>(Keys.F, false);
                case 'g':
                    return new Tuple<Keys, bool>(Keys.G, false);
                case 'h':
                    return new Tuple<Keys, bool>(Keys.H, false);
                case 'i':
                    return new Tuple<Keys, bool>(Keys.I, false);
                case 'j':
                    return new Tuple<Keys, bool>(Keys.J, false);
                case 'k':
                    return new Tuple<Keys, bool>(Keys.K, false);
                case 'l':
                    return new Tuple<Keys, bool>(Keys.L, false);
                case 'm':
                    return new Tuple<Keys, bool>(Keys.M, false);
                case 'n':
                    return new Tuple<Keys, bool>(Keys.N, false);
                case 'o':
                    return new Tuple<Keys, bool>(Keys.O, false);
                case 'p':
                    return new Tuple<Keys, bool>(Keys.P, false);
                case 'q':
                    return new Tuple<Keys, bool>(Keys.Q, false);
                case 'r':
                    return new Tuple<Keys, bool>(Keys.R, false);
                case 's':
                    return new Tuple<Keys, bool>(Keys.S, false);
                case 't':
                    return new Tuple<Keys, bool>(Keys.T, false);
                case 'u':
                    return new Tuple<Keys, bool>(Keys.U, false);
                case 'v':
                    return new Tuple<Keys, bool>(Keys.V, false);
                case 'w':
                    return new Tuple<Keys, bool>(Keys.W, false);
                case 'x':
                    return new Tuple<Keys, bool>(Keys.X, false);
                case 'y':
                    return new Tuple<Keys, bool>(Keys.Y, false);
                case 'z':
                    return new Tuple<Keys, bool>(Keys.Z, false);
                case '1':
                    return new Tuple<Keys, bool>(Keys.One, false);
                case '2':
                    return new Tuple<Keys, bool>(Keys.Two, false);
                case '3':
                    return new Tuple<Keys, bool>(Keys.Three, false);
                case '4':
                    return new Tuple<Keys, bool>(Keys.Four, false);
                case '5':
                    return new Tuple<Keys, bool>(Keys.Five, false);
                case '6':
                    return new Tuple<Keys, bool>(Keys.Six, false);
                case '7':
                    return new Tuple<Keys, bool>(Keys.Seven, false);
                case '8':
                    return new Tuple<Keys, bool>(Keys.Eight, false);
                case '9':
                    return new Tuple<Keys, bool>(Keys.Nine, false);
                case '0':
                    return new Tuple<Keys, bool>(Keys.Zero, false);
                case '-':
                    return new Tuple<Keys, bool>(Keys.DashUnderscore, false);
                case '+':
                    return new Tuple<Keys, bool>(Keys.PlusEquals, false);
                case '[':
                    return new Tuple<Keys, bool>(Keys.OpenBracketBrace, false);
                case ']':
                    return new Tuple<Keys, bool>(Keys.CloseBracketBrace, false);
                case ';':
                    return new Tuple<Keys, bool>(Keys.SemicolonColon, false);
                case '\'':
                    return new Tuple<Keys, bool>(Keys.SingleDoubleQuote, false);
                case ',':
                    return new Tuple<Keys, bool>(Keys.CommaLeftArrow, false);
                case '.':
                    return new Tuple<Keys, bool>(Keys.PeriodRightArrow, false);
                case '/':
                    return new Tuple<Keys, bool>(Keys.ForwardSlashQuestionMark, false);
                case '{':
                    return new Tuple<Keys, bool>(Keys.OpenBracketBrace, true);
                case '}':
                    return new Tuple<Keys, bool>(Keys.CloseBracketBrace, true);
                case ':':
                    return new Tuple<Keys, bool>(Keys.SemicolonColon, true);
                case '\"':
                    return new Tuple<Keys, bool>(Keys.SingleDoubleQuote, true);
                case '<':
                    return new Tuple<Keys, bool>(Keys.CommaLeftArrow, true);
                case '>':
                    return new Tuple<Keys, bool>(Keys.PeriodRightArrow, true);
                case '?':
                    return new Tuple<Keys, bool>(Keys.ForwardSlashQuestionMark, true);
                case '\\':
                    return new Tuple<Keys, bool>(Keys.BackslashPipe, false);
                case '|':
                    return new Tuple<Keys, bool>(Keys.BackslashPipe, true);
                case '`':
                    return new Tuple<Keys, bool>(Keys.Tilde, false);
                case '~':
                    return new Tuple<Keys, bool>(Keys.Tilde, true);
                case '!':
                    return new Tuple<Keys, bool>(Keys.One, true);
                case '@':
                    return new Tuple<Keys, bool>(Keys.Two, true);
                case '#':
                    return new Tuple<Keys, bool>(Keys.Three, true);
                case '$':
                    return new Tuple<Keys, bool>(Keys.Four, true);
                case '%':
                    return new Tuple<Keys, bool>(Keys.Five, true);
                case '^':
                    return new Tuple<Keys, bool>(Keys.Six, true);
                case '&':
                    return new Tuple<Keys, bool>(Keys.Seven, true);
                case '*':
                    return new Tuple<Keys, bool>(Keys.Eight, true);
                case '(':
                    return new Tuple<Keys, bool>(Keys.Nine, true);
                case ')':
                    return new Tuple<Keys, bool>(Keys.Zero, true);
                case ' ':
                    return new Tuple<Keys, bool>(Keys.Space, true);
                default:
                    return new Tuple<Keys, bool>(Keys.ForwardSlashQuestionMark, true);
            }
        }

    }
}
