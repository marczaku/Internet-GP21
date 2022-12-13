using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.UI;

// what exists in the game? what's the state? save game?
namespace Model
{
    public class Player
    {
        public event Action<int> HealthChanged;
        private int _health;
        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                HealthChanged?.Invoke(value);
            } 
        }
        public int MaxHealth;
        public int Gold;
    }

    public class Item
    {
        public int Level;
    }
}

// what interactions exist, what influence do they have on the model?
namespace Controller
{
    public class PlayerController
    {
        private readonly Model.Player _player;

        public PlayerController(Model.Player player)
        {
            _player = player;
        }

        public void TakeDamage(int damage)
        {
            _player.Health -= Math.Max(damage, 0);
        }
    }

    public class UpgradeController
    {
        private readonly Model.Item item;
        private readonly Model.Player player;

        public UpgradeController(Item item, Model.Player player)
        {
            this.item = item;
            this.player = player;
        }

        public void UpgradeItem()
        {
            this.player.Gold -= 100;
            item.Level++;
        }
    }
}

public class Cell
{
    public event Action<char> StateChanged;
    public int x, y;
    public char state;
}

public class Grid
{
    public event Action<Cell> CellAdded;
}

public class GridView : MonoBehaviour
{
    public Grid grid;
    public CellView cellPrefab;

    void Awake()
    {
        grid.CellAdded += GridOnCellAdded;
    }

    private void GridOnCellAdded(Cell obj)
    {
        var cellView = Instantiate(cellPrefab);
    }
}

public class CellView : MonoBehaviour
{
    public void SetUp(Cell cell)
    {
        transform.position = new Vector3(cell.x, cell.y);
        cell.StateChanged += CellOnStateChanged;
    }

    private void CellOnStateChanged(char obj)
    {
        // set icon or text or whatever
    }
}

public class PlayerController : MonoBehaviour
{
    public Text HealthText;
    public int Health;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Health += 10;
            HealthText.text = Health.ToString();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Health += 20;
            // whoops, forgot to call the other method
        }
    }
}

// Entitas - ECS
// View-MonoBehaviours // if(DeadChanged && newValue == true) => PlayDeathAnimation();
// Controller-Systems-Features // DeathFeature if(Health == 0) => Dead = true;
// Models-Components // Health-Component (int) Dead-Component (bool)

// how do we want to visualize it?
namespace Views
{
    public class HealthView : MonoBehaviour
    {
        public Model.Player player;
        public Text text;

        void Start()
        {
            player.HealthChanged += PlayerOnHealthChanged;
        }

        private void PlayerOnHealthChanged(int newHealth)
        {
            text.text = newHealth.ToString();
        }
    }
    public class HealthBarView : MonoBehaviour
    {
        public Model.Player player;
        public Controller.PlayerController playerController;
        public Image healthBar;

        void Start()
        {
            player.HealthChanged += PlayerOnHealthChanged;
        }

        private void PlayerOnHealthChanged(int newHealth)
        {
            healthBar.fillAmount = newHealth / 100f;
        }

        public void BuyHealthRefill()
        {
            playerController.TakeDamage(-100);
        }
    }
}


// Input
// Action [Drag Survivor From A to B] Validation (is it possible? does the survivor not have cooldown? is he alive?)
// Command [Remove SurvivorA from A] [Remove SurvivorB from B] [Add SurvivorA to B] [Add SurvivorB to A]
//          2. Iteration [Add Move Cooldown to Survivor A]

public interface ICommand{
    void Execute();
    void Undo();
}

public class Executor : MonoBehaviour
{
    Queue<ICommand> queue = new Queue<ICommand>();
    private Stack<ICommand> undo = new Stack<ICommand>();
    public void Enqueue(ICommand command){
        queue.Enqueue(command);
    }

    void Update(){
        while(queue.Count > 0){
            var command = queue.Dequeue();
            command.Execute();
            undo.Push(command);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            undo.Pop().Undo();
        }
    }

    void MovePlayer()
    {
        // send updted position over network
    }

    void DestroyRock()
    {
        // send rock destruction over network
    }
    
}
