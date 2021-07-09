using System;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Ad;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Data
{
    public class OnTrackContext : DbContext
    {
        public OnTrackContext(DbContextOptions<OnTrackContext> options)
            : base(options)
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Batting> Battings { get; set; }
        public DbSet<BowlingSeasonHG> BowlingSeasonHG { get; set; }
        public DbSet<BowlingSeasonHS> BowlingSeasonHS { get; set; }
        public DbSet<BowlingSeasonTeamHG> BowlingSeasonTeamHG { get; set; }
        public DbSet<BowlingSeasonTeamHS> BowlingSeasonTeamHS { get; set; }
        public DbSet<BowlingFixture> BowlingFixtures { get; set; }
        public DbSet<BowlingScore> BowlingScores { get; set; }
        public DbSet<BowlingRoster> BowlingRosters { get; set; }
        public DbSet<BowlingPlayerSeason> BowlingPlayerSeasons { get; set; }
        public DbSet<BowlingLeagueStanding> BowlingLeagueStandings { get; set; }
        public DbSet<BowlingGame> BowlingGames { get; set; }
        public DbSet<BowlingTeam> BowlingTeams { get; set; }
        public DbSet<BowlingTeamSeason> BowlingTeamSeasons { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<CourtType> CourtTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Fielding> Fieldings { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<FixtureProduct> FixtureProducts { get; set; }
        public DbSet<CricketFixture> CricketFixtures { get; set; }
        public DbSet<CricketRoster> CricketRosters { get; set; }
        public DbSet<CricketPlayerSeason> CricketPlayerSeasons { get; set; }
        public DbSet<CricketLeagueStanding> CricketLeagueStandings { get; set; }
        public DbSet<CricketScore> CricketScores { get; set; }
        public DbSet<ContactTrace> ContactTraces { get; set; }
        public DbSet<Flyer> Flyers { get; set; }
        public DbSet<Inventory> Inventorys { get; set; }
        public DbSet<Impression> Impressions { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<LeagueTable> LeagueTable { get; set; }
        public DbSet<CricketLeagueTable> CricketLeagueTable { get; set; }
        public DbSet<GoalsConcededByTeam> GoalsConcededByTeam { get; set; }
        public DbSet<GoalsScoredByTeam> GoalsScoredByTeam { get; set; }
        public DbSet<GoalsScoredByPlayer> GoalsScoredByPlayer { get; set; }
        public DbSet<RunsByPlayer> RunsByPlayer { get; set; }
        public DbSet<WicketsByPlayer> WicketsByPlayer { get; set; }
        public DbSet<CricketPremierLeagueTable> CricketPremierLeagueTable { get; set; }
        public DbSet<CricketFirstDivisionTable> CricketFirstDivisionTable { get; set; }
        public DbSet<PremierLeagueTable> PremierLeagueTable { get; set; }
        public DbSet<FirstDivisionTable> FirstDivisionTable { get; set; }
        public DbSet<CoronaLeagueTable> CoronaLeagueTable { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchInning> MatchInnings { get; set; }
        public DbSet<MatchTicket> MatchTickets { get; set; }
        public DbSet<CustomerTicket> CustomerTickets { get; set; }
        public DbSet<MatchRoster>  MatchRosters { get; set; }
        public DbSet<MatchStat> MatchStats { get; set; }
        public DbSet<MatchType> MatchTypes { get; set; }
        public DbSet<OutType> OutTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<PlayerSeason> PlayerSeasons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamSeason> TeamSeasons { get; set; }
        public DbSet<TennisFixture> TennisFixtures { get; set; }
        public DbSet<TennisGame> TennisGames { get; set; }
        public DbSet<TennisMatch> TennisMatches { get; set; }
        public DbSet<TennisPlayerSeason> TennisPlayerSeasons { get; set; }
        public DbSet<TennisRanking> TennisRankings { get; set; }
        public DbSet<TennisSet> TennisSets { get; set; }
        public DbSet<TennisTiebreak> TennisTiebreaks { get; set; }
        public DbSet<TennisTournament> TennisTournaments { get; set; }
        public DbSet<TournamentMatchType> TournamentMatchTypes { get; set; }
        public DbSet<TicketConfiguration> TicketConfigurations { get; set; }
        public DbSet<TicketMember> TicketMembers { get; set; }
        public DbSet<TicketCompany> TicketCompanys { get; set; }
        public DbSet<TicketCompanyUser> TicketCompanyUsers { get; set; }
        public DbSet<TicketTeam> TicketTeams { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<vTransfers> vTransfers { get; set; }
        public DbSet<SportEvent> SportEvents { get; set; }
        public DbSet<EventTicket> EventTickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AllUsers> AllUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ad>().ToTable("Ad");
            modelBuilder.Entity<BowlingRoster>().ToTable("BowlingRoster");
            modelBuilder.Entity<BowlingGame>().ToTable("BowlingGame");
            modelBuilder.Entity<BowlingScore>().ToTable("BowlingScore");
            modelBuilder.Entity<BowlingTeam>().ToTable("BowlingTeam");
            modelBuilder.Entity<BowlingTeamSeason>().ToTable("BowlingTeamSeason");
            modelBuilder.Entity<BowlingPlayerSeason>().ToTable("BowlingPlayerSeason");
            modelBuilder.Entity<BowlingLeagueStanding>().ToTable("BowlingLeagueStanding");
            modelBuilder.Entity<Coach>().ToTable("Coach");
            modelBuilder.Entity<CourtType>().ToTable("CourtType");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<ContactTrace>().ToTable("ContactTrace");
            modelBuilder.Entity<Inventory>().ToTable("Inventory");
            modelBuilder.Entity<Impression>().ToTable("Impression");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
            modelBuilder.Entity<ProductType>().ToTable("ProductType");
            modelBuilder.Entity<FixtureProduct>().ToTable("FixtureProduct");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<MatchTicket>().ToTable("MatchTicket");
            modelBuilder.Entity<CustomerTicket>().ToTable("CustomerTicket");
            modelBuilder.Entity<Field>().ToTable("Field");
            modelBuilder.Entity<TennisFixture>().ToTable("TennisFixture");
            modelBuilder.Entity<TennisGame>().ToTable("TennisGame");
            modelBuilder.Entity<TennisMatch>().ToTable("TennisMatch");
            modelBuilder.Entity<TennisPlayerSeason>().ToTable("TennisPlayerSeason");
            modelBuilder.Entity<TennisRanking>().ToTable("TennisRanking");
            modelBuilder.Entity<TennisSet>().ToTable("TennisSet");
            modelBuilder.Entity<TennisTiebreak>().ToTable("TennisTiebreak");
            modelBuilder.Entity<TennisTournament>().ToTable("TennisTournament");
            modelBuilder.Entity<TournamentMatchType>().ToTable("TournamentMatchType");
            modelBuilder.Entity<TicketConfiguration>().ToTable("TicketConfiguration");
            modelBuilder.Entity<TicketMember>().ToTable("TicketMember");
            modelBuilder.Entity<TicketCompany>().ToTable("TicketCompany");
            modelBuilder.Entity<TicketCompanyUser>().ToTable("TicketCompanyUser");
            modelBuilder.Entity<TicketTeam>().ToTable("TicketTeam");
            modelBuilder.Entity<SportEvent>().ToTable("SportEvent");
            modelBuilder.Entity<EventTicket>().ToTable("EventTicket");

            modelBuilder.Entity<Order>()
            .HasMany(c => c.OrderDetails)
            .WithOne(e => e.Order);

            modelBuilder.Entity<Fixture>().ToTable("Fixture")
                    .HasOne(x => x.HomeTeam)
                    .WithMany();
            modelBuilder.Entity<Fixture>().ToTable("Fixture")
                    .HasOne(x => x.AwayTeam)
                    .WithMany();

            modelBuilder.Entity<BowlingFixture>().ToTable("BowlingFixture")
                    .HasOne(x => x.HomeTeam)
                    .WithMany();
            modelBuilder.Entity<BowlingFixture>().ToTable("BowlingFixture")
                    .HasOne(x => x.AwayTeam)
                    .WithMany();

            modelBuilder.Entity<CricketFixture>().ToTable("CricketFixture")
                    .HasOne(x => x.HomeTeam)
                    .WithMany();
            modelBuilder.Entity<CricketFixture>().ToTable("CricketFixture")
                    .HasOne(x => x.AwayTeam)
                    .WithMany();

            modelBuilder.Entity<Batting>().ToTable("Batting");
            modelBuilder.Entity<Fielding>().ToTable("Fielding");
            modelBuilder.Entity<CricketPlayerSeason>().ToTable("CricketPlayerSeason");
            modelBuilder.Entity<CricketLeagueStanding>().ToTable("CricketLeagueStanding");
            modelBuilder.Entity<CricketScore>().ToTable("CricketScore");
            modelBuilder.Entity<CricketRoster>().ToTable("CricketRoster");
            modelBuilder.Entity<Flyer>().ToTable("Flyer");
            modelBuilder.Entity<Role>().ToTable("Role");

            modelBuilder.Entity<League>().ToTable("League");
            modelBuilder.Entity<LeagueTable>(entity => { entity.HasKey(e => new { e.TeamID, e.LeagueID }); });
            modelBuilder.Entity<CricketLeagueTable>(entity => { entity.HasKey(e => new { e.TeamID, e.LeagueID }); });
            modelBuilder.Entity<vTransfers>(entity => { entity.HasKey(e => e.PlayerID); });
            modelBuilder.Entity<CricketPremierLeagueTable>(entity => { entity.HasKey(e => e.TeamID); });
            modelBuilder.Entity<CricketFirstDivisionTable>(entity => { entity.HasKey(e => e.TeamID); });
            modelBuilder.Entity<PremierLeagueTable>(entity => { entity.HasKey(e => e.TeamID); });
            modelBuilder.Entity<FirstDivisionTable>(entity => { entity.HasKey(e => e.TeamID); });
            modelBuilder.Entity<CoronaLeagueTable>(entity => { entity.HasKey(e => e.TeamID); });
            modelBuilder.Entity<GoalsConcededByTeam>(entity => { entity.HasKey(e => new { e.TeamID, e.LeagueID }); });
            modelBuilder.Entity<GoalsScoredByTeam>(entity => { entity.HasKey(e => new { e.TeamID, e.LeagueID }); });
            modelBuilder.Entity<GoalsScoredByPlayer>(entity => { entity.HasKey(e => new { e.PlayerID, e.LeagueID }); }) ;
            modelBuilder.Entity<RunsByPlayer>(entity => { entity.HasKey(e => new { e.PlayerID, e.LeagueID }); });
            modelBuilder.Entity<WicketsByPlayer>(entity => { entity.HasKey(e => new { e.PlayerID, e.LeagueID }); });
            modelBuilder.Entity<BowlingSeasonHG>(entity => { entity.HasKey(e => new { e.PlayerID, e.LeagueID }); });
            modelBuilder.Entity<BowlingSeasonHS>(entity => { entity.HasKey(e => new { e.PlayerID, e.LeagueID }); });
            modelBuilder.Entity<BowlingSeasonTeamHG>(entity => { entity.HasKey(e => new { e.TeamID, e.LeagueID }); });
            modelBuilder.Entity<BowlingSeasonTeamHS>(entity => { entity.HasKey(e => new { e.TeamID, e.LeagueID }); });
            modelBuilder.Entity<AllUsers>(entity => { entity.HasKey(e => e.ID); });
            modelBuilder.Entity<Match>().ToTable("Match");
            modelBuilder.Entity<MatchInning>().ToTable("MatchInning");
            modelBuilder.Entity<MatchRoster>().ToTable("MatchRoster");
            modelBuilder.Entity<MatchStat>().ToTable("MatchStat");
            modelBuilder.Entity<MatchType>().ToTable("MatchType");
            modelBuilder.Entity<OutType>().ToTable("OutType");
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<Award>().ToTable("Award");
            modelBuilder.Entity<PlayerSeason>().ToTable("PlayerSeason");
            modelBuilder.Entity<Setting>().ToTable("Setting");
            modelBuilder.Entity<Season>().ToTable("Season");
            modelBuilder.Entity<Sport>().ToTable("Sport");
            modelBuilder.Entity<Team>().ToTable("Team");
            modelBuilder.Entity<Transfer>().ToTable("Transfer");
            modelBuilder.Entity<TeamSeason>().ToTable("TeamSeason");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<UserTeam>().ToTable("UserTeam");
            modelBuilder.Entity<UserType>().ToTable("UserType");

            modelBuilder.Query<spLiveFixtures>();
            modelBuilder.Query<spLiveCricketFixtures>();

        }
    }
}
