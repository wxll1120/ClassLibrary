using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using ClassLibrary.Transaction;
using Dapper;

namespace DeviceManage.DataAccess.SqlServer
{
    public class DataAccessBase
    {
        private string connectionString = string.Empty;

        protected IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection connection = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                connection = new SqlConnection(connectionString);
            }
            else
            {
                connection = GetTransaction(connectionString).Connection;
            }

            return connection;
        }

        private void OpenConnection(IDbConnection connection)
        {
            if (connection != null && connection.State.Equals(ConnectionState.Closed))
                connection.Open();
        }

        protected IDbTransaction BeginTransaction(string connectionString)
        {
            IDbTransaction transaction = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                IDbConnection connection = CreateConnection(connectionString);
                connection.Open();

                transaction = connection.BeginTransaction();
            }
            else
            {
                transaction = GetTransaction(connectionString);
            }

            return transaction;
        }

        protected void CloseConnection(IDbConnection connection)
        {
            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (connection != null && connection.State.Equals(ConnectionState.Open))
                    connection.Close();
            }
        }

        protected void Commit(IDbTransaction transaction)
        {
            if (!ClassLibrary.Transaction.Transaction.TransactionStart && transaction != null)
                transaction.Commit();
        }

        protected void Rollback(IDbTransaction transaction)
        {
            if (!ClassLibrary.Transaction.Transaction.TransactionStart && transaction != null)
                transaction.Rollback();
        }

        protected IDbCommand CreateCommand(string commandText, string connectionString = null,
            IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (string.IsNullOrEmpty(connectionString) && connection == null && transaction == null)
                throw new NullReferenceException("请至少为参数connectionString、connection、transaction中的一项指定值！");

            IDbCommand command = new SqlCommand(commandText);

            if (transaction != null)
            {
                command.Connection = transaction.Connection;
                command.Transaction = transaction;
            }
            else if (connection != null)
            {
                command.Connection = connection;
            }

            return command;
        }

        protected int ExecuteNonQuery(string commandText = null, IDbCommand command = null, string connectionString = null,
            IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (commandText == null && command == null)
                throw new NullReferenceException("请至少为参数commandText、command中的一项指定值！");

            if (string.IsNullOrEmpty(connectionString) && connection == null && transaction == null)
                throw new NullReferenceException("请至少为参数connectionString、connection、transaction中的一项指定值！");

            int result = 0;

            if (command == null)
            {
                command = CreateCommand(commandText, connectionString, connection, transaction);
            }

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    command.Connection = transaction.Connection;
                    command.Transaction = transaction;

                    result = command.ExecuteNonQuery();
                }
                else if (connection != null)
                {
                    command.Connection = connection;

                    result = command.ExecuteNonQuery();
                }
                else if (!string.IsNullOrEmpty(connectionString))
                {
                    using (IDbConnection conn = CreateConnection(connectionString))
                    {
                        conn.Open();
                        result = command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                string connString = connectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }
                else if (connection != null)
                {
                    connString = connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                command.Connection = tran.Connection;
                command.Transaction = tran;

                result = command.ExecuteNonQuery();
            }

            return result;
        }

        protected object ExecuteScalar(string commandText = null, IDbCommand command = null, string connectionString = null,
            IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (commandText == null && command == null)
                throw new NullReferenceException("请至少为参数commandText、command中的一项指定值！");

            if (string.IsNullOrEmpty(connectionString) && connection == null && transaction == null)
                throw new NullReferenceException("请至少为参数connectionString、connection、transaction中的一项指定值！");

            object result = null;

            if (command == null)
            {
                command = CreateCommand(commandText, connectionString, connection, transaction);
            }

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    command.Connection = transaction.Connection;
                    command.Transaction = transaction;

                    result = command.ExecuteScalar();
                }
                else if (connection != null)
                {
                    command.Connection = connection;

                    result = command.ExecuteScalar();
                }
                else if (!string.IsNullOrEmpty(connectionString))
                {
                    using (IDbConnection conn = CreateConnection(connectionString))
                    {
                        conn.Open();
                        result = command.ExecuteScalar();
                    }
                }
            }
            else
            {
                string connString = connectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }
                else if (connection != null)
                {
                    connString = connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                command.Connection = tran.Connection;
                command.Transaction = tran;

                result = command.ExecuteScalar();
            }

            return result;
        }

        protected int Execute(string commandText, string connectionString = null, IDbConnection connection = null,
            IDbTransaction transaction = null, int? commandTimeout = null,
            CommandType? commandType = null, object param = null)
        {
            if (string.IsNullOrEmpty(connectionString) && connection == null && transaction == null)
                throw new NullReferenceException("请至少为参数connectionString、connection、transaction中的一项指定值！");

            int result = 0;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);
                    result = transaction.Connection.Execute(commandText, param, transaction, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);
                    result = connection.Execute(commandText, param: param, commandTimeout: commandTimeout, commandType: commandType);
                }
                else if (!string.IsNullOrEmpty(connectionString))
                {
                    using (IDbConnection conn = CreateConnection(connectionString))
                    {
                        conn.Open();
                        result = conn.Execute(commandText, param: param, commandTimeout: commandTimeout, commandType: commandType);
                    }
                }
            }
            else
            {
                string connString = connectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }
                else if (connection != null)
                {
                    connString = connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Execute(commandText, param, tran, commandTimeout, commandType);
            }

            return result;
        }

        public object ExecuteObject(string commandText, string connectionString = null,
            IDbConnection connection = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null, object param = null)
        {
            if (string.IsNullOrEmpty(connectionString) && connection == null && transaction == null)
                throw new NullReferenceException("请至少为参数connectionString、connection、transaction中的一项指定值！");

            object result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);
                    result = transaction.Connection.ExecuteScalar(commandText, param, transaction, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);
                    result = connection.ExecuteScalar(commandText, param: param, commandTimeout: commandTimeout, commandType: commandType);
                }
                else if (!string.IsNullOrEmpty(connectionString))
                {
                    using (IDbConnection conn = CreateConnection(connectionString))
                    {
                        conn.Open();
                        result = conn.ExecuteScalar(commandText, param: param, commandTimeout: commandTimeout, commandType: commandType);
                    }
                }
            }
            else
            {
                string connString = connectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }
                else if (connection != null)
                {
                    connString = connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.ExecuteScalar(commandText, param, tran, commandTimeout, commandType);
            }

            return result;
        }

        public T ExecuteObject<T>(string commandText, string connectionString = null, IDbConnection connection = null,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null,
            object param = null)
        {
            if (string.IsNullOrEmpty(connectionString) && connection == null && transaction == null)
                throw new NullReferenceException("请至少为参数connectionString、connection、transaction中的一项指定值！");

            T result = default(T);

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);
                    result = transaction.Connection.ExecuteScalar<T>(commandText, param, transaction, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);
                    result = connection.ExecuteScalar<T>(commandText, param: param, commandTimeout: commandTimeout, commandType: commandType);
                }
                else if (!string.IsNullOrEmpty(connectionString))
                {
                    using (IDbConnection conn = CreateConnection(connectionString))
                    {
                        conn.Open();
                        result = conn.ExecuteScalar<T>(commandText, param: param, commandTimeout: commandTimeout, commandType: commandType);
                    }
                }
            }
            else
            {
                string connString = connectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }
                else if (connection != null)
                {
                    connString = connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.ExecuteScalar<T>(commandText, param, tran, commandTimeout, commandType);
            }

            return result;
        }

        protected IDataReader ExecuteReader(string commandText = null, IDbCommand command = null,
            CommandBehavior behavior = CommandBehavior.Default, string connectionString = null,
            IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (commandText == null && command == null)
                throw new NullReferenceException("请至少为参数commandText、command中的一项指定值！");

            if (string.IsNullOrEmpty(connectionString) && connection == null && transaction == null)
                throw new NullReferenceException("请至少为参数connectionString、connection、transaction中的一项指定值！");

            IDataReader reader = null;

            if (command == null)
            {
                command = CreateCommand(commandText, connectionString, connection, transaction);
            }

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    command.Connection = transaction.Connection;
                    command.Transaction = transaction;
                    OpenConnection(transaction.Connection);

                    reader = command.ExecuteReader();
                }
                else if (connection != null)
                {
                    command.Connection = connection;
                    OpenConnection(transaction.Connection);

                    reader = command.ExecuteReader();
                }
                else if (!string.IsNullOrEmpty(connectionString))
                {
                    using (IDbConnection conn = CreateConnection(connectionString))
                    {
                        conn.Open();
                        reader = command.ExecuteReader();
                    }
                }
            }
            else
            {
                string connString = connectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }
                else if (connection != null)
                {
                    connString = connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connectionString);

                command.Connection = tran.Connection;
                command.Transaction = tran;

                reader = command.ExecuteReader();
            }

            return reader;
        }

        public IDataReader ExecuteResult(string commandText, IDbConnection connection, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null, object param = null)
        {
            IDataReader reader = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);
                    reader = transaction.Connection.ExecuteReader(commandText, param, transaction, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);
                    reader = connection.ExecuteReader(commandText, param: param, commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                reader = tran.Connection.ExecuteReader(commandText, param, tran, commandTimeout, commandType);
            }

            return reader;
        }

        public IEnumerable<T> Query<T>(string commandText, IDbConnection connection, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, object param = null)
        {
            IEnumerable<T> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<T>(commandText, param, transaction, 
                        buffered, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<T>(commandText, param: param, buffered: buffered, 
                        commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<T>(commandText, param, tran, buffered: buffered, 
                    commandTimeout: commandTimeout, commandType: commandType);
            }

            return result;
        }

        public IEnumerable<dynamic> Query(string commandText, IDbConnection connection, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, object param = null)
        {
            IEnumerable<dynamic> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<dynamic>(commandText, param, transaction,
                        buffered, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<dynamic>(commandText, param: param, buffered: buffered,
                        commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<dynamic>(commandText, param, tran, buffered: buffered,
                    commandTimeout: commandTimeout, commandType: commandType);
            }

            return result;
        }

        public IEnumerable<object> Query(Type type, string commandText, IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null, object param = null)
        {
            IEnumerable<object> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query(type, commandText, param: param, transaction: transaction,
                        buffered: buffered, commandTimeout: commandTimeout, commandType: commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query(type, commandText, param: param, buffered: buffered,
                        commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query(type, commandText, param, tran, buffered: buffered,
                    commandTimeout: commandTimeout, commandType: commandType);
            }

            return result;
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string commandText, Func<TFirst, TSecond, TReturn> map, 
            IDbConnection connection, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", 
            int? commandTimeout = null, CommandType? commandType = null, object param = null)
        {
            IEnumerable<TReturn> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<TFirst, TSecond, TReturn>(commandText, map, param, 
                        transaction, buffered, splitOn, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<TFirst, TSecond, TReturn>(commandText, map, param: param,
                        buffered: buffered, splitOn: splitOn, commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<TFirst, TSecond, TReturn>(commandText, map, param,
                        tran, buffered, splitOn, commandTimeout, commandType);
            }

            return result;
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string commandText,
            Func<TFirst, TSecond, TThird, TReturn> map, IDbConnection connection, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null,
            object param = null)
        {
            IEnumerable<TReturn> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<TFirst, TSecond, TThird, TReturn>(commandText, map, param,
                        transaction, buffered, splitOn, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<TFirst, TSecond, TThird, TReturn>(commandText, map, param: param,
                        buffered: buffered, splitOn: splitOn, commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<TFirst, TSecond, TThird, TReturn>(commandText, map, param,
                        tran, buffered, splitOn, commandTimeout, commandType);
            }

            return result;
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string commandText,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IDbConnection connection, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null,
            object param = null)
        {
            IEnumerable<TReturn> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(commandText, map, param,
                        transaction, buffered, splitOn, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(commandText, map, param: param,
                        buffered: buffered, splitOn: splitOn, commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(commandText, map, param,
                        tran, buffered, splitOn, commandTimeout, commandType);
            }

            return result;
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string commandText,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
            CommandType? commandType = null, object param = null)
        {
            IEnumerable<TReturn> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(commandText, map, param,
                        transaction, buffered, splitOn, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(commandText, map, param: param,
                        buffered: buffered, splitOn: splitOn, commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(commandText, map, param,
                        tran, buffered, splitOn, commandTimeout, commandType);
            }

            return result;
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string commandText,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
            CommandType? commandType = null, object param = null)
        {
            IEnumerable<TReturn> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(commandText, map, param,
                        transaction, buffered, splitOn, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(commandText, map, param: param,
                        buffered: buffered, splitOn: splitOn, commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(commandText, map, param,
                        tran, buffered, splitOn, commandTimeout, commandType);
            }

            return result;
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string commandText,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
            CommandType? commandType = null, object param = null)
        {
            IEnumerable<TReturn> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(commandText, map, param,
                        transaction, buffered, splitOn, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(commandText, map, param: param,
                        buffered: buffered, splitOn: splitOn, commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(commandText, map, param,
                        tran, buffered, splitOn, commandTimeout, commandType);
            }

            return result;
        }

        public IEnumerable<TReturn> Query<TReturn>(string commandText, Type[] types, Func<object[], TReturn> map,
            IDbConnection connection, IDbTransaction transaction = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, object param = null)
        {
            IEnumerable<TReturn> result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<TReturn>(commandText, types, map, param,
                        transaction, buffered, splitOn, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<TReturn>(commandText, types, map, param: param,
                        buffered: buffered, splitOn: splitOn, commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<TReturn>(commandText, types, map, param,
                        tran, buffered, splitOn, commandTimeout, commandType);
            }

            return result;
        }

        public SqlMapper.GridReader QueryMultiple(string commandText, IDbConnection connection,
            IDbTransaction transaction = null, object param = null, int? commandTimeout = null,
                CommandType? commandType = null)
        {
            SqlMapper.GridReader result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.QueryMultiple(commandText, param, transaction, commandTimeout, commandType);
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.QueryMultiple(commandText, param, commandTimeout: commandTimeout, commandType: commandType);
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.QueryMultiple(commandText, param, tran, commandTimeout, commandType);
            }

            return result;
        }

        public T Load<T>(string commandText, IDbConnection connection, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, object param = null) where T : class
        {
            T result = null;

            if (!ClassLibrary.Transaction.Transaction.TransactionStart)
            {
                if (transaction != null)
                {
                    OpenConnection(transaction.Connection);

                    result = transaction.Connection.Query<T>(commandText, param, transaction,
                        buffered, commandTimeout, commandType).FirstOrDefault();
                }
                else if (connection != null)
                {
                    OpenConnection(connection);

                    result = connection.Query<T>(commandText, param: param, buffered: buffered,
                        commandTimeout: commandTimeout, commandType: commandType).FirstOrDefault();
                }
            }
            else
            {
                string connString = connection.ConnectionString;

                if (transaction != null)
                {
                    connString = transaction.Connection.ConnectionString;
                }

                IDbTransaction tran = GetTransaction(connString);

                result = tran.Connection.Query<T>(commandText, param, tran, buffered: buffered,
                    commandTimeout: commandTimeout, commandType: commandType).FirstOrDefault();
            }

            return result;
        }

        private IDbTransaction GetTransaction(string connectionString)
        {
            if (ClassLibrary.Transaction.Transaction.CurrentTransactions == null)
            {
                ClassLibrary.Transaction.Transaction.CurrentTransactions =
                    new Dictionary<string, ClassLibrary.Transaction.Transaction>();
            }

            if (!ClassLibrary.Transaction.Transaction.CurrentTransactions.ContainsKey(connectionString))
            {
                IDbConnection connection = new SqlConnection(connectionString); //CreateConnection(connectionString);
                connection.Open();

                IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Unspecified);

                ClassLibrary.Transaction.Transaction.CurrentTransactions.Add(connectionString,
                    new ClassLibrary.Transaction.CommittableTransaction(transaction));
            }

            return ClassLibrary.Transaction.Transaction.CurrentTransactions[connectionString].DbTransactionWrapper.DbTransaction;
        }
    }
}
