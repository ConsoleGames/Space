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

        private static readonly EntityManager entityManager = new EntityManager();
        private static readonly CharComponent asteroidCharacter = new CharComponent('O');

        private static bool running;

        private static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "Awesome Space Game!";

            Console.WindowWidth = 130;

            while (true)
            {
                var asteroids = (Console.WindowWidth * Console.WindowHeight) / 60;

                for (var i = 0; i < asteroids; ++i)
                    entityManager.AddEntities(makeAsteroid(new LineSegment(new Vector2(0, 0), new Vector2(Console.WindowWidth, Console.WindowHeight - 1))));

                entityManager.AddEntities(new Entity(new CharComponent('>'),
                    new ControlComponent<ShipControlOptions>(new Dictionary<ShipControlOptions, Action<Entity>>
                    {
                        { ShipControlOptions.None, (ship) => ship.GetComponent<MovementComponent>().Velocity = new Vector2(0, 0) },
                        { ShipControlOptions.Up, ship => ship.GetComponent<MovementComponent>().Velocity = new Vector2(0, -1) },
                        { ShipControlOptions.Down, ship => ship.GetComponent<MovementComponent>().Velocity = new Vector2(0, 1) },
                        { ShipControlOptions.Shoot, target =>
                            {
                                entityManager.AddEntities(new Entity(new CharComponent('~'),
                                    new BoundsCheckComponent<MovementComponent>((shot, movement) =>
                                        {
                                            if (movement.NextPosition.X >= Console.WindowWidth - 1)
                                            {
                                                movement.Velocity = new Vector2(Console.WindowWidth - movement.Position.X - 1, movement.Velocity.Y);
                                                entityManager.RemoveEntities(shot);
                                            }
                                        }),
                                    new MovementComponent(new Vector2(target.GetComponent<MovementComponent>().Position, deltaX: 1), new Vector2(1, 0)),
                                    new CollisionComponent((shot, check, position) =>
                                        {
                                            entityManager.RemoveEntities(shot);
                                            replaceAsteroid(check);
                                        }, getAsteroids),
                                    new RenderComponent()));
                            }}
                    }, new PlayerControlOptionsProvider()),
                    new BoundsCheckComponent<MovementComponent>((ship, movement) =>
                        {
                            if (movement.NextPosition.Y < 0)
                                movement.Velocity = new Vector2(movement.Velocity.X, movement.Velocity.Y - movement.NextPosition.Y);
                            else if (movement.NextPosition.Y > Console.WindowHeight - 2)
                                movement.Velocity = new Vector2(movement.Velocity.X, Console.WindowHeight - movement.Position.Y - 2);
                        }),
                    new MovementComponent(new Vector2(Console.WindowWidth / 8, Console.WindowHeight / 2)),
                    new CollisionComponent((target, check, position) =>
                        {
                            target.GetComponent<MovementComponent>().Position = position;
                            target.GetComponent<CharComponent>().Char = '#';
                            running = false;
                        }, getAsteroids),
                    new RenderComponent()));

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

                entityManager.ClearEntities();
                Console.Clear();
                Console.WriteLine("Final delay:" + (int)delay);

                //Console.ReadLine();

                Thread.Sleep(1000);
            }
        }

        private static Entity makeAsteroid(LineSegment spawnArea)
        {
            var speed = random.Next(-5, 0);
            return new Entity(asteroidCharacter,
                new BoundsCheckComponent<MovementComponent>((asteroid, movement) =>
                {
                    if (movement.NextPosition.X <= 0)
                    {
                        movement.Velocity = new Vector2(movement.Velocity.X - movement.NextPosition.X, movement.Velocity.Y);
                        replaceAsteroid(asteroid);
                    }
                }),
                new MovementComponent(new Vector2(random.Next(spawnArea.Start.X, spawnArea.End.X), random.Next(spawnArea.Start.Y, spawnArea.End.Y)), new Vector2(speed < -2 ? -1 : speed, 0)),
                new RenderComponent());
        }

        private static void replaceAsteroid(Entity asteroid)
        {
            entityManager.RemoveEntities(asteroid);
            entityManager.AddEntities(makeAsteroid(new LineSegment(new Vector2(Console.WindowWidth - 1, 0), new Vector2(1, Console.WindowHeight - 2))));
        }

        private static IEnumerable<Entity> getAsteroids()
        {
            return entityManager.Entities.Where(entity => entity.HasComponent<CharComponent>() && entity.GetComponent<CharComponent>() == asteroidCharacter);
        }
    }
}