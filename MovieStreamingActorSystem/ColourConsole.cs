﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStreamingActorSystem
{
    public static class ColourConsole
    {
        public static void WriteGreenLine(string text)
        {
            var beforeColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = beforeColour;
        }

        public static void WriteBlueLine(string text)
        {
            var beforeColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(text);
            Console.ForegroundColor = beforeColour;
        }
        public static void WriteCyanLine(string text)
        {
            var beforeColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
            Console.ForegroundColor = beforeColour;
        }

        public static void WriteYellowLine(string text)
        {
            var beforeColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = beforeColour;
        }
        public static void WriteRedLine(string text)
        {
            var beforeColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = beforeColour;
        }
    }
}