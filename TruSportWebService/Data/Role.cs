using System;
namespace OnTrackWebService.Data
{
    public static class Role
    {
        public const string AllUsers = "Administrator,Team Administrator,Match Commissioner";
        public const string Admin = "Administrator";
        public const string TeamAdmin = "Administrator,Team Administrator";
        public const string MatchCommissioner = "Administrator,Match Commissioner";
    }
}
