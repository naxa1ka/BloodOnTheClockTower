namespace BloodClockTower.Game
{
    public record VoteRole(bool IsInitiator, bool IsNominee, bool IsParticipant)
    {
        public bool IsIgnored => !IsInitiator && !IsNominee && !IsParticipant; 
        public static VoteRole Default => new(false, false, false);
            
        public VoteRole MarkInitiator => this with { IsInitiator = true };
        public VoteRole UnmarkInitiator => this with { IsInitiator = false };
            
        public VoteRole MarkNominee => this with { IsNominee = true };
        public VoteRole UnmarkNominee => this with { IsNominee = false };
            
        public VoteRole MarkParticipant => this with { IsParticipant = true };
        public VoteRole UnmarkParticipant => this with { IsParticipant = false };
    }
}