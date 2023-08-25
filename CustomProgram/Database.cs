using System;
using System.IO;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// The game database, created using Singleton pattern
    /// </summary>
    public class Database
    {
        
        private SplashKitSDK.Database _database; // The SplashKit Database
        private static Database _instance; // Singleton: the application uses a single instance of the database. ( Global Access )
        private Database()
        {
            _database = SplashKit.OpenDatabase("Game database", "Game database");
            Console.WriteLine("Database connected successfully");
            if (DataExist())
            {
                return;
            }
            Console.WriteLine("Database not initialized");
            try
            {
                InitCellData("DbInitializeSql.txt");
                Console.WriteLine("Database initialized!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static Database GetDatabase()
        {
            if (_instance == null)
                _instance = new Database();
            return _instance;
        }
        
        public QueryResult Query(string sql) => _database.RunSql(sql); // make query for the database
        public void FreeDB() // Free Database and all submitted queries(usually used at the end of the game)
        {
            SplashKit.FreeDatabase(_database);
            SplashKit.FreeAllQueryResults();
            Console.WriteLine("Database disconnected!");
        }
        public bool DataExist() // check to see whether the database contains information about the cells
        {
            QueryResult qr = Query("SELECT COUNT(*) FROM cells");
            return (qr.Successful && qr.QueryColumnForInt(0) > 0);
        }
        public void InitCellData(string filename) // Initialize Cell data information
        {
            StreamReader reader = new StreamReader(filename);
            try
            {
                int count = reader.ReadInteger();
                for (int i = 0; i < count; i++)
                {
                    Query(reader.ReadLine());
                }
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
