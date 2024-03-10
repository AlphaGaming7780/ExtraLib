using Game.Prefabs;

namespace Extra.Lib;

public static class UIObjectExtension
{
	public static UIObjectData ToComponentData(this UIObject UIObject)
	{
        return new UIObjectData
        {
            m_Group = ExtraLib.m_PrefabSystem.GetEntity(UIObject.m_Group),
            m_Priority = UIObject.m_Priority
        };
	}
}