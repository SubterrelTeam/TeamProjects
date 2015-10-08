using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RallyGame
{
    /*
     * Simple game project Game Rally
     * 
     * */

    #region PsevdoCode
    /*[The Game will be in two parts. first is the action second is the Information regarding the Game.]
                 * Remove the scrool bar!
                 * 
                 *(1). Move the Cars...
                 *(2). Move our Car...(key Pressed.)
                 *(3). Check if Other cars are hitting us...
                 *(4). Clear()
                 *(5). RE-Draw the playField...
                 *(6). Slow down the Programe...
                 *  
                 * */
    #endregion PsevdoCode

    class Rally
    {
        struct Car   //  
        {
            public int _xCoordinate;  // Properties of the car Object
            public int _yCoordinate;  // The Coordinates of the Object
            public char c;            // the figure
            public ConsoleColor color;  // the color.

        }
        // Disekcia of a Method() ID PrintOnPosition passing a parameters (...)
        static void PrintOnPosition(int x, int y, char c, ConsoleColor color = ConsoleColor.Gray)  
        {
            Console.SetCursorPosition(x, y);  // The Functionality of the Method
            Console.ForegroundColor = color;  // Determin the color
            Console.WriteLine(c);             //Command That Display
        }

        static void PrintStringOnPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.WriteLine(str);
        }

        static void Main()
        {
            double speed = 30.0;         // variable speed detarmine the speed of the "Object"
            int playableAreaWidth = 8;   // this is the Area where the user play...
            int clashes = 5;             // hits

            //Remove the scrool bar and create the PlayField
            Console.BufferHeight = Console.WindowHeight = 20;
            Console.BufferWidth = Console.WindowWidth = 30;

            // Instantiate an Object theCar which is the User Car.
            Car theCar = new Car();

            theCar._xCoordinate = 2;
            theCar._yCoordinate = Console.WindowHeight - 1;
            theCar.c = '^';
            theCar.color = ConsoleColor.Cyan;

            //Generate a Randomly choosen numbers then turn in ymboles mean cars on the Display.
            Random randomly = new Random();

            // Instantinate The   Object Cars si a generic List<> Basic Data Manadgment
            List<Car> cars = new List<Car>();

            while (true)
            {
                bool hit = false;
                speed++;
                if (speed > 400)
                {
                    speed = 400;
                    clashes++;
                }
                // Generic List  theOtherCars Basic Data Manadgment
                Car theOtherCars = new Car();

                //Object the Other Cars instantiate they prop (proparties)
                theOtherCars.color = ConsoleColor.DarkGreen;
                theOtherCars._xCoordinate = randomly.Next(0, playableAreaWidth);
                theOtherCars._yCoordinate = 0;
                theOtherCars.c = '&';
                cars.Add(theOtherCars);

                //step(1) Interact with the User
                if (Console.KeyAvailable)  // the Logic
                {
                    ConsoleKeyInfo pushTheTempo = Console.ReadKey(); // Interaction with the User.Creating an Object

                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }

                    // move logic
                    if (pushTheTempo.Key == ConsoleKey.LeftArrow) // This is when the leftArrow is pressed
                    {
                        if (theCar._xCoordinate - 1 >= 0) // possition of the car ,move the object by 1 point on the left
                        {
                            theCar._xCoordinate--; // Dicrementing --;
                        }
                    }

                    else if (pushTheTempo.Key == ConsoleKey.RightArrow) // For the RightArrow
                    {
                        if (theCar._xCoordinate + 1 < playableAreaWidth)  //Exception Handling
                        {
                            theCar._xCoordinate = theCar._xCoordinate + 1;  // Moving the user Car "Object" by 1 point on the Horizontal axsis(Ordinata) 
                        }

                    }
                }

                List<Car> newList = new List<Car>();  

                for (int i = 0; i < cars.Count; i++)
                {
                    Car oldCar = cars[i];
                    Car newCarr = new Car();

                    newCarr._xCoordinate = oldCar._xCoordinate;
                    newCarr._yCoordinate = oldCar._yCoordinate + 1;
                    newCarr.c = oldCar.c;
                    newCarr.color = oldCar.color;

                    if (newCarr._yCoordinate == theCar._yCoordinate && newCarr._xCoordinate == theCar._xCoordinate)
                    {
                        clashes--;  //Decrementing the variable clashes counting the hits.
                        hit = true;  // boolian 
                        speed += 50;

                        if (speed > 400)
                        {
                            speed = 400;
                        }
                        if (clashes <= 0) // When too many hits.
                        {
                            PrintStringOnPosition(8, 10, "Game OVER!!!", ConsoleColor.Red);
                            PrintStringOnPosition(8, 12, "Press [enter]to exit", ConsoleColor.Red);
                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                    }

                    if (newCarr._yCoordinate < Console.WindowHeight)
                    {
                        newList.Add(newCarr);
                    }

                }
                cars = newList;
                // step(4) Clear();
                Console.Clear();

                if (hit)
                {
                    cars.Clear();
                    PrintOnPosition(theCar._xCoordinate, theCar._yCoordinate, 'X', ConsoleColor.Red);  // when the player is hit.
                }
                else
                {
                    PrintStringOnPosition(8, 4, "Hits:" + clashes, ConsoleColor.White);
                    PrintStringOnPosition(8, 5, "Speed:" + speed, ConsoleColor.White);
                }

                PrintOnPosition(theCar._xCoordinate, theCar._yCoordinate, theCar.c, theCar.color);

                // Display the Car on the Console
                foreach (Car car in cars)
                {
                    PrintOnPosition(car._xCoordinate, car._yCoordinate, car.c, car.color); // Call the Mathod PrintOnPosition();
                }

                //step(6) Sleep()
                Thread.Sleep(600 - (int)speed);
            }
        }
    }
}
