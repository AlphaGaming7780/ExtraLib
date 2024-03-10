using Extra.Lib.Debugger;
using Game.Prefabs;
using Unity.Entities;

namespace Extra.Lib;

public static class EntityManagerExtension
{
	public static void AddOrSetComponentData<T>(this EntityManager entityManager, Entity entity, T componentData) where T : unmanaged, IComponentData 
	{
        if(entityManager.HasComponent<T>(entity)) entityManager.SetComponentData(entity, componentData);
        else entityManager.AddComponentData(entity, componentData);
	}
}