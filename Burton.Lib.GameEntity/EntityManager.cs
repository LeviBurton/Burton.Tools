using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.GameEntity
{
    public class EntityManager
    {
        public static EntityManager mInstance;

        public static EntityManager Instance
        {
            get
            {
                if (EntityManager.mInstance == null)
                {
                    EntityManager.mInstance = new EntityManager();
                }

                return EntityManager.mInstance;
            }
            set
            {
                mInstance = value;
            }


        }

        public SortedDictionary<int, BaseGameEntity> EntityMap = new SortedDictionary<int, BaseGameEntity>();

        public void RegisterEntity(BaseGameEntity NewEntity)
        {
            EntityMap.Add(NewEntity.ID, NewEntity);
        }

        public BaseGameEntity GetEntityFromID(int ID)
        {
            var Entity = EntityMap[ID];

            return Entity;
        }

        public void RemoveEntity(BaseGameEntity EntityToRemove)
        {
            var Entity = EntityMap[EntityToRemove.ID];

            if (Entity != null)
            {
                EntityMap.Remove(Entity.ID);
            }
        }

        public void Reset()
        {
            EntityMap.Clear();
        }
    }
}
