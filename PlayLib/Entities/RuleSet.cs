using System;
using System.Collections.Generic;

using CrazyEights.PlayLib.Enums;

namespace CrazyEights.PlayLib.Entities
{
    public class RuleSet
    {
        public bool this[RuleIds id]
        {
            get => Get(id);
            set => Update(id, value);
        }

        private IDictionary<RuleIds, bool> Rules { get; set; }

        public RuleSet()
        {
            Rules = new Dictionary<RuleIds, bool>();
            foreach (RuleIds ruleId in Enum.GetValues(typeof(RuleIds)))
            {
                Rules.Add(ruleId, false);
            }

            Rules[RuleIds.StackDrawEffect] = true;
            Rules[RuleIds.WildFinish] = true;
        }

        private bool Get(RuleIds id)
        {
            if (!Rules.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException("ruleId", $"Rule Id {id} is invalid");
            }

            return Rules[id];
        }

        private void Update(RuleIds id, bool value)
        {
            if (!Rules.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException("ruleId", $"Rule Id {id} is invalid");
            }

            Rules[id] = value;
        }
    }
}
