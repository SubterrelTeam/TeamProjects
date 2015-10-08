using System;
using System.Collections.Generic;
using System.Threading;

namespace FallingRocks
{
    struct Position
    {
        // X is the width of the console, Y is the height
        public int X, Y;

        // symbol or also called "rock"
        public string Symbol;

        // color of the "rock"
        public ConsoleColor Color;

        // constructor to create a new position
        public Position(int x, int y, string symbol, ConsoleColor color)
        {
            this.Symbol = symbol;

            this.Color = color;

            this.X = x;

            this.Y = y;
        }

    }

    class FallingRocks
    {
        // game level
        static int gameLevel = 0;

        // game speed
        static double gameSpeed = 150;

        //loop counter
        static int loopCounterLimit = 30;

        // object that will create random numbers and they will be used for the rocks and the colors
        static Random randomGenerator = new Random();

        // contains different symbols also called "rocks"
        static string[] rocksArray = new string[] { "^", "@", "*", "&", "+", "%", "$", "#", "!", ".", ";", "-" };

        // contains the color of the "rocks"
        static ConsoleColor[] rocksColors = new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.White };


        static void Main(string[] args)
        {
            Console.Write("Level " + gameLevel);

            // initial random "rock" index set to 0
            int randomRockIndex = 0;

            // initial random color index set to 0
            int randomColorIndex = 0;

            // initial symbol or also called "rock" set to empty string
            string rock = String.Empty;

            // hides the visibility of the cursor
            Console.CursorVisible = false;

            // sets console buffer height
            Console.BufferHeight = Console.WindowHeight;

            // holds the "dwarf" elements
            List<Position> dwarfElements = new List<Position>();

            // holds the "rocks" elements
            Queue<Position> rocksElements = new Queue<Position>();

            // Creates the "dwarf" that will run away from the "rocks"
            SetDwarfElements(dwarfElements);

            int currentLoopCounter = 0;

            // starting infinit loop, if we exit the loop, its a game over.
            while (true)
            {
                //Read user key
                if (Console.KeyAvailable)
                {
                    // setting the console cursor color to be green
                    Console.ForegroundColor = ConsoleColor.Green;

                    // read the pressed key from the keyboard
                    ConsoleKeyInfo pressedKey = Console.ReadKey();

                    //initial current direction
                    int currentDirection = 0;

                    // if we have pressed right arrow...
                    if (pressedKey.Key == ConsoleKey.RightArrow)
                    {
                        // 1 means right
                        currentDirection = 1;

                        // moving "dwarf" to the right
                        MoveDwarfToRight(dwarfElements, currentDirection);
                    }
                    // if we have pressed left arrow...
                    else if (pressedKey.Key == ConsoleKey.LeftArrow)
                    {
                        // -1 means left
                        currentDirection = -1;

                        // moving "dwarf" to the left
                        MoveDwarfToLeft(dwarfElements, currentDirection);
                    }

                }

                // get random number to pick "rock" from rocks array
                randomRockIndex = randomGenerator.Next(0, rocksArray.Length);

                // get random number to pick color from colors array
                randomColorIndex = randomGenerator.Next(0, rocksColors.Length);

                // pick a "rock" by the generated random rock number
                rock = rocksArray[randomRockIndex];

                // creates the "rock" at a random position on the very top of the console with random color
                Position rockPosition = new Position(randomGenerator.Next(1, Console.WindowWidth - 1), 1, rock, rocksColors[randomColorIndex]);

                // adding the "rock" position into a queue 
                rocksElements.Enqueue(rockPosition);

                // set the cursor at the "rock" position
                Console.SetCursorPosition(rockPosition.X, rockPosition.Y);

                // set the curson color to be the random color
                Console.ForegroundColor = rocksColors[randomColorIndex];

                // display the "rock" on the console
                Console.Write(rock);

                bool isGameOver = false;

                // Move all the "rocks" to the bottom ( "rocks falling" )
                MoveRocks(rocksElements, dwarfElements, out isGameOver);

                // if game is over, stops
                if (isGameOver)
                    return;

                // wait before re-display as much as the sleepTime value is (in milliseconds)
                Thread.Sleep((int)gameSpeed);

                // increase the current loop counter by one
                currentLoopCounter++;

                // if current loop counter is bigger than the value of the loop counter limit
                // it is time to go one level up and increase the speed of the "rock falling"
                if(currentLoopCounter > loopCounterLimit)
                {
                    // go one level up and speed up the "falling"
                    SetOneLevelUp();

                    // reset the value to 0
                    currentLoopCounter = 0;
                }
            }
        }

        private static void SetOneLevelUp()
        {
            // increase the level with one
            gameLevel++;

            // increase the game speed with 5 milliseconds
            gameSpeed -= 5;

            // set cursor at top-left position
            Console.SetCursorPosition(0, 0);

            // set its color to gray
            Console.ForegroundColor = ConsoleColor.Gray;

            // display new level
            Console.WriteLine("Level " + gameLevel);
            
        }

        private static void MoveRocks(Queue<Position> rocksQueue, List<Position> dwarfElements, out bool isGameOver)
        {
            // we presum game is not over
            isGameOver = false;

            // looping through "rocks" queue
            for (int i = 0; i < rocksQueue.Count; i++)
            {
                // get current "rock" position on the console
                Position currentRockPosition = rocksQueue.Dequeue();

                // get its color
                Console.ForegroundColor = currentRockPosition.Color;

                // set the console cursor on the "rock" positon
                Console.SetCursorPosition(currentRockPosition.X, currentRockPosition.Y);

                // set blank space on this positon (because the "rock" will be move down)
                Console.Write(" ");

                // moving the "rock" position with one down
                currentRockPosition.Y++;

                // if still smaller that the console height
                if (currentRockPosition.Y < Console.WindowHeight)
                {
                    // set the console cursor to the new position ( with one down )
                    Console.SetCursorPosition(currentRockPosition.X, currentRockPosition.Y);

                    // display it on the new curson postion (one down)
                    Console.Write(currentRockPosition.Symbol);

                    // add it back to the queue
                    rocksQueue.Enqueue(currentRockPosition);
                }

                // Check if the new "rock" position "hits" the "dwarf"
                isGameOver = IsGameOver(dwarfElements, currentRockPosition);

                // if true then exit the loop and the game will be over
                if (isGameOver)
                    return;

            }
        }

        private static bool IsGameOver(List<Position> dwarfElements, Position currentRockPositions)
        {
            // looping through dwarf elements ( "(", "o" and ")" )
            for (int i = 0; i < dwarfElements.Count; i++)
            {
                // if "rock" element is at the same position of any "dwarf" element the game is over
                if (dwarfElements[i].X == currentRockPositions.X && dwarfElements[i].Y == currentRockPositions.Y)
                {
                    // set the cursor position at the left-top corner
                    Console.SetCursorPosition(0, 0);

                    // set its color to gray
                    Console.ForegroundColor = ConsoleColor.Gray;

                    // display game over and level message
                    Console.WriteLine("Game Over ! Level " + gameLevel);

                    return true;
                }
            }
            return false;
        }

        private static void SetDwarfElements(List<Position> dwarfElements)
        {
            // "dwarf" will look like that: (o)
            string[] elementsArray = new string[] { "(", "o", ")" };

            // looping each symbol and set its posion on the console
            for (int i = 0; i < 3; i++)
            {
                // setting the position to be in the middle-bottom of the console
                // setting the symbol
                // setting its color to be green
                Position element = new Position((Console.WindowWidth / 2) + i, Console.WindowHeight - 1, elementsArray[i], ConsoleColor.Green);

                // adding this element to the list that will be collection all "dwarf" elements for later usage
                dwarfElements.Add(element);

                // setting the console color to be the color of the dwarf element
                Console.ForegroundColor = element.Color;

                // setting the cursor posion of the console
                Console.SetCursorPosition(element.X, element.Y);

                // wrting the element on the console with the set color and positon (from above)
                Console.Write(element.Symbol);
            }
        }

        private static void MoveDwarfToRight(List<Position> dwarfElements, int currentDirection)
        {
            // looping through dwarf elements ( "(", "o" and ")" ) to move them one by one to the right
            for (int i = dwarfElements.Count - 1; i >= 0; i--)
            {
                // if we are trying to move the out of the console border breaks the loop
                if (dwarfElements[i].X < 0 || dwarfElements[i].X >= (Console.WindowWidth - 2))
                {
                    break;
                }

                //else display the new position
                dwarfElements[i] = DisplayNewPosition(dwarfElements[i], currentDirection);
            }
        }

        private static void MoveDwarfToLeft(List<Position> dwarfElements, int currentDirection)
        {
            // looping through dwarf elements ( "(", "o" and ")" ) to move them one by one to the left
            for (int i = 0; i < dwarfElements.Count; i++)
            {
                // if we are trying to move the out of the console border breaks the loop
                if (dwarfElements[i].X <= 0 || dwarfElements[i].X > (Console.WindowWidth - 2))
                {
                    break;
                }

                //else display the new position
                dwarfElements[i] = DisplayNewPosition(dwarfElements[i], currentDirection);
            }
        }

        private static Position DisplayNewPosition(Position dwarfElement, int currentDirection)
        {
            // set the cursor at the place of the current "dwarf" element
            Console.SetCursorPosition(dwarfElement.X, dwarfElement.Y);

            // set it to be blank space
            Console.Write(" ");

            // set the new position
            Position currentPosition = new Position(dwarfElement.X + currentDirection, dwarfElement.Y, dwarfElement.Symbol, dwarfElement.Color);

            // set the cursor there
            Console.SetCursorPosition(currentPosition.X, currentPosition.Y);

            // display it on the new position
            Console.Write(currentPosition.Symbol);

            return currentPosition;

        }
    }
}

