using Game.Prefabs;
using Unity.Entities;

namespace Extra.Lib;

public static class UIGroupPrefabExtension
{
    public static void RemoveElement(this UIGroupPrefab uIGroupPrefab, Entity entity) 
    {
        Entity entity2 = ExtraLib.m_PrefabSystem.GetEntity(uIGroupPrefab);
        var uiGroupBuffer = ExtraLib.m_EntityManager.GetBuffer<UIGroupElement>(entity2);//.Add(new UIGroupElement(entity));
        var unlockRequirementBuffer = ExtraLib.m_EntityManager.GetBuffer<UnlockRequirement>(entity2);//.Add(new UnlockRequirement(entity, UnlockFlags.RequireAny));

        for(int i = 0; i < uiGroupBuffer.Length; i++) {
            if(uiGroupBuffer.ElementAt(i).m_Prefab == entity) {
                uiGroupBuffer.RemoveAt(i);
                break;
            }
        }

        for(int i = 0; i < unlockRequirementBuffer.Length; i++) {
            if(unlockRequirementBuffer.ElementAt(i).m_Prefab == entity) {
                unlockRequirementBuffer.RemoveAt(i);
                break;
            }
        }
    } 
}