using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace CrosshairDotRemover;

[HarmonyPatch(typeof(Ironsights), nameof(Ironsights.Update))]
public static class IronsightsUpdatePatch
{
    public static void Postfix(Ironsights __instance)
    {
        if (!Global.code)
            return;
        if (!__instance.WeaponBehaviorComponent)
            return;
        if (!(Time.timeScale > 0.0) || !(Time.deltaTime > 0.0) || !(Time.smoothDeltaTime > 0.0)) return;
        GameObject crosshairDot = FindCrosshairDot();

        Image? imageComp = crosshairDot.GetComponent<Image>();
        if (ShouldHideDot(__instance))
        {
            imageComp.enabled = false;
        }
        else if (!imageComp.enabled)
        {
            imageComp.enabled = true;
        }
    }

    private static GameObject FindCrosshairDot()
    {
        return Global.code.uiCombat.crosshairGroup.transform.parent.Find("dot_1").gameObject;
    }

    private static bool ShouldHideDot(Ironsights instance)
    {
        return IsPlayerZoomedOrCanBackstab(instance)
               && IsPlayerAlive()
               && IsWeaponReady(instance)
               && IsNotReloading(instance)
               && HasCurrentWeapon(instance)
               && IsValidWeaponState(instance);
    }

    private static bool IsPlayerZoomedOrCanBackstab(Ironsights instance)
    {
        return instance.FPSPlayerComponent.zoomed || instance.FPSPlayerComponent.canBackstab;
    }

    private static bool IsPlayerAlive()
    {
        return Global.code.Player.Health > 0.0;
    }

    private static bool IsWeaponReady(Ironsights instance)
    {
        return Time.time > instance.PlayerWeaponsComponent.switchTime + instance.WeaponBehaviorComponent.readyTimeAmt;
    }

    private static bool IsNotReloading(Ironsights instance)
    {
        return !instance.reloading;
    }

    private static bool HasCurrentWeapon(Ironsights instance)
    {
        return instance.PlayerWeaponsComponent.currentWeapon != 0;
    }

    private static bool IsValidWeaponState(Ironsights instance)
    {
        return instance.FPSPlayerComponent.canBackstab
               || instance.WeaponBehaviorComponent.zoomIsBlock && IsWeaponFireRateValid(instance)
               || !instance.WeaponBehaviorComponent.zoomIsBlock;
    }

    private static bool IsWeaponFireRateValid(Ironsights instance)
    {
        return Time.time > instance.WeaponBehaviorComponent.shootStartTime + instance.WeaponBehaviorComponent.fireRate
               && (!instance.WeaponBehaviorComponent.shootFromBlock || instance.WeaponBehaviorComponent.shootFromBlock);
    }
}