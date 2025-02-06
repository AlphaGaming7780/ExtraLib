using Game.Prefabs;

namespace ExtraLib.ClassExtension;

public static class UIObjectExtension
{
	public static UIObjectData ToComponentData(this UIObject UIObject)
	{
        return new UIObjectData
        {
            m_Group = EL.m_PrefabSystem.GetEntity(UIObject.m_Group),
            m_Priority = UIObject.m_Priority
        };
	}
}