using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchDisplay : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponent<Text>().text = TransactionManager.Instance.m_TouchText;
    }
}
