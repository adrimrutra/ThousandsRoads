using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Travel.Core.Data
{
	public interface IDbSession : IDisposable
	{
		IQueryable<T> AsQueryable<T>() where T : class;

		IDBSessionTransaction BeginTransaction();

		void InTransaction(Action<IDbSession> action);

		void Save<T>(T obj) where T : class;

		void Insert<T>(T obj) where T : class;

		void Delete<T>(T obj) where T : class;

		void Delete<T>(Func<T, bool> aPredicate) where T : class;

		void Delete<T>(IEnumerable<T> objs) where T : class;

		void ClearCache();
	}

	public interface IDBSessionTransaction : IDisposable
	{
		void Commit();

		void Rollback();
	}

	public enum DBTransactionState { Opened, Committed, Rollbacked }

	public class DBTransaction : IDBSessionTransaction
	{
		private DBTransactionState _state;

		private Action _onCommit;
		private Action _onRollback;

		internal DBTransaction(Action onCommit, Action onRollback)
		{
			_onCommit = onCommit;
			_onRollback = onRollback;
		}

		public void Commit()
		{
			_state = DBTransactionState.Committed;
			_onCommit();
		}

		public void Rollback()
		{
			_state = DBTransactionState.Rollbacked;
			_onRollback();
		}

		public void Dispose()
		{
			if (_state == DBTransactionState.Opened)
				Rollback();
		}
	}

	public abstract class DBSessionBase : IDbSession
	{
		private IDbTransaction _currentTransaction;
		private int _transactionLevel = 0;

		protected abstract IDbTransaction BeginInternalTransaction();

		protected bool IsInTransaction
		{
			get { return _transactionLevel != 0; }
		}

		public abstract IQueryable<T> AsQueryable<T>() where T : class;

		public abstract void Save<T>(T obj) where T : class;

		public abstract void Insert<T>(T obj) where T : class;

		public abstract void Delete<T>(T obj) where T : class;

		public abstract void Delete<T>(Func<T, bool> aPredicate) where T : class;

		public abstract void Delete<T>(IEnumerable<T> objs) where T : class;

		public abstract void ClearCache();

		public IDBSessionTransaction BeginTransaction()
		{
			if (_currentTransaction == null || _transactionLevel == 0)
			{
				_currentTransaction = BeginInternalTransaction();
			}
			_transactionLevel++;
			return new DBTransaction(
				() =>
				{
					_transactionLevel--;
					if (_transactionLevel == 0)
					{
						_currentTransaction.Commit();
						_currentTransaction = null;
					}
				},
				() =>
				{
					_transactionLevel--;
					if (_transactionLevel == 0)
					{
						_currentTransaction.Rollback();
						_currentTransaction = null;
					}
				});
		}

		public void InTransaction(Action<IDbSession> action)
		{
			using (var tran = BeginTransaction())
			{
				action(this);
				tran.Commit();
			}
		}

		#region IDisposable

		~DBSessionBase()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing) { }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion IDisposable
	}

	public class EFSession : DBSessionBase
	{
		public EFSession(DbContext aDataContext)
		{
			DataContext = ((IObjectContextAdapter)aDataContext).ObjectContext;
		}

		public ObjectContext DataContext { get; private set; }

		protected override IDbTransaction BeginInternalTransaction()
		{
			if (DataContext.Connection.State != ConnectionState.Open)
			{
				DataContext.Connection.Open();
			}
			return DataContext.Connection.BeginTransaction();
		}

		public override IQueryable<T> AsQueryable<T>()
		{
			return DataContext.CreateObjectSet<T>();
		}

		public override void Save<T>(T obj)
		{
			DataContext.SaveChanges();
		}

		public override void Insert<T>(T obj)
		{
			DataContext.CreateObjectSet<T>().AddObject(obj);
			DataContext.SaveChanges();
		}

		public override void Delete<T>(T obj)
		{
			DataContext.CreateObjectSet<T>().DeleteObject(obj);
			DataContext.SaveChanges();
		}

		public override void Delete<T>(System.Func<T, bool> aPredicate)
		{
			using (var transaction = BeginTransaction())
			{
				foreach (T item in AsQueryable<T>().Where(aPredicate))
					Delete(item);
				transaction.Commit();
			}
		}

		public override void Delete<T>(IEnumerable<T> objs)
		{
			using (var transaction = BeginTransaction())
			{
				foreach (T obj in objs.ToList())
					Delete(obj);
				transaction.Commit();
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				if (DataContext != null)
					DataContext.Dispose();
			}
		}

		public override void ClearCache()
		{
			throw new System.NotImplementedException();
		}
	}
}