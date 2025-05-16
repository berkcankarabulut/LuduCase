using System;
using CaseA.Character.Runtime;
using UnityEngine;

namespace CaseA.Skills.Runtime
{ 
    public class SkillEventArgs : EventArgs
    {
        public SkillBase Skill { get; }
        public Targetable Target { get; }
        public GameObject Source { get; }
        public SkillResult Result { get; }
        
        public SkillEventArgs(SkillBase skill, Targetable target, GameObject source, SkillResult result)
        {
            Skill = skill;
            Target = target;
            Source = source;
            Result = result;
        }
    }
}