using System.Collections.Generic;
using UnityEngine;

public class CharactersLibrary : MonoBehaviour
{
    private static CharactersLibrary instance;
    public static CharactersLibrary Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharactersLibrary>();   
            }

            return instance;
        }
    }


    [SerializeField]
    private List<CharacterProfile> m_allGameCharacters = new List<CharacterProfile>();
    public List<CharacterProfile> AllGameCharacters => m_allGameCharacters;


    public enum CharacterType
    {
        None,
        StoneGolem,
        IronGolem,
        IceGolem,
        FireGolem,
        CrystalGolem,
        ForestGolem,
        DarkGolem,

        LightDragon,
        DarkDragon,
        GreenDragon,
        GoldenDragon,
        RedDragon,
        JadeDragon,
        BlueDragon,
        SpectralDragon,
        ShadowDragon,

        EntHealer,
        EntKiller,
        EntDefender,
        EntKing,
        EntWarrior,

        FireElemental,
        EarthElemental,
        WindElemental,
        WaterElemental,

        ReptileWarrior,
        ReptileMage,
        ReptileReaper,
        ReptileHealer,

        DarkElementalOne,
        DarkElementalTwo,
        DarkElementalThree,

        SkeletMage,
        SkeletWarrior,
        Skelet,

        EntLeader,
        EntGuardian,

        StoneScorpion,
        Mimick,
        StoneMonster,
        PoisonSlime,
        UnderArmy,

        GreenOrc,
        RedOrc,
        WolfOrc,
        OrcBetrayer,
        OrcGienas,

        WaterDragonMutant,
        EyeSaurMutant,
        Kraken,
        KrakenMutant,
        ForestMonster,
        GorgouleMutant,
        DinosaurMonster,
        GrassMonster,
        HomaMutant,
        DragonMutant,
        GrasshopperMutant,
        SpiderMutant,

        GoblinsSquad,
        Barsuk,
        ForestMiphycal,
        ForestScares,
        FlyingGollandec,
        MechanicalHawk,
        FrozenWolf,
        Wolverine,
        Ghost,
        CursedTigers,
        SomethingInTheFar,
        BalanceKeeper,

        AngelKeeper,
        GhostArmy,
        Vampire,
        Witch,
        Thief,
        BirdMage,
        ShadowMage,
        ForestElf,
        DeathKeeper,
        PoisonKeeper,
        LightKnight,
        LightMage,
        LightPriest,
        SpiraleMage,
        SwordKeeper,
        DamagedMage,
        Enchantress,

        HellHound,
        HellDeath,
        HellExecuter,
        HellTwoHeadsHound,
        HellDemon,
        HellThreeHeadsHound,
        Satana,
        HellScareDemon,

        RedRabbit,
    }
    public enum CharacterAttackType
    {
        None,
        Ranged,
        Magic,
        Melee,
        Throwable
    }
    public enum CharacterArmorType
    {
        None,
        Light,
        Medium,
        Heavy,
        Magic
    }
    public enum CharacterElement
    {
        None,
        Wind,
        Water,
        Fire,
        Earth,
        Light,
        Dark
    }
}