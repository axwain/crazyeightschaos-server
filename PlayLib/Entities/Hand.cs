using System;
using System.Collections.Generic;

namespace CrazyEights.PlayLib.Entities
{
    public class Hand
    {
        public int Size { get => Cards.Count; }
        private readonly int INITIAL_CAPACITY = 20;
        private IDictionary<int, Card> Cards { get; set; }

        public Hand()
        {
            Cards = new Dictionary<int, Card>(INITIAL_CAPACITY);
        }

        public bool AddCard(int id, Card card)
        {
            if (Cards.ContainsKey(id))
            {
                return false;
            }

            Cards.Add(id, card);

            return true;
        }

        public bool RemoveCard(int id)
        {
            return Cards.Remove(id);
        }

        public Card PeekCard(int id)
        {
            return Cards[id];
        }
    }
}