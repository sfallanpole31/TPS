using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Singleton單例模式
可以確保生成對象只有一個實例存在
開發遊戲會希望某個類別只有一個實例化物件可以使用
*/
public class GameMangerSingleton
{
    private GameObject gameObject;

    //單例
    private static GameMangerSingleton m_Instance;
    //接口
    public static GameMangerSingleton Instance
    {
        get {
            if (m_Instance == null)
            {
                m_Instance = new GameMangerSingleton();
                m_Instance.gameObject = new GameObject("GameManager");
                m_Instance.gameObject.AddComponent<InputController>();
            }
            return m_Instance;
        }
    }


    //登記InputController 一個遊戲只會有一個
    //單例
    private InputController m_inputController;
    //接口
    public InputController InputController
    {
        get
        {
            if (m_inputController == null)
            {
                m_inputController = gameObject.GetComponent<InputController>();
            }
            return m_inputController;
        }
    }
}
