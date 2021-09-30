using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using System;
using System.Reflection;
using UnityEngine;

namespace Digitalroot.Valheim.ForsakenJVL
{
  // ReSharper disable once InconsistentNaming
  public class CMB_BurnWhenHeld : MonoBehaviour
  {
    private readonly System.Timers.Timer _timer;

    public CMB_BurnWhenHeld()
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        _timer = new System.Timers.Timer
        {
          AutoReset = true
          , Enabled = false
          , Interval = 10000
        };
        _timer.Elapsed += TimerElapsed;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        if (!Common.Utils.IsPlayerReady()) return;
        if (!Player.m_localPlayer.IsItemEquiped(gameObject.GetComponent<ItemDrop>().m_itemData)) return;
        Player.m_localPlayer.AddFireDamage(10);
      }
      catch (Exception ex)
      {
        Log.Error(Main.Instance, ex);
      }
    }

    [UsedImplicitly]
    public void Start()
    {
      Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      _timer.Elapsed += TimerElapsed;
      _timer.Start();
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
      Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      _timer.Elapsed -= TimerElapsed;
      _timer.Stop();
      _timer.Dispose();
    }
  }
}
