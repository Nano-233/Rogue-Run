using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest
{
   
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerHealthComponentManual()
    {
        var gameObject = new GameObject();
        var damageable = gameObject.AddComponent<Damageable>();
        gameObject.AddComponent<Animator>();
        Assert.AreEqual(100, damageable.Health);
        damageable.Hit(20, Vector2.zero);
        // Use yield to skip a frame.
        yield return new WaitForSeconds(2);
        Assert.AreEqual(80, damageable.Health);
        // Use the Assert class to test conditions.
    }
    
    [UnityTest]
    public IEnumerator PlayerHealthComponentAuto()
    {
        var gameObject = new GameObject();
        var damageable = gameObject.AddComponent<Damageable>();
        gameObject.AddComponent<Animator>();
        Assert.AreEqual(100, damageable.Health);
        for (int i = 0; i < 100; i++)
        {
            int value = Random.Range(0, 100);
            damageable.Hit(value, Vector2.zero);
            yield return null;
            Assert.AreEqual(100 - value, damageable.Health);
        }
        // Use the Assert class to test conditions.
    }
}
