namespace IgnitedBox.Entities
{
    public interface ITargetEntity<IProjectileType>
    {
        bool Trigger(IProjectileType projectile);
    }
}
