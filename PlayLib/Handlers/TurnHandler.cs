using System;
using System.Collections;
using System.Collections.Generic;

namespace CrazyEights.PlayLib.Handlers
{
    public class TurnHandler
    {
        public int NextId { get => PlayerIds[CurrentTurn]; }

        private int CurrentTurn { get; set; }
        private int Direction { get; set; }
        private int TotalSkips { get; set; }
        private IList<int> PlayerIds { get; set; }

        public TurnHandler(IList<int> playerIds, Func<int, int> getRandomInt)
        {
            PlayerIds = new List<int>(playerIds);
            CurrentTurn = getRandomInt(PlayerIds.Count);
            Direction = 1;
            TotalSkips = 0;
        }

        public void ReverseOrder()
        {
            Direction *= -1;
        }

        public void SkipTurns(int total)
        {
            TotalSkips = total != 0 ? total : PlayerIds.Count - 1;
        }

        public void ComputeNextTurn()
        {
            var relativeDirection = Direction;
            if (TotalSkips != 0)
            {
                var turnsToSkip = (TotalSkips % PlayerIds.Count) * Direction;
                CurrentTurn += turnsToSkip;
                if (TotalSkips < 0)
                {
                    relativeDirection = Direction * -1;
                }
                TotalSkips = 0;
            }

            CurrentTurn += relativeDirection;

            if (CurrentTurn < 0)
            {
                CurrentTurn = CurrentTurn + PlayerIds.Count;
            }

            if (CurrentTurn >= PlayerIds.Count)
            {
                CurrentTurn = CurrentTurn - PlayerIds.Count;
            }
        }
    }
}
