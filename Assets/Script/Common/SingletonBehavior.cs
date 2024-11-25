using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Singleton ��ü�� ����� ���� Class
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    /// <summary>
    /// Scene�� �ٲ� �� ��ü �ı����θ� �����Ѵ�.
    /// </summary>
    protected bool _isDestroyOnLoad = false;

    protected static T _instance;

    public static T Instance
    {
        get => _instance;
    }

    private void Awake()
    {
        Init();
    }

    protected virtual void OnDestory()
    {
        Dispose();
    }

    /// <summary>
    /// ��ü�� ó�� �ʱ�ȭ�ϴ� �޼ҵ�
    /// </summary>
    protected virtual void Init()
    {
        if (_instance == null)
        {
            _instance = (T)this;

            if (!_isDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ��ü�� �ı��� �� ȣ��Ǵ� �޼ҵ�
    /// </summary>
    protected virtual void Dispose()
    {
        _instance = null;
    }
}