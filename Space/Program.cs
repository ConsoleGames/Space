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
            Console.Title = "Awesome Space Game!";

            Console.WindowWidth = 130;

            while (true)
            {
                var entityManager = new EntityManager();

                var asteroids = new Entity[(Console.WindowWidth * Console.WindowHeight) / 60];
                var asteroidCharacter = new CharComponent('O');
                Func<IEnumerable<Entity>> getAsteroids = () => entityManager.Entities.Where(entity => entity.HasComponent<CharComponent>() && entity.GetComponent<CharComponent>() == asteroidCharacter);
                for (var i = 0; i < asteroids.Length; ++i)
                {
                    var speed = random.Next(-5, 0);
                    asteroids[i] = new Entity(asteroidCharacter,
                        new MovementComponent(new Vector2(random.Next(0, Console.WindowWidth - 1), random.Next(0, Console.WindowHeight - 1)), new Vector2(speed < -2 ? -1 : speed, 0)),
                        new BoundsCheckComponent<MovementComponent>(movement =>
                        {
                            if (movement.Position.X <= 0)
                                movement.Position = new Vector2(Console.WindowWidth - 1, random.Next(0, Console.WindowHeight - 1));
                        }),
                        new RenderComponent());
                }

                var ship = new Entity(new CharComponent('>'),
                    new MovementComponent(new Vector2(Console.WindowWidth / 8, Console.WindowHeight / 2)),
                    new ControlComponent<ShipControlOptions>(new Dictionary<ShipControlOptions, Action<Entity>>
                    {
                        { ShipControlOptions.None, (_) => {} },
                        { ShipControlOptions.Up, target => target.GetComponent<MovementComponent>().Move(y: -1) },
                        { ShipControlOptions.Down, target => target.GetComponent<MovementComponent>().Move(y: 1) },
                        { ShipControlOptions.Shoot, target =>
                            {
                                var shot = new Entity();
                                shot.AddComponents(new CharComponent('~'),
                                    new MovementComponent(new Vector2(target.GetComponent<MovementComponent>().Position, deltaX: 1), new Vector2(1, 0)),
                                    new BoundsCheckComponent<MovementComponent>(movement =>
                                        {
                                            if (movement.Position.X >= Console.WindowWidth - 1)
                                            {
                                                shot.GetComponent<MovementComponent>().Move(x: Console.WindowWidth - movement.Position.X - 1);
                                                entityManager.RemoveEntities(shot);
                                            }
                                        }),
                                    new CollisionComponent((collisionTarget, check, position) => entityManager.RemoveEntities(collisionTarget, check), getAsteroids),
                                    new RenderComponent());

                                entityManager.AddEntities(shot);
                            }}
                    }, new AIControlOptionsProvider()),
                    new BoundsCheckComponent<MovementComponent>(movement =>
                        {
                            if (movement.Position.Y < 0)
                                movement.Move(y: -movement.Position.Y);
                            else if (movement.Position.Y > Console.WindowHeight - 2)
                                movement.Move(y: Console.WindowHeight - 2 - movement.Position.Y);
                        }),
                    new CollisionComponent((target, check, position) =>
                        {
                            target.GetComponent<MovementComponent>().Position = check.GetComponent<MovementComponent>().Position;
                            target.GetComponent<CharComponent>().Char = '#';
                            running = false;
                        }, getAsteroids),
                    new RenderComponent());

                entityManager.AddEntities(asteroids);
                entityManager.AddEntities(ship);

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

                    if ((int)delay > 0)
                    {
                        Thread.Sleep((int)delay);
                        delay -= 0.01f;
                    }
                }

                Thread.Sleep(1000);

                Console.Clear();
                Console.WriteLine("Final delay:" + (int)delay);

                //Console.ReadLine();

                Thread.Sleep(1000);
            }
        }
    }
}