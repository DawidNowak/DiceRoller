using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DiceRoller.DataAccess.Models;

namespace DiceRoller.DataAccess.Context
{
    public interface IContext
    {
        string Path { get; }
        void CreateTable<T>() where T : Entity, new();
        void InsertOrReplace(object o);
        T[] GetAll<T>() where T : Entity, new();
        T GetById<T>(int id, bool eagerLoading = true) where T : Entity, new();
        IEnumerable<T> GetBy<T>(Expression<Func<T, bool>> where) where T : Entity, new();
        T GetByFirstOrDefault<T>(Expression<Func<T, bool>> where) where T : Entity, new();
    }
}
