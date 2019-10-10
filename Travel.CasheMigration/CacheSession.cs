using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterSystems.Data.CacheClient;
using InterSystems.Data.CacheTypes;
using System.Data;
using ThousandsRoads;
using System.Reflection;
using Travel.Core;
namespace Travel.CasheMigration
{
    public class CacheSession<Tsource, Tdest> : IDisposable
        where Tsource : CacheObject
        where Tdest : class
    {
        private CacheConnection cacheConnect;
        private CacheDataAdapter cacheAdapter;
        private readonly string tableName;
        private readonly string _connectionString;
        protected IObjectMapper mapper;
        public CacheCommand SelectCommand { get; private set; }
        public IObjectMapper Mapper { get { return mapper; } }

        public CacheSession(IObjectMapper mapper)
        {
            this.mapper = mapper;
            _connectionString = Cache.ConnectionString;
            this.cacheConnect = new CacheConnection(_connectionString);
            tableName = typeof(Tsource).FullName;
            cacheAdapter = new CacheDataAdapter(tableName, cacheConnect);
            CacheCommandBuilder cacheBuilder = new CacheCommandBuilder(cacheAdapter);
            SelectCommand = new CacheCommand();
            SelectCommand.Connection = this.cacheConnect;
        }
        public CacheSession(IObjectMapper mapper, string connectionString)
        {
            this.mapper = mapper;
            _connectionString = connectionString;

            this.cacheConnect = new CacheConnection(connectionString);
            tableName = typeof(Tsource).FullName;
            cacheAdapter = new CacheDataAdapter(tableName, cacheConnect);
            CacheCommandBuilder cacheBuilder = new CacheCommandBuilder(cacheAdapter);
            SelectCommand = new CacheCommand();
            SelectCommand.Connection = this.cacheConnect;
        }
        public IEnumerable<Tdest> GetAll()
        {
            cacheAdapter = new CacheDataAdapter(this.SelectCommand);
            DataSet dataSet = new DataSet();
            cacheAdapter.Fill(dataSet, tableName);
            DataTable dataTable = dataSet.Tables[tableName];
            return TableToList<Tdest>(dataTable);
        }
		public IEnumerable<T> GetAll<T>()
		{
			cacheAdapter = new CacheDataAdapter(this.SelectCommand);
			DataSet dataSet = new DataSet();
			cacheAdapter.Fill(dataSet, tableName);
			DataTable dataTable = dataSet.Tables[tableName];
			return TableToList<T>(dataTable);
		}
        public dynamic GetCustom(CacheCommand query)
        {
            cacheAdapter = new CacheDataAdapter(query);
            DataSet dataSet = new DataSet();
            cacheAdapter.Fill(dataSet, tableName);
            DataTable dataTable = dataSet.Tables[tableName];
            return dataTable;
        }
        public dynamic GetScalar(CacheCommand query)
        {
            return query.ExecuteScalar();
        }

        private List<T> TableToList<T>(DataTable table)
        {
            List<T> rez = new List<T>();

            foreach (DataRow rw in table.Rows)
            {
                T item = Activator.CreateInstance<T>();
                foreach (DataColumn cl in table.Columns)
                {
                    var propertyName = cl.ColumnName == "ID" ? "Id" : cl.ColumnName;
                    PropertyInfo pi = typeof(T).GetProperty(propertyName);

                    if (pi != null && rw[cl] != DBNull.Value)
                        pi.SetValue(item, ChangeType(rw[cl], pi.PropertyType));

                }
                rez.Add(item);
            }
            return rez;

        }

        private object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                t = Nullable.GetUnderlyingType(t); ;
            }
			if (t.IsEnum)
			{
				return Enum.Parse(t, value.ToString());
			}
            return Convert.ChangeType(value, t);
        }

        public Tdest Get(int id)
        {
            MethodInfo method = typeof(Tsource).GetMethod("OpenId", new Type[] { typeof(CacheConnection), typeof(string) });
            var result = method.Invoke(null, new object[] { cacheConnect, id.ToString() });
            return mapper.Map<Tsource, Tdest>((Tsource)result);// (T)result;
        }

        public Tsource GetReal(int id)
        {
            MethodInfo method = typeof(Tsource).GetMethod("OpenId", new Type[] { typeof(CacheConnection), typeof(string) });
            var result = method.Invoke(null, new object[] { cacheConnect, id.ToString() });
            return (Tsource)result;// (T)result;
        }

        public Tdest Add(Tsource field)
        {
            MethodInfo method = typeof(Tsource).GetMethod("Save", new Type[] { });
            var res = (CacheStatus)method.Invoke(field, new object[] { });
            if (!res.IsOK)
                throw new CacheException(res.Message);
            return mapper.Map<Tsource, Tdest>((Tsource)field);
        }
        public void Delete(int id)
        {
            MethodInfo method = typeof(Tsource).GetMethod("DeleteId", new Type[] { typeof(CacheConnection), typeof(string) });
            var result = method.Invoke(null, new object[] { cacheConnect, id.ToString() });
        }

        public CacheConnection Open()
        {
            if (cacheConnect.State != ConnectionState.Open)
            {
                cacheConnect.ConnectionString = _connectionString;
                cacheConnect.Open();
            }

            return cacheConnect;
        }
        public void Close()
        {
            cacheConnect.Close();
        }

        public void Dispose()
        {
            cacheConnect.Close();
        }
    }
}
