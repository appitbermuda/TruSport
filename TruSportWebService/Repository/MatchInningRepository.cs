using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class MatchInningRepository : IOnTrackRepository<MatchInning>
    {
        OnTrackContext _context;

        public MatchInningRepository(OnTrackContext context)
        {
            _context = context;
        }
        public async Task Delete(string id)
        {
            var matchStat = _context.MatchInnings.FirstOrDefault(e => e.ID == id);

            _context.MatchInnings.Remove(matchStat);

            await _context.SaveChangesAsync();
        }

        public async Task<MatchInning> Get(string id)
        {
            return await _context.MatchInnings.Include(e => e.BattingTeam).Include(e => e.FieldingTeam).Include(e => e.Fixture).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<List<MatchInning>> GetByFixture(string fixtureID)
        {
            try
            {
                var matchInnings = await _context.MatchInnings.Include(e => e.BattingTeam).Include(e => e.FieldingTeam).Include(e => e.Fixture).Where(e => e.FixtureID == fixtureID).ToListAsync();

                return matchInnings;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetByFixture");
            }

            return null;
        }

        public async Task<bool> AddNewInning(string fixtureID)
        {
            try
            {
                var fixture = await _context.CricketFixtures.FirstOrDefaultAsync(e => e.ID == fixtureID);
                var matchInning = await _context.MatchInnings.Include(e => e.BattingTeam).Include(e => e.FieldingTeam).Include(e => e.Fixture).Where(e => e.FixtureID == fixtureID).ToListAsync();

                if (matchInning == null)
                {
                    MatchInning newInning = new MatchInning
                    {
                        BattingTeamID = fixture.HomeTeamID,
                        FieldingTeamID = fixture.AwayTeamID,
                        FixtureID = fixtureID,
                        Inning = 1,
                        Order = 1
                    };

                    _context.MatchInnings.Add(newInning);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    if(fixture.MatchType.Name == "T20")
                    {
                        var firstInning = matchInning.FirstOrDefault();

                        MatchInning newInning = new MatchInning
                        {
                            BattingTeamID = firstInning.FieldingTeamID,
                            FieldingTeamID = firstInning.BattingTeamID,
                            FixtureID = fixtureID,
                            Inning = 1,
                            Order = 2
                        };

                        _context.MatchInnings.Add(newInning);
                        await _context.SaveChangesAsync();

                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Match Inning");
            }

            return false;
        }

        public async Task Insert(MatchInning item)
        {
            try
            {
                var matchInning = await _context.MatchInnings.FirstOrDefaultAsync(e => e.FixtureID == item.FixtureID);

                if (matchInning == null)
                {
                    item.Order = 1;
                    item.Inning = 1;

                    _context.MatchInnings.Add(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Match Inning");
            }
        }

        public async Task InsertAll(List<MatchInning> items)
        {
            foreach (var item in items)
            {
                _context.MatchInnings.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(MatchInning item)
        {
            _context.MatchInnings.Update(item);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAll(List<MatchInning> items)
        {
            foreach (var item in items)
            {
                _context.MatchInnings.Update(item);
                await _context.SaveChangesAsync();
            }
        }

        public Task<IEnumerable<MatchInning>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
