namespace DAL.Utilities
{
    public static class EndpointBuilder
    {
        private const string Male = @"https://worldcup-vua.nullbit.hr/men";
        private const string Female = @"https://worldcup-vua.nullbit.hr/women";

        private static string GetTeamGender(string gender)
        {
            return gender.ToLower() switch
            {
                "male" => Male,
                "female" => Female,
                _ => string.Empty
            };
        }

        public static string GetTeamsEndpoint(string gender)
        {
            var endpoint = $@"{GetTeamGender(gender)}/teams";
            return endpoint;
        }

        public static string GetTeamResultsEndpoint(string gender)
        {
            var endpoint = $@"{GetTeamsEndpoint(gender)}/results";
            
            return endpoint;
        }

        public static string GetMatchesEndpoint(string gender)
        {
            var endpoint = $@"{GetTeamGender(gender)}/matches";
            
            return endpoint;
        }

  
    }
}
