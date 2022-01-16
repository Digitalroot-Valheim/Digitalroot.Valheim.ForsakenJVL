using BepInEx;
using BepInEx.Configuration;
using Digitalroot.Valheim.Common;
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
  public partial class Main : BaseUnityPlugin, ITraceableLogging
  {
    [UsedImplicitly] public static ConfigEntry<int> NexusId;
    public static Main Instance;
    private AssetBundle _assetBundle;

    public Main()
    {
      Instance = this;
      NexusId = Config.Bind("General", "NexusID", 1538, new ConfigDescription("Nexus mod ID for updates", null, new ConfigurationManagerAttributes { IsAdminOnly = false, Browsable = false, ReadOnly = true }));
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

        var assetFile = new FileInfo(Path.Combine(new FileInfo(typeof(Main).Assembly.Location).DirectoryName ?? throw new InvalidOperationException("Unable to load assetFile."), "forsakenmod"));

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
        PrefabManager.OnVanillaPrefabsAvailable += AddCustomItems;
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddCustomItems()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        AddBattleaxeLightning();
        AddBowFrost();
        AddSledgeFire();
        AddSledgePoison();
        AddSpearSpirit();
        AddUnarmedFenring();
        AddKnifeFrost();
        AddUnarmedFrost();
        AddUnarmedFrostOh();
        AddSwordFire();
        AddSwordLightning();
        AddShieldFire();
        AddSpellFire();

        _assetBundle.Unload(false);
        PrefabManager.OnVanillaPrefabsAvailable -= AddCustomItems;
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddSpellFire()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SpellFire/v801_SpellFire.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_spellfire";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_spellfire_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.OneHandedWeapon;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_weight = 0.1f;
        itemDrop.m_itemData.m_shared.m_maxDurability = 600f;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_blockPower = 0f;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 0f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 0f;
        itemDrop.m_itemData.m_shared.m_attackForce = 0f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_fire = 20f;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 7
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Crystal
              , Amount = 40
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyGoblinKing
              , Amount = 12
              , AmountPerLevel = 0
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.YagluthDrop
              , Amount = 36
              , AmountPerLevel = 2
            }
          }
        }));
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddShieldFire()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_ShieldFire/v801_ShieldFire.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_shieldfire";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_shieldfire_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.Shield;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_weight = 10f;
        itemDrop.m_itemData.m_shared.m_maxDurability = 600f;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_blockPower = 120f;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 6f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 8f;
        itemDrop.m_itemData.m_shared.m_attackForce = 0f;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 7
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Silver
              , Amount = 20
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Flametal
              , Amount = 40
              , AmountPerLevel = 12
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyGoblinKing
              , Amount = 10
              , AmountPerLevel = 0
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.YagluthDrop
              , Amount = 30
              , AmountPerLevel = 2
            }
          }
        }));
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddSwordLightning()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SwordLightning/v801_SwordLightning.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_swordlightning";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_swordlightning_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.OneHandedWeapon;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_maxDurability = 700f;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 2f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 2f;
        itemDrop.m_itemData.m_shared.m_attackForce = 8f;
        itemDrop.m_itemData.m_shared.m_blockPower = 30f;
        itemDrop.m_itemData.m_shared.m_damages.m_slash = 40f;
        itemDrop.m_itemData.m_shared.m_damages.m_pierce = 40f;
        itemDrop.m_itemData.m_shared.m_damages.m_lightning = 70f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_slash = 5;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_pierce = 5;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_lightning = 10f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 20f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_damageMultiplier = 1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackStamina = 30f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactor = 0.3f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactorRotation = 0.3f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_forceMultiplier = 10f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_staggerMultiplier = 6f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_lowerDamagePerHit = false;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 7
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Crystal
              , Amount = 40
              , AmountPerLevel = 40
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Flametal
              , Amount = 60
              , AmountPerLevel = 60
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyGoblinKing
              , Amount = 12
              , AmountPerLevel = 0
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.YagluthDrop
              , Amount = 36
              , AmountPerLevel = 2
            }
          }
        }));
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void AddSwordFire()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SwordFire/v801_SwordFire.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_swordfire";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_swordfire_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.TwoHandedWeapon;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_weight = 30f;
        itemDrop.m_itemData.m_shared.m_maxDurability = 800f;
        itemDrop.m_itemData.m_shared.m_equipDuration = 1f;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_blockPower = 70f;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 4f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 8f;
        itemDrop.m_itemData.m_shared.m_attackForce = 0f;
        itemDrop.m_itemData.m_shared.m_movementModifier = -0.25f;
        itemDrop.m_itemData.m_shared.m_damages.m_slash = 0f;
        itemDrop.m_itemData.m_shared.m_damages.m_fire = 46f;
        itemDrop.m_itemData.m_shared.m_damages.m_frost = 46f;
        itemDrop.m_itemData.m_shared.m_damages.m_lightning = 46f;
        itemDrop.m_itemData.m_shared.m_damages.m_spirit = 46f;
        itemDrop.m_itemData.m_shared.m_damages.m_poison = 46f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_slash = 0f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_fire = 10f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_frost = 10f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_lightning = 10f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_spirit = 10f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_poison = 10f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 25f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackAnimation = "battleaxe_attack";
        itemDrop.m_itemData.m_shared.m_attack.m_attackChainLevels = 3;
        itemDrop.m_itemData.m_shared.m_attack.m_speedFactor = 0.1f;
        itemDrop.m_itemData.m_shared.m_attack.m_speedFactorRotation = 0.1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackAnimation = "battleaxe_secondary";
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackChainLevels = 0;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackStamina = 30f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactor = 0.1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactorRotation = 0.1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_damageMultiplier = 1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_forceMultiplier = 10f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_staggerMultiplier = 6f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_lowerDamagePerHit = false;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 7
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Iron
              , Amount = 40
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Crystal
              , Amount = 40
              , AmountPerLevel = 15
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Silver
              , Amount = 36
              , AmountPerLevel = 12
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyEikthyr
              , Amount = 12
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

    private void AddUnarmedFrostOh()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_UnarmedFrost/v801_UnarmedFrostOH.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_unarmedfrostoh";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_unarmedfrostoh_description";
        // itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_maxDurability = 700f;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 2f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 2f;
        itemDrop.m_itemData.m_shared.m_attackForce = 8f;
        itemDrop.m_itemData.m_shared.m_blockPower = 90f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 1f;
        itemDrop.m_itemData.m_shared.m_damages.m_blunt = 90f;
        itemDrop.m_itemData.m_shared.m_damages.m_frost = 80f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_blunt = 5f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_frost = 10f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 20f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackStamina = 20f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactor = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactorRotation = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_damageMultiplier = 1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_forceMultiplier = 10f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_staggerMultiplier = 6f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackRange = 3.5f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackAngle = 180f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackRayWidth = 0.3f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_lowerDamagePerHit = false;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 6
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.FreezeGland
              , Amount = 10
              , AmountPerLevel = 2
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyHatchling
              , Amount = 4
              , AmountPerLevel = 0
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.DragonTear
              , Amount = 20
              , AmountPerLevel = 5
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyDragonQueen
              , Amount = 4
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

    private void AddUnarmedFrost()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_UnarmedFrost/v801_UnarmedFrost.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_unarmedfrost";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_unarmedfrost_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.OneHandedWeapon;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_maxDurability = 700f;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 2f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 2f;
        itemDrop.m_itemData.m_shared.m_attackForce = 8f;
        itemDrop.m_itemData.m_shared.m_blockPower = 70f;
        itemDrop.m_itemData.m_shared.m_damages.m_blunt = 70f;
        itemDrop.m_itemData.m_shared.m_damages.m_frost = 90f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_blunt = 5f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_frost = 10f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 15f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackAnimation = "knife_secondary";
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackRayWidth = 0.3f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackStamina = 20f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactor = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactorRotation = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_damageMultiplier = 1.25f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_forceMultiplier = 10f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_staggerMultiplier = 6f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackRange = 2.5f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackAngle = 50f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackRayWidth = 0.3f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_lowerDamagePerHit = false;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 6
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.FreezeGland
              , Amount = 20
              , AmountPerLevel = 4
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyHatchling
              , Amount = 10
              , AmountPerLevel = 0
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.DragonTear
              , Amount = 80
              , AmountPerLevel = 20
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyDragonQueen
              , Amount = 8
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

    #region KnifeFrost

    private void AddKnifeFrost()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/weapons/v801_KnifeFrost/v801_KnifeFrost.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_knifefrost";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_knifefrost_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.OneHandedWeapon;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_weight = 1.5f;
        itemDrop.m_itemData.m_shared.m_maxDurability = 500f;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_backstabBonus = 4f;
        itemDrop.m_itemData.m_shared.m_damages.m_slash = 0f;
        itemDrop.m_itemData.m_shared.m_damages.m_pierce = 75f;
        itemDrop.m_itemData.m_shared.m_damages.m_frost = 75f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_slash = 0f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_pierce = 5f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_frost = 5f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 15f;
        itemDrop.m_itemData.m_shared.m_attack.m_speedFactor = 0.1f;
        itemDrop.m_itemData.m_shared.m_attack.m_speedFactorRotation = 0.1f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackHeight = 1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_damageMultiplier = 1.15f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackStamina = 30f;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 6
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.FreezeGland
              , Amount = 24
              , AmountPerLevel = 6
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Needle
              , Amount = 60
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.DragonTear
              , Amount = 80
              , AmountPerLevel = 20
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyDragonQueen
              , Amount = 8
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

    #endregion

    #region BattleaxeLightning

    private void AddBattleaxeLightning()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_BattleaxeLightning/v801_BattleaxeLightning.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_battleaxelightning";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_battleaxelightning_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.OneHandedWeapon;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_maxDurability = 400f;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_weight = 2.5f;
        itemDrop.m_itemData.m_shared.m_attackForce = 12f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 6f;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 6f;
        itemDrop.m_itemData.m_shared.m_damages.m_slash = 75f;
        itemDrop.m_itemData.m_shared.m_damages.m_lightning = 40f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_slash = 5f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_lightning = 5f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 30f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackAnimation = "swing_longsword";
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackAnimation = "atgeir_secondary";
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackChainLevels = 0;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackStamina = 20f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactor = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactorRotation = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_damageMultiplier = 0.75f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_forceMultiplier = 10f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_staggerMultiplier = 6f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackRange = 3.5f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackAngle = 360f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackRayWidth = 0.3f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_lowerDamagePerHit = false;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
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

    #endregion

    #region BowFrost

    private void AddBowFrost()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_BowFrost/v801_BowFrost.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_bowfrost";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_bowfrost_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.Bow;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_maxDurability = 600f;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_weight = 10f;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 6f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 8f;
        itemDrop.m_itemData.m_shared.m_attackForce = 0f;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 6
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.FreezeGland
              , Amount = 20
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Crystal
              , Amount = 20
              , AmountPerLevel = 6
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.DragonTear
              , Amount = 20
              , AmountPerLevel = 15
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyDragonQueen
              , Amount = 6
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

    #endregion

    #region SledgeFire

    private void AddSledgeFire()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SledgeFire/v801_SledgeFire.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_sledgefire";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_sledgefire_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.TwoHandedWeapon;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_maxDurability = 750f;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_weight = 4f;
        itemDrop.m_itemData.m_shared.m_blockPower = 80f;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 6f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 6f;
        itemDrop.m_itemData.m_shared.m_attackForce = 6f;
        itemDrop.m_itemData.m_shared.m_damages.m_blunt = 40f;
        itemDrop.m_itemData.m_shared.m_damages.m_pierce = 100f;
        itemDrop.m_itemData.m_shared.m_damages.m_fire = 112f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_blunt = 3f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_pierce = 10f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_fire = 5f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 20f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackAnimation = "battleaxe_attack";
        itemDrop.m_itemData.m_shared.m_attack.m_attackChainLevels = 3;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackAnimation = "swing_sledge";
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackStamina = 35f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactor = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactorRotation = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_forceMultiplier = 10f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_staggerMultiplier = 6f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_lowerDamagePerHit = false;

        // prefab.AddComponent<CMB_BurnWhenHeld>();

        var statusEffect = ScriptableObject.CreateInstance<SE_Burning>();

        if (statusEffect == null)
        {
          throw new NullReferenceException(nameof(statusEffect));
        }

        statusEffect.m_fireDamagePerHit = 1f;
        statusEffect.m_damageInterval = 1f;
        itemDrop.m_itemData.m_shared.m_equipStatusEffect = statusEffect;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 6
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Flametal
              , Amount = 60
              , AmountPerLevel = 15
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.SurtlingCore
              , Amount = 60
              , AmountPerLevel = 15
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.BlackMetal
              , Amount = 60
              , AmountPerLevel = 15
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophySurtling
              , Amount = 60
              , AmountPerLevel = 8
            }
          }
        }));
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    #endregion

    #region SledgePoison

    private void AddSledgePoison()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SledgePoison/v801_SledgePoison.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_sledgepoison";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_sledgepoison_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.TwoHandedWeapon;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_maxDurability = 550f;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_weight = 4f;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 6f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 6f;
        itemDrop.m_itemData.m_shared.m_attackForce = 12f;
        itemDrop.m_itemData.m_shared.m_blockPower = 40f;
        itemDrop.m_itemData.m_shared.m_damages.m_blunt = 140f;
        itemDrop.m_itemData.m_shared.m_damages.m_poison = 80f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_blunt = 5f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_poison = 10f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 20f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackAnimation = "battleaxe_attack";
        itemDrop.m_itemData.m_shared.m_attack.m_attackChainLevels = 3;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackAnimation = "swing_sledge";
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_attackStamina = 35f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactor = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactorRotation = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_forceMultiplier = 10f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_staggerMultiplier = 6f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_lowerDamagePerHit = false;

        var statusEffect = itemDrop.m_itemData.m_shared.m_equipStatusEffect as SE_Poison;

        if (statusEffect == null)
        {
          throw new NullReferenceException(nameof(statusEffect));
        }

        statusEffect.m_damageInterval = 1f;
        statusEffect.m_damagePerHit = 1f;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 4
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.WitheredBone
              , Amount = 20
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Guck
              , Amount = 20
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.BlackMetal
              , Amount = 40
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyBonemass
              , Amount = 6
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

    #endregion

    #region SpearSpirit

    private void AddSpearSpirit()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_SpearSpirit/v801_SpearSpirit.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_spearspirit";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_spearspirit_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.OneHandedWeapon;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_maxDurability = 500f;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.2f;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 1f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 1f;
        itemDrop.m_itemData.m_shared.m_attackForce = 12f;
        itemDrop.m_itemData.m_shared.m_damages.m_pierce = 75f;
        itemDrop.m_itemData.m_shared.m_damages.m_spirit = 75f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_pierce = 5f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_spirit = 5f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 20f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactor = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactorRotation = 0.2f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_damageMultiplier = 0.75f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_forceMultiplier = 10f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_staggerMultiplier = 6f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_lowerDamagePerHit = false;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 4
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.ElderBark
              , Amount = 40
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Crystal
              , Amount = 12
              , AmountPerLevel = 4
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.AncientSeed
              , Amount = 40
              , AmountPerLevel = 15
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyTheElder
              , Amount = 3
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

    #endregion

    #region UnarmedFenring

    private void AddUnarmedFenring()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        var prefab = _assetBundle.LoadAsset<GameObject>("Assets/Custom_Items/v801_UnarmedFenring/v801_UnarmedFenring.prefab");

        if (prefab == null)
        {
          throw new NullReferenceException(nameof(prefab));
        }

        var itemDrop = prefab.GetComponent<ItemDrop>();

        if (itemDrop == null)
        {
          throw new NullReferenceException(nameof(itemDrop));
        }

        itemDrop.m_itemData.m_shared.m_name = "$item_forsaken_unarmedfenring";
        itemDrop.m_itemData.m_shared.m_description = "$item_forsaken_unarmedfenring_description";
        itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.OneHandedWeapon;
        itemDrop.m_itemData.m_shared.m_equipDuration = 0.25f;
        itemDrop.m_itemData.m_shared.m_maxDurability = 800f;
        itemDrop.m_itemData.m_shared.m_maxQuality = 4;
        itemDrop.m_itemData.m_shared.m_maxStackSize = 1;
        itemDrop.m_itemData.m_shared.m_variants = 1;
        itemDrop.m_itemData.m_shared.m_weight = 2f;
        itemDrop.m_itemData.m_shared.m_blockPower = 30f;
        itemDrop.m_itemData.m_shared.m_timedBlockBonus = 2f;
        itemDrop.m_itemData.m_shared.m_deflectionForce = 2f;
        itemDrop.m_itemData.m_shared.m_attackForce = 2f;
        itemDrop.m_itemData.m_shared.m_movementModifier = 0f;
        itemDrop.m_itemData.m_shared.m_damages.m_slash = 60f;
        itemDrop.m_itemData.m_shared.m_damages.m_pierce = 60f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_slash = 5f;
        itemDrop.m_itemData.m_shared.m_damagesPerLevel.m_pierce = 5f;
        itemDrop.m_itemData.m_shared.m_attack.m_attackStamina = 15f;
        itemDrop.m_itemData.m_shared.m_attack.m_speedFactor = 0.1f;
        itemDrop.m_itemData.m_shared.m_attack.m_speedFactorRotation = 0.1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactor = 0.1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_speedFactorRotation = 0.1f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_damageMultiplier = 1.15f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_forceMultiplier = 10f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_staggerMultiplier = 6f;
        itemDrop.m_itemData.m_shared.m_secondaryAttack.m_lowerDamagePerHit = false;

        ItemManager.Instance.AddItem(new CustomItem(prefab, false, new ItemConfig
        {
          CraftingStation = Common.Names.Vanilla.CraftingStationNames.Forge
          , MinStationLevel = 5
          , Requirements = new[]
          {
            new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.FreezeGland
              , Amount = 40
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Silver
              , Amount = 60
              , AmountPerLevel = 10
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.Chain
              , Amount = 4
              , AmountPerLevel = 2
            }
            , new RequirementConfig
            {
              Item = Common.Names.Vanilla.ItemDropNames.TrophyFenring
              , Amount = 10
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

    #endregion

    #region Implementation of ITraceableLogging

    /// <inheritdoc />
    public string Source => Namespace;

    /// <inheritdoc />
    public bool EnableTrace { get; }

    #endregion
  }
}
