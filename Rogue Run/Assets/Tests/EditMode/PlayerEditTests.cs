using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerEditTests
{
    [Test]
    public void SetDarkness()
    {
        var gameObject = new GameObject();
        var controller = gameObject.AddComponent<PlayerController>();
        for (int i = 0; i < 100; i++)
        {
            int value = Random.Range(int.MinValue, int.MaxValue);
            controller.DarknessCount = value;
            Assert.AreEqual(value, controller.DarknessCount);
        }
        
    }
    
    
    // A Test behaves as an ordinary method
    [Test]
    public void AddDarknessManual()
    {
        var gameObject = new GameObject();
        var controller = gameObject.AddComponent<PlayerController>();
        int start = controller.DarknessCount;
        controller.AddDarkness(50);
        Assert.AreEqual(start + 50, controller.DarknessCount);

    }
    
    // A Test behaves as an ordinary method
    [Test]
    public void AddDarknessAuto()
    {
        var gameObject = new GameObject();
        var controller = gameObject.AddComponent<PlayerController>();
        for (int i = 0; i < 100; i++)
        {
            int value = Random.Range(int.MinValue, int.MaxValue);
            controller.DarknessCount = 0;
            controller.AddDarkness(value);
            Assert.AreEqual(value, controller.DarknessCount);
        }
        
    }
    
    // A Test behaves as an ordinary method
    [Test]
    public void MoveSpeed()
    {
        var gameObject = new GameObject();
        var controller = gameObject.AddComponent<PlayerController>();
        Assert.Greater(0, controller.movementSpeed);
        controller.CanMove = false;
        Assert.AreEqual(0, controller.movementSpeed);
    }
    
    // A Test behaves as an ordinary method
    [Test]
    public void InitiateHealth()
    {
        var gameObject = new GameObject();
        var damageable = gameObject.AddComponent<Damageable>();
        Assert.AreEqual(100, damageable.Health);
    }

}
