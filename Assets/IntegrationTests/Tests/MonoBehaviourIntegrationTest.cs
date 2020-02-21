using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScript
    {
        #region Examples
        //Example
        // A Test behaves as an ordinary method
        [Test]
        public void NewTestScriptSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        //Example
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        //Example
        [UnityTest]
        public IEnumerator MonoBehaviourTest_Works()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();
        }

        //Example 
        public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
        {
            private int frameCount;
            public bool IsTestFinished
            {
                get { return frameCount > 10; }
            }

            void Update()
            {
                frameCount++;
            }
        }
        //Example
        #endregion

        [UnityTest]
        public IEnumerator MonobehaviourTestWithSceneLoading()
        {
            SceneManager.LoadScene(0);
            yield return null;
            yield return new MonoBehaviourTest<MonobehaviourToTestWithSceneLoading>();
        }


        public class MonobehaviourToTestWithSceneLoading : MonoBehaviour, IMonoBehaviourTest
        {
            public bool finished;
            float timeToWait = 1f;
            float currentTime = 0;
            public bool IsTestFinished => finished;

            void Update()
            {
                if (currentTime >= timeToWait)
                {
                    GameObject[] gameObjects = SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects();
                    for(var i = 0; i < gameObjects.Length; i++)
                        if(gameObjects[i].name == "CubeToFind")
                        {
                            finished = true;
                            float speed = gameObjects[i].GetComponent<ScriptToTest>().speed;
                            Assert.IsTrue( gameObjects[i].transform.position.z >= 1f * currentTime * speed);
                        }

                }

                currentTime += Time.deltaTime;
            }
        }
    }
}
