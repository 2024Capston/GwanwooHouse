public interface ILoader
{
    /// <summary>
    /// 게임에 필요한 모든 객체를 Load한다.
    /// </summary>
    public void Load();

    /// <summary>
    /// 게임에 사용한 모든 객체를 Destory한다.
    /// </summary>
    public void Destroy();
}