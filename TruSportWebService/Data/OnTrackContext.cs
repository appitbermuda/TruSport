using System;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Models;

namespace OnTrackWebService.Data
{
    public class OnTrackContext : DbContext
    {
        public OnTrackContext(DbContextOptions<OnTrackContext> options)
            : base(options)
        {
        }

        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Flyer> Flyers { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<LeagueTable> LeagueTable { get; set; }
        public DbSet<GoalsConcededByTeam> GoalsConcededByTeam { get; set; }
        public DbSet<GoalsScoredByTeam> GoalsScoredByTeam { get; set; }
        public DbSet<GoalsScoredByPlayer> GoalsScoredByPlayer { get; set; }
        public DbSet<PremierLeagueTable> PremierLeagueTable { get; set; }
        public DbSet<FirstDivisionTable> FirstDivisionTable { get; set; }
        public DbSet<CoronaLeagueTable> CoronaLeagueTable { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchRoster>  MatchRosters { get; set; }
        public DbSet<MatchStat> MatchStats { get; set; }
        public DbSet<MatchType> MatchTypes { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerSeason> PlayerSeasons { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamSeason> TeamSeasons { get; set; }
        public DbSet<Transfers> Transfers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AllUsers> AllUsers { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coach>().ToTable("Coach");
            modelBuilder.Entity<Field>().ToTable("Field");
            modelBuilder.Entity<Fixture>().ToTable("Fixture");
            modelBuilder.Entity<Flyer>().ToTable("Flyer");
            //modelBuilder.Entity<Fixture>()
            //    .HasOne(ht => ht.HomeTeam)
            //    .WithMany(p => p.Fixtures)
            //    .HasForeignKey(ht => ht.HomeTeamID);

            modelBuilder.Entity<League>().ToTable("League");
            //modelBuilder.Entity<LeagueTable>().ToTable("LeagueTable");
            modelBuilder.Entity<LeagueTable>(entity => { entity.HasKey(e => new { e.TeamID, e.LeagueID }); });
            modelBuilder.Entity<Transfers>(entity => { entity.HasKey(e => e.PlayerID); });
            modelBuilder.Entity<PremierLeagueTable>(entity => { entity.HasKey(e => e.TeamID); });
            modelBuilder.Entity<FirstDivisionTable>(entity => { entity.HasKey(e => e.TeamID); });
            modelBuilder.Entity<CoronaLeagueTable>(entity => { entity.HasKey(e => e.TeamID); });
            modelBuilder.Entity<GoalsConcededByTeam>(entity => { entity.HasKey(e => new { e.TeamID, e.LeagueID }); });
            modelBuilder.Entity<GoalsScoredByTeam>(entity => { entity.HasKey(e => new { e.TeamID, e.LeagueID }); });
            modelBuilder.Entity<GoalsScoredByPlayer>(entity => { entity.HasKey(e => new { e.PlayerID, e.LeagueID }); }) ;
            modelBuilder.Entity<AllUsers>(entity => { entity.HasKey(e => e.ID); });

            modelBuilder.Entity<Match>().ToTable("Match");
            modelBuilder.Entity<MatchRoster>().ToTable("MatchRoster");
            modelBuilder.Entity<MatchStat>().ToTable("MatchStat");
            modelBuilder.Entity<MatchType>().ToTable("MatchType");
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<PlayerSeason>().ToTable("PlayerSeason");
            modelBuilder.Entity<Season>().ToTable("Season");
            modelBuilder.Entity<Team>().ToTable("Team");
            modelBuilder.Entity<TeamSeason>().ToTable("TeamSeason");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserTeam>().ToTable("UserTeam");
            modelBuilder.Entity<UserType>().ToTable("UserType");

            modelBuilder.Query<spLiveFixtures>();

        }
    }
}
