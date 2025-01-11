using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using HW1.Models;
using System.Xml.Linq;
using System.Reflection.PortableExecutable;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, User user)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@userName", user.Name);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@upassword", user.Password);

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        if (paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }

        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method update a USER to the user table 
    //--------------------------------------------------------------------------------------------------
    public User UpdateUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", user.Id);
        paramDic.Add("@userName", user.Name);
        paramDic.Add("@email", user.Email);
        paramDic.Add("@password", user.Password);
        cmd = CreateCommandWithStoredProcedureGeneral("ChangeUsersDetails", con, paramDic);

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return user;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method insert a USER to the users table 
    //--------------------------------------------------------------------------------------------------
    public int InsertUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userName", user.Name); 
        paramDic.Add("@email", user.Email);
        paramDic.Add("@password", user.Password);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_InsertNewUser", con, paramDic);       // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // מחזיר את מס השורות שהושפעו
            return numEffected; // if the SP returnes 0 - this user already exist. 
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }//check

    //--------------------------------------------------------------------------------------------------
    // This method insert a USER to the users table 
    //--------------------------------------------------------------------------------------------------
    public User LogInUserCheck(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", user.Email);
        paramDic.Add("@password", user.Password);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_DetailsCheckForLogIn", con, paramDic);       // create the command

        User u = new User();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read()) //true - חזרו רשומות מהפרוצדורה
            {
                u.Id = Convert.ToInt32(dataReader["id"]);
                u.Name = dataReader["userName"].ToString();
                u.Email = dataReader["email"].ToString();
                u.Password = dataReader["user_password"].ToString(); 
            }
            else // false - לא חזרו רשומות מהפרוצדורה
            {
                return null; // there no rows returned from the SP, means the details log in are NOT correct 
            }
            return u;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method add a GAME to the GamesUser table 
    //--------------------------------------------------------------------------------------------------
    public int AddGameForUser(int gameID, int userID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@GameID", gameID);
        paramDic.Add("@UserID", userID);

        cmd = CreateCommandWithStoredProcedureGeneral("PurchaseGame", con, paramDic);       // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // return num of rows effected
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method delete a GAME from the GamesUser table 
    //--------------------------------------------------------------------------------------------------
    public int DeleteGame(int gameID, int userID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@GameID", gameID);
        paramDic.Add("@UserID", userID);

        cmd = CreateCommandWithStoredProcedureGeneral("DeleteGamePurchase", con, paramDic);       // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // return num of rows effected
            return numEffected; 
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // This method read GAMES from the Games table 
    //---------------------------------------------------------------------------------
    public List<Game> ReadAllGames()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Game> gamesList = new List<Game>();

        cmd = CreateCommandWithStoredProcedureGeneral("SP_ReadGames", con, null);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Game g = new Game();

                g.AppID = Convert.ToInt32(dataReader["AppID"]);
                g.Name = dataReader["Name"].ToString();
                /* g.ReleaseDate = !dataReader.IsDBNull(dataReader.GetOrdinal("Release_date")) &&
                             DateTime.TryParse(dataReader["Release_date"].ToString(), out DateTime releaseDate)
                             ? releaseDate
                             : DateTime.MinValue;

                 */
                g.ReleaseDate = dataReader["Release_date"].ToString();
                g.Price = Convert.ToSingle(dataReader["Price"]);
                g.Description= dataReader["description"].ToString();
                g.HeaderImage = dataReader["Header_image"].ToString();
                g.Website = dataReader["Website"].ToString();
                g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                g.ScoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                g.Recommendation = dataReader["Recommendations"].ToString();
                g.Publisher = dataReader["Developers"].ToString();
                g.NumberOfPurchases = Convert.ToInt32(dataReader["NumberOfPurchases"]);

                gamesList.Add(g);
            }
            return gamesList;
        }
       
        catch (Exception ex)
        {
           throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //---------------------------------------------------------------------------------
    // This method read USERS GAMES (WISH LIST)
    //---------------------------------------------------------------------------------
    public List<Game> ReadUserGames(int userId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Game> gamesList = new List<Game>();

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", userId);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_ReadUserGamesByUserID", con, paramDic);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Game g = new Game();

                g.AppID = Convert.ToInt32(dataReader["AppID"]); ;
                g.Name = dataReader["Name"].ToString();
                g.ReleaseDate = dataReader["Release_date"].ToString();
                g.Price = Convert.ToSingle(dataReader["Price"]);
                g.Description = dataReader["description"].ToString();
                g.HeaderImage = dataReader["Header_image"].ToString();
                g.Website = dataReader["Website"].ToString();
                g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                g.ScoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                g.Recommendation = dataReader["Recommendations"].ToString();
                g.Publisher = dataReader["Developers"].ToString();
                g.NumberOfPurchases = Convert.ToInt32(dataReader["NumberOfPurchases"]);

                gamesList.Add(g);
            }
            return gamesList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //---------------------------------------------------------------------------------
    // This method read GAMES by min PRICE from the Games table 
    //---------------------------------------------------------------------------------
    public List<Game> ReadGamesAbovePrice(float minPrice, int userId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Game> GamesListByPrice = new List<Game>();

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@minPrice", minPrice);
        paramDic.Add("@UserID", userId);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_ReadGamesAbovePrice", con, paramDic);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Game g = new Game();

                g.AppID = Convert.ToInt32(dataReader["AppID"]);
                g.Name = dataReader["Name"].ToString();
                g.ReleaseDate = dataReader["Release_date"].ToString();
                g.Price = Convert.ToSingle(dataReader["Price"]);
                g.Description = dataReader["description"].ToString();
                g.HeaderImage = dataReader["Header_image"].ToString();
                g.Website = dataReader["Website"].ToString();
                g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                g.ScoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                g.Recommendation = dataReader["Recommendations"].ToString();
                g.Publisher = dataReader["Developers"].ToString();
                g.NumberOfPurchases = Convert.ToInt32(dataReader["NumberOfPurchases"]);

                GamesListByPrice.Add(g);
            }
            return GamesListByPrice;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }


    }

    //---------------------------------------------------------------------------------
    // This method read GAMES by min SCORE from the Games table 
    //---------------------------------------------------------------------------------
    public List<Game> ReadGamesAboveScore(int minScore, int userId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Game> GamesListByScore = new List<Game>();

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@minScore", minScore);
        paramDic.Add("@UserID", userId);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_ReadGamesAboveScore", con, paramDic);

        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Game g = new Game();

                g.AppID = Convert.ToInt32(dataReader["AppID"]);
                g.Name = dataReader["Name"].ToString();
                g.ReleaseDate = dataReader["Release_date"].ToString();
                g.Price = Convert.ToSingle(dataReader["Price"]);
                g.Description = dataReader["description"].ToString();
                g.HeaderImage = dataReader["Header_image"].ToString();
                g.Website = dataReader["Website"].ToString();
                g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                g.ScoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                g.Recommendation = dataReader["Recommendations"].ToString();
                g.Publisher = dataReader["Developers"].ToString();
                g.NumberOfPurchases = Convert.ToInt32(dataReader["NumberOfPurchases"]);

                GamesListByScore.Add(g);
            }
            return GamesListByScore;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }


    }

   
}
