using UnityEngine;

public class GameManger : MonoBehaviour
{
    private static GameManger _instance;//Player, Instance 모두 접근만 가능한 저장 장치, player,_instance가 실질적으로 변하고 값이 저장되는 값

    public static GameManger Instance//싱글톤인 GameManger는 static으로 작성
    {
        get
        {
            if (_instance == null)//방어 코드
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
            if (_instance == this)//왜 대문자가 아닌가???
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
