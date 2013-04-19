using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class engineBD
{
    #region private variables (preferences)

    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;
    private string tble = "accounts";

    #endregion

    #region public variables (specify preferences)

    public string table
    {
        get
        {
            return tble;
        }
        set
        {
            try
            {
                if (CheckTableExist(value) == true)
                    tble = value;
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Error when setting table value: " + ex.Message);
            }
        }
    }

    #endregion

    #region functions

    public engineBD()
    {
        Initialize();
    }

    /// <summary>
    /// Initialize values
    /// </summary>
    private void Initialize()
    {
        server = "localhost";
        database = "forcsh";
        uid = "root";
        password = "";
        string connectionString;
        connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
    }

    /// <summary>
    /// check existing of specified table
    /// </summary>
    /// <param name="table">name of table to check</param>
    /// <returns>
    /// true if table exist and false if it is not.
    /// </returns>
    public bool CheckTableExist(string table)
    {
        try
        {
            if (OpenConnection() == true)
            {
                try
                {
                    string query = "SHOW TABLES LIKE '" + table;
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    if (cmd.ExecuteReader().HasRows == true)
                    {
                        CloseConnection();
                        return true;
                    }
                    else
                    {
                        CloseConnection();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Error when checking table: " + ex.Message);
                }
            }
            else
            {
                throw new ArgumentException("Connection error. Check your network: Error to connect to DB");
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Connection error. Check your network: " + ex.Message);
        }
    }

    /// <summary>
    /// open connection to database
    /// </summary>
    /// <returns>
    /// true if connection opened successfully, false if operation was not successfull
    /// </returns>
    public bool OpenConnection()
    {
        try
        {
            if (connection.Ping() == false)
                connection.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    throw new ArgumentException("Ошибка при подключении к базе, проверьте ваше соединение");
                break;

                case 1045:
                    throw new ArgumentException("Не правильный логин или пароль для подключения к базе");
                break;
                default:
                    throw new ArgumentException("Ошибка при подключении к базе:" + ex.Message);
                break;
            }
            return false;
        }
    }

    /// <summary>
    /// close connection to database
    /// </summary>
    /// <returns>
    /// returns true if connection closed succesfully, 
    /// else returns true
    /// </returns>
    public bool CloseConnection()
    {
        try
        {
            if (connection.Ping() == true)
                connection.Close();
            return true;
        }
        catch(Exception ex)
        {
            throw new ArgumentException("Ошибка при закрытии соединения" + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// take value from column with any condition
    /// </summary>
    /// <param name="columnName">name of column from which you need data</param>
    /// <param name="conditionColumnName">name of column, that will be checked</param>
    /// <param name="condition"></param>
    /// <returns> 
    /// Arrat of result strings, that returns DB.
    /// </returns>
    public string[] takeValue(string columnName, string conditionColumnName, string condition)
    {
        string[] result = new string[0];
        MySqlDataReader dataReader = null;
        try
        {
            if (OpenConnection() == true)
            {
                 try
                {
                    string query = "SELECT  `" + columnName +
                                    "` FROM  `" + tble +
                                    "` WHERE " + conditionColumnName + " = '" +
                                    condition + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows == true)
                    {
                        result = new string[dataReader.FieldCount];
                        for (int i = 0; i < result.Length; i++)
                        {
                            dataReader.Read();
                            result[i] = dataReader[columnName].ToString();
                        }
                    }
                    dataReader.Close();
                    CloseConnection();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Error when taking data: " + ex.Message);
                }
            }
            else
            {
                throw new ArgumentException("Connection error. Check your network: Error to connect to DB");
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Connection error. Check your network: " + ex.Message);
        }

    }

    /// <summary>
    /// insert a row to database
    /// </summary>
    /// <param name="login">String to write in login column</param>
    /// <param name="password">nString to write in password column</param>
    /// <returns> 
    /// true if data added successfully, and false if operation was not successfull
    /// </returns>
    public bool addValue(string login, string password)
    {
        try
        {
            if (OpenConnection() == true)
            {
                string query = "INSERT " + database + "." + tble + "(id, login, password) VALUES(NULL, '" +
                    login + "', '" + password + "')";
                MySqlCommand cmd = null;
                try
                {
                    cmd = new MySqlCommand(query, connection);

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        CloseConnection();
                        return false;
                    }
                    else
                    {
                        CloseConnection();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    throw new ArgumentException("Error when adding data: " + ex.Message);
                }
            }
            else
            {
                throw new ArgumentException("Connection error. Check your network: Error to connect to DB");
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Connection error. Check your network: " + ex.Message);
        }
    }

    #endregion
}
