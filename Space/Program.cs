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

            Game.FieldWidth = 130;

            while (true)
            {
                for (var i = 0; i < Game.Asteroids; ++i)
                    entityManager.AddEntities(makeAsteroid(new LineSegment(new Vector2(0, 0), new Vector2(Game.FieldWidth, Game.FieldHeight))));

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
                                            if (movement.NextPosition.X >= Game.FieldWidth)
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

                                Game.Points -= Game.Points > 100 ? 100 : Game.Points;
                                ++Game.Shots;
                            }}
                    }, new PlayerControlOptionsProvider()),
                    new BoundsCheckComponent<MovementComponent>((ship, movement) =>
                        {
                            if (movement.NextPosition.Y < 0)
                                movement.Velocity = new Vector2(movement.Velocity.X, movement.Velocity.Y - movement.NextPosition.Y);
                            else if (movement.NextPosition.Y >= Game.FieldHeight)
                                movement.Velocity = new Vector2(movement.Velocity.X, Game.FieldHeight - movement.Position.Y);
                        }),
                    new MovementComponent(new Vector2(Game.FieldWidth / 8, Game.FieldHeight / 2)),
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
                Game.Points = -1;
                Game.Shots = 0;
                while (running)
                {
                    Console.Clear();

                    ++Game.Points;
                    Console.SetCursorPosition(0, Game.FieldHeight + 1);
                    Console.Write(Game.SeparationLine);
                    Console.SetCursorPosition(0, Game.FieldHeight + 2);
                    Console.Write("Points: " + Game.Points + "    Shots Fired: " + Game.Shots);

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
                Console.WriteLine("Points: " + Game.Points);
                Console.ReadLine();
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
                new MovementComponent(new Vector2(random.Next(spawnArea.Start.X, spawnArea.End.X + 1), random.Next(spawnArea.Start.Y, spawnArea.End.Y + 1)), new Vector2(speed < -2 ? -1 : speed, 0)),
                new RenderComponent());
        }

        private static void replaceAsteroid(Entity asteroid)
        {
            entityManager.RemoveEntities(asteroid);
            entityManager.AddEntities(makeAsteroid(new LineSegment(new Vector2(Game.FieldWidth - 1, 0), new Vector2(1, Game.FieldHeight))));
        }

        private static IEnumerable<Entity> getAsteroids()
        {
            return entityManager.Entities.Where(entity => entity.HasComponent<CharComponent>() && entity.GetComponent<CharComponent>() == asteroidCharacter);
        }
    }
}