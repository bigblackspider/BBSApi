using System;
using System.Collections.Generic;

namespace BBSApi.Data
{
    public interface IDataServer
    {
        void Create<T>(T entity) where T : class;
        void Delete<T>(Func<T, bool> predicate, T entity) where T : class;
        void DeleteAll<T>() where T : class;
        long NextId<T>() where T : class;
        IList<T> GetAll<T>() where T : class;
        void Update<T>(Func<T, bool> predicate, T entity) where T : class;
    }
}