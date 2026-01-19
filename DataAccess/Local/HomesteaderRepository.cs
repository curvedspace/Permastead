
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

			-- PROCEDURE
			DROP TABLE IF EXISTS Procedure;
			CREATE TABLE IF NOT EXISTS Procedure(
				Id INTEGER PRIMARY KEY,
				Category VARCHAR (255) NOT NULL,
				Name TEXT NOT NULL,
				Content TEXT NOT NULL,
				AuthorId INTEGER,
				CreationDate TIMESTAMP NOT NULL,
				LastUpdated TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
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

			-- STARTER TYPE
			DROP TABLE IF EXISTS StarterType;
			CREATE TABLE IF NOT EXISTS StarterType(
				Id INTEGER PRIMARY KEY,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description TEXT NOT NULL,		
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId INTEGER
			);

			-- PRESERVATION TYPE
			DROP TABLE IF EXISTS PreservationType;
			CREATE TABLE IF NOT EXISTS PreservationType(
				Id INTEGER PRIMARY KEY,
				Description TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId INTEGER
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
				Address TEXT NULL,
				Comment TEXT NULL,
				Phone VARCHAR (200) NULL,		
				OnSite BOOLEAN,
				Tags TEXT
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
				EventStartDate TIMESTAMP,
				EventEndDate TIMESTAMP,
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
				AuthorId integer,
				ImageStoreId integer,
				Tags TEXT
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


			-- MEASUREMENT TYPE
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

			-- COMMENT TYPE
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

			-- INVENTORY TYPE
			DROP TABLE IF EXISTS InventoryType;
			CREATE TABLE IF NOT EXISTS InventoryType(
				Id INTEGER PRIMARY KEY,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId INTEGER
			);

			-- HARVEST TYPE
			DROP TABLE IF EXISTS HarvestType;
			CREATE TABLE IF NOT EXISTS HarvestType(
				Id INTEGER PRIMARY KEY,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId INTEGER
			);

			-- INVENTORY GROUP
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
				Igroup TEXT,
				Itype TEXT,
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
				AuthorId INTEGER,
				Tags TEXT
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

			-- ANIMAL
			DROP TABLE IF EXISTS Animal;
			CREATE TABLE IF NOT EXISTS Animal (
				Id INTEGER PRIMARY KEY,
				Name TEXT NULL,
				Nickname TEXT NULL,
				Breed TEXT NULL,
				AnimalTypeId INTEGER NULL,
				Birthday TIMESTAMP NULL,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP NOT NULL,
				IsPet BOOLEAN NOT NULL,
				Comment TEXT NULL,
				AuthorId INTEGER NULL,
				Tags TEXT
			);

			-- ANIMAL TYPE
			DROP TABLE IF EXISTS AnimalType;
			CREATE TABLE AnimalType (
				Id INTEGER PRIMARY KEY,
				Code varchar(50) NOT NULL,
				Description TEXT NOT NULL,
				AuthorId INTEGER NULL,
				CreationDate timestamp NULL,
				StartDate timestamp NOT NULL,
				EndDate timestamp NULL
			);

			-- ANIMAL OBSERVATION
			DROP TABLE IF EXISTS AnimalObservation;
			CREATE TABLE IF NOT EXISTS AnimalObservation(
				Id INTEGER PRIMARY KEY,
				AnimalId INTEGER NOT NULL,
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
				PacketCount INTEGER,
				Exchange BOOLEAN,
				Species TEXT,
				PlantId INTEGER,
				VendorId INTEGER,
				AuthorId INTEGER,
				StarterTypeId INTEGER
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
				AuthorId INTEGER
			);


			-- PRESERVATION OBSERVATION
			DROP TABLE IF EXISTS PreservationObservation;
			CREATE TABLE IF NOT EXISTS PreservationObservation(
				Id INTEGER PRIMARY KEY,
				PreservationId INTEGER NOT NULL,
				Comment TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				CommentTypeId INTEGER,
				AuthorId INTEGER
			);

			-- PRESERVATION
			DROP TABLE IF EXISTS Preservation;
			CREATE TABLE Preservation (
				Id INTEGER PRIMARY KEY,
				Description text NOT NULL,
				CreationDate timestamp NULL,
				StartDate timestamp NOT NULL,
				EndDate timestamp NULL,
				Rating INTEGER NULL,
				Measurement INTEGER NULL,
				MeasurementTypeId INTEGER NULL,
				AuthorId INTEGER NULL,
				PreservationTypeId INTEGER NOT NULL,
				Comment text NULL,
				HarvestId INTEGER NULL
			);

			-- HARVEST
			DROP TABLE IF EXISTS Harvest;
			CREATE TABLE Harvest (
				Id INTEGER PRIMARY KEY,
				Description text NOT NULL,
				HarvestTypeId INTEGER NOT NULL,
				HarvestEntityId INTEGER NOT NULL,
				Measurement INTEGER NOT NULL,
				MeasurementTypeId INTEGER NOT NULL,
				Comment text NULL,
				CreationDate timestamp NULL,
				HarvestDate timestamp NOT NULL,
				AuthorId INTEGER NULL
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
            
            q = new Quote { Description = "Short-term results come from intensity. Long-term results come from consistency.", AuthorName = "Shane Parrish" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "The patience you need for big things, is developed by your patience with the little things.", AuthorName = "Kevin Kelly" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "We’ve lost too much of the beauty of life. I believe that we’re being asked to realign ourselves with the earth.", AuthorName = "Janisse Ray" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Education is the kindling of a flame, not the filling of a vessel.", AuthorName = "Socrates" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Prefer knowledge to wealth, for the one is transitory, the other perpetual.", AuthorName = "Socrates" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "If I had my life to live over, I would start barefoot earlier in the spring and stay that way later in the fall.", AuthorName = "Nadine Stair" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "A lack of patience changes the outcome.", AuthorName = "Shane Parrish" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);

            q = new Quote { Description = "Permaculture is an integrated, evolving system of perennial and self-perpetuating plants and animal species useful to man.", AuthorName = "Bill Mollison" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "You don’t have a snail problem, you have a duck deficiency.", AuthorName = "Bill Mollison" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Permaculture challenges what we're doing and thinking - and to that extent it's sedition.", AuthorName = "Bill Mollison" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "I'm certain I don't know what permaculture is. That's what I like about it - it's not dogmatic. But you've got to say it's about the only organized system of design that ever was. And that makes it extremely eerie.", AuthorName = "Bill Mollison" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "When the idea of permaculture came to me, it was like a shift in the brain, and suddenly I couldn't write it down fast enough.", AuthorName = "Bill Mollison" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "If you only do one thing, collect rainwater.", AuthorName = "Bill Mollison" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Nature's book always contains the truth; we must only learn to read it.", AuthorName = "Sepp Holzer" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "None of the greatest assets of a farm is the sheer ecstasy of life.", AuthorName = "Joel Salatin" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Know you food, know your farmers, and know your kitchen.", AuthorName = "Joel Salatin" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "We ask for too much salvation by legislation. All we need to do is empower individuals with the right philosophy and the right information to opt out en masse.", AuthorName = "Joel Salatin" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "While we wait for life, life passes.", AuthorName = "Seneca" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "It's not that we have little time, but more that we waste a good deal of it.", AuthorName = "Seneca" };
            QuoteRepository.Insert(DataConnection.GetLocalDataSource(), q);
            
            q = new Quote { Description = "Time discovers truth.", AuthorName = "Seneca" };
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
            
            ct = new CommentType { Code = "YIR", Description = "Year In Review", Author = new Person(1) };
            CommentTypeRepository.Insert(ct);
            
            ct = new CommentType { Code = "A", Description = "Animal", Author = new Person(1) };
            CommentTypeRepository.Insert(ct);
            
            ct = new CommentType { Code = "S", Description = "Seeds", Author = new Person(1) };
            CommentTypeRepository.Insert(ct);
            
            ct = new CommentType { Code = "C", Description = "Contacts", Author = new Person(1) };
            CommentTypeRepository.Insert(ct);
            
            ct = new CommentType { Code = "PG", Description = "Planting", Author = new Person(1) };
            CommentTypeRepository.Insert(ct);
            
            ct = new CommentType { Code = "PR", Description = "Preservation", Author = new Person(1) };
            CommentTypeRepository.Insert(ct);
            
            ct = new CommentType { Code = "I", Description = "Inventory", Author = new Person(1) };
            CommentTypeRepository.Insert(ct);

			#endregion

			#region Vendor

			var v = new Vendor { Code = "HM", Description = "Homemade", Rating = 0 };
			VendorRepository.Insert(v);
			
			v = new Vendor { Code = "UNKNOWN", Description = "Unknown", Rating = 0 };
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
            
            mu = new MeasurementUnit { Code = "item", Description = "Item(s)", Author = new Person(1) };
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
            
            var p = new Plant { Author = new Person(1), Code = "AGRIM", Description = "Agrimony", Family = "Rosaceae", Tags = "herb astringent bitter diuretic hepatic vulnerary anti-inflammatory",  Comment = "Agrimony herb, scientifically known as Agrimonia eupatoria, is a perennial herbaceous plant belonging to the Rosaceae family. It is native to temperate regions of the Northern Hemisphere and is used for its medicinal properties. Agrimony is traditionally used for conditions affecting the digestive system and liver, as well as for mucosal health and skin issues."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ALDER", Description = "Alder", Family = "Betuloideae", Tags = "herb bitter tree nitrogen-fixer",  Comment = "Alders are trees of the genus Alnus in the birch family Betulaceae. The genus includes about 35 species of monoecious trees and shrubs, a few reaching a large size, distributed throughout the north temperate zone with a few species extending into Central America, as well as the northern and southern Andes. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ALLHEAL", Description = "Allheal", Family = "Lamiaceae", Tags = "herb",  Comment = "Prunella vulgaris, the common self-heal, heal-all, woundwort, heart-of-the-earth, carpenter's herb, brownwort or blue curls, is a herbaceous plant in the mint family Lamiaceae. Prunella vulgaris is edible. The young leaves and stems can be eaten raw in salads; the plant as a whole can be boiled and eaten as a leaf vegetable; and the aerial parts of the plant can be powdered and brewed in a cold infusion to make a beverage. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ALMOND", Description = "Almond", Family = "Rosaceae", Tags = "herb",  Comment = "The almond (Prunus amygdalus, syn. Prunus dulcis) is a species of tree from the genus Prunus. Along with the peach, it is classified in the subgenus Amygdalus, distinguished from the other subgenera by corrugations on the shell (endocarp) surrounding the seed. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ALOEV", Description = "Aloe Vera", Family = "Asphodelaceae", Tags = "herb",  Comment = "Aloe vera is a succulent plant species of the genus Aloe. It is widely distributed, and is considered an invasive species in many world regions. An evergreen perennial, it originates from the Arabian Peninsula, but also grows wild in tropical, semi-tropical, and arid climates around the world. It is cultivated for commercial products, mainly as a topical treatment used over centuries. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ANGELICA", Description = "Angelica", Family = "Apiaceae", Tags = "herb",  Comment = "Angelica archangelica, commonly known as angelica, garden angelica, wild celery, and Norwegian angelica, is a biennial plant from the family Apiaceae, a subspecies of which is cultivated for its sweetly scented edible stems and roots. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ANISEHYS", Description = "Anise Hyssop", Family = "Lamiaceae", Tags = "herb pollinator",  Comment = "Anise hyssop, Agastache foeniculum, is a short-lived herbaceous perennial with blue flowers and fragrant foliage that can be used as an ornamental or in the herb garden. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "APPLE", Description = "Apple Tree", Family = "Rosaceae", Tags = "tree",  Comment = "An apple is a round, edible fruit produced by an apple tree (Malus spp., among them the domestic or orchard apple; Malus domestica). Apple trees are cultivated worldwide and are the most widely grown species in the genus Malus. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ARNICA", Description = "Arnica", Family = "Asteraceae", Tags = "herb",  Comment = "Arnica is a genus of perennial, herbaceous plants in the sunflower family (Asteraceae). The genus name Arnica may be derived from the Greek arni, \"lamb\", in reference to the plants' soft, hairy leaves."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ARUGULA", Description = "Arugula" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "UNK", Description = "Unknown" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "PEPPER", Description = "Pepper", Family = "Solanaceae", Comment = "The terms bell pepper (US, Canada, Philippines), pepper or sweet pepper (UK, Ireland, Canada, South Africa, Zimbabwe), and capsicum (Australia, Bangladesh, India, Malaysia, New Zealand, Pakistan and Sri Lanka) are often used for any of the large bell-shaped peppers, regardless of their color. The fruit is simply referred to as a \"pepper\", or additionally by color (\"green pepper\" or red, yellow, orange, purple, brown, black)."};
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

            p = new Plant { Author = new Person(1), Code = "OREGANO", Description = "Oregano", Tags = "herb"};
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "CATNIP", Description = "Catnip", Tags = "herb"};
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "SQUASH", Description = "Squash" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "MINT", Description = "Mint", Tags = "herb"};
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "CHIVE", Description = "Chives" };
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "YARROW", Description = "Yarrow", Tags = "herb bitter"};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "APPLE", Description = "Apple Tree", Tags = "tree" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "RASP", Description = "Raspberry" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "CHERRY", Description = "Cherry Tree", Tags = "tree" }; 
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "RHUB", Description = "Rhubarb" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "TOMATO", Description = "Tomato" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "GARLIC", Description = "Garlic", Tags = "herb warming anthelmintic alterative anti-septic anti-spasmodic carminative diaphoretic tonic vulnerary"};
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

            p = new Plant { Author = new Person(1), Code = "CURRANT", Description = "Currant" };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "WWOOD", Description = "Wormwood", Family = "Asteraceae", Comment = "Wormwood is characterized by its aromatic, herbaceous, perennial nature and is known for its bitter taste and strong sage-like odor when crushed.", Tags = "herb bitter anthelmintic"};
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
				DueDate = DateTime.Today.AddDays(28),
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
            
            #region StarterTypes
            
            var st = new StarterType() { Code = "SEED", Description = "Seeds" };
            StarterTypeRepository.Insert(st);
            
            st = new StarterType() { Code = "BROOT", Description = "Bare Root" };
            StarterTypeRepository.Insert(st);
            
            st = new StarterType() { Code = "CUT", Description = "Cutting" };
            StarterTypeRepository.Insert(st);
            
            st = new StarterType() { Code = "PURCHPLNT", Description = "Purchased Plant" };
            StarterTypeRepository.Insert(st);
            
            st = new StarterType() { Code = "SAPL", Description = "Sapling" };
            StarterTypeRepository.Insert(st);
            
            #endregion
            
            #region AnimalTypes
            
            var animalType = new AnimalType() { Code = "CAT", Description = "Cat" };
            AnimalTypeRepository.Insert(animalType);
            
            animalType = new AnimalType() { Code = "DUCK", Description = "Duck" };
            AnimalTypeRepository.Insert(animalType);
            
            animalType = new AnimalType() { Code = "DOG", Description = "Dog" };
            AnimalTypeRepository.Insert(animalType);
            
            animalType = new AnimalType() { Code = "COW", Description = "Cow" };
            AnimalTypeRepository.Insert(animalType);
            
            animalType = new AnimalType() { Code = "GOOSE", Description = "Goose" };
            AnimalTypeRepository.Insert(animalType);
            
            animalType = new AnimalType() { Code = "CHICK", Description = "Chicken" };
            AnimalTypeRepository.Insert(animalType);
            
            animalType = new AnimalType() { Code = "GOAT", Description = "Goat" };
            AnimalTypeRepository.Insert(animalType);
            
            animalType = new AnimalType() { Code = "FISH", Description = "Fish" };
            AnimalTypeRepository.Insert(animalType);
            
            animalType = new AnimalType() { Code = "PIG", Description = "Pig" };
            AnimalTypeRepository.Insert(animalType);
            
            animalType = new AnimalType() { Code = "RABBIT", Description = "Rabbit" };
            AnimalTypeRepository.Insert(animalType);
            
            #endregion
            
            #region PreservationTypes
            
            var pType = new FoodPreservationType() { Description = "Canning" };
            PreservationTypeRepository.Insert(pType);
            
            pType = new FoodPreservationType() { Description = "Fermentation" };
            PreservationTypeRepository.Insert(pType);
            
            pType = new FoodPreservationType() { Description = "Pickling" };
            PreservationTypeRepository.Insert(pType);
            
            pType = new FoodPreservationType() { Description = "Freezing" };
            PreservationTypeRepository.Insert(pType);
            
            pType = new FoodPreservationType() { Description = "Dehydration" };
            PreservationTypeRepository.Insert(pType);
            
            pType = new FoodPreservationType() { Description = "Tincture" };
            PreservationTypeRepository.Insert(pType);
            
            
            #endregion
            
            #region HarvestTypes
            
            var harvestType = new HarvestType() { Description = "Plant" };
            HarvestTypeRepository.Insert(harvestType);
            
            harvestType = new HarvestType() { Description = "Animal" };
            HarvestTypeRepository.Insert(harvestType);
            
            harvestType = new HarvestType() { Description = "Materials" };
            HarvestTypeRepository.Insert(harvestType);
            
            harvestType = new HarvestType() { Description = "Other" };
            HarvestTypeRepository.Insert(harvestType);
            
            #endregion
            
            #region SeedPacket
            
            var sp = new SeedPacket() { Description = "Not Available", Instructions = "Not available", Author = Person.Gaia(), 
	            Vendor = new Vendor(1), Seasonality = new Seasonality(1),  Plant = new Plant(1)};
            SeedPacketRepository.Insert(sp);
            
            #endregion

            #region Events
            
            // adding in Celtic moons as holidays, obviously these can be replaced with another system should someone want

            var startDate = new DateTime(DateTime.Today.Year, 11, 24);
            var year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            var e = new AnEvent()
            {
	            Description = "Elder Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 12, 23), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
        
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 12, 24);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Birch Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year+1, 1, 20), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 1, 21);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Rowan Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 2, 17), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 2, 18);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Ash Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 3, 17), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 3, 18);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Alder Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 4, 14), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);

            startDate = new DateTime(DateTime.Today.Year, 4, 15);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Willow Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 5, 12), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 5, 13);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Hawthorn Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 6, 9), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 6, 10);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Oak Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 7, 7), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 7, 8);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Holly Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 8, 4), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 8, 5);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Hazel Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 9, 1), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 9, 2);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Vine Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 9, 29), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 9, 30);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Ivy Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 10, 27), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            startDate = new DateTime(DateTime.Today.Year, 10, 28);
            year = startDate >= DateTime.Today ? startDate.Year - 1 : startDate.Year;
            startDate = new DateTime(year, startDate.Month, startDate.Day);
            e = new AnEvent()
            {
	            Description = "Reed Moon", AnEventType = new AnEventType() { Id = 7 },
	            Frequency = new Frequency(1), EventStartDate = startDate,
	            EventEndDate = new DateTime(year, 11, 23), LastTriggerDate = startDate,
	            Assignee = new Person(1), Assigner = new Person(1)
            };
            AnEventRepository.Insert(e);
            
            #endregion
            
		}
    }
}
