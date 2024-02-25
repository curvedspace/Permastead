
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using Models;

namespace DataAccess.Local
{
    public static class HomesteaderRepository
    {
        /// <summary>
        /// Verifies the database either exists or can be created.
        /// </summary>
        /// <param name="forceReset">If true, forces a database recreation.</param>
        /// <returns>True if the local database exists, False if it can't be found or created.</returns>
        public static bool DatabaseExists(bool forceReset = false, bool populateDefaultData = true)
        {
            var rtnValue = false;
            var expectedPath = DataAccess.DataConnection.GetDefaultDatabaseLocation();

            var fi = new FileInfo(expectedPath);

            try
            {
                if (fi.Exists && !forceReset)
                {
                    rtnValue = true;
                }
                else
                {
                    //create a fresh default database in the user's home folder
                    if (fi.Directory != null)
                    {
	                    fi.Directory.Create();
	                    CreateDatabase(populateDefaultData);
	                    rtnValue = true;
                    }
                    else
                    {
	                    rtnValue = false;
                    }
                    
                }
            }
            catch (Exception)
            {
                rtnValue = true;
            }         

            return rtnValue;
        }

        /// <summary>
        /// Creates the Sqlite database, or recreates it if it already exists.
        /// </summary>
        /// <returns></returns>
        public static bool CreateDatabase(bool populateDefaultData)
        {
            var rtnValue = false;

            CreateSchema();
            if (populateDefaultData) CreateData();

            return rtnValue;
        }

        /// <summary>
        /// Creates the Sqlite schema.
        /// </summary>
        private static void CreateSchema()
        {
            var sql = @"-- CODE TABLES SECTION

			-- QUOTES
			DROP TABLE IF EXISTS Quote;
			CREATE TABLE IF NOT EXISTS Quote(
				Id INTEGER PRIMARY KEY,
				Description TEXT NOT NULL,
				AuthorName TEXT,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- SETTINGS
			DROP TABLE IF EXISTS Settings;
			CREATE TABLE IF NOT EXISTS Settings(
				Code TEXT PRIMARY KEY,
				Description TEXT NOT NULL,
				CreationDate TIMESTAMP,
				LastUpdated TIMESTAMP
			);

			-- IMAGE STORE GROUP
			DROP TABLE IF EXISTS ImageStoreGroup;
			CREATE TABLE IF NOT EXISTS ImageStoreGroup(
				Id INTEGER PRIMARY KEY,
				Description TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- IMAGE STORE
			DROP TABLE IF EXISTS ImageStore;
			CREATE TABLE IF NOT EXISTS ImageStore(
				Id INTEGER PRIMARY KEY,
				ImageGroupId INTEGER NULL,
				FileName TEXT NOT NULL,
				ImageBlob BLOB NULL,
				CreationDate TIMESTAMP
			);

			-- LOCATION
			DROP TABLE IF EXISTS Location;
			CREATE TABLE IF NOT EXISTS Location(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description TEXT NOT NULL,
				AuthorId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- FREQUENCY
			DROP TABLE IF EXISTS Frequency;
			CREATE TABLE IF NOT EXISTS Frequency(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description TEXT NOT NULL,
				AuthorId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- HARDINESS ZONE
			DROP TABLE IF EXISTS HardinessZone;
			CREATE TABLE IF NOT EXISTS HardinessZone(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description TEXT NOT NULL,
				AuthorId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- LAND PLOT
			DROP TABLE IF EXISTS LandPlot;
			CREATE TABLE IF NOT EXISTS LandPlot(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description TEXT NOT NULL,
				LocationId INTEGER,
				HardinessZoneId INTEGER,
				AuthorId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- GARDEN BED TYPE
			DROP TABLE IF EXISTS GardenBedType;
			CREATE TABLE IF NOT EXISTS GardenBedType(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description TEXT NOT NULL,
				AuthorId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- GARDEN BED
			DROP TABLE IF EXISTS GardenBed;
			CREATE TABLE IF NOT EXISTS GardenBed(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description TEXT NOT NULL,
				LocationId INTEGER,
				PermacultureZone INTEGER,
				GardenBedTypeId INTEGER,
				AuthorId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- CYCLES
			DROP TABLE IF EXISTS Cycle;
			CREATE TABLE IF NOT EXISTS Cycle(
				Id INTEGER PRIMARY KEY,
				Description TEXT NOT NULL,
				NumberOfDays INTEGER,
				AuthorId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- RECIPE
			DROP TABLE IF EXISTS Recipe;
			CREATE TABLE IF NOT EXISTS Recipe(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description TEXT NOT NULL,
				FeedSourceId integer,
				AuthorId integer,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- FEED SOURCE
			DROP TABLE IF EXISTS FeedSource;
			CREATE TABLE IF NOT EXISTS FeedSource(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- VENDOR
			DROP TABLE IF EXISTS Vendor;
			CREATE TABLE IF NOT EXISTS Vendor(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				Rating INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- INGREDIENT
			DROP TABLE IF EXISTS Ingredient;
			CREATE TABLE IF NOT EXISTS Ingredient(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				Notes TEXT,
				PreferredVendorId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			--EVENT TYPE
			DROP TABLE IF EXISTS EventType;
			CREATE TABLE IF NOT EXISTS EventType(
				Id INTEGER PRIMARY KEY,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			--TODO TYPE
			DROP TABLE IF EXISTS ToDoType;
			CREATE TABLE IF NOT EXISTS ToDoType(
				Id INTEGER PRIMARY KEY,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			--TODO STATUS
			DROP TABLE IF EXISTS ToDoStatus;
			CREATE TABLE IF NOT EXISTS ToDoStatus(
				Id INTEGER PRIMARY KEY,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- RECIPE INGREDIENTS
			DROP TABLE IF EXISTS RecipeIngredient;
			CREATE TABLE IF NOT EXISTS RecipeIngredient(
				Id INTEGER PRIMARY KEY,
				RecipeId integer NOT NULL,
				IngredientId integer NOT NULL,
				CreationDate TIMESTAMP NOT NULL,
				AuthorId integer
			);

			-- RECIPE STEPS
			DROP TABLE IF EXISTS RecipeStep;
			CREATE TABLE IF NOT EXISTS RecipeStep(
				Id INTEGER PRIMARY KEY,
				RecipeId integer NOT NULL,
				StepNumber integer NOT NULL,
				StepDescription TEXT,
				CreationDate TIMESTAMP NOT NULL,
				AuthorId integer
			);

			-- PERSON
			DROP TABLE IF EXISTS Person;
			CREATE TABLE IF NOT EXISTS Person(
				Id INTEGER PRIMARY KEY,
				FirstName VARCHAR (2000) NOT NULL,
				LastName VARCHAR (2000) NOT NULL,
				CreationDate timestamp,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				Company VARCHAR (2000) NULL,
				Email VARCHAR (200) NULL,
				Comment TEXT NULL,
				Phone VARCHAR (200) NULL,		
				OnSite BOOLEAN
			);

			--TODO 
			DROP TABLE IF EXISTS ToDo;
			CREATE TABLE IF NOT EXISTS ToDo(
				Id INTEGER PRIMARY KEY,
				Description TEXT NOT NULL,
				ToDoTypeId INTEGER NOT NULL,
				AssignerId INTEGER,
				AssigneeId INTEGER,
				CreationDate TIMESTAMP NOT NULL,
				StartDate TIMESTAMP NOT NULL,
				ToDoStatusId INTEGER NOT NULL,
				DueDate TIMESTAMP,
				PercentDone INTEGER,
				LastUpdatedDate TIMESTAMP
			);

			--EVENT 
			DROP TABLE IF EXISTS Event;
			CREATE TABLE IF NOT EXISTS Event(
				Id INTEGER PRIMARY KEY,
				Description TEXT NOT NULL,
				EventTypeId INTEGER NOT NULL,
				AssignerId INTEGER,
				AssigneeId INTEGER,
				CreationDate TIMESTAMP NOT NULL,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				FrequencyId INTEGER,
				ToDoTrigger BOOLEAN,
				WarningDays INTEGER,
				LastTriggerDate TIMESTAMP,
				LastUpdatedDate TIMESTAMP
			);

			-- PRODUCT TYPE
			DROP TABLE IF EXISTS ProductType;
			CREATE TABLE IF NOT EXISTS ProductType(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- DELIVERY TYPE
			DROP TABLE IF EXISTS DeliveryStatus;
			CREATE TABLE IF NOT EXISTS DeliveryStatus(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- DELIVERY METHOD
			DROP TABLE IF EXISTS DeliveryMethod;
			CREATE TABLE IF NOT EXISTS DeliveryMethod(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- PRODUCT
			DROP TABLE IF EXISTS Product;
			CREATE TABLE IF NOT EXISTS Product(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				CreationDate TIMESTAMP,
				Price decimal NOT NULL,
				ProductTypeId integer,
				RecipeId integer,
				AuthorId integer
			);


			-- PLANT
			DROP TABLE IF EXISTS Plant;
			CREATE TABLE IF NOT EXISTS Plant(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				CreationDate TIMESTAMP,
				Comment TEXT,
				Family VARCHAR (2000),
				Url VARCHAR (2000),
				AuthorId integer
			);

			-- SEASONALITY
			DROP TABLE IF EXISTS Seasonality;
			CREATE TABLE IF NOT EXISTS Seasonality(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId integer
			);

			-- GUILD
			DROP TABLE IF EXISTS Guild;
			CREATE TABLE IF NOT EXISTS Guild(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId integer
			);

			-- GUILD PLANT
			DROP TABLE IF EXISTS GuildPlant;
			CREATE TABLE IF NOT EXISTS GuildPlant(
				Id INTEGER PRIMARY KEY,
				GuildId integer,
				PlantId integer,
				CreationDate TIMESTAMP
			);


			-- MEASUREMENTTYPE
			DROP TABLE IF EXISTS MeasurementType;
			CREATE TABLE IF NOT EXISTS MeasurementType(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId integer
			);

			-- COMMENTTYPE
			DROP TABLE IF EXISTS CommentType;
			CREATE TABLE IF NOT EXISTS CommentType(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId integer
			);

			-- INVENTORYTYPE
			DROP TABLE IF EXISTS InventoryType;
			CREATE TABLE IF NOT EXISTS InventoryType(
				Id INTEGER PRIMARY KEY,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId INTEGER
			);

			-- INVENTORYGROUP
			DROP TABLE IF EXISTS InventoryGroup;
			CREATE TABLE IF NOT EXISTS InventoryGroup(
				Id INTEGER PRIMARY KEY,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId INTEGER
			);

			-- INVENTORY
			DROP TABLE IF EXISTS Inventory;
			CREATE TABLE IF NOT EXISTS Inventory(
				Id INTEGER PRIMARY KEY,
				Description VARCHAR (2000) NOT NULL,
				InventoryGroupId INTEGER NOT NULL,
				InventoryTypeId INTEGER NOT NULL,
				OriginalValue REAL,
				CurrentValue REAL,
				Quantity INTEGER,
				ForSale INTEGER,
				Brand TEXT,
				Notes TEXT,
				Room TEXT,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				LastUpdated TIMESTAMP NOT NULL,
				AuthorId INTEGER
			);

			-- INVENTORY OBSERVATION
			DROP TABLE IF EXISTS InventoryObservation;
			CREATE TABLE IF NOT EXISTS InventoryObservation(
				Id INTEGER PRIMARY KEY,
				InventoryId INTEGER NOT NULL,
				Comment TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				CommentTypeId INTEGER,
				AuthorId INTEGER
			);

			-- OBSERVATION
			DROP TABLE IF EXISTS Observation;
			CREATE TABLE IF NOT EXISTS Observation(
				Id INTEGER PRIMARY KEY,
				Comment TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				CommentTypeId INTEGER,
				AuthorId INTEGER
			);

			-- ENTITY
			DROP TABLE IF EXISTS Entity;
			CREATE TABLE IF NOT EXISTS Entity(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId integer
			);

			-- CYCLE ENTITY
			DROP TABLE IF EXISTS CycleEntity;
			CREATE TABLE IF NOT EXISTS CycleEntity(
				Id INTEGER PRIMARY KEY,
				CycleId INTEGER NOT NULL,
				EntityId INTEGER NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId integer
			);

			-- SEED PACKET
			DROP TABLE IF EXISTS SeedPacket;
			CREATE TABLE IF NOT EXISTS SeedPacket(
				Id INTEGER PRIMARY KEY,
				Code TEXT,
				Description TEXT NOT NULL,
				Instructions TEXT,
				DaysToHarvest INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				Generations INTEGER,
				SeasonalityId INTEGER,
				Species TEXT,
				PlantId INTEGER,
				VendorId INTEGER,
				AuthorId INTEGER
			);

			-- SEED PACKET OBSERVATION
			DROP TABLE IF EXISTS SeedPacketObservation;
			CREATE TABLE IF NOT EXISTS SeedPacketObservation(
				Id INTEGER PRIMARY KEY,
				SeedPacketId INTEGER NOT NULL,
				Comment TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				CommentTypeId INTEGER,
				AuthorId INTEGER
			);

			-- PLANTING STATE
			DROP TABLE IF EXISTS PlantingState;
			CREATE TABLE IF NOT EXISTS PlantingState(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			--PLANTING
			DROP TABLE IF EXISTS Planting;
			CREATE TABLE IF NOT EXISTS Planting(
				Id INTEGER PRIMARY KEY,
				Description TEXT NOT NULL,
				PlantId INTEGER,
				SeedPacketId INTEGER,
				GardenBedId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				PlantingStateId INTEGER,
				YieldRating INTEGER,
				AuthorId INTEGER,
				Comment TEXT
			);

			-- PLANTING OBSERVATION
			DROP TABLE IF EXISTS PlantingObservation;
			CREATE TABLE IF NOT EXISTS PlantingObservation(
				Id INTEGER PRIMARY KEY,
				PlantingId INTEGER NOT NULL,
				Comment TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				CommentTypeId INTEGER,
				AuthorId INTEGER
			);

			-- PERSON OBSERVATION
			DROP TABLE IF EXISTS PersonObservation;
			CREATE TABLE IF NOT EXISTS PersonObservation(
				Id INTEGER PRIMARY KEY,
				PersonId INTEGER NOT NULL,
				Comment TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				CommentTypeId INTEGER,
				AuthorId INTEGER,
				OnSite INTEGER
			);

			--FERMENTATION
			DROP TABLE IF EXISTS Fermentation;
			CREATE TABLE IF NOT EXISTS Fermentation(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description TEXT NOT NULL,
				RecipeId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				Rating INTEGER,
				Amount INTEGER,
				AmountMeasurementTypeId INTEGER,
				AuthorId INTEGER
			);" +
            "";
            
            using (IDbConnection connection = new SqliteConnection(DataConnection.GetLocalDataSource()))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Fills the database with example data.
        /// </summary>
        private static void CreateData()
        {
            // fill database with default data - use Dapper and objects to populate

            #region Quotes

            var q = new Quote { Description = "Tend to your garden.", AuthorName = "Voltaire" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);

            q = new Quote { Description = "They that dance must pay the piper.", AuthorName = "Scottish Proverb" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);

            q = new Quote { Description = "You can solve all the world's problems in a garden.", AuthorName = "Geoff Lawton" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);

            q = new Quote { Description = "Permaculture gives us a toolkit for moving from a culture of fear and scarcity to one of love and abundance.", AuthorName = "Toby Hemenway" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);

            q = new Quote { Description = "Despite all of our accomplishments we owe our existence to a six-inch layer of topsoil and the fact that it rains.", AuthorName = "Felipe Nalpak" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);

            q = new Quote { Description = "Permaculture is revolution disguised as gardening.", AuthorName = "Mike Feingold" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);

            q = new Quote { Description = "Let your food be your medicine and your medicine be your food.", AuthorName = "Hippocrates" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Information is the critical potential resource. It becomes a resource only when obtained and acted upon.", AuthorName = "Bill Mollison" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Gardening adds years to your life and life to your years.", AuthorName = "Anonymous" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Life begins the day you start a garden.", AuthorName = "Chinese Proverb" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);

            q = new Quote { Description = "Garden as though you will live forever.", AuthorName = "William Kent" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "There are no gardening mistakes, only experiments.", AuthorName = "Janet Kilburn Phillips" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "A society grows great when old men plant trees whose shade they know they shall never sit in.", AuthorName = "Greek Proverb" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "The garden suggests there might be a place where we can meet nature halfway.", AuthorName = "Michael Pollan" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Plant and your spouse plants with you; weed and you weed alone.", AuthorName = "Jean Jacques Rousseau" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "When the world wearies and society fails to satisfy, there is always the garden.", AuthorName = "Minnie Aumonier" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "A garden is always a series of losses set against a few triumphs, like life itself.", AuthorName = "May Sarton" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Gardeners, I think, dream bigger dreams than emperors.", AuthorName = "Mary Cantwell" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "In the spring, at the end of the day, you should smell like dirt.", AuthorName = "Margaret Atwood" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "What is a weed? A plant whose virtues have never been discovered.", AuthorName = "Ralph Waldo Emerson" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "To dwell is to garden.", AuthorName = "Martin Heidegger" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Use plants to bring life.", AuthorName = "Douglas Wilson" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "A lawn is nature under totalitarian rule.", AuthorName = "Michael Pollan" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Use plants to bring life.", AuthorName = "Douglas Wilson" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "I need my friends, I need my house, I need my garden.", AuthorName = "Miranda Richardson" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Debt is a form of slavery.", AuthorName = "Jack Spirko" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "We come from the earth, we return to the earth, and in between, we garden.", AuthorName = "Anonymous" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "When you are standing on the ground, you are really standing on the rooftop of another world.", AuthorName = "Dr. M. Jill Clapperton" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "To forget how to dig the earth and to tend the soil is to forget ourselves.", AuthorName = "Mohandas K. Gandhi" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
           
            q = new Quote { Description = "Buy land, they’re not making it anymore.", AuthorName = "Mark Twain" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            #endregion
            
            #region Settings
            
            SettingsRepository.Insert("FNAME","Homesteadr");
            SettingsRepository.Insert("LNAME","Person");
            SettingsRepository.Insert("HNAME","My Homestead");
            SettingsRepository.Insert("LOC","");
            SettingsRepository.Insert("CTRY","");
            SettingsRepository.Insert("NOSTRPUB","");
            SettingsRepository.Insert("NOSTRPRIV","");
            
            #endregion
	        
            #region ImageGroup
            
            #endregion

            #region FeedSource

            var fs = new FeedSource { Code = "NA", Description = "Not Available" };
            FeedSourceRepository.Insert(fs);

            fs = new FeedSource { Code = "INTERNAL", Description = "Internal" };
            FeedSourceRepository.Insert(fs);

            #endregion

            #region PlantingState

            var ps = new PlantingState() { Code = "SD", Description = "Seedling" };
            PlantingStateRepository.Insert(ps);

            ps = new PlantingState { Code = "IP", Description = "In Pot" };
            PlantingStateRepository.Insert(ps);
            
            ps = new PlantingState { Code = "IG", Description = "In Ground" };
            PlantingStateRepository.Insert(ps);
            
            ps = new PlantingState { Code = "H", Description = "Harvested" };
            PlantingStateRepository.Insert(ps);
            
            ps = new PlantingState { Code = "DEAD", Description = "Deceased" };
            PlantingStateRepository.Insert(ps);

            #endregion
            
            #region Seasonality

            var s = new Seasonality() { Code = "A", Description = "Annual" };
            SeasonalityRepository.Insert(s);

            s = new Seasonality { Code = "B", Description = "Biennial" };
            SeasonalityRepository.Insert(s);
            
            s = new Seasonality { Code = "P", Description = "Perennial" };
            SeasonalityRepository.Insert(s);

            #endregion
            
            #region CommentType

            var ct = new CommentType { Code = "JOURNAL", Description = "Journal", Author = new Person(1) };
            CommentTypeRepository.Insert(ct);

            ct = new CommentType { Code = "NEWS", Description = "News", Author = new Person(1) };
            CommentTypeRepository.Insert(ct);

			#endregion

			#region Vendor

			var v = new Vendor { Code = "UNKNOWN", Description = "Unknown", Rating = 0 };
            VendorRepository.Insert(v);

            #endregion

            #region Person

            var person = new Person { FirstName = "System", LastName = "Agent", OnSite = true };
            PersonRepository.Insert(person);

            person = new Person { FirstName = "Homesteader", LastName = "Person", OnSite = true };
            PersonRepository.Insert(person);

            #endregion

            #region Location

            var loc = new Location { Code = "HOME", Description = "My Homestead", Author = new Person(1) };
            LocationRepository.Insert(loc);

            #endregion

            #region MeasurementUnit

            var mu = new MeasurementUnit { Code = "M", Description = "Metre(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            mu = new MeasurementUnit { Code = "cm", Description = "Centimetre(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            mu = new MeasurementUnit { Code = "mm", Description = "Millimetre(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            mu = new MeasurementUnit { Code = "L", Description = "Litre(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            mu = new MeasurementUnit { Code = "ml", Description = "Millilitre(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            mu = new MeasurementUnit { Code = "g", Description = "Gram(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            mu = new MeasurementUnit { Code = "mg", Description = "Milligram(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            mu = new MeasurementUnit { Code = "gal", Description = "Gallon(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            mu = new MeasurementUnit { Code = "tsp", Description = "Teaspoon(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            mu = new MeasurementUnit { Code = "tbsp", Description = "Tablespoon(s)", Author = new Person(1) };
            MeasurementUnitRepository.Insert(mu);

            #endregion

            #region Inventory
            
            // -- Groups
            var ig = new InventoryGroup { Author = new Person(1), Description = "Unknown" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Electronics" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Cookware" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Dishware" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Drinkware" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Vehicles" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Tools" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Musical Instruments" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Furniture" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Artwork" };
            InventoryGroupRepository.Insert(ig);
            
            ig = new InventoryGroup { Author = new Person(1), Description = "Energy" };
            InventoryGroupRepository.Insert(ig);

            // -- Types
            var iType = new InventoryType { Author = new Person(1), Description = "Unknown" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Computers" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Computer Storage" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Computer Monitor" };
            InventoryTypeRepository.Insert(iType);

            iType = new InventoryType { Author = new Person(1), Description = "Cymbal" };
            InventoryTypeRepository.Insert(iType);

            iType = new InventoryType { Author = new Person(1), Description = "Guitar" };
            InventoryTypeRepository.Insert(iType);

            iType = new InventoryType { Author = new Person(1), Description = "Drums" };
            InventoryTypeRepository.Insert(iType);

            iType = new InventoryType { Author = new Person(1), Description = "Television" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Chair" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Sofa" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Bed" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Painting" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Sculpture" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Car" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Truck" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Bicycle" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Generator" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Solar Panel" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Glass" };
            InventoryTypeRepository.Insert(iType);
            
            iType = new InventoryType { Author = new Person(1), Description = "Plate" };
            InventoryTypeRepository.Insert(iType);
            
            #endregion
            
            #region Plants
            
            var p = new Plant { Author = new Person(1), Code = "UNK", Description = "Unknown" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "PEPPER", Description = "Pepper" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "POTATO", Description = "Potato" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "CORN", Description = "Corn" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "PEA", Description = "Pea" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BEAN", Description = "Bean" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "PARSLEY", Description = "Parsley" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "OREGANO", Description = "Oregano" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "CATNIP", Description = "Catnip" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "SQUASH", Description = "Squash" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "MINT", Description = "Mint" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "CHIVE", Description = "Chives" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "ARUGULA", Description = "Arugula" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "YARROW", Description = "Yarrow" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "APPLE", Description = "Apple Tree" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "RASP", Description = "Raspberry" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "CHERRY", Description = "Cherry Tree" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "RHUB", Description = "Rhubard" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "TOMATO", Description = "Tomato" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "GARLIC", Description = "Garlic" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "CABBAGE", Description = "Cabbage" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "LETTUCE", Description = "Lettuce" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ONION", Description = "Onion" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BASIL", Description = "Basil" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "STRAWBERRY", Description = "Strawberry" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "DILL", Description = "Dill" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "PUMPKIN", Description = "Pumpkin" };
            PlantRepository.Insert(p);

            #endregion

            #region ToDos

            var tdt = new ToDoType { Description = "Unknown" };
            ToDoTypeRepository.Insert(tdt);
			            
            tdt = new ToDoType { Description = "Miscellaneous" };
            ToDoTypeRepository.Insert(tdt);

            tdt = new ToDoType { Description = "Gardening" };
            ToDoTypeRepository.Insert(tdt);

            tdt = new ToDoType { Description = "Household" };
            ToDoTypeRepository.Insert(tdt);

            tdt = new ToDoType { Description = "Business" };
            ToDoTypeRepository.Insert(tdt);

            tdt = new ToDoType { Description = "Educational" };
            ToDoTypeRepository.Insert(tdt);
            
            tdt = new ToDoType { Description = "Financial" };
            ToDoTypeRepository.Insert(tdt);


            var tds = new ToDoStatus { Description = "Unknown" };
            ToDoStatusRepository.Insert(tds);

            tds = new ToDoStatus { Description = "Acknowledged" };
            ToDoStatusRepository.Insert(tds);

            tds = new ToDoStatus { Description = "In Progress" };
            ToDoStatusRepository.Insert(tds);

            tds = new ToDoStatus { Description = "Abandoned" };
            ToDoStatusRepository.Insert(tds);

            tds = new ToDoStatus { Description = "Complete" };
            ToDoStatusRepository.Insert(tds);


			var todo = new ToDo
			{
				Description = "Learn about Permastead",
				Assignee = new Person(2),
				Assigner = new Person(1),
				DueDate = DateTime.Today.AddMonths(1),
				ToDoStatus = new ToDoStatus(1),
				ToDoType = new ToDoType(6),	//educational
				PercentDone = 0
			};
            ToDoRepository.Insert(todo);

            #endregion

            #region Ingredients

            var i = new Ingredient { Code = "WATER", Description = "Water", PreferredVendor = new Vendor(1) };
            IngredientRepository.Insert(i);

            i = new Ingredient { Code = "RAWHONEY", Description = "Raw Honey", PreferredVendor = new Vendor(1) };
            IngredientRepository.Insert(i);

            i = new Ingredient { Code = "YEAST", Description = "Yeast", PreferredVendor = new Vendor(1) };
            IngredientRepository.Insert(i);

            i = new Ingredient { Code = "TSALT", Description = "Table Salt", PreferredVendor = new Vendor(1) };
            IngredientRepository.Insert(i);

            i = new Ingredient { Code = "SEASALT", Description = "Sea Salt", PreferredVendor = new Vendor(1) };
            IngredientRepository.Insert(i);

			#endregion

			#region Recipes

			var r = new Recipe { Code = "NA", Description = "Not Available", Author = new Person(1), FeedSource = new FeedSource(1) };
            RecipeRepository.Insert(r);

            r = new Recipe { Code = "BM", Description = "Blueberry Mead", Author = new Person(2), FeedSource = new FeedSource(2) };
            RecipeRepository.Insert(r);

            r = new Recipe { Code = "PM", Description = "Plain Mead", Author = new Person(2), FeedSource = new FeedSource(2) };
            RecipeRepository.Insert(r);

            r = new Recipe { Code = "BA", Description = "Blueberry Apple Mead", Author = new Person(2), FeedSource = new FeedSource(2) };
            RecipeRepository.Insert(r);

            r = new Recipe { Code = "RM", Description = "Raspberry Mead", Author = new Person(2), FeedSource = new FeedSource(2) };
            RecipeRepository.Insert(r);

            r = new Recipe { Code = "C", Description = "Complexity Mead", Author = new Person(2), FeedSource = new FeedSource(2) };
            RecipeRepository.Insert(r);

            r = new Recipe { Code = "VM", Description = "Voivod Mead", Author = new Person(2), FeedSource = new FeedSource(2) };
            RecipeRepository.Insert(r);

            #endregion

            #region Observations

            var obs = new Observation { Author = new Person(1), AsOfDate = DateTime.Today, Comment = "Permastead is currently in early alpha. Welcome to the journey.", CommentType = new CommentType(2) };
            ObservationRepository.Insert(DataConnection.GetLocalDataSource(), obs);

            #endregion

            #region Guilds

            var guild = new Guild { Code = "3SIS", Description = "Three Sisters", Author = new Person(1) };
            GuildRepository.Insert(guild);

            #endregion

            #region GardenBeds

            var gbt = new GardenBedType { Code = "IG", Description = "In Ground", Author = new Person(1) };
            GardenBedTypeRepository.Insert(gbt);

            gbt = new GardenBedType { Code = "HM", Description = "Hugel Mound", Author = new Person(1) };
            GardenBedTypeRepository.Insert(gbt);

            gbt = new GardenBedType { Code = "RB", Description = "Raised Bed", Author = new Person(1) };
            GardenBedTypeRepository.Insert(gbt);

            gbt = new GardenBedType { Code = "P", Description = "Pot", Author = new Person(1) };
            GardenBedTypeRepository.Insert(gbt);

            var gb = new GardenBed
            {
	            Code = "GRD", Description = "Ground", Author = Person.Gaia(), Type = new GardenBedType(1),
	            PermacultureZone = 0, Location = new Location(1)
            };
            GardenBedRepository.Insert(gb);
            
            #endregion
            
            #region Frequency
            
            var freq = new Frequency() { Code = "Y", Description = "Yearly" };
            FrequencyRepository.Insert(freq);
            
            freq = new Frequency() { Code = "M", Description = "Monthly" };
            FrequencyRepository.Insert(freq);
            
            freq = new Frequency() { Code = "W", Description = "Weekly" };
            FrequencyRepository.Insert(freq);
            
            freq = new Frequency() { Code = "D", Description = "Daily" };
            FrequencyRepository.Insert(freq);
            
            #endregion
            
            #region EventTypes
            
            var et = new AnEventType() { Description = "Unknown" };
            AnEventTypeRepository.Insert(et);
            
            et = new AnEventType() { Description = "Birthday" };
            AnEventTypeRepository.Insert(et);
            
            et = new AnEventType() { Description = "Anniversary" };
            AnEventTypeRepository.Insert(et);
            
            et = new AnEventType() { Description = "Gardening" };
            AnEventTypeRepository.Insert(et);
            
            et = new AnEventType() { Description = "Household" };
            AnEventTypeRepository.Insert(et);
            
            et = new AnEventType() { Description = "Financial" };
            AnEventTypeRepository.Insert(et);
            
            et = new AnEventType() { Description = "Holiday" };
            AnEventTypeRepository.Insert(et);
            
            #endregion
            
            #region SeedPacket
            
            var sp = new SeedPacket() { Description = "Not Available", Instructions = "Not available", Author = Person.Gaia(), Vendor = new Vendor(1), Seasonality = new Seasonality(1),  Plant = new Plant(1)};
            SeedPacketRepository.Insert(sp);
            
            #endregion

        }

    }
}
