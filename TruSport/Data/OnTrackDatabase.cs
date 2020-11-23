using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using TruSport.Model;
using TruSport.Services;

namespace TruSport.Data
{
    public class OnTrackDatabase
    {
        readonly SQLiteAsyncConnection database;

        public OnTrackDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Coach>().Wait();
            database.CreateTableAsync<Customer>().Wait();
            database.CreateTableAsync<CustomerOrder>().Wait();
            database.CreateTableAsync<CreditCard>().Wait();
            database.CreateTableAsync<Token>().Wait();
            database.CreateTableAsync<NotiAlert>().Wait();
            database.CreateTableAsync<BowlingFixture>().Wait();
            database.CreateTableAsync<CricketFixture>().Wait();
            database.CreateTableAsync<DefaultSport>().Wait();
            database.CreateTableAsync<Favourite>().Wait();
            database.CreateTableAsync<Field>().Wait();
            database.CreateTableAsync<Fixture>().Wait();
            database.CreateTableAsync<Product>().Wait();
            database.CreateTableAsync<Order>().Wait();
            database.CreateTableAsync<League>().Wait();
            database.CreateTableAsync<Match>().Wait();
            database.CreateTableAsync<MatchTicket>().Wait();
            database.CreateTableAsync<MatchType>().Wait();
            database.CreateTableAsync<Player>().Wait();
            database.CreateTableAsync<Sport>().Wait();
            database.CreateTableAsync<Team>().Wait();
            database.CreateTableAsync<User>().Wait();
            database.CreateTableAsync<UserType>().Wait();
        }

        //Coach
        public async Task<List<CreditCard>> GetCreditCards(string email)
        {
            try
            {
                //PasswordHasher passwordHasher = new PasswordHasher();
                
                var creditCards = await database.Table<CreditCard>().Where(e => e.Email == email).ToListAsync();

                return creditCards;
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public async Task<CreditCard> GetCreditCard(int ID)
        {
            try
            {
                //PasswordHasher passwordHasher = new PasswordHasher();

                var creditCard = await database.Table<CreditCard>().FirstOrDefaultAsync(e => e.ID == ID);

                //creditCard.CardNumber = passwordHasher.DecryptString(creditCard.CardNumber);

                return creditCard;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<int> Insert(CreditCard item)
        {
            try
            {
                if(item.IsDefault)
                {
                    var creditcards = await database.Table<CreditCard>().Where(e => e.Email == item.Email).ToListAsync();

                    if (creditcards != null && creditcards.Count > 0)
                    {
                        creditcards.ForEach(e => e.IsDefault = false);

                        await database.UpdateAllAsync(creditcards);
                    }
                }

                item.Last4 = "****-****-****-" + item.CardNumber.Substring(item.CardNumber.Length - 4, 4);
                //PasswordHasher passwordHasher = new PasswordHasher();
                //item.CardNumber = passwordHasher.EncryptString(item.CardNumber);

                await database.InsertAsync(item);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Card");
            }

            return -1;
        }

        public async Task<int> Update(CreditCard item)
        {
            try
            {
                if (item.IsDefault)
                {
                    var creditcards = await database.Table<CreditCard>().Where(e => e.Email == item.Email).ToListAsync();

                    if (creditcards != null && creditcards.Count > 0)
                    {
                        creditcards.ForEach(e => e.IsDefault = false);

                        await database.UpdateAllAsync(creditcards);
                    }
                }

                item.Last4 = "****-****-****-" + item.CardNumber.Substring(item.CardNumber.Length - 4, 4);
                //PasswordHasher passwordHasher = new PasswordHasher();
                //item.CardNumber = passwordHasher.EncryptString(item.CardNumber);

                await database.UpdateAsync(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Card");
            }

            return -1;
        }

        public Task<int> Delete(int ID)
        {
            return database.DeleteAsync<CreditCard>(ID);
        }

        //Order
        public async Task<List<CustomerOrder>> GetMatchDayOrder(string email)
        {
            List<CustomerOrder> orders = new List<CustomerOrder>();
            try
            {
                var customer = await GetCustomerByIDAsync(email);
                orders = await database.Table<CustomerOrder>().Where(e => e.CustomerID == customer.ID).ToListAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetMatchDayOrder");
            }

            return orders;
        }

        public async Task<List<MatchTicket>> GetMatchDayTickets(string email)
        {
            List<MatchTicket> matchTickets = new List<MatchTicket>();
            try
            {
                var customer = await GetCustomerByIDAsync(email);
                //orders = await database.Table<CustomerOrder>().Where(e => e.CustomerID == customer.ID).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetMatchDayOrder");
            }

            return matchTickets;
        }

        public async Task<List<Order>> GetOrderHistory(string email)
        {
            List<Order> orders = new List<Order>();
            try
            {
                var customer = await GetCustomerByIDAsync(email);
                orders = await database.Table<Order>().Where(e => e.CustomerID == customer.ID).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetMatchDayOrder");
            }

            return orders;
        }

        public Task<int> Insert(Order item)
        {
            return database.InsertAsync(item);
        }

        public Task<int> DeleteOrder(int ID)
        {
            return database.DeleteAsync<Order>(ID);
        }

        public Task<int> DeleteOrders()
        {
            return database.DeleteAllAsync<Order>();
        }

        //Coach
        public async Task<List<Coach>> GetCoaches()
        {
            return await database.Table<Coach>().ToListAsync();
        }

        public async Task<List<Coach>> GetCoachesByTeam(string TeamID)
        {
            return await database.Table<Coach>().Where(e => e.TeamID == TeamID).ToListAsync();
        }

        public Task<int> ImportCoaches(List<Coach> item)
        {
            return database.InsertAllAsync(item);
        }

        public Task<int> ClearCoaches()
        {
            return database.DeleteAllAsync<Coach>();
        }

        //Sport
        public async Task<List<Sport>> GetSports()
        {
            return await database.Table<Sport>().ToListAsync();
        }

        public async Task<Sport> GetSport(string name)
        {
            return await database.Table<Sport>().FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower());
        }

        public Task<int> ImportSports(List<Sport> item)
        {
            return database.InsertAllAsync(item);
        }

        public async Task<int> ImportIfNotExistsSports(List<Sport> item)
        {
            var sports = await GetSports();

            if (sports != null && sports.Count > 0)
            {
                var newSports = item.Where(e => sports.Any(d => d.Name == e.Name)).ToList();

                if (newSports != null && newSports.Count > 0)
                    return await database.InsertAllAsync(newSports);
            }
            else
                return await database.InsertAllAsync(item);

            return -1;
        }

        public Task<int> ClearSports()
        {
            return database.DeleteAllAsync<Coach>();
        }

        //DefaultSport

        public Task<DefaultSport> GetDefaultSport()
        {
            return database.Table<DefaultSport>().FirstOrDefaultAsync();
        }

        public async Task SetDefaultSport(Sport sport)
        {
            try
            {
                DefaultSport setSport = new DefaultSport();
                setSport.Sport = sport.Name;
                setSport.SportID = sport.ID;

                var defaultSport = await GetDefaultSport();

                if (defaultSport == null)
                    await database.InsertAsync(setSport);
                else
                {
                    defaultSport.SportID = setSport.SportID;
                    defaultSport.Sport = setSport.Sport;

                    await database.UpdateAsync(defaultSport);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        //Favourite
        public async Task<List<Favourite>> GetFavourites()
        {
            return await database.Table<Favourite>().ToListAsync();
        }

        public async Task<bool> IsTeamFavourite(string TeamID)
        {
            var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.TeamID == TeamID);

            return favourite != null;
        }

        public async Task<bool> IsBowlingTeamFavourite(string TeamID)
        {
            var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.TeamID == TeamID && e.Sport == "Bowling");

            return favourite != null;
        }

        public async Task<bool> IsCricketTeamFavourite(string TeamID)
        {
            var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.TeamID == TeamID && e.Sport == "Cricket");

            return favourite != null;
        }

        public async Task<bool> IsFootballTeamFavourite(string TeamID)
        {
            var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.TeamID == TeamID && e.Sport == "Football");

            return favourite != null;
        }

        public async Task<bool> IsBowlingFixtureFavourite(string FixtureID)
        {
            var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.BowlingFixtureID == FixtureID && e.Sport == "Bowling");

            return favourite != null;
        }

        public async Task<bool> IsCricketFixtureFavourite(string FixtureID)
        {
            var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.CricketFixtureID == FixtureID && e.Sport == "Cricket");

            return favourite != null;
        }

        public async Task<bool> IsFootballFixtureFavourite(string FixtureID)
        {
            var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.FixtureID == FixtureID && e.Sport == "Football");

            return favourite != null;
        }

        public async Task<bool> IsFixtureFavourite(string FixtureID)
        {
            var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.FixtureID == FixtureID);

            return favourite != null;
        }

        public async Task<List<Favourite>> GetBowlingTeamFavourites()
        {
            try
            {
                TeamService teamService = new TeamService();
                var favourites = await database.Table<Favourite>().Where(e => e.Type == "Team" && e.Sport == "Bowling").ToListAsync();

                if (favourites.Count > 0)
                {
                    var teamsList = await teamService.GetBowlingTeams();
                    //var teams = teamsList.Where(e => e.Season.IsCurrent).Select(e => e.Team);
                    var teams = teamsList.Where(e => e.TeamSeasons.Any(d => d.Season.IsCurrent));

                    var teamFavourites = (from favourite in favourites
                                          join team in teams on favourite.TeamID equals team.ID
                                          select new Favourite
                                          {
                                              Team = team,
                                              League = team.League,
                                              Type = favourite.Type,
                                              TeamID = favourite.TeamID,
                                              Sport = "Bowling"
                                          }).ToList();


                    return teamFavourites;
                }
            }
            catch (Exception ex)
            { }

            return null;
        }

        public async Task<List<Favourite>> GetCricketTeamFavourites()
        {
            try
            {
                TeamService teamService = new TeamService();
                var favourites = await database.Table<Favourite>().Where(e => e.Type == "Team" && e.Sport == "Cricket").ToListAsync();

                if (favourites.Count > 0)
                {
                    var teamsList = await teamService.GetCricketTeams();
                    //var teams = teamsList.Where(e => e.Season.IsCurrent).Select(e => e.Team);
                    var teams = teamsList.Where(e => e.TeamSeasons.Any(d => d.Season.IsCurrent));

                    var teamFavourites = (from favourite in favourites
                                          join team in teams on favourite.TeamID equals team.ID
                                          select new Favourite
                                          {
                                              Team = team,
                                              League = team.League,
                                              Type = favourite.Type,
                                              TeamID = favourite.TeamID,
                                              Sport = "Cricket"
                                          }).ToList();


                    return teamFavourites;
                }
            }
            catch (Exception ex)
            { }

            return null;
        }

        public async Task<List<Favourite>> GetFootballTeamFavourites()
        {
            try
            {
                TeamService teamService = new TeamService();
                var favourites = await database.Table<Favourite>().Where(e => e.Type == "Team" && e.Sport == "Football").ToListAsync();

                if (favourites.Count > 0)
                {
                    var teamsList = await teamService.GetFootballTeams();
                    //var teams = teamsList.Where(e => e.Season.IsCurrent).Select(e => e.Team);
                    var teams = teamsList.Where(e => e.TeamSeasons.Any(d => d.Season.IsCurrent));

                    var teamFavourites = (from favourite in favourites
                                          join team in teams on favourite.TeamID equals team.ID
                                          select new Favourite
                                          {
                                              Team = team,
                                              League = team.League,
                                              Type = favourite.Type,
                                              TeamID = favourite.TeamID,
                                              Sport = "Football"
                                          }).ToList();


                    return teamFavourites;
                }
            }
            catch (Exception ex)
            { }

            return null;
        }


        public async Task<List<Favourite>> GetBowlingFixtureFavourites()
        {
            try
            {
                FixtureService fixtureService = new FixtureService();
                var favourites = await database.Table<Favourite>().Where(e => e.Type == "Fixture" && e.Sport == "Bowling").ToListAsync();

                if (favourites.Count > 0)
                {
                    var fixtures = await fixtureService.GetUpcomingBowlingFixtures();

                    var fixtureFavourites = (from favourite in favourites
                                             join fixture in fixtures on favourite.BowlingFixtureID equals fixture.ID
                                             select new Favourite
                                             {
                                                 BowlingFixture = fixture,
                                                 Type = favourite.Type,
                                                 BowlingFixtureID = favourite.BowlingFixtureID,
                                                 Sport = "Bowling"
                                             }).ToList();

                    var needToDelete = favourites.Where(e => !fixtureFavourites.Any(d => d.BowlingFixtureID == e.BowlingFixtureID));

                    foreach(var favourite in needToDelete)
                    {
                        await DeleteBowlingFixtureFavourite(favourite.BowlingFixtureID);
                    }

                    //foreach (var favourite1 in favourites)
                    //{
                    //    if(fixtureFavourites.FirstOrDefault(e => e.BowlingFixtureID == favourite1.BowlingFixtureID) == null)
                    //        await DeleteBowlingFixtureFavourite(favourite1.BowlingFixtureID);                        
                    //}

                    return fixtureFavourites;
                }

                return null;
            }
            catch (Exception ex)
            {

            }

            return null;
        }


        public async Task<List<Favourite>> GetCricketFixtureFavourites()
        {
            try
            {
                FixtureService fixtureService = new FixtureService();
                var favourites = await database.Table<Favourite>().Where(e => e.Type == "Fixture" && e.Sport == "Cricket").ToListAsync();

                if (favourites.Count > 0)
                {
                    var fixtures = await fixtureService.GetCricketFixtures();

                    var fixtureFavourites = (from favourite in favourites
                                             join fixture in fixtures on favourite.CricketFixtureID equals fixture.ID
                                             select new Favourite
                                             {
                                                 CricketFixture = fixture,
                                                 Type = favourite.Type,
                                                 CricketFixtureID = favourite.CricketFixtureID,
                                                 Sport = "Cricket"
                                             }).ToList();

                    foreach (var fixture in fixtureFavourites)
                    {
                        if (fixture.CricketFixture.Date < DateTime.Now.AddDays(1))
                        {
                            fixtureFavourites.Remove(fixture);
                            await DeleteCricketFixtureFavourite(fixture.FixtureID);
                        }
                    }

                    return fixtureFavourites;
                }

                return null;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<List<Favourite>> GetFootballFixtureFavourites()
        {
            try
            {
                FixtureService fixtureService = new FixtureService();
                var favourites = await database.Table<Favourite>().Where(e => e.Type == "Fixture" && e.Sport == "Football").ToListAsync();

                if (favourites.Count > 0)
                {
                    var fixtures = await fixtureService.GetFootballFixtures();

                    var fixtureFavourites = (from favourite in favourites
                                             join fixture in fixtures on favourite.FixtureID equals fixture.ID
                                             select new Favourite
                                             {
                                                 Fixture = fixture,
                                                 Type = favourite.Type,
                                                 FixtureID = favourite.FixtureID,
                                                 Sport = "Football"
                                             }).ToList();

                    foreach (var fixture in fixtureFavourites)
                    {
                        if (fixture.Fixture.Date < DateTime.Now.AddDays(1))
                        {
                            fixtureFavourites.Remove(fixture);
                            await DeleteFootballFixtureFavourite(fixture.FixtureID);
                        }
                    }

                    return fixtureFavourites;
                }

                return null;
            }
            catch (Exception ex)
            { }

            return null;
        }


        public async Task<List<Favourite>> GetFavouriteFixturesBySport(string SportID)
        {
            try
            {
                FixtureService fixtureService = new FixtureService();
                var favourites = await database.Table<Favourite>().Where(e => e.Type == "Fixture").ToListAsync();

                if (favourites.Count > 0)
                {
                    var fixtures = await fixtureService.GetSportFixtures(SportID);

                    var fixtureFavourites = (from favourite in favourites
                                             join fixture in fixtures on favourite.FixtureID equals fixture.ID
                                             select new Favourite
                                             {
                                                 Fixture = fixture,
                                                 Type = favourite.Type,
                                                 FixtureID = favourite.FixtureID,
                                             }).ToList();

                    foreach (var fixture in fixtureFavourites)
                    {
                        if (fixture.Fixture.Date < DateTime.Now.AddDays(1))
                        {
                            fixtureFavourites.Remove(fixture);
                            //await DeleteFixtureFavourite(fixture.FixtureID);
                        }
                    }

                    return fixtureFavourites;
                }

                return null;
            }
            catch (Exception ex)
            { }

            return null;
        }

        public async Task<Favourite> GetFavouriteByBowlingFixtureID(string FixtureID)
        {
            try
            {
                var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.BowlingFixtureID == FixtureID);

                return favourite;
            }
            catch (Exception ex)
            { }

            return null;
        }

        public async Task<Favourite> GetFavouriteByCricketFixtureID(string FixtureID)
        {
            try
            {
                var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.CricketFixtureID == FixtureID);
                
                return favourite;
            }
            catch (Exception ex)
            { }

            return null;
        }

        public async Task<Favourite> GetFavouriteByFootballFixtureID(string FixtureID)
        {
            try
            {
                var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.FixtureID == FixtureID);

                return favourite;
            }
            catch (Exception ex)
            { }

            return null;
        }

        public async Task<Favourite> GetFavouriteByTeamID(string TeamID)
        {
            try
            {
                var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.TeamID == TeamID);

                return favourite;
            }
            catch (Exception ex)
            { }

            return null;
        }

        public Task<int> ClearFavourites()
        {
            return database.DeleteAllAsync<Favourite>();
        }

        public async Task<int> SaveBowlingFavourite(Favourite item)
        {
            try
            {
                item.Sport = "Bowling";
                return await database.InsertAsync(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Save Bowling Favourite");
            }
            return -1;
        }

        public async Task<int> SaveCricketFavourite(Favourite item)
        {
            try
            {
                item.Sport = "Cricket";
                return await database.InsertAsync(item);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Save Cricket Favourite");
            }
            return -1;
        }

        public async Task<int> SaveFootballFavourite(Favourite item)
        {
            try
            {
                item.Sport = "Football";
                return await database.InsertAsync(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Save Football Favourite");
            }
            return -1;
        }

        public async Task<int> DeleteTeamFavourite(string TeamID)
        {
            try
            {
                var favourite = await GetFavouriteByTeamID(TeamID);
                return await database.DeleteAsync(favourite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Delete Team Favourite");
            }
            return -1;
        }

        public async Task<int> DeleteBowlingFixtureFavourite(string FixtureID)
        {

            try
            {
                var favourite = await GetFavouriteByBowlingFixtureID(FixtureID);
                return await database.DeleteAsync(favourite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Delete Bowling Favourite");
            }
            return -1;
        }

        public async Task<int> DeleteCricketFixtureFavourite(string FixtureID)
        {
            
            try
            {
                var favourite = await GetFavouriteByCricketFixtureID(FixtureID);
                return await database.DeleteAsync(favourite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Delete Cricket Favourite");
            }
            return -1;
        }

        public async Task<int> DeleteFootballFixtureFavourite(string FixtureID)
        {

            try
            {
                var favourite = await GetFavouriteByFootballFixtureID(FixtureID);
                return await database.DeleteAsync(favourite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Delete Football Favourite");
            }
            return -1;
        }

        //Field
        public async Task<List<Field>> GetFields()
        {
            return await database.Table<Field>().ToListAsync();
        }

        public async Task<Field> GetFieldByID(string ID)
        {
            return await database.Table<Field>().Where(e => e.ID == ID).FirstOrDefaultAsync();
        }

        public Task<int> ImportFields(List<Field> item)
        {
            return database.InsertAllAsync(item);
        }

        public Task<int> ClearFields()
        {
            return database.DeleteAllAsync<Field>();
        }

        //Fixture
        public async Task<List<Fixture>> GetFixtures()
        {
            return await database.Table<Fixture>().ToListAsync();
        }

        public async Task<List<Fixture>> GetFixturesByTeam(string TeamID)
        {
            return await database.Table<Fixture>().Where(e => e.AwayTeamID == TeamID || e.HomeTeamID == TeamID).ToListAsync();
        }

        public async Task<Fixture> GetFixtureByID(string ID)
        {
            return await database.Table<Fixture>().Where(e => e.ID == ID).FirstOrDefaultAsync();
        }

        public Task<int> ImportFixtures(List<Fixture> item)
        {
            return database.InsertAllAsync(item);
        }

        public Task<int> ClearFixtures()
        {
            return database.DeleteAllAsync<Fixture>();
        }

        //League
        public async Task<List<League>> GetLeagues()
        {
            return await database.Table<League>().ToListAsync();
        }

        public async Task<League> GetLeagueByID(string ID)
        {
            return await database.Table<League>().Where(e => e.ID == ID).FirstOrDefaultAsync();
        }

        public async Task<League> GetLeagueByName(string Name)
        {
            return await database.Table<League>().Where(e => e.Name == Name).FirstOrDefaultAsync();
        }

        public Task<int> ImportLeagues(List<League> item)
        {
            return database.InsertAllAsync(item);
        }

        public Task<int> ClearLeagues()
        {
            return database.DeleteAllAsync<League>();
        }

        //Match
        public async Task<List<Match>> GetMatches()
        {
            return await database.Table<Match>().ToListAsync();
        }

        public async Task<List<Match>> GetMatchByFixture(string FixtureID)
        {
            return await database.Table<Match>().Where(e => e.FixtureID == FixtureID).ToListAsync();
        }

        public async Task<Match> GetMatchByID(string ID)
        {
            return await database.Table<Match>().Where(e => e.ID == ID).FirstOrDefaultAsync();
        }

        public Task<int> ImportMatches(List<Match> item)
        {
            return database.InsertAllAsync(item);
        }

        public Task<int> ClearMatches()
        {
            return database.DeleteAllAsync<Match>();
        }

        //Match Type
        public async Task<List<MatchType>> GetMatchTypes()
        {
            return await database.Table<MatchType>().ToListAsync();
        }

        public async Task<MatchType> GetMatchTypeByID(string ID)
        {
            return await database.Table<MatchType>().Where(e => e.ID == ID).FirstOrDefaultAsync();
        }

        public Task<int> ImportMatchTypes(List<MatchType> item)
        {
            return database.InsertAllAsync(item);
        }

        public Task<int> ClearMatchTypes()
        {
            return database.DeleteAllAsync<MatchType>();
        }

        //Player
        public async Task<List<Player>> GetPlayers()
        {
            return await database.Table<Player>().ToListAsync();
        }

        public async Task<List<Player>> GetPlayersByTeam(string TeamID)
        {
            //return await database.Table<Player>().Where(e => e.TeamID == TeamID).ToListAsync();
            return await database.Table<Player>().ToListAsync();
        }

        public async Task<Player> GetPlayerByID(string ID)
        {
            return await database.Table<Player>().Where(e => e.ID == ID).FirstOrDefaultAsync();
        }

        public Task<int> ImportPlayers(List<Player> item)
        {
            return database.InsertAllAsync(item);
        }

        public Task<int> ClearPlayers()
        {
            return database.DeleteAllAsync<Player>();
        }

        //Teams
        public async Task<List<Team>> GetTeams()
        {
            return await database.Table<Team>().ToListAsync();
        }

        public async Task<Team> GetTeamsByID(string ID)
        {
            return await database.Table<Team>().Where(e => e.ID == ID).FirstOrDefaultAsync();
        }

        public Task<int> ImportTeams(List<Team> item)
        {
            return database.InsertAllAsync(item);
        }

        public Task<int> ClearTeams()
        {
            return database.DeleteAllAsync<Team>();
        }

        //USER
        public async Task<User> GetUserByIDAsync(string ID)
        {
            return await database.Table<User>().Where(i => i.ID == ID).FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserLoggedIn()
        {
            //User user = await database.Table<User>().Where(i => i.IsLoggedIn == true).FirstOrDefaultAsync();

            //if (user != null)
            //{
            //    App.UserID = user.ID;
            //    App.UserFirstName = user.FirstName;
            //    App.UserLastName = user.LastName;
            //    App.UserFullName = user.FirstName + " " + user.LastName;
            //    App.UserType = user.UserTypeID;
            //    return true;
            //}

            return false;
        }

        public Task<int> SaveUser(User item)
        {
            return database.InsertAsync(item);
        }

        public async Task<int> UpdateUser(User item)
        {
            var thisUserExist = await database.Table<User>().Where(e => e.ID == item.ID).FirstOrDefaultAsync();

            if (thisUserExist != null)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public Task<int> DeleteUser(User item)
        {
            return database.DeleteAsync(item);
        }

        //Customer
        public async Task<Customer> GetCustomerByIDAsync(string email)
        {
            return await database.Table<Customer>().Where(i => i.Email == email).FirstOrDefaultAsync();
        }

        //public async Task<bool> IsUserLoggedIn()
        //{
        //    //User user = await database.Table<User>().Where(i => i.IsLoggedIn == true).FirstOrDefaultAsync();

        //    //if (user != null)
        //    //{
        //    //    App.UserID = user.ID;
        //    //    App.UserFirstName = user.FirstName;
        //    //    App.UserLastName = user.LastName;
        //    //    App.UserFullName = user.FirstName + " " + user.LastName;
        //    //    App.UserType = user.UserTypeID;
        //    //    return true;
        //    //}

        //    return false;
        //}

        public Task<int> SaveCustomer(Customer item)
        {
            return database.InsertAsync(item);
        }

        public async Task<int> UpdateCustomer(Customer item)
        {
            var thisCustomerExist = await database.Table<Customer>().Where(e => e.ID == item.ID).FirstOrDefaultAsync();

            if (thisCustomerExist != null)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public Task<int> DeleteCustomer(Customer item)
        {
            return database.DeleteAsync(item);
        }

        public async Task<bool> SignIn(Customer customer)
        {
            try
            {
                await database.InsertAsync(customer);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        public async Task<bool> SignOut()
        {
            try
            {
                await database.DeleteAllAsync<User>();
                await database.DeleteAllAsync<Customer>();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        public async Task<bool> CustomerAuthenticated(string email)
        {
            try
            {
                var user = await database.Table<Customer>().FirstOrDefaultAsync(e => e.Email == email);

                if (user != null)
                    return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        //User Type
        public async Task<List<UserType>> GetUserTypes()
        {
            return await database.Table<UserType>().ToListAsync();
        }

        public async Task<UserType> GetUserTypeByID(string ID)
        {
            return await database.Table<UserType>().Where(e => e.ID == ID).FirstOrDefaultAsync();
        }

        public Task<int> ImportUserTypes(List<UserType> item)
        {
            return database.InsertAllAsync(item);
        }

        public Task<int> ClearUserTypes()
        {
            return database.DeleteAllAsync<UserType>();
        }

        //Token
        public async Task SaveToken(string Token)
        {
            try
            {
                var token = await database.Table<Token>().FirstOrDefaultAsync();

                if(token != null)
                {
                    token.TokenId = Token;

                    await database.UpdateAsync(token);
                }
                else
                {
                    token = new Token
                    {
                        TokenId = Token
                    };


                    await database.InsertAsync(token);
                }
            }
            catch(Exception ex)
            {

            }
        }

        public async Task<string> GetToken()
        {
            try
            {
                var token = await database.Table<Token>().FirstOrDefaultAsync();
                return token.TokenId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Token
        public async Task SaveAlertSetting(List<NotiAlert> alertsSettings)
        {
            try
            {
                var alerts = await database.Table<NotiAlert>().ToListAsync();

                if (alerts != null)
                {
                    foreach(var alertSetting in alertsSettings)
                    {
                        foreach(var alert in alerts)
                        {
                            if(alert.Sport == alertSetting.Sport)
                            {
                                alert.IsAlert = alertSetting.IsAlert;
                            }
                        }
                    }

                    await database.UpdateAllAsync(alerts);
                }
                else
                {
                    await database.InsertAllAsync(alertsSettings);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SaveAlertSetting(NotiAlert alert)
        {
            try
            {
                var thisAlert = await database.Table<NotiAlert>().FirstOrDefaultAsync(e => e.Sport == alert.Sport);

                if (thisAlert != null)
                {
                    thisAlert.IsAlert = alert.IsAlert;
                    await database.UpdateAsync(thisAlert);
                }
                else
                {
                    await database.InsertAsync(alert);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<NotiAlert>> GetAlertSettings()
        {
            try
            {
                var alerts = await database.Table<NotiAlert>().ToListAsync();
                return alerts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string[]> GetTags()
        {
            try
            {
                //public static string[] SubscriptionTags { get; set; } = { "default", "football", "cricket" };
                var alerts = await database.Table<NotiAlert>().ToListAsync();

                List<string> alertTags = new List<string>();
                alertTags.Add("default");

                var alertSettingTags = alerts.Where(e => e.IsAlert).Select(e => e.Sport).ToArray();
                foreach (var alert in alertSettingTags)
                {
                    alertTags.Add(alert);
                }

                string[] tags = alertTags.ToArray();
                
                return tags;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
