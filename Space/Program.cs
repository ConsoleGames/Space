using ConsoleEngine;
using ConsoleEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Space
{
    internal class Program
    {
        private static Random random = new Random();

        private static void Main(string[] args)
        {
            Console.WriteLine(Console.LargestWindowWidth + " / " + Console.LargestWindowHeight);
            Console.WriteLine(Console.WindowWidth + " / " + Console.WindowHeight);

            Console.SetCursorPosition(10, 10);
            Console.Write("10/10");

            Console.CursorVisible = false;

            var entityManager = new EntityManager();
            var asteroidCharacter = new CharComponent('O');
            for (var i = 0; i < 30; ++i)
                entityManager.AddEntity("asteroid" + i, asteroidCharacter, new CoordinateComponent((uint)random.Next(0, Console.WindowWidth - 1), (uint)random.Next(0, Console.WindowHeight - 1)));

            entityManager.AddEntity("ship", new CharComponent('>'), new CoordinateComponent(10, 15));
            var ship = entityManager.Entities["ship"];

            var running = true;
            var stopNextTurn = false;
            var firstRun = true;
            var delay = 30f;
            while (running)
            {
                if (stopNextTurn)
                    running = false;

                Console.Clear();

                foreach (var entityEntry in entityManager.Entities)
                {
                    var entity = entityEntry.Value;
                    var coordinates = (CoordinateComponent)entity[typeof(CoordinateComponent)];
                    Console.SetCursorPosition((int)coordinates.X, (int)coordinates.Y);
                    Console.Write(((CharComponent)entity[typeof(CharComponent)]).Char);

                    if (entityEntry.Key.StartsWith("asteroid"))
                    {
                        if (coordinates.X <= 0)
                        {
                            coordinates.X = (uint)(Console.WindowWidth - 1);
                            coordinates.Y = (uint)random.Next(0, Console.WindowHeight - 1);
                        }
                        else
                            --coordinates.X;

                        if (coordinates == (CoordinateComponent)ship[typeof(CoordinateComponent)])
                        {
                            ((CharComponent)ship[typeof(CharComponent)]).Char = '#';
                            stopNextTurn = true;
                        }
                    }
                }

                if (firstRun)
                {
                    Thread.Sleep(1000);
                    firstRun = false;
                }

                if (running && !stopNextTurn && Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    var shipCoord = (CoordinateComponent)ship[typeof(CoordinateComponent)];
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (shipCoord.Y > 0)
                                --shipCoord.Y;
                            break;

                        case ConsoleKey.DownArrow:
                            if (shipCoord.Y < Console.WindowHeight - 2)
                                ++shipCoord.Y;
                            break;
                    }
                }

                Thread.Sleep((int)delay);
                delay -= 0.01f;
            }

            Thread.Sleep(1000);

            Console.Clear();
            Console.WriteLine("Final delay:" + (int)delay);

            Console.ReadLine();
        }
    }
}