using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BBSApi.Data.Properties;
using ServiceStack.Redis;

namespace BBSApi.Data
{
    //Based on: http://stackoverflow.com/questions/30818784/generic-object-cache
    public class RedisMemoryProvider<T> where T : class
    {
        private static readonly PooledRedisClientManager m = new PooledRedisClientManager(Settings.Default.RedisServer);

        private readonly IDictionary<Type, List<object>> _cache = new ConcurrentDictionary<Type, List<object>>();

        public RedisMemoryProvider()
        {
            LoadIntoCache<T>();
        }

        /// <summary>
        ///     Load {T} into object cache from Data Store.
        /// </summary>
        /// <typeparam name="T">class</typeparam>
        private void LoadIntoCache<T>() where T : class
        {
            _cache[typeof (T)] = GetAll<T>().Cast<object>().ToList();
        }

        /// <summary>
        ///     Find Single {T} in object cache.
        /// </summary>
        /// <typeparam name="T">class</typeparam>
        /// <param name="predicate">linq statement</param>
        /// <returns></returns>
        public T Read(Func<T, bool> predicate)
        {
            List<object> list;
            if (_cache.TryGetValue(typeof (T), out list))
            {
                return list.Cast<T>().Where(predicate).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        ///     Find List<T>(predicate) in cache.
        /// </summary>
        /// <typeparam name="T">class</typeparam>
        /// <param name="predicate">linq statement</param>
        /// <returns></returns>
        public List<T> FindBy<T>(Func<T, bool> predicate) where T : class
        {
            List<object> list;
            if (_cache.TryGetValue(typeof (T), out list))
            {
                return list.Cast<T>().Where(predicate).ToList();
            }
            return new List<T>();
        }

        public T FindById<T>(long id)
        {
            using (var ctx = m.GetClient())
            {
                var foundItem = ctx.GetById<T>(id);
                return foundItem;
            }
        }

        public IList<T> FindByIds<T>(long[] ids)
        {
            using (var ctx = m.GetClient())
            {
                var foundItems = ctx.GetByIds<T>(ids);
                return foundItems;
            }
        }

        public void Create<T>(T entity) where T : class
        {
            List<object> list;
            if (!_cache.TryGetValue(typeof (T), out list))
            {
                list = new List<object>();
            }
            list.Add(entity);
            _cache[typeof (T)] = list;
            Store(entity);
        }

        /// <summary>
        ///     Delete single {T} from cache and Data Store.
        /// </summary>
        /// <typeparam name="T">class</typeparam>
        /// <param name="entity">class object</param>
        public void Delete<T>(T entity) where T : class
        {
            List<object> lis;
            if (_cache.TryGetValue(typeof (T), out lis))
            {
                lis.Remove((T)entity);
                _cache[typeof (T)] = lis;

                RedisDelete(entity);
            }
        }

        public void DeleteAll<T>() where T : class
        {
            List<object> lis;
            if (_cache.TryGetValue(typeof(T), out lis))
            {
                _cache[typeof (T)] = new List<object>();
                RedisDeleteAll<T>();
            }
        }

        public long Next<T>() where T : class
        {
            long id = 1;

            using (var ctx = m.GetClient())
            {
                try
                {
                    id = ctx.As<T>().GetNextSequence();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return id;
        }

        public IList<T> GetAll<T>() where T : class
        {
            using (var ctx = m.GetClient())
            {
                try
                {
                    return ctx.As<T>().GetAll();
                }
                catch (Exception err)
                {
                    Debug.WriteLine(err.Message);
                    return new List<T>();
                }
            }
        }

        public void Update<T>(Func<T, bool> predicate, T entity) where T : class
        {
            List<object> list;

            if (_cache.TryGetValue(typeof (T), out list))
            {
                var existing = list.Cast<T>().FirstOrDefault(predicate);
                if (existing != null)
                    list.Remove(existing);
                list.Add(entity);
                _cache[typeof (T)] = list;
                Store(entity);
            }
        }

        public bool ExpireAt(string keyName, int expireInSeconds)
        {
            using (var client = new RedisNativeClient(Settings.Default.RedisServer))
            {
                return client.Expire(keyName, expireInSeconds);
            }
        }

        public long GetTtl(string keyName)
        {
            using (var client = new RedisNativeClient(Settings.Default.RedisServer))
            {
                return client.Ttl(keyName);
            }
        }

        public void Set(string keyName, string content)
        {
            using (var client = new RedisNativeClient(Settings.Default.RedisServer))
            {
                client.Set(keyName, Encoding.UTF8.GetBytes(content));
            }
        }

        public string Get(string keyName)
        {
            using (var client = new RedisNativeClient(Settings.Default.RedisServer))
            {
                return Encoding.UTF8.GetString(client.Get(keyName));
            }
        }

        public IDictionary<string, string> GetInfo()
        {
            using (var client = new RedisNativeClient(Settings.Default.RedisServer))
            {
                return client.Info;
            }
        }

        public bool Ping()
        {
            using (var client = new RedisNativeClient(Settings.Default.RedisServer))
            {
                return client.Ping();
            }
        }

        #region Private methods 

        private void Store<T>(T entity) where T : class
        {
            using (var ctx = m.GetClient())
            {
                ctx.Store(entity);
            }
        }
        private void RedisDeleteAll<T>()
        {
            using (var ctx = m.GetClient())
            {
                ctx.As<T>().DeleteAll();
            }
        }
        private void RedisDelete<T>(T entity) where T : class
        {
            using (var ctx = m.GetClient())
            {
                ctx.As<T>().Delete(entity);
            }
        }

        private T Find<T>(long id) where T : class
        {
            using (var ctx = m.GetClient())
            {
                return ctx.As<T>().GetById(id);
            }
        }

        #endregion
    }
}