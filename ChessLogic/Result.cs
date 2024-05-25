namespace ChessLogic
{
    public class Result
    {
        public Player Winner { get; }
        public ReasonOfTheEnd Reason {  get; }

        public Result(Player winner, ReasonOfTheEnd reason)
        {
            Winner = winner;
            Reason = reason;
        }
        
        public static Result Win (Player winner)
        {
            return new Result(winner, ReasonOfTheEnd.Checkmate);
        }

        public static Result Draw(ReasonOfTheEnd reason)
        {
            return new Result(Player.None, reason);
        }    
    }
}
