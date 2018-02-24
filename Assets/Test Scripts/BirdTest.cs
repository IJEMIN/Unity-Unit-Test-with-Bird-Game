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
            
            GameControl.Instance.Reset();
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
        public IEnumerator _Bird_Jump_to_Bird_Speed_Not_Negative()
        {
            bird.Jump();

            yield return null;

            yield return new WaitForFixedUpdate();

            Assert.GreaterOrEqual(bird.GetComponent<Rigidbody2D>().velocity.y,0);        
        }
        

        [UnityTest]
        public IEnumerator _Jump_After_2sec_to_Bird_Y_Speed_Not_Negative()
        {


            yield return new WaitForSeconds(2.0f);
            
            bird.Jump();
            
            yield return null;

            yield return new WaitForFixedUpdate();


            Assert.GreaterOrEqual(bird.GetComponent<Rigidbody2D>().velocity.y,0);
        }

        [UnityTest]
        public IEnumerator _Bird_Trigger_Collide_With_Column_Tag_to_Add_Score()
        {

            var score = GameControl.Instance.score;

            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.tag = "Column";
            
            bird.SendMessage("OnTriggerEnter2D",collider);
            
            yield return null;
            
            Assert.AreEqual(score + 1,GameControl.Instance.score);
        }

        [UnityTest]
        public IEnumerator _Bird_Trigger_Collide_With_Not_Column_Tag_to_Not_Change_Score()
        {
            var score = GameControl.Instance.score;
            
            var collider = new GameObject().AddComponent<BoxCollider2D>();

            yield return null;

            bird.SendMessage("OnTriggerEnter2D",collider);
            
            yield return null;
            
            Assert.AreEqual(GameControl.Instance.score,score);
        }
        
    }
    
    public class GameControlTest
    {

        [SetUp]
        public void BeforeEveryTest()
        {
            GameControl.Instance.scoreText = new GameObject("Score Text").AddComponent<Text>();
            GameControl.Instance.gameOvertext = new GameObject("Game Over Text");
            GameControl.Instance.Reset();

        }
        
        
        [Test]
        public void _Bird_Scored_then_Displayd_Text_Contain_Updated_Score()
        {
            
            GameControl.Instance.BirdScored();

            Assert.IsTrue(GameControl.Instance.scoreText.text.Contains(GameControl.Instance.score.ToString()));
        }

        [UnityTest]
        public IEnumerator _Set_Control_ScroollSpeed_0_then_Scrolling_Object_Move_0_Horizontal_Distance()
        {
            var scrollingObject = new GameObject().AddComponent<ScrollingObject>();
            
            GameControl.Instance.scrollSpeed = 0f;

            yield return null;
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0f, scrollingObject.GetComponent<Rigidbody2D>().velocity.x);
        }
        
        [UnityTest]
        public IEnumerator _Set_GameOver_True_then_Scrolling_Object_Move_0_Distance()
        {
            var scrollingObject = new GameObject().AddComponent<ScrollingObject>();

            GameControl.Instance.gameOver = true;

            yield return null;
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(Vector2.zero, scrollingObject.GetComponent<Rigidbody2D>().velocity);
        }
    }


}