using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class zikken : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    private Transform _player;
  public Transform[] goals;
  private int destNum = 0;
  public NavMeshAgent agent;
    void Start()
    {
        agent.SetDestination(goals[destNum].position);
        // NavMeshAgentの移動速度を設定
        _navMeshAgent.speed = 60f;          // デフォルトは3.5。大きい値に設定して移動速度を上げる
        _navMeshAgent.acceleration = 100f; // 加速を設定。デフォルトは8。値を増やすことで素早く速度に到達
        _navMeshAgent.angularSpeed = 3600f;
    }     // 回転速度。デフォルトは120。高い値で素早く方向転換
    void Update()
    {
        

        // もし、目的地までの距離が 0.5 より下ならば、nextGoal を実行する
        if(agent.remainingDistance < 0.04f){
            nextGoal();
        }
        
        // プレイヤーの位置に向かって移動
        //_navMeshAgent.SetDestination(_player.position);
    }

    void nextGoal(){
        destNum += 1;
        if(destNum == 3){
            destNum = 0;
        }
        Debug.Log(destNum);
        // 目的地を destNum 番目の位置にする
        agent.destination = goals[destNum].position;

        //Debug.Log(destNum); 
    }
    
}
