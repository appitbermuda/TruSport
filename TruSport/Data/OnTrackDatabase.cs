using System;
using System.Collections.Generic;
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
            database.CreateTableAsync<Favourite>().Wait();
            database.CreateTableAsync<Field>().Wait();
            database.CreateTableAsync<Fixture>().Wait();
            database.CreateTableAsync<League>().Wait();
            database.CreateTableAsync<Match>().Wait();
            database.CreateTableAsync<MatchType>().Wait();
            database.CreateTableAsync<Player>().Wait();
            database.CreateTableAsync<Team>().Wait();
            database.CreateTableAsync<User>().Wait();
            database.CreateTableAsync<UserType>().Wait();
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

        public async Task<bool> IsFixtureFavourite(string FixtureID)
        {
            var favourite = await database.Table<Favourite>().FirstOrDefaultAsync(e => e.FixtureID == FixtureID);

            return favourite != null;
        }

        public async Task<List<Favourite>> GetTeamFavourites()
        {
            try
            {
                TeamService teamService = new TeamService();
                var favourites = await database.Table<Favourite>().Where(e => e.Type == "Team").ToListAsync();

                if (favourites.Count > 0)
                {
                    var teamsList = await teamService.GetTeams();
                    //var teams = teamsList.Where(e => e.Season.IsCurrent).Select(e => e.Team);
                    var teams = teamsList.Where(e => e.Season.IsCurrent);

                    var teamFavourites = (from favourite in favourites
                                          join team in teams on favourite.TeamID equals team.TeamID
                                          select new Favourite
                                          {
                                              Team = team.Team,
                                              League = team.League,
                                              Type = favourite.Type,
                                              TeamID = favourite.TeamID
                                          }).ToList();


                    return teamFavourites;
                }
            }
            catch (Exception ex)
            { }

            return null;
        }

        public async Task<List<Favourite>> GetFixtureFavourites()
        {
            try
            {
                FixtureService fixtureService = new FixtureService();
                var favourites = await database.Table<Favourite>().Where(e => e.Type == "Fixture").ToListAsync();

                if (favourites.Count > 0)
                {
                    var fixtures = await fixtureService.GetFixtures();

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
                            await DeleteFixtureFavourite(fixture.FixtureID);
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

        public async Task<Favourite> GetFavouriteByFixtureID(string FixtureID)
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

        public Task<int> SaveFavourite(Favourite item)
        {
            return database.InsertAsync(item);
        }

        public async Task<int> DeleteTeamFavourite(string TeamID)
        {
            var favourite = await GetFavouriteByTeamID(TeamID);
            return await database.DeleteAsync(favourite);
        }

        public async Task<int> DeleteFixtureFavourite(string FixtureID)
        {
            var favourite = await GetFavouriteByFixtureID(FixtureID);
            return await database.DeleteAsync(favourite);
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
    }
}
