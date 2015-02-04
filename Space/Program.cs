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
                entityManager.AddEntity("asteroid" + i, new Entity(asteroidCharacter, new MovementComponent(new Coordinate(random.Next(0, Console.WindowWidth - 1), random.Next(0, Console.WindowHeight - 1)), new Velocity(-1, 0))));

            var ship = new Entity(new CharComponent('>'), new MovementComponent(new Coordinate(10, 15)));
            entityManager.AddEntity("ship", ship);

            var running = true;
            var stopNextTurn = false;
            var firstRun = true;
            var delay = 30f;
            while (running)
            {
                if (stopNextTurn)
                    running = false;

                Console.Clear();
                entityManager.RenderEntities();
                entityManager.UpdateEntities();

                foreach (var entityEntry in entityManager.Entities)
                {
                    var movement = entityEntry.Value.GetComponent<MovementComponent>();

                    if (entityEntry.Key.StartsWith("asteroid"))
                    {
                        if (movement.Position.X <= 0)
                            movement.Position = new Coordinate(Console.WindowWidth - 1, random.Next(0, Console.WindowHeight - 1));

                        if (movement.Position == ship.GetComponent<MovementComponent>().Position)
                        {
                            ship.GetComponent<CharComponent>().Char = '#';
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
                    var shipMovement = ship.GetComponent<MovementComponent>();
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (shipMovement.Position.Y > 0)
                                shipMovement.Position = new Coordinate(shipMovement.Position.X, shipMovement.Position.Y - 1);
                            break;

                        case ConsoleKey.DownArrow:
                            if (shipMovement.Position.Y < Console.WindowHeight - 2)
                                shipMovement.Position = new Coordinate(shipMovement.Position.X, shipMovement.Position.Y + 1);
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