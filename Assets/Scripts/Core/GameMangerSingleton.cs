using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Singleton��ҼҦ�
�i�H�T�O�ͦ���H�u���@�ӹ�Ҧs�b
�}�o�C���|�Ʊ�Y�����O�u���@�ӹ�Ҥƪ���i�H�ϥ�
*/
public class GameMangerSingleton
{
    private GameObject gameObject;

    //���
    private static GameMangerSingleton m_Instance;
    //���f
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


    //�n�OInputController �@�ӹC���u�|���@��
    //���
    private InputController m_inputController;
    //���f
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
