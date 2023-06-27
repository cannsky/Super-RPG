using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Queue<int> attackQueue = new Queue<int>();
    public bool isAttackAnimationPlaying = false;
    
    public float animationSpeed = 1f;
    
    private Transform currentSwordEffect;
    private int randomNumber, lastRandomNumber;
    private Player player;
    public float[,] sparkleArray = {
        {0.65f, 1f, 0.005f},
        {0.65f, 1f, 0.005f},
        {0.8f, 1.2f, 0.4f}
    };
    
    void Start(){
        this.player = gameObject.GetComponent<Player>();
        foreach(var swordEffect in player.swordEffects){
            swordEffect.GetComponent<ParticleSystem>().Stop();
            var emission = swordEffect.GetComponent<ParticleSystem>().emission;
            emission.enabled = false;
        }
    }
    
    public void Attack()
    {
        //Create a random number between 1 and 4: 1, 2, 3.
        randomNumber = Random.Range(1, 10);
        randomNumber = (randomNumber < 3) ? 3 : Random.Range(1, 3);
        //If the last random number and the generated random number are same, change the random number
        randomNumber = (lastRandomNumber != randomNumber) ? randomNumber : ((randomNumber < 2) ? randomNumber + 1 : randomNumber - 1);
        //Update the animation according to the random number
        attackQueue.Enqueue(randomNumber);
        //Update last random number
        lastRandomNumber = randomNumber;
    }
    
    public void AttackAnimation(){
        int number = attackQueue.Dequeue();
        isAttackAnimationPlaying = true;
        player.animator.SetBool("Run", false);
        currentSwordEffect = Instantiate(player.swordEffects[number - 1],player.swordEffects[number - 1].position, player.swordEffects[number - 1].rotation);
        currentSwordEffect.localScale = new Vector3(2f, 2f, 2f);
        currentSwordEffect.GetComponent<ParticleSystem>().Stop();
        var emission = currentSwordEffect.GetComponent<ParticleSystem>().emission;
        emission.enabled = false;
        StartCoroutine(startSparkles(number));
    }
    
    IEnumerator startSparkles(int number){
        if(!player.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) yield return new WaitForSeconds(0.25f);
        player.animator.SetBool("Melee Right Attack 0" + number, true);
        yield return new WaitForSeconds(sparkleArray[number - 1, 0] / animationSpeed);
        currentSwordEffect.GetComponent<ParticleSystem>().Play();
        var emission = currentSwordEffect.GetComponent<ParticleSystem>().emission;
        emission.enabled = true;
        StartCoroutine(stopSparkles(number));
    }
    
    IEnumerator stopSparkles(int number){
        yield return new WaitForSeconds(sparkleArray[number - 1, 1] / animationSpeed);
        currentSwordEffect.GetComponent<ParticleSystem>().Stop();
        var emission = currentSwordEffect.GetComponent<ParticleSystem>().emission;
        emission.enabled = false;
        yield return new WaitForSeconds(sparkleArray[number - 1, 2] / animationSpeed);
        isAttackAnimationPlaying = false;
        Destroy(currentSwordEffect.gameObject, 0.8f);
    }
    
}
