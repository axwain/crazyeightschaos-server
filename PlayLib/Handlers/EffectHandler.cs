using System;
using System.Collections.Generic;

using CrazyEights.PlayLib.Enums;

namespace CrazyEights.PlayLib.Handlers
{
    public class EffectHandler
    {
        private IDictionary<Effects, Action<int>> EffectMap { get; set; }

        public EffectHandler()
        {
            EffectMap = new Dictionary<Effects, Action<int>>(4);
        }

        public bool Register(Effects effect, Action<int> command)
        {
            if (effect != Effects.None)
            {
                EffectMap[effect] = command;
                return true;
            }

            return false;
        }

        public void Execute(Effects effect, int args)
        {
            if (effect != Effects.None)
            {
                EffectMap[effect](args);
            }
        }
    }
}