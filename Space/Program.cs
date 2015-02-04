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

        private static bool running;

        private static void Main(string[] args)
        {
            Console.CursorVisible = false;

            var entityManager = new EntityManager();

            var asteroidCharacter = new CharComponent('O');

            var asteroids = new Entity[30];
            for (var i = 0; i < 30; ++i)
                asteroids[i] = new Entity(asteroidCharacter,
                    new MovementComponent(new Coordinate(random.Next(0, Console.WindowWidth - 1), random.Next(0, Console.WindowHeight - 1)), new Velocity(-1, 0)),
                    new BoundsCheckComponent<MovementComponent>(movement =>
                        {
                            if (movement.Position.X <= 0)
                                movement.Position = new Coordinate(Console.WindowWidth - 1, random.Next(0, Console.WindowHeight - 1));
                        }),
                    new RenderComponent());

            var ship = new Entity(new CharComponent('>'),
                new MovementComponent(new Coordinate(10, 15)),
                new CollisionComponent((target, check) =>
                    {
                        target.GetComponent<CharComponent>().Char = '#';
                        running = false;
                    }, asteroids),
                new RenderComponent());

            entityManager.AddEntity("asteroid", asteroids); //Will be named asteroid0 to asteroid29
            entityManager.AddEntity("ship", ship);

            running = true;
            var firstRun = true;
            var delay = 30f;
            while (running)
            {
                Console.Clear();
                entityManager.UpdateEntities();

                if (firstRun)
                {
                    Thread.Sleep(1000);
                    firstRun = false;
                }

                if (running && Console.KeyAvailable)
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