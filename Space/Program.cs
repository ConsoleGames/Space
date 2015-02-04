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

            var asteroids = new Entity[(Console.WindowWidth * Console.WindowHeight) / 60];
            var asteroidCharacter = new CharComponent('O');
            for (var i = 0; i < asteroids.Length; ++i)
                asteroids[i] = new Entity(asteroidCharacter,
                    new MovementComponent(new Coordinate(random.Next(0, Console.WindowWidth - 1), random.Next(0, Console.WindowHeight - 1)), new Velocity(-1, 0)),
                    new BoundsCheckComponent<MovementComponent>(movement =>
                        {
                            if (movement.Position.X <= 0)
                                movement.Position = new Coordinate(Console.WindowWidth - 1, random.Next(0, Console.WindowHeight - 1));
                        }),
                    new RenderComponent());

            var ship = new Entity(new CharComponent('>'),
                new MovementComponent(new Coordinate(10, Console.WindowHeight / 2)),
                new ControlComponent<ShipControlOptions>(new Dictionary<ShipControlOptions, Action<Entity>>
                    {
                        { ShipControlOptions.None, (_) => {} },
                        { ShipControlOptions.Up, target =>
                            {
                                var movement = target.GetComponent<MovementComponent>();
                                movement.Position = new Coordinate(movement.Position, deltaY: -1);
                            }
                        },
                        { ShipControlOptions.Down, target =>
                            {
                                var movement = target.GetComponent<MovementComponent>();
                                movement.Position = new Coordinate(movement.Position, deltaY: 1);
                            }
                        }
                    }, new PlayerControlOptionsProvider()),
                new BoundsCheckComponent<MovementComponent>(movement =>
                    {
                        if (movement.Position.Y < 0)
                            movement.Position = new Coordinate(movement.Position.X, 0);
                        else if (movement.Position.Y > Console.WindowHeight - 2)
                            movement.Position = new Coordinate(movement.Position.X, Console.WindowHeight - 2);
                    }),
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