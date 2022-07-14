using System;
using PulsarModLoader;
using HarmonyLib;
using UnityEngine;
namespace Universal_Teleport
{
    public class Mod : PulsarMod
    {
        public override string Version => "1.0";

        public override string Author => "pokegustavo";

        public override string ShortDescription => "Makes all teleport CPU universal";

        public override string Name => "Universal Teleport";

        public override string HarmonyIdentifier()
        {
            return "pokegustavo.universaltp";
        }
    }

    [HarmonyPatch(typeof(PLTeleportationScreen), "Update")]
    class Patch 
    {
        static void Postfix(PLTeleportationScreen __instance, ref UITexture[] ___ClassTargets) 
        {
            if (PLEncounterManager.Instance.PlayerShip == null) return;
            bool Hasteleport = false;
            foreach (PLShipComponent plshipComponent in PLEncounterManager.Instance.PlayerShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_CPU, false))
            {
                if (plshipComponent != null && (plshipComponent.SubType == (int)ECPUClass.E_CLASS_TELEPORT_CAP || plshipComponent.SubType == (int)ECPUClass.E_CLASS_TELEPORT_PIL || plshipComponent.SubType == (int)ECPUClass.E_CLASS_TELEPORT_WPN || plshipComponent.SubType == (int)ECPUClass.E_CLASS_TELEPORT_SCI || plshipComponent.SubType == (int)ECPUClass.E_CLASS_TELEPORT_ENG))
                {
                    Hasteleport = true;
                    break;
                }
            }
            if (Hasteleport) 
            {
                for(int i = 0; i < ___ClassTargets.Length; i++) 
                {
                    PLPawn plpawn = null;
                    PLPlayer cachedFriendlyPlayerOfClass = PLServer.Instance.GetCachedFriendlyPlayerOfClass(i);
                    if (cachedFriendlyPlayerOfClass != null)
                    {
                        plpawn = cachedFriendlyPlayerOfClass.GetPawn();
                    }
                    if (plpawn != null && plpawn.SpawnedInArena && plpawn.IsDead)
                    {
                        plpawn = null;
                    }
                    if(plpawn != null) 
                    {
                        ___ClassTargets[i].alpha = 0.8f;
                    }
                    else 
                    {
                        ___ClassTargets[i].alpha = 0.1f;
                    }
                }
            }
        }
    }
}
