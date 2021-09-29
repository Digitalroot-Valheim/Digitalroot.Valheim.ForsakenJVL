using BepInEx;
using BepInEx.Configuration;
using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Digitalroot.Valheim.ForsakenJVL
{
  [BepInPlugin(Guid, Name, Version)]
  [BepInDependency(Jotunn.Main.ModGuid)]
  [BepInIncompatibility("com.bepinex.plugins.forsaken")]
  [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
  public class Main : BaseUnityPlugin, ITraceableLogging
  {
    public const string Version = "1.0.0";
    public const string Name = "Digitalroot ForsakenJVL";
    public const string Guid = "digitalroot.valheim.mods.forsaken.jvl";
    public const string Namespace = "Digitalroot.Valheim." + nameof(ForsakenJVL);
    [UsedImplicitly] public static ConfigEntry<int> NexusId;
    private Harmony _harmony;
    public static Main Instance;
    private AssetBundle _assetBundle;

    public Main()
    {
      Instance = this;
      NexusId = Config.Bind("General", "NexusID", 000, new ConfigDescription("Nexus mod ID for updates", null, new ConfigurationManagerAttributes { IsAdminOnly = false, Browsable = false, ReadOnly = true }));
#if DEBUG
      EnableTrace = true;
      Log.RegisterSource(Instance);
#else
      EnableTrace = false;
#endif
      Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    [UsedImplicitly]
    private void Awake()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        var assetFile = new FileInfo(Path.Combine(BepInEx.Paths.PluginPath, "Digitalroot.Valheim.ForsakenJVL", "forsakenmod"));

        if (!assetFile.Exists)
        {
          Log.Error(Instance, $"Unable to find asset file 'forsakenmod', please make sure 'forsakenmod' and 'Digitalroot.Valheim.ForsakenJVL.dll' are in {assetFile.DirectoryName}");
          Log.Error(Instance, $"Digitalroot.Valheim.ForsakenJVL is not loaded.");
          return;
        }

        _assetBundle = AssetUtils.LoadAssetBundle(assetFile.FullName);


#if DEBUG
        foreach (var assetName in _assetBundle.GetAllAssetNames())
        {
          Log.Trace(Main.Instance, assetName);
        }
#endif

        _itemPrefabBattleaxeLightning = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_BattleaxeLightning/v801_BattleaxeLightning.prefab");
        _itemPrefabSpearSpirit = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SpearSpirit/v801_SpearSpirit.prefab");
        _itemPrefabSledgePoison = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SledgePoison/v801_SledgePoison.prefab");
        _itemPrefabSledgeFire = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SledgeFire/v801_SledgeFire.prefab");
        _itemPrefabUnarmedFrost = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_UnarmedFrost/v801_UnarmedFrost.prefab");
        _itemPrefabUnarmedFrostOh = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_UnarmedFrost/v801_UnarmedFrostOH.prefab");
        _itemPrefabUnarmedFenring = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_UnarmedFenring/v801_UnarmedFenring.prefab");
        _itemPrefabSwordFire = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SwordFire/v801_SwordFire.prefab");
        _itemPrefabSwordLightning = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SwordLightning/v801_SwordLightning.prefab");
        _itemPrefabShieldFire = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_ShieldFire/v801_ShieldFire.prefab");
        _itemPrefabSpellFire = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SpellFire/v801_SpellFire.prefab");
        _itemPrefabBowFrost = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_BowFrost/v801_BowFrost.prefab");
        _itemPrefabKnifeFrost = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/weapons/v801_KnifeFrost/v801_KnifeFrost.prefab");
        _assetBundle.Unload(false);
        PrefabManager.OnVanillaPrefabsAvailable += RegisterObjects;
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private static void RegisterObjects()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        AddBattleaxeLightning();
        // AddSpearSpirit();
        // AddSledgePoison();
        // AddSledgeFire();
        // AddUnarmedFrost();
        // AddUnarmedFrostOh();
        // AddSwordFire();
        // AddSwordLightning();
        // AddUnarmedFenring();
        // AddShieldFire();
        // AddSpellFire();
        // AddBowFrost();
        // AddKnifeFrost();

        PrefabManager.OnVanillaPrefabsAvailable -= RegisterObjects;
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private static void AddBattleaxeLightning()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        if (_itemPrefabBattleaxeLightning == null)
        {
          throw new NullReferenceException(nameof(_itemPrefabBattleaxeLightning));
        }

        var itemDrop = _itemPrefabBattleaxeLightning.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        // var statusEffect = itemDrop.m_itemData.m_shared.m_consumeStatusEffect as SE_Stats;
        //
        // if (statusEffect == null)
        // {
        //   throw new NullReferenceException(nameof(statusEffect));
        // }
        //
        // statusEffect.m_cooldown = _secondWindCooldown.Value;
        // statusEffect.m_ttl = _secondWindTtl.Value;
        // statusEffect.m_runStaminaDrainModifier = _secondWindrunDrain.Value;
        // statusEffect.m_jumpStaminaUseModifier = _secondWindjumpDrain.Value;
        // statusEffect.m_staminaRegenMultiplier = _secondWindRegen.Value;

        ItemManager.Instance.AddItem(new CustomItem(_itemPrefabBattleaxeLightning, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.FineWood
              , Amount = 30
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.LeatherScraps
              , Amount = 12
              , AmountPerLevel = 4
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Silver
              , Amount = 90
              , AmountPerLevel = 25
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyEikthyr
              , Amount = 1
              , AmountPerLevel = 0
            }
          }
        }));
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    #region Implementation of ITraceableLogging

    /// <inheritdoc />
    public string Source => Namespace;

    /// <inheritdoc />
    public bool EnableTrace { get; }

    #endregion


    // Token: 0x04000003 RID: 3
    private static GameObject _itemPrefabBattleaxeLightning;

    // Token: 0x04000004 RID: 4
    private static GameObject _itemPrefabSpearSpirit;

    // Token: 0x04000005 RID: 5
    private static GameObject _itemPrefabSledgePoison;

    // Token: 0x04000006 RID: 6
    private static GameObject _itemPrefabSledgeFire;

    // Token: 0x04000007 RID: 7
    private static GameObject _itemPrefabUnarmedFrost;

    // Token: 0x04000008 RID: 8
    private static GameObject _itemPrefabUnarmedFrostOh;

    // Token: 0x04000009 RID: 9
    private static GameObject _itemPrefabUnarmedFenring;

    // Token: 0x0400000A RID: 10
    private static GameObject _itemPrefabSwordFire;

    // Token: 0x0400000B RID: 11
    private static GameObject _itemPrefabSwordLightning;

    // Token: 0x0400000C RID: 12
    private static GameObject _itemPrefabShieldFire;

    // Token: 0x0400000D RID: 13
    private static GameObject _itemPrefabSpellFire;

    // Token: 0x0400000E RID: 14
    private static GameObject _itemPrefabBowFrost;

    // Token: 0x0400000F RID: 15
    private static GameObject _itemPrefabKnifeFrost;
  }
}
