using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CreateJsonTest
{
    private ButtonsManager _buttonsManager;

    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject = new GameObject();
            
        _buttonsManager = gameGameObject.AddComponent<ButtonsManager>();
    }


    [TearDown]
    public void Teardown()
    {
        Object.Destroy(_buttonsManager.gameObject);
    }




    [UnityTest]
    public IEnumerator SetAndGetDataButtonsInJson()
    {
        _buttonsManager.SetData(11, 210, true, false, 3);

        yield return new WaitForSeconds(0.1f);

        ButtonData buttonData = _buttonsManager.GetData(11);     


        Assert.AreEqual(11, buttonData.id);
        Assert.AreEqual(210, buttonData.score);
        Assert.AreEqual(true, buttonData.isActive);
        Assert.AreEqual(false, buttonData.isPassed);
        Assert.AreEqual(3, buttonData.activeStarsCount);
    }
}
