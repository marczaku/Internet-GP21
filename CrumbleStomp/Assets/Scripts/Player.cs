using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class GameState
{
    private Vector3[] cratePositions;
    private GameObject[] activeStones;
    private GameObject[] inactiveStones;
    private Vector3 playerPositon;
}

class MoveCommand : ICommand
{
    private readonly GameObject target;
    private readonly Vector3 _direction;

    public MoveCommand(GameObject target, Vector3 direction)
    {
        this.target = target;
        _direction = direction;
    }
    public void Execute()
    {
        this.oldState = StateCreator.CreateState():
        target.transform.position += _direction;
    }

    public void Undo()
    {
        this.oldState.ApplyToGame();
    }
}

class DisableGameObjectsCommand : ICommand
{
    private readonly GameObject[] targets;
    private readonly bool[] oldStates;

    public DisableGameObjectsCommand(GameObject[] targets)
    {
        this.targets = targets;
        this.oldStates = new bool[targets.Length];
    }
    public void Execute()
    {
        for (var i = 0; i < targets.Length; i++)
        {
            var target = targets[i];
            this.oldStates[i] = target.activeSelf;
            target.SetActive(false);
        }
    }

    public void Undo()
    {
        for (var i = 0; i < targets.Length; i++)
        {
            var target = targets[i];
            target.SetActive(oldStates[i]);
        }
    }
}

public class Player : MonoBehaviour
{
    private Executor executor;

    void Awake()
    {
        this.executor = FindObjectOfType<Executor>();
    }
    
    void Update()
    {
        CheckDirection(KeyCode.A, Vector3.left);
        CheckDirection(KeyCode.D, Vector3.right);
        CheckDirection(KeyCode.W, Vector3.up);
        CheckDirection(KeyCode.S, Vector3.down);
        if (Input.GetKeyDown(KeyCode.F))
        {
            var colliders = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Stone"));
            executor.Enqueue(new DisableGameObjectsCommand(colliders.Select(collider => collider.gameObject).ToArray()));
        }
    }

    void CheckDirection(KeyCode keyCode, Vector3 direction)
    {
        if (Input.GetKeyDown(keyCode))
            executor.Enqueue(new MoveCommand(gameObject, direction));
    }
}
