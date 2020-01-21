using Extensions.DataTypeHelpers;
using Extensions.Enums;
using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;

namespace Extensions.DatabaseHelp
{
    /// <summary>Provides an extension into SQLite commands.</summary>
    public static class SQLiteHelper
    {
        /// <summary>This method fills a DataSet with data from a table.</summary>
        /// <param name="con">Connection information</param>
        /// <param name="sql">SQL query to be executed</param>
        /// <returns>Returns DataSet with queried results</returns>
        public static Task<DataSet> FillDataSet(string con, string sql) => FillDataSet(con, new SQLiteCommand { CommandText = sql });

        /// <summary>This method fills a DataSet with data from a table.</summary>
        /// <param name="con">Connection information</param>
        /// <param name="cmd">SQLite command to be executed</param>
        /// <returns>Returns DataSet with queried results</returns>
        public static async Task<DataSet> FillDataSet(string con, SQLiteCommand cmd)
        {
            DataSet ds = new DataSet();
            SQLiteConnection connection = new SQLiteConnection(con);
            cmd.Connection = connection;
            await Task.Run(() =>
            {
                try
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(ds);
                }
                catch (SQLiteException ex)
                {
                    Application.Current.Dispatcher.Invoke(() => new Notification(ex.Message, "Error Filling DataSet", NotificationButton.OK).ShowDialog());
                }
                finally
                {
                    connection.Close();
                }
            }).ConfigureAwait(false);
            return ds;
        }

        /// <summary>This method fills a DataSet with data from a table.</summary>
        /// <param name="cmd">SQLite command to be executed</param>
        /// <param name="con">Connection information</param>
        /// <returns>Returns DataSet with queried results</returns>
        [Obsolete("This method has its parameters backwards. Use FillDataSet(string con, SQLiteCommand cmd) instead.", true)]
        public static async Task<DataSet> FillDataSet(SQLiteCommand cmd, string con)
        {
            DataSet ds = new DataSet();
            SQLiteConnection connection = new SQLiteConnection(con);
            cmd.Connection = connection;
            await Task.Run(() =>
            {
                try
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(ds);
                }
                catch (SQLiteException ex)
                {
                    Application.Current.Dispatcher.Invoke(() => new Notification(ex.Message, "Error Filling DataSet", NotificationButton.OK).ShowDialog());
                }
                finally
                {
                    connection.Close();
                }
            }).ConfigureAwait(false);
            return ds;
        }

        /// <summary>Gets the next index from the SQLITE_SEQUENCE table for a passed table's autoincrement value</summary>
        /// <param name="connectionString">Connection string for the database</param>
        /// <param name="tableName">Name of the table whose autoincrement value being requested</param>
        /// <returns>Autoincrement value being requested</returns>
        public static async Task<int> GetNextIndex(string connectionString, string tableName)
        {
            DataSet ds = await FillDataSet(connectionString, $"SELECT * FROM SQLITE_SEQUENCE WHERE [name] = '{tableName}'");

            return ds.Tables[0].Rows.Count > 0 ? Int32Helper.Parse(ds.Tables[0].Rows[0]["seq"]) + 1 : 1;
        }

        /// <summary>Executes commands.</summary>
        /// <param name="con">Connection information</param>
        /// <param name="commands">Commands to be executed</param>
        /// <returns>Returns true if command(s) executed successfully</returns>
        public static async Task<bool> ExecuteCommand(string con, params SQLiteCommand[] commands)
        {
            bool success = false;
            if (!string.IsNullOrWhiteSpace(con))
            {
                SQLiteConnection connection = new SQLiteConnection(con);

                await Task.Run(() =>
                {
                    try
                    {
                        connection.Open();
                        foreach (SQLiteCommand command in commands)
                        {
                            command.Connection = connection;
                            command.Prepare();
                            command.ExecuteNonQuery();
                        }
                        success = true;
                    }
                    catch (SQLiteException ex)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            new Notification(ex.Message, "Error Executing Command", NotificationButton.OK)
                                .ShowDialog();
                        });
                    }
                    finally
                    {
                        connection.Close();
                    }
                }).ConfigureAwait(false);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    new Notification("Connection string cannot be empty!", "Cannot Connect To Database",
                        NotificationButton.OK).ShowDialog();
                });
            }
            return success;
        }
    }
}