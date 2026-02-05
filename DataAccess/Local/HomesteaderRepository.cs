
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
            
            p = new Plant { Author = new Person(1), Code = "ARONIA", Description = "Aronia Bush", Family = "Rosaceae", Tags = "",  Comment = "Aronia is a genus of deciduous shrubs, the chokeberries, in the family Rosaceae native to eastern North America and most commonly found in wet woods and swamps."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ACHOKE", Description = "Artichoke", Family = "Asteraceae", Tags = "",  Comment = "The globe artichoke, also known by the names French artichoke and green artichoke in the U.S., is a variety of a species of thistle cultivated as food."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ARUGULA", Description = "Arugula", Family = "Brassicaceae", Tags = "",  Comment = "Rocket, eruca, or arugula is an edible annual plant in the family Brassicaceae used as a leaf vegetable for its fresh, tart, bitter, and peppery flavor."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ASHWAG", Description = "Ashwagandha", Family = "Solanaceae", Tags = "herb nervine",  Comment = "Ashwagandha, also known as Withania somnifera, is a medicinal herb commonly used in traditional Ayurvedic medicine for its purported health benefits."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ASPARAGUS", Description = "Asparagus", Family = "Asparagaceae", Tags = "",  Comment = "Asparagus (Asparagus officinalis) is a perennial flowering plant native to Eurasia, widely cultivated for its edible young shoots, known as spears."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "ASTRAG", Description = "Astragalus", Family = "Fabaceae", Tags = "herb",  Comment = "Astragalus is a large genus of over 3,000 species of herbs and small shrubs, belonging to the legume family Fabaceae."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "AOLIVE", Description = "Autumn Olive", Family = "Elaeagnaceae", Tags = "nitrogen-fixer",  Comment = "Elaeagnus umbellata is known as Japanese silverberry, umbellata oleaster, autumn olive,autumn elaeagnus, spreading oleaster, autumnberry, or autumn berry."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BALLOONFLWR", Description = "Balloon Flower", Family = "Campanulaceae", Tags = "herb expectorant anti-inflammatory",  Comment = "In traditional medicine, particularly in Chinese and Japanese practices, the root of the balloon flower is used as a key herbal remedy."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BASIL", Description = "Basil", Family = "Lamiaceae", Tags = "herb aromatic",  Comment = "Basil, also called great basil, is a culinary herb of the family Lamiaceae (mints). It is a tender plant, and is used in cuisines worldwide. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BAYBERRY", Description = "Bayberry", Family = "Myricaceae", Tags = "herb astringent anti-inflammatory",  Comment = "Bayberry is a native North American shrub known for its fragrant, aromatic leaves and waxy, blue-gray berries that persist through winter, providing food for birds and other wildlife."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BEAN", Description = "Bean", Family = "Fabaceae", Tags = "nitrogen-fixer", Comment = "A bean is the seed of several plants in the family Fabaceae, which are used as vegetables for human or animal food. "};
            PlantRepository.Insert(p);

            p = new Plant { Author = new Person(1), Code = "BEARDT", Description = "Beardtongue", Family = "Plantaginaceae", Tags = "", Comment = "Penstemon, the beardtongues, is a large genus of roughly 280 species of flowering plants native to North America from northern Canada to Central America. It is the largest genus of flowering plants endemic to North America. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BEEBALM", Description = "Bee Balm", Family = "Lamisceae", Tags = "herb", Comment = "Monarda is a genus of flowering plants in the mint family, Lamiaceae. The genus is endemic to North America. Common names include bergamot, bee balm, horsemint, and oswego tea, the first being inspired by the fragrance of the leaves, which is reminiscent of bergamot orange (Citrus bergamia)." };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BEECH", Description = "Beech Tree", Family = "Fagaceae", Tags = "tree", Comment = "The beech tree, belonging to the genus Fagus, is a deciduous tree native to temperate and subtropical regions of Eurasia and North America, with 14 accepted species divided into two subgenera: Englerianae and Fagus." };
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BEET", Description = "Beet", Family = "Amaranthaceae", Tags = "", Comment = "The beetroot (British English) or beet (North American English) is the taproot portion of a Beta vulgaris subsp. vulgaris plant in the Conditiva Group. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BEGRASS", Description = "Blue Eyed Grass", Family = "Iridaceae", Tags = "", Comment = "Blue-eyed grass is a common name for plants in the genus Sisyrinchium, which belongs to the iris family (Iridaceae)."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BERGAMOT", Description = "Bergamot", Family = "Lamiaceae", Tags = "herb ", Comment = "Monarda fistulosa, the wild bergamot or bee balm, is a wildflower in the mint family Lamiaceae, widespread and abundant as a native plant in much of North America."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BESUSAN", Description = "Black Eyed Susan", Family = "Asteraceae", Tags = "", Comment = "Rudbeckia hirta, commonly called black-eyed Susan, is a North American flowering plant in the family Asteraceae, native to Eastern and Central North America and naturalized in the Western part of the continent as well as in China. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BFBUSH", Description = "Butterfly Bush", Family = "Scrophulariaceae", Tags = "", Comment = "Buddleja, also historically given as Buddlea is a genus comprising over 140 species of flowering plants endemic to Asia, Africa, and the Americas."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BLKBERRY", Description = "Blackberry", Family = "Rosaceae", Tags = "herb ", Comment = "The blackberry is an edible fruit produced by many species in the genus Rubus in the family Rosaceae, hybrids among these species within the subgenus Rubus, and hybrids between the subgenera Rubus and Idaeobatus. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BLOCUST", Description = "Black Locust", Family = "Fabaceae", Tags = "tree ", Comment = "Robinia pseudoacacia, commonly known in its native territory as black locust, is a medium-sized hardwood deciduous tree, belonging to the tribe Robinieae of the legume family Fabaceae."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BLUEBERRY", Description = "Blueberry", Family = "Ericaceae", Tags = "", Comment = "Blueberry is a widely distributed and widespread group of perennial flowering plant with blue or purple berries. "};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BMUSTARD", Description = "Black Mustard", Family = "Brassicaceae", Tags = "", Comment = "Rhamphospermum nigrum, black mustard, is an annual plant native to cooler regions of North Africa, temperate regions of Europe, and parts of Asia."};
            PlantRepository.Insert(p);
            
            p = new Plant { Author = new Person(1), Code = "BONESET", Description = "Boneset", Family = "", Tags = "", Comment = "Eupatorium perfoliatum, known as common boneset or just boneset, is a North American perennial plant in the family Asteraceae. It is a common native to the Eastern United States and Canada, widespread from Nova Scotia to Florida, west as far as Texas, Nebraska, the Dakotas, and Manitoba. "};
            PlantRepository.Insert(p);
            
			p = new Plant { Author = new Person(1), Code = "BORAGE", Description = "Borage", Family = "Boraginaceae", Tags = "", Comment = "Borage is such a sensuous plant with its delicate blue flowers - so beautiful to the eyes, surprisingly refreshing to taste, and so very light in the heart!"};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "BOXWOOD", Description = "Boxwood", Family = "Buxaceae", Tags = "", Comment = "The boxes are native to western and southern Europe, southwest, southern and eastern Asia, Africa, Madagascar, northernmost South America, Central America, Mexico and the Caribbean, with the majority of species being tropical or subtropical; only the European and some Asian species are frost-tolerant. "};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "BSAUSAGE", Description = "Blue Sausage Fruit", Family = "Lardizabalaceae", Tags = "", Comment = "Blue Sausage Fruit is a very unique flowering shrub, growing 13-15 feet tall. Its yellowish-green flowers transform into 6 inch long blue pods, with edible flesh. "};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "BUFFBERRY", Description = "Buffaloberry", Family = "Elaeagnaceae", Tags = "", Comment = "Buffalo berries, scientifically known as Shepherdia, belong to the Elaeagnaceae family. They are native to northern and western North America and are characterized by their dark red berries with white dots, which are rough to the touch and have a rather bitter taste."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "BUNCHBERRY", Description = "Bunchberry", Family = "Cornaceae", Tags = "", Comment = "The Cornaceae, the dogwood family, are a cosmopolitan family of flowering plants in the order Cornales. "};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "BURDOCK", Description = "Burdock", Family = "Asteraceae", Tags = "herb alterative anti-inflammatory bitter hepatic nutritive tonic vulnerary", Comment = "Arctium lappa, commonly called greater burdock, gobō, edible burdock, lappa, beggar's buttons, thorny burr, or happy major is a Eurasian species of plants in the family Asteraceae, cultivated in gardens for its root used as a vegetable."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "BWALNUT", Description = "Black Walnut", Family = "Juglandaceae", Tags = "tree ", Comment = "The black walnut tree (Juglans nigra) is native to central and eastern North America and is valued for its wood and edible nuts. "};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "BWHEAT", Description = "Buckwheat", Family = "Polygonaceae", Tags = "", Comment = "Buckwheat is a flowering plant in the knotweed family Polygonaceae cultivated for its grain-like seeds and as a cover crop."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CABBAGE", Description = "Cabbage", Family = "Brassicaceae", Tags = "", Comment = "Cabbage, comprising several cultivars of Brassica oleracea, is a leafy green, red (purple), or white (pale green) biennial plant grown as an annual vegetable crop for its dense-leaved heads."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CARROT", Description = "Carrot", Family = "Apiaceae", Tags = "", Comment = "The carrot is a root vegetable, typically orange in color, though heirloom variants including purple, black, red, white, and yellow cultivars exist, all of which are domesticated forms of the wild carrot, Daucus carota, native to Europe and Southwestern Asia. "};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CATMINT", Description = "Catmint", Family = "Lamiaceae", Tags = "herb ", Comment = "Nepeta nepetella, common name lesser cat-mint, is a low-growing species of catnip belonging to the family Lamiaceae."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CATNIP", Description = "Catnip", Family = "Lamiaceae", Tags = "herb antispasmodic aromatic bitter diaphoretic ", Comment = "Nepeta cataria, commonly known as catnip and catmint, is a species of the genus Nepeta in the mint family, native to southern and eastern Europe, the Middle East, and Central Asia."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CAULI", Description = "Cauliflower", Family = "Brassicaceae", Tags = "" , Comment = "Cauliflower is one of several vegetables cultivated from the species Brassica oleracea in the genus Brassica, which is in the Brassicaceae (or mustard) family."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CBINE", Description = "Columbine", Family = "Ranunculaceae", Tags = "", Comment = "Aquilegia (common names: granny's bonnet, columbine) is a genus of about 130 species of perennial plants that are found in meadows, woodlands, and at higher elevations throughout the Northern Hemisphere, known for the spurred petals of their flowers."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CHICAB", Description = "Chinese Cabbage", Family = "Brassicaceae", Tags = "", Comment = "Chinese cabbage (Brassica rapa, subspecies pekinensis and chinensis) is either of two cultivar groups of leaf vegetables often used in Chinese cuisine: the Pekinensis Group (napa cabbage) and the Chinensis Group (bok choy)."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CFOOT", Description = "Catsfoot", Family = "Asteraceae", Tags = "herb astringent diuretic ", Comment = "Antennaria dioica is an evergreen, herbaceous perennial plant growing to 10–20 cm tall, with a rosette of basal spoon-shaped leaves 4 cm long, and 1 cm broad at their broadest near the apex; and smaller leaves arranged spirally up the flowering stems. "};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CHARD", Description = "Chard", Family = "Amaranthaceae", Tags = "", Comment = "Chard is a green leafy vegetable."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CHERRY", Description = "Cherry ", Family = "Rosaceae", Tags = "tree ", Comment = "A cherry is the fruit of many plants of the genus Prunus, and is a fleshy drupe (stone fruit)."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CHERVIL", Description = "Chervil", Family = "Apiaceae", Tags = "", Comment = "Chervil, sometimes called French parsley or garden chervil (to distinguish it from similar plants also called chervil), is a delicate annual herb related to parsley."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CHICKWEED", Description = "Chickweed", Family = "Caryophyllaceae", Tags = "", Comment = "Stellaria media, chickweed, is an annual flowering plant in the family Caryophyllaceae."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CHICORY", Description = "Chicory", Family = "Asteraceae", Tags = "herb ", Comment = "Common chicory (Cichorium intybus) is a somewhat woody, perennial herbaceous plant of the family Asteraceae, usually with bright blue flowers, rarely white or pink."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CHIVE", Description = "Chives", Family = "Amaryllidaceae", Tags = "herb ", Comment = "Chives, scientific name Allium schoenoprasum, is a species of flowering plant in the family Amaryllidaceae. A perennial plant, A. schoenoprasum is widespread in nature across much of Eurasia and North America. "};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "GCHIVE", Description = "Garlic Chives", Family = "Amaryllidaceae", Tags = "herb ", Comment = "Garlic chives have been widely cultivated for centuries in East Asia for their culinary value."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CILANTRO", Description = "Cilantro", Family = "Apiaceae", Tags = "herb ", Comment = "Coriander, also known as cilantro is an annual herb in the family Apiaceae."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CLEAVERS", Description = "Cleavers", Family = "Rubiaceae", Tags = "herb astringent diuretic hepatic tonic vulnerary lymphatic ", Comment = "Cleavers, scientifically known as Galium aparine, is an annual, herbaceous plant in the Rubiaceae family, commonly found in temperate regions across Europe, Northern Africa, Asia, and North America."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CLOVER", Description = "Clover", Family = "Fabaceae", Tags = "alterative ", Comment = "Clovers, also called trefoils, are plants of the genus Trifolium (from Latin tres 'three' and folium 'leaf'), consisting of about 300 species of flowering plants in the legume family Fabaceae originating in Europe. "};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CMILE", Description = "Chamomile", Family = "Asteraceae", Tags = "herb ", Comment = "Chamomile (American English) or camomile (British English; see spelling differences) is the common name for several daisy-like plants of the family Asteraceae. "};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CODON", Description = "Codonopsis", Family = "Campanulaceae", Tags = "herb ", Comment = "Codonopsis is a genus of flowering plant in the family Campanulaceae."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "COMFREY", Description = "Comfrey", Family = "Boraginaceae", Tags = "herb anti-inflammatory astringent demulcent vulnerary ", Comment = "Symphytum is a genus of flowering plants in the borage family, Boraginaceae, known by the common name comfrey, from the Latin confervere to 'heal' or literally to 'boil together', referring to uses in ancient traditional medicine."};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "CORN", Description = "Corn", Family = "Poaceae", Tags = "", Comment = "Cereal grain cultivated worldwide, used as food, animal feed, and industrial product." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "COSMOS", Description = "Cosmos", Family = "Asteraceae", Tags = "", Comment = "Annual flowering plant with daisy-like blooms in various colors, attracts pollinators." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "CPOPPY", Description = "California Poppy", Family = "Papaveraceae", Tags = "herb ", Comment = "State flower of California, produces bright orange flowers, used in herbal medicine." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "CRABAPPLE", Description = "Crabapple", Family = "Rosaceae", Tags = "tree ", Comment = "Small ornamental tree producing tart fruit, used in jellies and traditional medicine." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "CUCUMBER", Description = "Cucumber", Family = "Cucurbitaceae", Tags = "", Comment = "Warm-season vine producing crisp edible fruits, used in salads and pickling." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "CURRANT", Description = "Currant", Family = "Ribes", Tags = "", Comment = "Deciduous shrub producing small berries, rich in vitamin C and antioxidants." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "DAISY", Description = "Daisy", Family = "Asteraceae", Tags = "", Comment = "Common wildflower with white petals and yellow center, used in herbal teas." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "DANDE", Description = "Dandelion", Family = "Asteraceae", Tags = "herb bitter drying diuretic hepatic tonic nutritive alterative ", Comment = "All parts used medicinally, roots for liver support, leaves as diuretic." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "DELPH", Description = "Delphinium", Family = "Ranunculaceae", Tags = "", Comment = "Tall flowering plant with blue or purple spikes, toxic if ingested." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "DIANTHUS", Description = "Dianthus", Family = "Caryophyllaceae", Tags = "", Comment = "Perennial with fragrant pink or white flowers, used in potpourri and traditional medicine." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "DILL", Description = "Dill", Family = "Apiaceae", Tags = "", Comment = "Annual herb with feathery leaves, used in cooking and for digestive support." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "ECAMPANE", Description = "Elecampane", Family = "Asteraceae", Tags = "herb diaphoretic expectorant bitter ", Comment = "Root used for respiratory support, especially for coughs and bronchitis." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "ECHINACEA", Description = "Echinacea", Family = "Asteraceae", Tags = "herb ", Comment = "Native North American herb used to support immune function and wound healing." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "ECNUT", Description = "Earth Chestnut", Family = "Apiaceae", Tags = "", Comment = "Wild edible plant with chestnut-like flavor, root used as food and medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "ELDBERRY", Description = "Elderberry", Family = "Adoxaceae", Tags = "herb ", Comment = "Shrub with dark berries used in syrups and supplements for immune support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "ELM", Description = "Elm Tree", Family = "Ulmaceae", Tags = "", Comment = "Large deciduous tree historically important for timber, now threatened by Dutch elm disease" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "EPRIM", Description = "Evening Primrose", Family = "Onagraceae", Tags = "herb anti-spasmodic bitter demulcent nutritive hepatic anti-inflammatory ", Comment = "Seed oil rich in gamma-linolenic acid (GLA), used for hormonal balance" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "ERBUD", Description = "Eastern Redbud", Family = "Fabaceae", Tags = "nitrogen-fixer tree ", Comment = "Small ornamental tree with pink flowers in spring, fixes nitrogen in soil" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "FENNEL", Description = "Fennel", Family = "Apiaceae", Tags = "herb carminative ", Comment = "Anise-flavored herb used in cooking and for digestive relief" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "FEVERFEW", Description = "Feverfew", Family = "Asteraceae", Tags = "herb ", Comment = "Traditional herb used for migraine prevention and anti-inflammatory support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "FINDIGO", Description = "False Indigo", Family = "Fabaceae", Tags = "", Comment = "Perennial shrub with blue flowers, historically used as blue dye and medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "FLAX", Description = "Flax", Family = "Linaceae", Tags = "", Comment = "Annual plant producing seeds and fiber, used for oil and textiles." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "FOXGLOVE", Description = "Foxglove", Family = "Plantaginaceae", Tags = "", Comment = "Toxic plant with tall spikes of tubular flowers, source of digitalis for heart medicine." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "FSORREL", Description = "French Sorrel", Family = "Polygonaceae", Tags = "", Comment = "Perennial with sour-tasting leaves, rich in vitamin C and used in salads." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GARLIC", Description = "Garlic", Family = "Amaryllidaceae", Tags = "herb warming alterative anti-septic anti-spasmodic carminative diaphoretic tonic vulnerary anthelmintic ", Comment = "Allium species with strong flavor, used for cardiovascular and immune support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GBURDOCK", Description = "Greater Burdock", Family = "Asteraceae", Tags = "herb alterative anti-inflammatory bitter hepatic nutritive tonic vulnerary diuretic ", Comment = "Root used for skin conditions and detoxification, rich in inulin" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GCHERRY", Description = "Ground Cherry", Family = "Solanaceae", Tags = "", Comment = "Small fruiting plant with edible golden berries, rich in antioxidants" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GELDER", Description = "Ground Elder", Family = "Apiaceae", Tags = "herb", Comment = "Wild plant with large leaves, sometimes confused with deadly nightshade." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GINGER", Description = "Ginger", Family = "Zingiberaceae", Tags = "herb emmenagogue sialagogue rubefacient galactagogue ", Comment = "Rhizome used for nausea, inflammation, and digestive support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GINKGO", Description = "Ginkgo", Family = "Ginkgoaceae", Tags = "", Comment = "Ancient tree species with fan-shaped leaves, used for cognitive support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GINSENG", Description = "Ginseng", Family = "Araliaceae", Tags = "herb ", Comment = "Adaptogenic root used to support energy, stress resilience, and mental clarity" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GKHENRY", Description = "Good King Henry", Family = "Amaranthaceae", Tags = "", Comment = "Wild edible plant with tender leaves, rich in vitamins and minerals" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GKOLA", Description = "Gotu Kola", Family = "Apiaceae", Tags = "herb nervine warming ", Comment = "Climbing herb used for cognitive function and wound healing" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GOJI", Description = "Goji Berry", Family = "Solanaceae", Tags = "herb nutritive ", Comment = "Fruit rich in antioxidants, traditionally used in Chinese medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GOOSEBERRY", Description = "Gooseberry", Family = "Grossulariaceae", Tags = "", Comment = "Shrub producing tart berries, used in jams and traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GOUMI", Description = "Goumi Berry", Family = "Elaeagnaceae", Tags = "", Comment = "Shrub with sweet-tart berries rich in vitamin C and antioxidants" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GRAPE", Description = "Grapes", Family = "Vitaceae", Tags = "", Comment = "Fruit from vine, used for wine, juice, and dried forms like raisins" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "GROD", Description = "Goldenrod", Family = "Asteraceae", Tags = "herb bitter ", Comment = "Native plant with yellow flowers, traditionally used for urinary and respiratory support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HARDYO", Description = "Hardy Orange", Family = "Rutaceae", Tags = "", Comment = "Shrub with fragrant white flowers and orange fruit, used in traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HASKAP", Description = "Haskap", Family = "Caprifoliaceae", Tags = "", Comment = "Shrub with blue-black berries rich in antioxidants and vitamin C" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HAWTHORN", Description = "Hawthorn", Family = "Rosaceae", Tags = "tree ", Comment = "Shrub with red berries, used for cardiovascular support and calming" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HAZNUT", Description = "Hazelnut", Family = "Betulaceae", Tags = "tree ", Comment = "Deciduous tree producing edible nuts, used in cooking and traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HBASIL", Description = "Holy Basil", Family = "Lamiaceae", Tags = "herb warming ", Comment = "Adaptogenic herb used for stress support and immune function" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HBVINE", Description = "Hummingbird Vine", Family = "Bignoniaceae", Tags = "", Comment = "Climbing plant with tubular red flowers, attracts hummingbirds" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HEARTNUT", Description = "Heartnut", Family = "Juglandaceae", Tags = "tree ", Comment = "Cultivated variety of walnut with sweet, easy-to-crack nuts" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HHOCK", Description = "Hollyhock", Family = "Malvaceae", Tags = "", Comment = "Tall flowering plant with showy blooms, used in traditional medicine and gardens" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HHOUND", Description = "Horehound", Family = "Lamiaceae", Tags = "herb ", Comment = "Perennial herb with bitter leaves, used for coughs and respiratory support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HIBFLOWER1", Description = "Hibiscus Flower", Family = "Malvaceae", Tags = "", Comment = "Tropical plant with large flowers, used in teas and traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HKIWI", Description = "Hardy Kiwi", Family = "Actinidiaceae", Tags = "", Comment = "Climbing vine producing small sweet kiwi fruits, cold-hardy variety" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HOCUST", Description = "Honey Locust", Family = "Fabaceae", Tags = "nitrogen-fixer tree ", Comment = "Large tree with thorny branches, produces edible pods and fixes nitrogen" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HONF", Description = "Honesty Flower", Family = "Brassicaceae", Tags = "", Comment = "Annual plant with silvery seed pods, used in dried flower arrangements" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HOPS", Description = "Hops", Family = "Cannabaceae", Tags = "herb ", Comment = "Climbing plant with cone-like flowers, used in beer brewing and herbal medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HOSTA", Description = "Hosta", Family = "Asparagaceae", Tags = "", Comment = "Ornamental plant with large foliage, used in shade gardens" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HRADISH", Description = "Horseradish", Family = "Brassicaceae", Tags = "herb bitter aromatic ", Comment = "Root with pungent flavor, used as condiment and for respiratory support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HTAIL", Description = "Horsetail", Family = "Equisetaceae", Tags = "herb astringent diuretic bitter ", Comment = "Ancient plant with high silica content, used for skin and urinary support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HYACINTH", Description = "Hyacinth", Family = "Asparagaceae", Tags = "", Comment = "Bulbous plant with fragrant flowers, used in perfumery and ornamental gardening" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HYDR", Description = "Hydrangea", Family = "Hydrangeaceae", Tags = "", Comment = "Ornamental shrub with large flower clusters, used in landscaping and floral arrangements" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "HYSSOP", Description = "Hyssop", Family = "Lamiaceae", Tags = "herb ", Comment = "Aromatic herb used for respiratory support and digestive issues" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "IRIS", Description = "Iris", Family = "Iridaceae", Tags = "", Comment = "Perennial with showy flowers, rhizomes used in traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "JINDIGO", Description = "Japanese Indigo", Family = "Polygonaceae", Tags = "", Comment = "Herb used for dye and traditional medicine, rich in antioxidants" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "JLADDER", Description = "Jacob's Ladder", Family = "Polemoniaceae", Tags = "", Comment = "Perennial with delicate white flowers, used in traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "JOSTAB", Description = "Jostaberry", Family = "Grossulariaceae", Tags = "", Comment = "Hybrid of blackcurrant and redcurrant, producing tart berries" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "KALE", Description = "Kale", Family = "Brassicaceae", Tags = "", Comment = "Leafy green vegetable rich in vitamins and antioxidants, used in cooking" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "KOMATSUNA", Description = "Komatsuna", Family = "Brassicaceae", Tags = "", Comment = "Leafy green vegetable similar to spinach, used in Asian cuisine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LAVENDER", Description = "Lavender", Family = "Lamiaceae", Tags = "herb astringent aromatic bitter carminative ", Comment = "Aromatic herb used for calming, sleep support, and skin care" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LBALM", Description = "Lemon Balm", Family = "Lamiaceae", Tags = "", Comment = "Aromatic herb used for calming, digestive support, and sleep" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LEAR", Description = "Lamb's Ear", Family = "Lamiaceae", Tags = "", Comment = "Soft, fuzzy-leaved plant used in gardens and traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LEMGRASS", Description = "Lemongrass", Family = "Poaceae", Tags = "", Comment = "Aromatic grass used in cooking and for digestive and calming support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LETTUCE", Description = "Lettuce", Family = "Brassicaceae", Tags = "", Comment = "Leafy green vegetable used in salads and sandwiches" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LEUCOJUM", Description = "Leucojum", Family = "Amaryllidaceae", Tags = "", Comment = "Bulbous plant with white flowers, toxic if ingested" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LILVALLEY", Description = "Lily of the Valley", Family = "Asparagaceae", Tags = "herb ", Comment = "Perennial with fragrant white flowers, toxic but used in traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LILY", Description = "Lily", Family = "Liliaceae", Tags = "", Comment = "Ornamental plant with large flowers, some species are toxic" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LIMEBALM", Description = "Lime Balm", Family = "Lamiaceae", Tags = "", Comment = "Aromatic herb used for calming, digestive support, and sleep" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LINBERRY", Description = "Lingonberry", Family = "Ericaceae", Tags = "", Comment = "Shrub with tart red berries, used in jams and traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LINDEN", Description = "Linden Tree", Family = "Malvaceae", Tags = "tree herb ", Comment = "Deciduous tree with fragrant flowers used for calming and respiratory support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LKNAPWEED", Description = "Lesser Knapweed", Family = "Asteraceae", Tags = "", Comment = "Perennial with purple flower heads, used in traditional medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LOVAGE", Description = "Lovage", Family = "Apiaceae", Tags = "", Comment = "Herb with celery-like flavor, used in cooking and for digestive support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LQUARTER", Description = "Lamb's Quarter", Family = "Amaranthaceae", Tags = "", Comment = "Wild edible plant with tender leaves, rich in vitamins and minerals" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "LUPINE", Description = "Lupine", Family = "Fabaceae", Tags = "", Comment = "Perennial with colorful flower spikes, fixes nitrogen in soil" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MAPLE", Description = "Maple Tree", Family = "Sapindaceae", Tags = "", Comment = "Deciduous tree known for colorful fall foliage and sweet sap used to make maple syrup" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MARIGOLD", Description = "Marigold", Family = "Asteraceae", Tags = "", Comment = "Bright orange or yellow flowering annual used as ornamental and companion plant" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MARJORAM", Description = "Marjoram", Family = "Lamiaceae", Tags = "herb", Comment = "Sweet aromatic herb similar to oregano, used in Mediterranean cuisine and herbal medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MELON", Description = "Melon", Family = "Cucurbitaceae", Tags = "", Comment = "Sweet fleshy fruit including cantaloupe, honeydew, and watermelon varieties" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MGLORY", Description = "Morning Glory", Family = "Convolvulaceae", Tags = "", Comment = "Fast-growing climbing vine with trumpet-shaped flowers that open in the morning" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MILKWEED", Description = "Milkweed", Family = "Apocynaceae", Tags = "", Comment = "Native perennial with milky sap, essential host plant for monarch butterflies" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MINT", Description = "Mint", Family = "Lamiaceae", Tags = "herb ", Comment = "Aromatic perennial herb with refreshing flavor, spreads vigorously and used in teas and cooking" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MLAR", Description = "Medlar", Family = "Rosaceae", Tags = "", Comment = "Small deciduous tree producing brown fruits eaten when bletted (partially decayed)" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MMALLOW", Description = "Marshmallow", Family = "Malvaceae", Tags = "herb cooling demulcent anti-inflammatory diuretic vulnerary ", Comment = "Mucilaginous herb with soothing properties, root traditionally used to make confection and in herbal medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MMINT", Description = "Mountain Mint", Family = "Lamiaceae", Tags = "herb diaphoretic antispasmodic carminative ", Comment = "Native aromatic herb with minty scent, attracts beneficial insects and used medicinally" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MTHISTLE", Description = "Milk Thistle", Family = "Asteraceae", Tags = "herb ", Comment = "Spiny plant with purple flowers, seeds used for liver support in herbal medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MUGWORT", Description = "Mugwort", Family = "Asteraceae", Tags = "herb bitter ", Comment = "Aromatic perennial with silvery leaves, used in traditional medicine and as culinary herb" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MULBTREE", Description = "Mulberry Tree", Family = "Moraceae", Tags = "tree ", Comment = "Deciduous tree producing sweet edible berries, leaves used to feed silkworms" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MULLEIN", Description = "Mullein", Family = "Scrophulariaceae", Tags = "herb demulcent ", Comment = "Tall biennial with fuzzy leaves and yellow flower spike, used for respiratory support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MUSCARI", Description = "Muscari", Family = "Asparagaceae", Tags = "", Comment = "Small spring bulb with clusters of blue grape-like flowers, also called grape hyacinth" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "MWORT", Description = "Motherwort", Family = "Lamiaceae", Tags = "herb cooling heart ", Comment = "Perennial herb traditionally used for heart health and calming nervous system" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "NIGELLA", Description = "Nigella", Family = "Ranunculaceae", Tags = "", Comment = "Annual plant with delicate flowers and black seeds (black cumin) used as spice and medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "OAK", Description = "Oak Tree", Family = "Fagaceae", Tags = "tree herb ", Comment = "Large deciduous or evergreen tree with acorns, bark used medicinally for astringent properties" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "OAT", Description = "Oat", Family = "Poaceae", Tags = "herb ", Comment = "Cereal grain used for food and animal feed, milky oat tops used as nervine tonic" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "ONION", Description = "Onion", Family = "Amaryllidaceae", Tags = "", Comment = "Bulbous vegetable with pungent flavor and aroma, fundamental ingredient in cooking worldwide" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "OREGANO", Description = "Oregano", Family = "Lamiaceae", Tags = "", Comment = "Aromatic perennial herb with robust flavor, essential in Italian and Mediterranean cuisine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "OSAGEO", Description = "Osage Orange", Family = "Moraceae", Tags = "", Comment = "Thorny tree producing large green brain-like fruits, wood prized for fence posts and bows" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PARSLEY", Description = "Parsley", Family = "Apiaceae", Tags = "", Comment = "Biennial herb with bright green leaves used as garnish and flavoring in cooking" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PARSNIP", Description = "Parsnip", Family = "Apiaceae", Tags = "", Comment = "Root vegetable with sweet nutty flavor, similar to carrot but cream-colored" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PAWPAW", Description = "Pawpaw Tree", Family = "Annonaceae", Tags = "tree ", Comment = "Native North American tree producing large tropical-tasting fruit with custard-like texture" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PEA", Description = "Pea", Family = "Leguminosae", Tags = "nitrogen-fixer ", Comment = "Cool-season legume with edible pods and seeds, fixes nitrogen in soil" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PEACH", Description = "Peach Tree", Family = "Rosaceae", Tags = "", Comment = "Deciduous fruit tree producing fuzzy-skinned sweet stone fruits" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PEANUT", Description = "Peanut", Family = "Fabaceae", Tags = "", Comment = "Legume that develops pods underground, seeds used for oil and protein-rich food" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PEAR", Description = "Pear", Family = "Rosaceae", Tags = "", Comment = "Deciduous tree producing sweet bell-shaped fruits with grainy texture" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PEPPER", Description = "Pepper", Family = "Solanaceae", Tags = "", Comment = "Warm-season vegetable producing sweet or hot fruits in various colors and sizes" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PEPPERMINT", Description = "Peppermint", Family = "Lamiaceae", Tags = "herb ", Comment = "Aromatic hybrid mint with cooling menthol flavor, used in teas, candies, and medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PEYOTE", Description = "Peyote", Family = "Cactaceae", Tags = "", Comment = "Small spineless cactus containing psychoactive alkaloids, used in Native American ceremonies" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PINE", Description = "Pine", Family = "Pinaceae", Tags = "tree herb ", Comment = "Evergreen conifer with needle-like leaves, needles and resin used medicinally" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PLANTAIN", Description = "Plantain", Family = "Plantaginaceae", Tags = "herb vulnerary anti-catarrhal ", Comment = "Common lawn weed with oval leaves, used topically for wounds and stings" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PLUM", Description = "Plum Tree", Family = "Rosaceae", Tags = "tree ", Comment = "Deciduous fruit tree producing smooth-skinned sweet or tart stone fruits" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "POPPY", Description = "Poppy", Family = "Papaveraceae", Tags = "", Comment = "Annual or perennial with showy flowers, some species produce opium alkaloids" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "POTATO", Description = "Potato", Family = "Solanaceae", Tags = "", Comment = "Tuberous crop plant producing starchy edible underground tubers, staple food worldwide" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PRINTREE", Description = "Princess Tree", Family = "Paulowniaceae", Tags = "", Comment = "Fast-growing deciduous tree with large leaves and purple flowers, also called Paulownia" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PRSM", Description = "Persimmon", Family = "Ebenaceae", Tags = "", Comment = "Deciduous tree producing sweet orange fruits that must ripen fully to avoid astringency" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "PUMPKIN", Description = "Pumpkin", Family = "Cucurbitaceae", Tags = "", Comment = "Large orange winter squash with edible flesh and seeds, popular for autumn decoration" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "QALACE", Description = "Queen Anne's Lace", Family = "Apiaceae", Tags = "", Comment = "Wild carrot with white lacy flower clusters, edible root but easily confused with toxic lookalikes" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "RADISH", Description = "Radish", Family = "Brassicaceae", Tags = "", Comment = "Fast-growing root vegetable with crisp peppery flavor, available in many colors and sizes" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "RASP", Description = "Raspberry", Family = "Rosaceae", Tags = "herb ", Comment = "Thorny perennial producing sweet red berries, leaves used as herbal tea for women's health" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "RHODRN", Description = "Rhododendron", Family = "Ericaceae", Tags = "", Comment = "Evergreen or deciduous shrub with showy flower clusters, many species are toxic" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "RHUB", Description = "Rhubarb", Family = "Polygonaceae", Tags = "", Comment = "Perennial with tart edible stalks used in cooking, leaves are toxic due to oxalic acid" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "RMARY", Description = "Rosemary", Family = "Lamiaceae", Tags = "herb ", Comment = "Evergreen aromatic shrub with needle-like leaves, used in cooking and for memory support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "ROSARUG", Description = "Rosa Rugosa", Family = "Rosaceae", Tags = "herb ", Comment = "Hardy rose species with fragrant flowers and large vitamin C-rich hips used medicinally" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "ROWAN", Description = "Rowan", Family = "Rosaceae", Tags = "tree herb ", Comment = "Small tree with white flowers and red berries, also called mountain ash, berries used in preserves" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "RROOT", Description = "Rhodiola", Family = "Crassulaceae", Tags = "adaptogen herb ", Comment = "Arctic root plant used as adaptogen for stress, energy, and mental performance" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "RUE", Description = "Rue", Family = "Rutaceae", Tags = "herb ", Comment = "Bitter aromatic herb with blue-green leaves, used historically in medicine but can cause skin sensitivity" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "RUTAB", Description = "Rutabaga", Family = "Brassicaceae", Tags = "", Comment = "Root vegetable cross between turnip and cabbage with yellow flesh and sweet flavor" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "RWOOD", Description = "Redwood Tree", Family = "Cupressaceae", Tags = "", Comment = "Massive evergreen conifer among tallest trees on earth, native to California coast" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SAGE", Description = "Sage", Family = "Lamiaceae", Tags = "herb ", Comment = "Aromatic perennial herb with gray-green leaves used in cooking and herbal medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SANISE1", Description = "Snow Anise", Family = "Lamiaceae", Tags = "", Comment = "Aromatic herb with anise-flavored leaves, tolerant of cold climates" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SASKBERRY", Description = "Saskatoon Berry", Family = "Rosaceae", Tags = "tree ", Comment = "Deciduous shrub native to North America producing sweet purple berries similar to blueberries" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SAVORY", Description = "Savory", Family = "Lamiaceae", Tags = "", Comment = "Aromatic herb with peppery flavor used in cooking, available in summer and winter varieties" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SCHDRA", Description = "Schisandra", Family = "Schisandraceae", Tags = "herb", Comment = "Woody vine producing red berries with five flavors, used in traditional Chinese medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SCULLCAP", Description = "Scullcap", Family = "Lamiaceae", Tags = "herb", Comment = "Perennial herb with blue flowers traditionally used for calming and nervous system support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SEACEL", Description = "Sea Celery", Family = "Apiaceae", Tags = "", Comment = "Coastal plant with celery-like flavor, grows in salt marshes and used as wild vegetable" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SHIIMUSH", Description = "Shiitake Mushroom", Family = "Omphalotaceae", Tags = "", Comment = "Edible fungus native to East Asia with rich umami flavor, widely cultivated for culinary use" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SHISO", Description = "Shiso", Family = "Lamiaceae", Tags = "herb", Comment = "Japanese aromatic herb with purple or green leaves, used in Asian cuisine and garnishes" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SIBGENG", Description = "Siberian Ginseng", Family = "Araliaceae", Tags = "herb", Comment = "Adaptogenic shrub with spiny stems, root used in herbal medicine for energy and stress support" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SJWART", Description = "Saint John's Wort", Family = "Hypericaceae", Tags = "herb", Comment = "Flowering plant with yellow blooms traditionally used for mood support and wound healing" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SKIRRET", Description = "Skirret", Family = "Apiaceae", Tags = "", Comment = "Perennial root vegetable with sweet-tasting white roots, historically popular in European cuisine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SNAPD", Description = "Snapdragon", Family = "Plantaginaceae", Tags = "", Comment = "Colorful ornamental flower with distinctive snap-like blooms in various colors" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SNETTLE", Description = "Stinging Nettle", Family = "Urticaceae", Tags = "herb nutritive", Comment = "Perennial plant with stinging hairs, highly nutritious and used as food and herbal medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SOLSEAL", Description = "Solomon's seal", Family = "Asparagaceae", Tags = "herb", Comment = "Woodland perennial with arching stems and white bell-shaped flowers, root used in herbal medicine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SORREL", Description = "Sorrel", Family = "Polygonaceae", Tags = "", Comment = "Leafy green vegetable with tart, lemony flavor due to oxalic acid content" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SPEA", Description = "Sweet Pea", Family = "Fabaceae", Tags = "", Comment = "Climbing annual plant with fragrant colorful flowers, grown as ornamental" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SPEARMINT", Description = "Spearmint", Family = "Lamiaceae", Tags = "herb", Comment = "Aromatic perennial mint with sweet flavor, widely used in teas, cooking, and confections" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SPSHUB", Description = "Siberian Pea Shrub", Family = "Fabaceae", Tags = "", Comment = "Hardy nitrogen-fixing shrub with edible pea-like seeds, tolerant of extreme cold" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SQUILL", Description = "Squill ", Family = "Asparagaceae", Tags = "", Comment = "Bulbous plant with blue or white flowers, some species used medicinally and as rodenticide" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "STRAWBERRY", Description = "Strawberry", Family = "Rosaceae", Tags = "", Comment = "Low-growing perennial producing sweet red fruits, widely cultivated worldwide" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SUMSQ", Description = "Summer Squash", Family = "Cucurbitaceae", Tags = "", Comment = "Fast-growing warm-season vegetable harvested immature, includes zucchini and yellow squash varieties" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SUNCHOKE", Description = "Sunchoke", Family = "Asteraceae", Tags = "", Comment = "Perennial sunflower producing edible tubers with nutty flavor, also called Jerusalem artichoke" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "SUNFLOWER", Description = "Sunflower", Family = "Asteraceae", Tags = "", Comment = "Tall annual plant with large yellow flower heads, cultivated for seeds and oil production" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "TARRAGON", Description = "Tarragon", Family = "Asteraceae", Tags = "herb", Comment = "Aromatic perennial herb with anise-like flavor, essential in French cuisine" };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "THYME", Description = "Thyme", Family = "Lamiaceae", Tags = "herb", Comment = "Aromatic herb with small leaves used in cooking and traditional medicine." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "TITH", Description = "Tithonia", Family = "Asteraceae", Tags = "", Comment = "Mexican sunflower with bright orange-red flowers, often used as ornamental plant." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "TOBCO", Description = "Tobacco", Family = "Solanaceae", Tags = "", Comment = "Commercial crop plant cultivated for its leaves which are processed for smoking and other uses." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "TOMATO", Description = "Tomato", Family = "Solanaceae", Tags = "", Comment = "Popular fruit vegetable with red, juicy fruits widely used in cooking worldwide." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "TROCKET", Description = "Turkish Rocket", Family = "Brassicaceae", Tags = "", Comment = "Leafy green vegetable with peppery flavor, similar to arugula." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "TUMERIC", Description = "Tumeric", Family = "Zingiberaceae", Tags = "herb", Comment = "Rhizomatous herbaceous plant with bright yellow-orange root used as spice and natural dye." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "TURNIP", Description = "Turnip", Family = "Brassicaceae", Tags = "", Comment = "Root vegetable with white flesh and purple-tinged skin, used in cooking." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "UNK", Description = "Unknown", Family = "Unknown", Tags = "", Comment = "A plant that you cannot identify."};
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "USNEA", Description = "Usnea", Family = "Parmeliaceae", Tags = "herb astringent vulnerary anti-inflammatory antispasmodic" , Comment = "Usnea is a genus of fruticose lichens in the large family Parmeliaceae. The genus, which currently contains roughly 130 species, was established by Michel Adanson in 1763. "};
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "VALERIAN", Description = "Valerian", Family = "Caprifoliaceae", Tags = "herb aromatic nervine antispasmodic carminative sedative anodyne", Comment = "Common Valerian Root is one of the most well known herbal sedatives. It is commonly found in calming and sleepy time teas. " };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "VERVAIN", Description = "Vervain", Family = "Verbenaceae", Tags = "herb cooling diuretic diaphoretic bitter emetic emmenagogue" , Comment = "Verbena, also known as vervain or verveine, is a genus in the family Verbenaceae. It contains about 150 species of annual and perennial herbaceous or semi-woody flowering plants. "};
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "VIOLET", Description = "Violet Flower", Family = "Violaceae", Tags = "herb cooling anti-inflammatory nutritive", Comment = "Viola tricolor is a common European wild flower, growing as an annual or short-lived perennial." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "WBETONY", Description = "Wood Betony", Family = "Unknown", Tags = "", Comment = "Betonica officinalis, common name betony is a species of flowering plant in the mint family Lamiaceae, native to Europe, western Asia, and northern Africa."};
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "WCRESS", Description = "Watercress", Family = "Brassicaceae", Tags = "", Comment = "Watercress or yellowcress (Nasturtium officinale) is a species of aquatic flowering plant in the cabbage family Brassicaceae. Watercress is a rapidly growing perennial plant native to Europe and Asia." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "WHAZEL", Description = "Witch Hazel", Family = "Hamamelidaceae", Tags = "herb astringent anti-inflammatory vulnerary", Comment = "The witch-hazels are deciduous shrubs or (rarely) small trees growing to 3 to 7.5 m tall, even more rarely to 12 m tall. " };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "WMELON", Description = "Watermelon", Family = "Cucurbitaceae", Tags = "", Comment = "Watermelon (Citrullus lanatus) is a flowering plant species of the Cucurbitaceae family and the name of its edible fruit. A scrambling and trailing vine-like plant, it is a highly cultivated fruit worldwide, with more than 1,000 varieties." };
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "WSORREL", Description = "Wood Sorrel", Family = "Oxalidaceae", Tags = "", Comment = "The Oxalidaceae, or wood sorrel family, are a small family of five genera of herbaceous plants, shrubs and small trees, with the great majority of the 570 species in the genus Oxalis (wood sorrels)."};
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "WSQUASH", Description = "Winter Squash", Family = "Cucurbitaceae", Tags = "", Comment = "Winter squash is an annual fruit representing several squash species within the genus Cucurbita. Late-growing, less symmetrical, odd-shaped, rough or warty varieties, small to medium in size, but with long-keeping qualities and hard rinds, are usually called winter squash. "};
			PlantRepository.Insert(p);

			p = new Plant { Author = new Person(1), Code = "WWOOD", Description = "Wormwood", Family = "Asteraceae", Comment = "Wormwood is characterized by its aromatic, herbaceous, perennial nature and is known for its bitter taste and strong sage-like odor when crushed.", Tags = "herb bitter anthelmintic"};
			PlantRepository.Insert(p);
			
			p = new Plant { Author = new Person(1), Code = "YARROW", Description = "Yarrow", Tags = "herb bitter cooling amphoteric anti-septic astringent aromatic diaphoretic diuretic expectorant hepatic styptic vulnerary", Comment = "Achillea millefolium, or common yarrow, is a flowering plant in the family Asteraceae. Growing to 1 metre (3+1⁄2 feet) tall, it is characterized by small whitish flowers, a tall stem of fernlike leaves, and a pungent odor. "};
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

            var obs = new Observation { Author = new Person(1), AsOfDate = DateTime.Today, Comment = "Permastead is currently in beta testing. Welcome to the journey.", CommentType = new CommentType(2) };
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
            
            freq = new Frequency() { Code = "O", Description = "Once" };
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
            
            pType = new FoodPreservationType() { Description = "Vinegar" };
            PreservationTypeRepository.Insert(pType);
            
            pType = new FoodPreservationType() { Description = "Honey" };
            PreservationTypeRepository.Insert(pType);
            
            
            #endregion
            
            #region HarvestTypes
            
            var harvestType = new HarvestType() { Description = "Plant" };
            HarvestTypeRepository.Insert(harvestType);
            
            harvestType = new HarvestType() { Description = "Animal" };
            HarvestTypeRepository.Insert(harvestType);
            
            harvestType = new HarvestType() { Description = "Materials" };
            HarvestTypeRepository.Insert(harvestType);
            
            harvestType = new HarvestType() { Description = "Energy" };
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
