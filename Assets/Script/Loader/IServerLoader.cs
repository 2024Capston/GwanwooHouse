public interface IServerLoader
{
    /// <summary>
    /// 네트워크 객체를 Spawn한다.
    /// </summary>
    public void Spawn();

    /// <summary>
    /// 네트워크 객체를 DeSpawn한다.
    /// </summary>
    public void Despawn();
}