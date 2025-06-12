using ACTPhotoIDViewer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTPhotoIDViewer.helpers
{

    public class SQLDataAccess
    {
        public string StringServer { get; set; }
        public string StringTargetServer { get; set; }
        public string StringDatabase { get; set; }
        public string StringTargetDatabase { get; set; }
        public string connString { get; set; }
        public string targetConnString { get; set; }
        public JObject SettingsConfig { get; set; }
        public string FileSettings { get; set; }
        public string sqlCommand { get; set; }
        //public List<int> DoorIn { get; set; }
        //public List<int>  DoorOut { get; set; }
        public string doorInList { get; set; }
        public string doorOutList { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string? stringUser { get; set; }
        public string? stringPassword { get; set; }

        public SQLDataAccess(string fileSettings)
        {
            FileSettings = fileSettings; //IoC.Get<FileLocationViewModel>().FileSettings;

            SettingsConfig = ConfigHelper.LoadConfig(FileSettings);

            StringServer = (string)SettingsConfig["Server"];
            StringDatabase = (string)SettingsConfig["Database"];
            IntegratedSecurity = (bool)SettingsConfig["IntegratedSecurity"];
            //StringTargetServer = (string)SettingsConfig["TargetServer"];
            //StringTargetDatabase = (string)SettingsConfig["TargetDatabase"];

            stringUser = (string)SettingsConfig["UserID"];

            stringPassword = (string)SettingsConfig["Password"];


            if (IntegratedSecurity)
            {
                connString = $"Server={StringServer};Database={StringDatabase}; Integrated Security={IntegratedSecurity}; Encrypt=false;";
                targetConnString = $"Server={StringTargetServer};Database={StringTargetDatabase}; Integrated Security={IntegratedSecurity}; Encrypt=false;";
            }
            else
            {
                //string decryptedPassword = new CryptoHelper().Decrypt(stringPassword);
                //connString = $"Server={StringServer};Database={StringDatabase}; Integrated Security={IntegratedSecurity}; User ID={stringUser}; Password={decryptedPassword}; Encrypt=false;";
                //targetConnString = $"Server={StringTargetServer};Database={StringTargetDatabase}; Integrated Security={IntegratedSecurity}; User ID={stringUser}; Password={decryptedPassword}; Encrypt=false;";
            }



        }





        public async Task TestConnection()
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    //MessageBox.Show("Connection Successful");
                    Console.WriteLine("Connection Successful");

                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Connection Failed due to {ex.Message}");
                    Console.WriteLine($"Connection Failed due to {ex.Message}");
                }
            }
        }

        public async Task TestTargetConnection()
        {
            using (SqlConnection connection = new SqlConnection(targetConnString))
            {
                try
                {
                    connection.Open();
                    //Messag.eBox.Show("Connection Successful");
                    Console.WriteLine("Connection Successful");
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Connection Failed due to {ex.Message}");

                    Console.WriteLine($"Connection Failed due to {ex.Message}");
                    Console.WriteLine($"Creating Database");

                    string masterConnStr = $"Server={StringTargetServer};Database=master;Integrated Security={IntegratedSecurity}; Encrypt=false;";
                    // SQL to create database
                    string createDbSql = $"CREATE DATABASE [{StringTargetDatabase}]";

                    using (SqlConnection conn = new SqlConnection(masterConnStr))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(createDbSql, conn))
                        {
                            cmd.ExecuteNonQuery();
                            Console.WriteLine($"Database '{StringTargetDatabase}' created.");
                        }
                    }

                    // Connection string to the new database
                    string dbConnStr = $"Server={StringTargetServer};Database={StringTargetDatabase};Integrated Security={IntegratedSecurity}; Encrypt=false;";

                    // SQL to create table
                    string createTableSql = @"
                        CREATE TABLE Users (
                        Id INT PRIMARY KEY IDENTITY,
                        UserNumber INT NOT NULL,
                        FirstName NVARCHAR(100),
                        LastName NVARCHAR(100),
                        CardNumber INT,
                        Photo VARBINARY(MAX) NULL
                        )";

                    using (SqlConnection conn = new SqlConnection(dbConnStr))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(createTableSql, conn))
                        {
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Table 'Users' created.");
                        }
                    }

                }
            }

        }

        //public BindableCollection<EventLogModel> GetLogReport(string StartDate, string EndDate)
        //{
        //    DateTime _startDate = StartDate.ToDateTime();
        //    DateTime _endDate = EndDate.ToDateTime() + new TimeSpan(23, 59, 59);

        //    string p1 = _startDate.ToString("yyyy-MM-dd HH:mm:ss");
        //    string p2 = _endDate.ToString("yyyy-MM-dd HH:mm:ss");


        //    sqlCommand = $"Select EventID, [When], TimeStamp, Event, Controller, Door, EventData, OriginalForeName, OriginalSurname from Log " +
        //        $"where [When] between \'{p1}\' and \'{p2}\' " +
        //        $"and ((Event=50) or (Event=52)) " +
        //        $"and (Door in (" + doorInList + ") or Door in (" + doorOutList + ")) " +
        //        //$"and (Door = 1) " + // and (EventData=9)
        //        //$" and ((Door in (1)) or (Door in (1))) " +  //$"and (Door = 1) " + // and (EventData=9)
        //        $"Order by [When]";

        //    BindableCollection<EventLogModel> output = new BindableCollection<EventLogModel>();

        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand(sqlCommand, conn);


        //            conn.Open();

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            output.Clear();

        //            while (reader.Read())
        //            {
        //                EventLogModel row = new EventLogModel()
        //                {
        //                    EventID = (Int32)reader[0],
        //                    When = (DateTime)reader[1],
        //                    TimeStamp = (Int32)reader[2],
        //                    Event = (Int16)reader[3],
        //                    Controller = (Int16)reader[4],
        //                    Door = (Int16)reader[5],
        //                    EventData = (Int32)reader[6],
        //                    OriginalForename = reader[7].ToString(),
        //                    OriginalSurname = reader[8].ToString()
        //                };

        //                output.Add(row);
        //            }

        //            reader.Close();

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    return output;
        //}


        //public ObservableCollection<UserModel> GetUsers(List<String> users)
        //{
        //    ObservableCollection<UserModel> userLists = new ObservableCollection<UserModel>();
        //    foreach (String user in users)
        //    {
        //        sqlCommand = $"Select UserNumber, [Group], UserField1, UserField2, Forename, Surname from Users where UserNumber = {user}";
        //        UserModel output = new UserModel();
        //        using (SqlConnection conn = new SqlConnection(connString))
        //        {
        //            try
        //            {
        //                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
        //                conn.Open();
        //                SqlDataReader reader = cmd.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    output.UserNumber = reader[0].ToString();
        //                    output.UserGroup = GetGroupName(reader[1].ToString());
        //                    output.UserField1 = reader[2].ToString();
        //                    output.UserField2 = reader[3].ToString();
        //                    //output.Name = reader[4].ToString() + " " + reader[5].ToString();
        //                    output.Forename = reader[4].ToString();
        //                    output.Surname = reader[5].ToString();
        //                }
        //                reader.Close();
        //                userLists.Add(output);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //        }
        //    }
        //    return userLists;

        //}

        //public string GetGroupName(string group)
        //{
        //    string groupName = "";
        //    sqlCommand = $"Select Name from UserGroups where [Group No] = {group}";
        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand(sqlCommand, conn);
        //            conn.Open();
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                groupName = reader[0].ToString();
        //            }
        //            reader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    return groupName;
        //}

        public ObservableCollection<UserModel> GetUserstoList()
        {
            ObservableCollection<UserModel> userLists = new ObservableCollection<UserModel>();
            sqlCommand = $"Select  UserNumber, CardNo, Forename, Surname, Photo from Users";

            UserModel output = new UserModel();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        output = new UserModel()
                        {
                            UserNumber = Int32.Parse(reader[0].ToString()),
                            CardNumber = Int32.Parse(reader[1].ToString()),
                            FirstName = reader[2].ToString(),
                            LastName = reader[3].ToString(),
                            Photo = reader[4] as byte[] // Assuming Photo is a byte array in the database
                        };
                        userLists.Add(output);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    Console.WriteLine(ex.Message);
                }
                return userLists;
            }

            //ObservableCollection<UserFileModel> userLists = new ObservableCollection<UserModel>();
            //    foreach (String user in users)
            //    {
            //        sqlCommand = $"Select UserNumber, [Group], UserField1, UserField2, Forename, Surname from Users where UserNumber = {user}";
            //        UserModel output = new UserModel();
            //        using (SqlConnection conn = new SqlConnection(connString))
            //        {
            //            try
            //            {
            //                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
            //                conn.Open();
            //                SqlDataReader reader = cmd.ExecuteReader();
            //                while (reader.Read())
            //                {
            //                    output.UserNumber = reader[0].ToString();
            //                    output.UserGroup = GetGroupName(reader[1].ToString());
            //                    output.UserField1 = reader[2].ToString();
            //                    output.UserField2 = reader[3].ToString();
            //                    //output.Name = reader[4].ToString() + " " + reader[5].ToString();
            //                    output.Forename = reader[4].ToString();
            //                    output.Surname = reader[5].ToString();
            //                }
            //                reader.Close();
            //                userLists.Add(output);
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.Message);
            //            }
            //        }
            //    }
            //    return userLists;


        }

        public async Task UpsertUser(UserModel user)
        {
            string sql = @"
                MERGE INTO Users AS Target
                USING (SELECT @UserNumber AS UserNumber, @CardNumber AS CardNumber, @FirstName AS FirstName, @LastName AS LastName, @Photo AS Photo) AS Source
                    ON Target.UserNumber = Source.UserNumber
                WHEN MATCHED THEN
                    UPDATE SET FirstName = Source.FirstName, LastName = Source.LastName, Photo = Source.Photo
                WHEN NOT MATCHED THEN
                    INSERT (UserNumber, CardNumber, FirstName, LastName, Photo)
                    VALUES (Source.UserNumber, Source.CardNumber, Source.FirstName, Source.LastName, Source.Photo);";

            using (SqlConnection conn = new SqlConnection(targetConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserNumber", user.UserNumber);
                    cmd.Parameters.AddWithValue("@CardNumber", user.CardNumber);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = (object?)user.Photo ?? DBNull.Value;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public UserModel GetUser(string cardNo)
        {
            UserModel user = new UserModel();
            sqlCommand = $"Select UserNumber, CardNumber, FirstName, LastName, Photo from Users where CardNumber = {cardNo}";
            UserModel output = new UserModel();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        output.UserNumber = Int32.Parse(reader[0].ToString());
                        output.CardNumber = Int32.Parse(reader[1].ToString());
                        output.FirstName = reader[2].ToString();
                        output.LastName = reader[3].ToString();
                        output.Photo = reader[4] as byte[]; // Assuming Photo is a byte array in the database
                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }

           
                return output;

        }
    }
}




