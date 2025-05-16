<!DOCTYPE html>
<html lang="en"> 
<body>

<h1>⚔️ Combat Skill System for Unity</h1>
<p>A flexible and extensible combat and skill system for Unity games featuring damage types, status effects, health management, and combat logging.</p>

<h2>🌟 Overview</h2>
<p>This system provides a complete framework for implementing RPG-style combat mechanics in Unity games. It includes support for:</p>
<ul>
  <li>Direct and over-time damage/healing effects</li>
  <li>Type-based damage resistance</li>
  <li>Character stats and health management</li>
  <li>Status effect application and management</li>
  <li>Combat logging and visualization</li>
  <li>Editor tools for testing and debugging</li>
</ul>

<h2>🏗️ Architecture</h2>
<p>The system is built with modularity and extensibility in mind, using a combination of:</p>
<ul>
  <li>ScriptableObjects for skill and damage type definitions</li>
  <li>Component-based character attributes</li>
  <li>Interface-driven design for flexibility</li>
  <li>Factory pattern for creating effects</li>
  <li>Observer pattern for event handling</li>
  <li>Editor extensions for testing</li>
</ul>

<h3>Core Systems Diagram</h3>
<pre>
┌─────────────────┐    ┌───────────────┐    ┌────────────────┐
│ Character       │    │ Combat Skills │    │ Status Effects │
│ - Health        │◄───┤ - Damage      │───►│ - DoT/HoT      │
│ - Resistances   │    │ - Healing     │    │ - Buffs/Debuffs│
│ - Effects       │    │ - Utility     │    │ - Resistances  │
└─────────────────┘    └───────────────┘    └────────────────┘
         ▲                    ▲                    ▲
         │                    │                    │
         └────────────────────┼────────────────────┘
                              │
                     ┌────────┴────────┐
                     │   Combat Log    │
                     │ - Console       │
                     │ - Editor Window │
                     └─────────────────┘
</pre>

<h2>🧩 Key Components</h2>

<h3>Character Framework</h3>
<ul>
  <li><strong>Targetable</strong>: Base abstract class for any entity that can be targeted by skills</li>
  <li><strong>HealthSystem</strong>: Manages character health, death, and critical state</li>
  <li><strong>ResistanceSystem</strong>: Handles damage resistance calculations</li>
  <li><strong>RegenerationSystem</strong>: Controls health regeneration over time</li>
</ul>

<h3>Skills</h3>
<p><strong>SkillBase</strong>: Abstract base class for all skills</p>
<p>Concrete implementations:</p>
<ul>
  <li><strong>DamageSkill</strong>: Direct damage with critical hit chance</li>
  <li><strong>HealingSkill</strong>: Direct healing</li>
  <li><strong>DamageOverTimeSkill</strong>: Damage applied over duration</li>
  <li><strong>HealOverTimeSkill</strong>: Healing applied over duration</li>
  <li><strong>ResistanceSkill</strong>: Temporarily modifies resistance to damage types</li>
  <li><strong>HealthModifierSkill</strong>: Modifies max health and regeneration</li>
</ul>

<h3>Status Effects</h3>
<ul>
  <li><strong>StatusEffectBase</strong>: Base for all status effects</li>
  <li><strong>OverTimeEffect</strong>: Applies damage/healing at intervals</li>
  <li><strong>ResistanceModifierEffect</strong>: Temporarily changes damage resistance</li>
  <li><strong>HealthModifierEffect</strong>: Alters max health and regeneration</li>
</ul>

<h3>Damage Types</h3>
<ul>
  <li><strong>DamageTypeSO</strong>: ScriptableObject for defining damage types</li>
  <li><strong>IDamageType</strong>: Interface for damage type implementation</li>
  <li><strong>DamageTypeDatabase</strong>: Repository of all available damage types</li>
</ul>

<h3>Combat Logging</h3>
<ul>
  <li><strong>CombatLogger</strong>: Runtime component for logging combat events</li>
  <li><strong>CombatLogWindow</strong>: Editor window for visualizing combat events</li>
</ul>

<h2>💻 Usage Examples</h2>

<h3>Creating a Character</h3>
<pre><code>public class Enemy : Targetable
{
    // Customize as needed
}
</code></pre>

<h3>Creating Damage Types</h3>
<pre><code>// Using the ScriptableObject creation menu
// Assets > Create > Game > Damage Type

// Or via code
var fireType = ScriptableObject.CreateInstance&lt;DamageTypeSO&gt;();
fireType.name = "Fire";
</code></pre>

<h3>Creating and Using Skills</h3>
<pre><code>var fireball = ScriptableObject.CreateInstance&lt;DamageSkill&gt;();
fireball.name = "Fireball";

var target = FindObjectOfType&lt;Enemy&gt;();
fireball.Execute(target, gameObject);
</code></pre>

<h3>Applying Status Effects</h3>
<pre><code>var targetCharacter = GetComponent&lt;Targetable&gt;();

targetCharacter.Effects.ApplyDamageOverTime(
    damagePerTick: 5f,
    duration: 10f,
    tickInterval: 1f,
    damageType: fireDamageType,
    effectName: "Burning",
    source: gameObject
);
</code></pre>

<h2>🛠️ Editor Tools</h2>
<p>The system includes several editor tools to help with development and testing:</p>
<ul>
  <li><strong>SkillManagerEditor</strong>: Custom inspector for testing skills in the editor</li>
  <li><strong>TargetableEditor</strong>: Visualizes health and status effects</li>
  <li><strong>CombatLogWindow</strong>: Dedicated window for viewing combat events</li>
</ul>
<p>Access the combat log window via: <strong>Game > Combat Log</strong> menu.</p>

<h2>🔄 Events</h2>
<ul>
  <li><strong>SkillEvents.OnSkillExecuted</strong>: Fired when a skill is used</li>
  <li><strong>SkillEvents.OnEffectTick</strong>: Fired when a tick of an over-time effect occurs</li>
  <li><strong>HealthSystem.OnHealthChanged</strong>: Fired when health changes</li>
  <li><strong>HealthSystem.OnDeath</strong>: Fired when a character dies</li>
  <li><strong>HealthSystem.OnCriticalHealth</strong>: Fired when a character reaches critical health</li>
</ul>

<h2>📋 Implementation Recommendations</h2>
<ul>
  <li>Extend the base classes: Create your game-specific skills by extending the provided base classes</li>
  <li>Use ScriptableObjects: Create skill and damage type definitions as assets</li>
  <li>Implement character classes: Extend the Targetable class for your specific character types</li>
  <li>Hook into events: Subscribe to the provided events for UI updates and game logic</li>
  <li>Use the editor tools: Leverage the custom editors for testing and debugging</li>
</ul>

<h2>🧪 Testing the System</h2>
<ul>
  <li>Create character prefabs with the Targetable component</li>
  <li>Create skill ScriptableObjects</li>
  <li>Use the SkillManager to test skill execution in play mode</li>
  <li>Monitor the Combat Log window to see detailed information</li>
</ul>

<h2>🧠 Design Considerations</h2>
<ul>
  <li><strong>Performance</strong>: The system uses object pooling and optimization techniques where appropriate</li>
  <li><strong>Extensibility</strong>: All major components can be extended or replaced</li>
  <li><strong>Debugging</strong>: Comprehensive logging and visualization tools</li>
  <li><strong>Maintainability</strong>: Clean separation of concerns through interface-based design</li>
</ul>

</body>
</html>
