using System;
using System.Collections.Generic;
using System.Linq;
using BBSApi.Core.Models.Customer;
using BBSApi.Core.Models.Mail;
using BBSApi.Core.Models.Web;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BBSApi.Data
{
    public class DataServerRedis : IDataServer
    {
        public void Create<T>(T entity) where T : class
        {
            var r = new RedisMemoryProvider<T>();
            r.Create(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            var r = new RedisMemoryProvider<T>();
            r.Delete(entity);
        }

        public void DeleteAll<T>() where T : class
        {
            var r = new RedisMemoryProvider<T>();
            r.DeleteAll<T>();
        }

        public long Next<T>() where T : class
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
            r.Update(predicate,entity);
        }
    }
}