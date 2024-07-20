using LeafLINQWebAPI.Model;
using WebApi.Data;

namespace LeafLINQWebAPI.Data;

public static class DBLoader
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<LeafLINQContext>();

        Initialize(context);
    }
    public static void Initialize(LeafLINQContext context)
    {

        // Change your location for the excel files. 
        //var csvFilePathBase = "C:\\Users\\justi\\Desktop\\User.csv";
        //var csvFilePathBase = "C:\\Users\\61405\\Desktop\\BackupFiles";
        //C:\\RMIT\\Programming Project\\BackupFiles\\

        var csvFilePathBase = "C:\\RMIT\\Programming Project\\BackupFiles\\";
        var csvFilePathUser = csvFilePathBase + "User.csv";
        var csvFilePathPlant = csvFilePathBase + "Plant.csv";
        var csvFilePathPlantGroup = csvFilePathBase + "PlantGroup.csv";
        var csvFilePathSetting = csvFilePathBase + "Setting.csv";

        if (!context.User.Any())
        {

            string key = "";
            try
            {
                // Read all lines from the CSV file
                var lines = File.ReadAllLines(csvFilePathUser);

                // Skip header if it exists
                var entities = lines.Skip(1).Select(line =>
                {
                    var columns = line.Split(',');
                    int userFldCount = 1;
                    key = columns[0];
                    return new User
                    {

                        // Map CSV columns to properties
                        //Id = Int32.Parse(columns[userFldCount++]),
                        FullName = columns[userFldCount++],
                        Email = columns[userFldCount++],
                        PicUrl = columns[userFldCount++],
                        //Desc = columns[userFldCount++],
                        CurrentLoginDate = DateTime.Parse(columns[userFldCount++]),
                        LastLoginDate = DateTime.Parse(columns[userFldCount++]),
                        UserType = char.Parse(columns[6]),
                        PasswordHash = columns[7],

                        // Map more columns as needed
                    };
                });

                // Add entities to DbContext and save changes
                context.User.AddRange(entities);
                context.SaveChanges();

                Console.WriteLine("Data for table 'User' inserted successfully.");
            } catch (FileNotFoundException)
            {
                Console.WriteLine($"Data file '{csvFilePathUser}' not found. Error loading table 'User'");
            } catch (Exception ex) { Console.WriteLine($"Error loading table 'User'\n{ex.ToString()} {key}"); }
        }

        if (!context.Plant.Any())
        {
            string key2 = "";
            string userID = "";

            try 
            { 
                // Read all lines from the CSV file
                var plantLines = File.ReadAllLines(csvFilePathPlant);
            
                // Skip header if it exists
                var entities = plantLines.Skip(1).Select(plantLine =>
                {
                    int plantCount = 1;
                    var plantColumns = plantLine.Split(',');

                    Console.WriteLine($"Plant ID = {plantColumns[0]} - {plantColumns[1]} - User id = {plantColumns[7]}");

                    key2 = plantColumns[0];
                    userID = plantColumns[7];
                    return new Plant
                    {
                        // Map CSV columns to properties
                        //Id = Int32.Parse(plantColumns[plantCount++]),
                        Name = plantColumns[plantCount++],
                        Desc = plantColumns[plantCount++],
                        PicUrl = plantColumns[plantCount++],
                        Location = plantColumns[plantCount++],
                        Level = plantColumns[plantCount++],
                        LastWateredDate = DateTime.Parse(plantColumns[plantCount++]),
                        HealthCheckStatus = HealthCheckStatus.Healthy,
                        UserId = Int32.Parse(plantColumns[8]),
                        //DeviceId = plantColumns[plantCount++],
                                           
                       
                    };
                });

                // Add entities to context and save changes
                context.Plant.AddRange(entities);
                context.SaveChanges();

                Console.WriteLine("Data inserted into table 'Plant' successfully.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Data file '{csvFilePathPlant}' not found. Error loading table 'Plant'");
            }
            catch (Exception ex) { Console.WriteLine($"Error loading table 'Plant'\n{ex.ToString()} key = {key2} userID = {userID
                }"); }

        }
        
        if(!context.Setting.Any())
        {
            try
            { 
                // Read all lines from the CSV file
                var lines = File.ReadAllLines(csvFilePathSetting);

                // Skip header if it exists
                var entities = lines.Skip(1).Select(line =>
                {
                    var columns = line.Split(',');
                    int settingCount = 1; // Skip first field

                    
                    if (columns[2] == "1")
                    {
                        columns[2] = "TRUE";
                    } else
                    {
                        columns[2] = "FALSE";
                    }

                    if (columns[3] == "1")
                    {
                        columns[3] = "TRUE";
                    }
                    else
                    {
                        columns[3] = "FALSE";
                    }

                    Console.WriteLine($"Settings ID = {columns[0]} temp = {columns[1]} Phone Pref = {columns[2]} Email Pref = {columns[3]}");
                    return new Setting
                    {
                        // Map CSV columns to properties
                        //id = Int32.Parse(columns[0]),
                    
                        TemperatureUnit = char.Parse(columns[settingCount++]),
                        PhonePreference = bool.Parse(columns[settingCount++]),
                        EmailPreference = bool.Parse(columns[settingCount++]),
                        UserId = Int32.Parse(columns[settingCount++]),
                        // Map more columns as needed
                    };
                });

                // Add entities to context and save changes
                context.Setting.AddRange(entities);
                context.SaveChanges();

                Console.WriteLine("Data inserted into table 'Setting' successfully.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Data file '{csvFilePathSetting}' not found. Error loading table 'Setting'");
            }
            catch (Exception ex) { Console.WriteLine($"Error loading table 'Setting'\n{ex.ToString()}"); }
        }
    }
}
