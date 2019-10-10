using System;

namespace Travel.CasheMigration
{
    class CacheException : Exception
    {
        public CacheException() { }
        public CacheException(string message) : base(message) { }
    }
}
