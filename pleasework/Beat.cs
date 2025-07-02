using Microsoft.Xna.Framework;
using System;

namespace pleasework
{
    public class Beat
    {
        public enum SpawnCorner
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        public SpawnCorner Corner { get; private set; }
        public double ArrivalTime { get; private set; }
        public bool IsHit { get; set; }
        public bool IsMissed { get; set; }

        private const float Speed = 150f; // pixels per second

        public Beat(SpawnCorner corner, double arrivalTime)
        {
            Corner = corner;
            ArrivalTime = arrivalTime;
        }

        public Vector2 GetPosition(double currentTime, Vector2 centre)
        {
            double timeUntilArrival = ArrivalTime - currentTime;

            if (timeUntilArrival <= 0)
                return centre;

            float distanceFromCentre = (float)(Speed * timeUntilArrival);

            Vector2 direction = Corner switch
            {
                SpawnCorner.TopLeft => new Vector2(-1, -1),
                SpawnCorner.TopRight => new Vector2(1, -1),
                SpawnCorner.BottomLeft => new Vector2(-1, 1),
                SpawnCorner.BottomRight => new Vector2(1, 1),
                _ => Vector2.Zero
            };
            direction.Normalize();

            return centre + direction * distanceFromCentre;
        }
    }
}
