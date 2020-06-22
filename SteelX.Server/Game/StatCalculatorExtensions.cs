using System;
using System.Linq;
using Data.Model;
using Data.Model.Items;
using GameServer.Config;
using GameServer.Config.Poo;

namespace GameServer.Game
{
    public static class StatCalculatorExtensions
    {
        /// <summary>
        /// Calculates all stats required for gameplay on this unit
        /// </summary>
        /// <param name="unit"></param>
        public static void CalculateStats(this Unit unit)
        {
            unit.CalculateMaxHealth();
            unit.CalculateOverheatParameters();
        }
        
        /// <summary>
        /// Calculates the max health and assigns it for a unit
        /// </summary>
        /// <param name="unit"></param>
        private static void CalculateMaxHealth(this Unit unit)
        {
            var headHp = PooReader.Head.First(h => h.TemplateId == unit.Head.TemplateId).HitPoints;
            var chestHp = PooReader.Chest.First(h => h.TemplateId == unit.Chest.TemplateId).HitPoints;
            var armHp = PooReader.Arm.First(h => h.TemplateId == unit.Arms.TemplateId).HitPoints;
            var legHp = PooReader.Leg.First(h => h.TemplateId == unit.Legs.TemplateId).HitPoints;
            var boosterHp = PooReader.Booster.First(h => h.TemplateId == unit.Backpack.TemplateId).HitPoints;

            // calculate
            // TODO: User ability growth
            unit.MaxHealth = headHp + chestHp + armHp + legHp + boosterHp;
            
            Console.WriteLine("Calculated max hp of {0} for unit {1}", unit.MaxHealth, unit.Id);
        }

        /// <summary>
        /// Calculates the overheat parameters for this unit
        /// </summary>
        /// <param name="unit"></param>
        private static void CalculateOverheatParameters(this Unit unit)
        {
            var armStats = PooReader.Arm.First(h => h.TemplateId == unit.Arms.TemplateId);
            
            // Calculate weaponset stats
            // TODO: Handle 2h weapons
            unit.WeaponSet1Left.CalculateWeaponParameters(armStats);
            unit.WeaponSet1Right.CalculateWeaponParameters(armStats);
            
            unit.WeaponSet2Left.CalculateWeaponParameters(armStats);
            unit.WeaponSet2Right.CalculateWeaponParameters(armStats);
            
        }

        /// <summary>
        /// Calculates the damage and overheat parameters for this weapon
        /// </summary>
        /// <param name="weapon"></param>
        private static void CalculateWeaponParameters(this Weapon weapon, Arm armStats)
        {
            WeaponBase weaponStats;

            switch (weapon.Type)
            {
                case 7:
                    weaponStats = PooReader.Gun.First(g => g.TemplateId == weapon.TemplateId);
                    break;
                
                // TODO: Error handling here
                default:
                    return;
            }
            
            // Stats
            weapon.MaxOverheat = armStats.Endurance;
            weapon.OverheatRecovery = armStats.Recovery;
            weapon.Damage = weaponStats.Damage;
            weapon.OverheatPerShot = weaponStats.OverheatPoint;
            weapon.NormalRecovery = weaponStats.OverheatRecovery;
            weapon.IsAutomatic = weaponStats.WeaponType == WeaponType.machingun;
            if (weapon.IsAutomatic)
            {
                var gunStats = (Gun) weaponStats;
                weapon.ReloadTime = gunStats.ReloadTime;
                weapon.NumberOfShots = gunStats.NumberOfShots;
            }
            
            Console.WriteLine("Calculated weapon stats: damage {0}, overheat: {1} {2} {3} {4} for weapon {5}", 
                weapon.Damage, weapon.OverheatPerShot, weapon.NormalRecovery, weapon.MaxOverheat, weapon.OverheatRecovery, weapon.Id);
        }
    }
}