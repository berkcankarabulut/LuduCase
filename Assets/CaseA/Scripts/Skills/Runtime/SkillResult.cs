using CaseA.Character.Runtime;
using CaseA.Health.Runtime;

namespace CaseA.Skills.Runtime
{
    public struct SkillResult
    {
        public bool Success;
        public ITargetable Target;

        public float DamageDealt;
        public float HealingDone;
        public bool IsCritical;
        public float InitialDamage;
        public float ResistanceValue;
        public float DamageReduction;

        public string EffectApplied;
        public float EffectDuration;

        public static SkillResult Failed => new SkillResult { Success = false };
    }
}