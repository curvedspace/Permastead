
using System.Data;
using Microsoft.VisualBasic;
using Models;
using Npgsql;

namespace DataAccess.Server
{
    public static class HomesteaderRepository
    {
        /// <summary>
        /// Verifies the database either exists or can be created.
        /// </summary>
        /// <param name="forceReset">If true, forces a database recreation.</param>
        /// <returns>True if the local database exists, False if it can't be found or created.</returns>
        public static bool DatabaseExists(bool forceReset = false)
        {
            var rtnValue = false;

            try
            {
				CreateDatabase();
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
        private static bool CreateDatabase()
        {
            var rtnValue = false;

            try
            {
	            CreateSchema();
	            CreateData();
            }
            catch (Exception e)
            {
	            Console.WriteLine(e);
            }

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
				Id SERIAL PRIMARY KEY,
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
				Id integer primary key generated always as identity,
				Description TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- IMAGE STORE
			DROP TABLE IF EXISTS ImageStore;
			CREATE TABLE IF NOT EXISTS ImageStore(
				Id integer primary key generated always as identity,
				ImageGroupId INTEGER NULL,
				FileName TEXT NOT NULL,
				ImageBlob BYTEA NULL,
				CreationDate TIMESTAMP
			);

			-- LOCATION
			DROP TABLE IF EXISTS Location;
			CREATE TABLE IF NOT EXISTS Location(
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- VENDOR
			DROP TABLE IF EXISTS Vendor;
			CREATE TABLE IF NOT EXISTS Vendor(
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			--TODO TYPE
			DROP TABLE IF EXISTS ToDoType;
			CREATE TABLE IF NOT EXISTS ToDoType(
				Id integer primary key generated always as identity,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			--TODO STATUS
			DROP TABLE IF EXISTS ToDoStatus;
			CREATE TABLE IF NOT EXISTS ToDoStatus(
				Id integer primary key generated always as identity,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- RECIPE INGREDIENTS
			DROP TABLE IF EXISTS RecipeIngredient;
			CREATE TABLE IF NOT EXISTS RecipeIngredient(
				Id integer primary key generated always as identity,
				RecipeId integer NOT NULL,
				IngredientId integer NOT NULL,
				CreationDate TIMESTAMP NOT NULL,
				AuthorId integer
			);

			-- RECIPE STEPS
			DROP TABLE IF EXISTS RecipeStep;
			CREATE TABLE IF NOT EXISTS RecipeStep(
				Id integer primary key generated always as identity,
				RecipeId integer NOT NULL,
				StepNumber integer NOT NULL,
				StepDescription TEXT,
				CreationDate TIMESTAMP NOT NULL,
				AuthorId integer
			);

			-- PERSON
			DROP TABLE IF EXISTS Person;
			CREATE TABLE IF NOT EXISTS Person(
				Id integer primary key generated always as identity,
				FirstName VARCHAR (2000) NOT NULL,
				LastName VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				Company VARCHAR (2000) NULL,
				Email VARCHAR (200) NULL,
				Address TEXT NULL,
				Comment TEXT NULL,
				Phone VARCHAR (200) NULL,
				OnSite BOOLEAN
			);

			--TODO 
			DROP TABLE IF EXISTS ToDo;
			CREATE TABLE IF NOT EXISTS ToDo(
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- DELIVERY TYPE
			DROP TABLE IF EXISTS DeliveryStatus;
			CREATE TABLE IF NOT EXISTS DeliveryStatus(
				Id integer primary key generated always as identity,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- DELIVERY METHOD
			DROP TABLE IF EXISTS DeliveryMethod;
			CREATE TABLE IF NOT EXISTS DeliveryMethod(
				Id integer primary key generated always as identity,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			-- PRODUCT
			DROP TABLE IF EXISTS Product;
			CREATE TABLE IF NOT EXISTS Product(
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
				GuildId integer,
				PlantId integer,
				CreationDate TIMESTAMP
			);


			-- MEASUREMENTTYPE
			DROP TABLE IF EXISTS MeasurementType;
			CREATE TABLE IF NOT EXISTS MeasurementType(
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId INTEGER
			);

			-- INVENTORYGROUP
			DROP TABLE IF EXISTS InventoryGroup;
			CREATE TABLE IF NOT EXISTS InventoryGroup(
				Id integer primary key generated always as identity,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				AuthorId INTEGER
			);

			-- INVENTORY
			DROP TABLE IF EXISTS Inventory;
			CREATE TABLE IF NOT EXISTS Inventory(
				Id integer primary key generated always as identity,
				Description VARCHAR (2000) NOT NULL,
				Igroup TEXT,
				Itype TEXT,
				OriginalValue REAL,
				CurrentValue REAL,	
				Brand TEXT,
				Notes TEXT,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				LastUpdated TIMESTAMP NOT NULL,
				AuthorId INTEGER,
				Room TEXT,
				Quantity INTEGER,
				ForSale BOOLEAN
			);

			-- OBSERVATION
			DROP TABLE IF EXISTS Observation;
			CREATE TABLE IF NOT EXISTS Observation(
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
				Description TEXT NOT NULL,
				Instructions TEXT,
				DaysToHarvest INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				PlantId INTEGER,
				VendorId INTEGER,
				AuthorId INTEGER,
				Code TEXT,
				Generations INTEGER,
				SeasonalityId INTEGER,
				PacketCount INTEGER,
				Exchange BOOLEAN,
				Species TEXT
			);

			-- SEED PACKET OBSERVATION
			DROP TABLE IF EXISTS SeedPacketObservation;
			CREATE TABLE IF NOT EXISTS SeedPacketObservation(
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
				Code VARCHAR (50) UNIQUE NOT NULL,
				Description VARCHAR (2000) NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP
			);

			--PLANTING
			DROP TABLE IF EXISTS Planting;
			CREATE TABLE IF NOT EXISTS Planting(
				Id integer primary key generated always as identity,
				Description TEXT NOT NULL,
				PlantId INTEGER,
				SeedPacketId INTEGER,
				GardenBedId INTEGER,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				YieldRating INTEGER,
				AuthorId INTEGER,
				Comment TEXT,
				IsPlanted BOOLEAN,
				IsStaged BOOLEAN,
				PlantingStateId INTEGER
			);

			-- PLANTING OBSERVATION
			DROP TABLE IF EXISTS PlantingObservation;
			CREATE TABLE IF NOT EXISTS PlantingObservation(
				Id integer primary key generated always as identity,
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
				Id integer primary key generated always as identity,
				PersonId INTEGER NOT NULL,
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
				AuthorId INTEGER NULL
			);

			-- ANIMAL TYPE
			DROP TABLE IF EXISTS AnimalType;
			CREATE TABLE IF NOT EXISTS AnimalType (
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
				Id integer primary key generated always as identity,
				AnimalId INTEGER NOT NULL,
				Comment TEXT NOT NULL,
				CreationDate TIMESTAMP,
				StartDate TIMESTAMP NOT NULL,
				EndDate TIMESTAMP,
				CommentTypeId INTEGER,
				AuthorId INTEGER
			);

			--FERMENTATION
			DROP TABLE IF EXISTS Fermentation;
			CREATE TABLE IF NOT EXISTS Fermentation(
				Id integer primary key generated always as identity,
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
            
            using (IDbConnection connection = new NpgsqlConnection(DataConnection.GetServerDataSource()))
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
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);

            q = new Quote { Description = "They that dance must pay the piper.", AuthorName = "Scottish Proverb" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);

            q = new Quote { Description = "You can solve all the world's problems in a garden.", AuthorName = "Geoff Lawton" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);

            q = new Quote { Description = "Permaculture gives us a toolkit for moving from a culture of fear and scarcity to one of love and abundance.", AuthorName = "Toby Hemenway" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);

            q = new Quote { Description = "Despite all of our accomplishments we owe our existence to a six-inch layer of topsoil and the fact that it rains.", AuthorName = "Felipe Nalpak" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);

            q = new Quote { Description = "Permaculture is revolution disguised as gardening.", AuthorName = "Mike Feingold" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);

            q = new Quote { Description = "Let your food be your medicine and your medicine be your food.", AuthorName = "Hippocrates" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "Information is the critical potential resource. It becomes a resource only when obtained and acted upon.", AuthorName = "Bill Mollison" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "Gardening adds years to your life and life to your years.", AuthorName = "Anonymous" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "Life begins the day you start a garden.", AuthorName = "Chinese Proverb" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);

            q = new Quote { Description = "Garden as though you will live forever.", AuthorName = "William Kent" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "There are no gardening mistakes, only experiments.", AuthorName = "Janet Kilburn Phillips" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "A society grows great when old men plant trees whose shade they know they shall never sit in.", AuthorName = "Greek Proverb" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "The garden suggests there might be a place where we can meet nature halfway.", AuthorName = "Michael Pollan" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "Plant and your spouse plants with you; weed and you weed alone.", AuthorName = "Jean Jacques Rousseau" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "When the world wearies and society fails to satisfy, there is always the garden.", AuthorName = "Minnie Aumonier" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "A garden is always a series of losses set against a few triumphs, like life itself.", AuthorName = "May Sarton" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "Gardeners, I think, dream bigger dreams than emperors.", AuthorName = "Mary Cantwell" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "In the spring, at the end of the day, you should smell like dirt.", AuthorName = "Margaret Atwood" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "What is a weed? A plant whose virtues have never been discovered.", AuthorName = "Ralph Waldo Emerson" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "To dwell is to garden.", AuthorName = "Martin Heidegger" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "Use plants to bring life.", AuthorName = "Douglas Wilson" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "A lawn is nature under totalitarian rule.", AuthorName = "Michael Pollan" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "Use plants to bring life.", AuthorName = "Douglas Wilson" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "I need my friends, I need my house, I need my garden.", AuthorName = "Miranda Richardson" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "Debt is a form of slavery.", AuthorName = "Jack Spirko" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "We come from the earth, we return to the earth, and in between, we garden.", AuthorName = "Anonymous" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "When you are standing on the ground, you are really standing on the rooftop of another world.", AuthorName = "Dr. M. Jill Clapperton" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            q = new Quote { Description = "To forget how to dig the earth and to tend the soil is to forget ourselves.", AuthorName = "Mohandas K. Gandhi" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
           
            q = new Quote { Description = "Buy land, they’re not making it anymore.", AuthorName = "Mark Twain" };
            QuoteRepository.Insert(DataConnection.GetServerDataSource(), q);
            
            #endregion
            
        }

    }
}
