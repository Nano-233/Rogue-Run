using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Damageable _damageable; //player hp component
    public Image HealthBar; //green part of hp bar
    
    public static UIController instance;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = _damageable.Health / 100f;
    }
}
