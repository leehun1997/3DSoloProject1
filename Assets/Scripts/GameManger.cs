using UnityEngine;

public class GameManger : MonoBehaviour
{
    private static GameManger _instance;//Player, Instance ��� ���ٸ� ������ ���� ��ġ, player,_instance�� ���������� ���ϰ� ���� ����Ǵ� ��

    public static GameManger Instance//�̱����� GameManger�� static���� �ۼ�
    {
        get
        {
            if (_instance == null)//��� �ڵ�
            {
                _instance = new GameObject("GameManger").AddComponent<GameManger>();
            }
            return _instance;
        }
    }

    private Player _player;

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance == this)//�� �빮�ڰ� �ƴѰ�???
            {
                Destroy(gameObject);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
