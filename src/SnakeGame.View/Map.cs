﻿using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Helpers;
using SnakeGame.View.ExternalInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SnakeGame.View.Extensions;
using SnakeGame.View.Frames;

namespace SnakeGame.View
{
    public class Map : IMap
    {
        private char BorderSymbol { get; }
        private ConsoleColor BorderColor { get; }
        private IMapConfiguration MapConfiguration { get; }
        private string[] LastText {get; set;}

        private const int LeftIndent = 0;
        private const int TopIndent = 1;
        private const int BorderWidth = 1;

        public Map(IMapConfiguration mapConfiguration, ConsoleColor borderColor, char borderSymbol)
        {
            MapConfiguration = mapConfiguration;

            BorderColor = borderColor;
            BorderSymbol = borderSymbol;
            LastText = Array.Empty<string>();
        }

        public void Clear(IEnumerable<IFrameObject> clearedFrame)
        {
            #region Check for null

            NullHandlingHelper.ExternalCheckForNull<IEnumerable<IFrameObject>>(clearedFrame);

            #endregion

            var emptySymbol = ' ';

            clearedFrame
                .SelectMany(frameObject => frameObject.Location)
                .ToList()
                .ForEach(point => DrawPointOn(point, emptySymbol, default));
        }

        public void DrawMap()
        {
            var lastColor = Console.ForegroundColor;

            Console.ForegroundColor = BorderColor;
            try
            {
                for (int x = 0; x < MapConfiguration.Width + (BorderWidth * 2); x++)
                {
                    Console.CursorLeft = x + LeftIndent;
                    Console.CursorTop = TopIndent;
                    Console.Write(BorderSymbol);

                    Console.CursorLeft = x + LeftIndent;
                    Console.CursorTop = MapConfiguration.Height + TopIndent + BorderWidth;
                    Console.Write(BorderSymbol);

                }

                for (int y = 0; y < MapConfiguration.Height + (BorderWidth * 2); y++)
                {
                    Console.CursorLeft = LeftIndent;
                    Console.CursorTop = y + TopIndent;
                    Console.Write(BorderSymbol);

                    Console.CursorLeft = MapConfiguration.Width + LeftIndent + BorderWidth;
                    Console.CursorTop = y + TopIndent;
                    Console.Write(BorderSymbol);
                }
            }
            catch(ArgumentOutOfRangeException){ }
            finally
            {
                Console.ForegroundColor = lastColor;
            }
        }

        public void DrawPointOn(PositivePoint point, char presentationSymbol, ConsoleColor color)
        {
            var lastColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            try
            {
                Console.CursorTop = point.Y + TopIndent + BorderWidth;
                Console.CursorLeft = point.X + LeftIndent + BorderWidth;
                Console.Write(presentationSymbol);
            }
            catch (ArgumentOutOfRangeException) { }
            finally
            {
                Console.ForegroundColor = lastColor;
            }
        }

        public void DrawLineOverMap(string line)
        {
            #region Check for null

            NullHandlingHelper.ExternalCheckForNull<string>(line);

            #endregion

            int overMapLineNumber = TopIndent - 1;

            ClearLine(overMapLineNumber);

            DrawCenteredLine(line, overMapLineNumber);
        }

        public void DrawText(string text)
        {
            #region Check for null

            NullHandlingHelper.ExternalCheckForNull<string>(text);

            #endregion

            ClearLastText();

            string[] lines = text.Split('\n');

            if (lines.Length > MapConfiguration.Height)
            {
                var message = $"Too many lines in the {nameof(text)} - " +
                              $"\n{text}," +
                              $"\nPlease, reduce the lines number." +
                              $"\nActual map height - {MapConfiguration.Height}." +
                              $"\nActual number of lines - {text.Length}.";
                throw new ArgumentException(message);
            }

            var firstLineMustBeOn = CalculateFirstLine(lines);

            int lineNumber = firstLineMustBeOn;
            foreach (string line in lines)
            {
                DrawCenteredLine(line, lineNumber);

                lineNumber++;
            }

            LastText = lines;
        }

        private int CalculateFirstLine(string[] lines)
        {
            int centerLineNumber = (MapConfiguration.Height + BorderWidth * 2) / 2 + TopIndent;
            int firstLineMustBeOn = centerLineNumber - (lines.Length / 2);
            return firstLineMustBeOn;
        }

        private void DrawCenteredLine(string line, int numberOfLine)
        {
            #region Check value and for null

            NullHandlingHelper.InternalCheckForNull<string>(line);
            Debug.Assert(numberOfLine >= 0, nameof(numberOfLine));

            #endregion

            string print = line.SubstringIfPossible(0, MapConfiguration.Width);

            int centerY = (MapConfiguration.Width / 2) + LeftIndent + BorderWidth;

            int space = centerY - (print.Length / 2);

            Console.CursorLeft = space;
            Console.CursorTop = numberOfLine;
            Console.Write(line);
        }

        private void ClearLastText()
        {
            int startLine = CalculateFirstLine(LastText);

            for (int counter = 0; counter < LastText.Length; counter++)
            {
                ClearLine(startLine);

                startLine++;
            }
        }

        private void ClearLine(int number)
        {
            #region Check value

            Debug.Assert(number >= 0, nameof(number));

            #endregion

            string clearingLine = new string(' ', MapConfiguration.Width);
            int startingPosition = LeftIndent + BorderWidth;

            Console.CursorLeft = startingPosition;
            Console.CursorTop = number;
            Console.Write(clearingLine);
        }
    }
}