﻿using System;
using System.Collections.Generic;
using System.Linq;
using DiceRoller.DataAccess.Models;
using SQLite;
using Xamarin.Forms.Internals;

namespace DiceRoller.DataAccess.Context
{
    public class DiceContext : IContext
    {
        private readonly SQLiteConnection _conn;
        private Dictionary<Type, Action<Entity>> _includeActions;
        
        public DiceContext(IDbPathHelper helper)
        {
            Path = helper.GetLocalFilePath();
            _conn = new SQLiteConnection(Path);

            _includeActions = new Dictionary<Type, Action<Entity>>
            {
                [typeof(Game)] = game => IncludeGameRelations((Game)game),
                [typeof(Dice)] = dice => IncludeDiceRelations((Dice)dice),
                [typeof(DiceWall)] = diceWall => IncludeDiceWallRelations((DiceWall)diceWall)
            };
        }

        public string Path { get; }

        public void CreateTable<T>() where T : Entity, new()
        {
            _conn.CreateTable<T>();
        }

        public void InsertOrReplace(object o)
        {
            _conn.InsertOrReplace(o);
        }

        public T GetById<T>(int id, bool eagerLoading = true) where T : Entity, new()
        {
            var entity = _conn.Table<T>().FirstOrDefault(x => x.Id == id);
            if (entity == null || !eagerLoading) return entity;

            _includeActions[typeof(T)].Invoke(entity);
            return entity;
        }


        private void IncludeGameRelations(Game game)
        {
            game.Dice = _conn.Table<Dice>().Where(d => d.GameId == game.Id).ToArray();
            game.Dice.ForEach(d =>
            {
                d.Game = game;
                d.Walls = _conn.Table<DiceWall>().Where(x => x.DiceId == d.Id).ToArray();
                d.Walls.ForEach(w => w.Dice = d);
            });

        }

        private void IncludeDiceRelations(Dice dice)
        {
            dice.Game = _conn.Table<Game>().First(x => x.Id == dice.GameId);
            dice.Game.Dice = _conn.Table<Dice>().Where(d => d.GameId == dice.Game.Id).ToArray();

            dice.Walls = _conn.Table<DiceWall>().Where(x => x.DiceId == dice.Id).ToArray();
            dice.Walls.ForEach(w => w.Dice = dice);
        }

        private void IncludeDiceWallRelations(DiceWall wall)
        {
            wall.Dice = _conn.Table<Dice>().First(x => x.Id == wall.DiceId);
            wall.Dice.Walls = _conn.Table<DiceWall>().Where(w => w.DiceId == wall.Dice.Id).ToArray();

            wall.Dice.Game = _conn.Table<Game>().First(x => x.Id == wall.Dice.GameId);
            wall.Dice.Game.Dice = _conn.Table<Dice>().Where(d => d.GameId == wall.Dice.Game.Id).ToArray();
        }
    }
}
