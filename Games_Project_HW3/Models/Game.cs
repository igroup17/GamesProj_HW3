using System.Runtime.InteropServices;

namespace HW1.Models
{
    public class Game
    {
        int appID;
        string name;
        string releaseDate;
        float price;
        string description;
        string headerImage;
        string website;
        bool windows;
        bool mac;
        bool linux;
        int scoreRank;
        string recommendation;
        string publisher;
        //string image;
        int numberOfPurchases;

        public Game() { }
             

        public Game(int appID, string name, string releaseDate, float price, string description, string headerImage, string website, bool windows, bool mac, bool linux, int scoreRank, string recommendation, string publisher, int numberOfPurchases)
        {
            AppID = appID;
            Name = name;
            ReleaseDate = releaseDate;
            Price = price;
            Description = description;
            HeaderImage = headerImage;
            Website = website;
            Windows = windows;
            Mac = mac;
            Linux = linux;
            ScoreRank = scoreRank;  
            Recommendation = recommendation;
            Publisher = publisher;
            //Image = image;
            NumberOfPurchases = numberOfPurchases; // new
        }

     

        public int AppID { get => appID; set => appID = value; }
        public string Name { get => name; set => name = value; }
        public string ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public float Price { get => price; set => price = value; }
        public string Description { get => description; set => description = value; }
        public string HeaderImage { get => headerImage; set => headerImage = value; }
        public string Website { get => website; set => website = value; }
        public bool Windows { get => windows; set => windows = value; }
        public bool Mac { get => mac; set => mac = value; }
        public bool Linux { get => linux; set => linux = value; }
        public int ScoreRank { get => scoreRank; set => scoreRank = value; }
        public string Recommendation { get => recommendation; set => recommendation = value; }
        public string Publisher { get => publisher; set => publisher = value; }
       // public string Image { get => image; set => image = value; }
        public int NumberOfPurchases { get => numberOfPurchases; set => numberOfPurchases = value; } // new

        public int AddGame(int GameId, int UserId)
        {
            DBservices dbs = new DBservices();
            return dbs.AddGameForUser(GameId, UserId);
        }

         public List<Game> Read()
        {
            // new:
            DBservices dbs = new DBservices();
            return dbs.ReadAllGames();
        }

        // GET GAMES LIST FOR USER ID
        static public List<Game> ReadUserGames(int userId)
        {
            // new:
            DBservices dbs = new DBservices();
            return dbs.ReadUserGames(userId);
        }

        // GET GAMES LIST BY MIN PRICE
        public List<Game> GetByMinPrice(float minPrice, int userId)
        {
            DBservices dbs = new DBservices();
            return dbs.ReadGamesAbovePrice(minPrice, userId);
        }

        // GET GAMES BY MIN SCORE
        public List<Game> GetByMinScore(int minScore, int userId)
        {
            DBservices dbs = new DBservices();
            return dbs.ReadGamesAbovePrice(minScore, userId);
        }


        // DELETE GAME BY ID FROM THE STATIC LIST
        public int DeleteById(int AppID, int UserId)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteGame(AppID, UserId);
        }
    }
}
