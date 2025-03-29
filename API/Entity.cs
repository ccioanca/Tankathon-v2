using Godot;
using Tankathon.API;

namespace Tankathon.API
{
    public class Entity
    {
        public EntityType eType { get; }
        public Vector2 globalPosition { get; }
        public float rotation { get; }
        public float distanceTo { get; }

        public Entity()
        {
            eType = EntityType.Unknown;
            globalPosition = new Vector2();
            rotation = 0f;
            distanceTo = -1f;
        }

        public Entity(EntityType eType, Vector2 globalPosition, float rotation, float distanceTo)
        {
            this.eType = eType;
            this.globalPosition = globalPosition;
            this.rotation = rotation;
            this.distanceTo = distanceTo;
        }

    }
}
