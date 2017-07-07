using Extensions.Enums;
using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;

namespace Extensions.DatabaseHelp
{
    /// <summary>Provides an extension into SQLite commands.</summary>
    public static class SQLite
    {
        /// <summary>This method fills a DataSet with data from a table.</summary>
        /// <param name="sql">SQL query to be executed</param>
        /// <param name="con">Connection information</param>
        /// <returns>Returns DataSet with queried results</returns>
        public static async Task<DataSet> FillDataSet(string sql, string con) => await FillDataSet(
            new SQLiteCommand { CommandText = sql }, con);

        /// <summary>This method fills a DataSet with data from a table.</summary>
        /// <param name="cmd">SQLite command to be executed</param>
        /// <param name="con">Connection information</param>
        /// <returns>Returns DataSet with queried results</returns>
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
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        new Notification(ex.Message, "Error Filling DataSet", NotificationButtons.OK).ShowDialog();
                    });
                }
                finally
                {
                    connection.Close();
                }
            });
            return ds;
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
                    catch (Exception ex)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            new Notification(ex.Message, "Error Executing Command", NotificationButtons.OK)
                                .ShowDialog();
                        });
                    }
                    finally
                    {
                        connection.Close();
                    }
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    new Notification("Connection string cannot be empty!", "Cannot Connect To Database",
                        NotificationButtons.OK).ShowDialog();
                });
            }
            return success;
        }
    }
}