using System;
using System.Collections.Generic;

namespace BBSApi.Data
{
    public class DataServerRedis : IDataServer
    {
        public void Create<T>(T entity) where T : class
        {
            var r = new RedisMemoryProvider<T>();
            r.Create(entity);
        }

        public void Delete<T>(Func<T, bool> predicate, T entity) where T : class
        {
            var r = new RedisMemoryProvider<T>();
            r.Delete(predicate, entity);
        }

        public void DeleteAll<T>() where T : class
        {
            var r = new RedisMemoryProvider<T>();
            r.DeleteAll<T>();
        }

        public long NextId<T>() where T : class
        {
            var r = new RedisMemoryProvider<T>();
            return r.Next<T>();
        }

        public IList<T> GetAll<T>() where T : class
        {
            var r = new RedisMemoryProvider<T>();
            return r.GetAll<T>();
        }

        public void Update<T>(Func<T, bool> predicate, T entity) where T : class
        {
            var r = new RedisMemoryProvider<T>();
            r.Update(predicate, entity);
        }
    }
}