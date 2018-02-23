using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TestTools;
using NSubstitute;
using UnityEditor.Animations;
using UnityEngine.UI;


namespace FlappyBird.PlayModeTest
{
    public class BirdTest
    {
        private Bird bird;
        

        [SetUp]
        public void BeforeEveryTest()
        {
            var instance = new GameObject("Bird");

            instance.AddComponent<Rigidbody2D>();
            instance.AddComponent<Animator>();
            

            bird = instance.AddComponent<Bird>();

            bird.upForce = 500f;
        }

        [UnityTest]
        public IEnumerator _Bird_Jump_to_Play_Flap_Animation()
        {
            var animatorController = Resources.Load<AnimatorController>("Bird");
            var animator = bird.GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            
            bird.Jump();

            yield return null;
            
  
            Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        }
        


        [Test]
        public void _Bird_Collide_to_Bird_Died()
        {

            bird.SendMessage("OnCollisionEnter2D", new Collision2D());

            Assert.AreEqual(true, bird.IsDead);

        }

        [UnityTest]
        public IEnumerator _Down_Fire1_Button_to_Bird_Jump_Up()
        {
            float posY = bird.transform.position.y;

            var unityInput = Substitute.For<IUnityInputService>();
            unityInput.GetButtonDown("Fire1").Returns(true);

            bird.UnityInput = unityInput;

            yield return null;

            yield return new WaitForFixedUpdate();

            Assert.Greater(bird.transform.position.y,posY);
        }


        [UnityTest]
        public IEnumerator _Down_Fire1_Button_to_Bird_Speed_Not_Negative()
        {
            var unityInput = Substitute.For<IUnityInputService>();
            unityInput.GetButtonDown("Fire1").Returns(true);

            bird.UnityInput = unityInput;

            yield return null;

            yield return new WaitForFixedUpdate();

            Assert.GreaterOrEqual(bird.GetComponent<Rigidbody2D>().velocity.y,0);        
        }
        

        [UnityTest]
        public IEnumerator _Down_Fire1_After_2sec_to_Bird_Y_Speed_Not_Negative()
        {


            yield return new WaitForSeconds(2.0f);
            var unityInput = Substitute.For<IUnityInputService>();
            unityInput.GetButtonDown("Fire1").Returns(true);
            bird.UnityInput = unityInput;
            
            yield return null;

            yield return new WaitForFixedUpdate();


            Assert.GreaterOrEqual(bird.GetComponent<Rigidbody2D>().velocity.y,0);
        }

        [UnityTest]
        public IEnumerator _Bird_Trigger_Collide_With_Column_Tag_to_Add_Score()
        {
            var gameContorl = new GameObject().AddComponent<GameControl>();

            yield return null;

            var score = gameContorl.score;

            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.tag = "Column";
            
            bird.SendMessage("OnTriggerEnter2D",collider);
            
            yield return null;
            
            Assert.Greater(gameContorl.score,score);
        }

        [UnityTest]
        public IEnumerator _Bird_Trigger_Collide_With_Not_Column_Tag_to_Not_Change_Score()
        {
            var gameContorl = new GameObject().AddComponent<GameControl>();

            yield return null;

            var score = gameContorl.score;

            bird.SendMessage("OnTriggerEnter2D",new Collider2D());
            
            yield return null;
            
            Assert.AreEqual(gameContorl.score,score);
        }
        
    }
    
    public class GameControlTest
    {
        private GameControl gameControl;
        
        [SetUp]
        public void BeforeEveryTest()
        {
            if (GameControl.instance != null)
            {
                GameObject.Destroy(GameControl.instance.gameObject);
            }
            
            gameControl = new GameObject("GameControl").AddComponent<GameControl>();
            
            gameControl.scoreText = new GameObject().AddComponent<Text>();
            gameControl.gameOvertext = new GameObject();

        }

        [Test]
        public void _Bird_Scored_then_Displayd_Text_Contain_Updated_Score()
        {
            gameControl.BirdScored();

            Assert.IsTrue(gameControl.scoreText.text.Contains(gameControl.score.ToString()));
        }

        [UnityTest]
        public IEnumerator _Set_Control_ScroollSpeed_0_then_Scrolling_Object_Move_0_Horizontal_Distance()
        {
            var scrollingObject = new GameObject().AddComponent<ScrollingObject>();
            
            gameControl.scrollSpeed = 0f;

            yield return null;
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0f, scrollingObject.GetComponent<Rigidbody2D>().velocity.x);
        }
        
        [UnityTest]
        public IEnumerator _Set_GameOver_True_then_Scrolling_Object_Move_0_Distance()
        {
            var scrollingObject = new GameObject().AddComponent<ScrollingObject>();

            gameControl.gameOver = true;

            yield return null;
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(Vector2.zero, scrollingObject.GetComponent<Rigidbody2D>().velocity);
        }
        

    }


}