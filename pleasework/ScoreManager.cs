namespace pleasework
{
    public class ScoreManager
    {
        public int Score { get; private set; }
        public int Combo { get; private set; }

        public void RegisterHit()
        {
            Combo++;
            Score += 100 * Combo;
        }

        public void RegisterMiss()
        {
            Combo = 0;
        }
    }
}
