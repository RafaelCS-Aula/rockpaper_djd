using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rockpaper_djd
{
    public class HealthBehaviour : MonoBehaviour, IUseTeams
    {

        [SerializeField] private int _maxHp;
        private int _currentHp;
        [SerializeField] private int _startingHp;
        [SerializeField] private bool startWithMaxHp = true;

        [SerializeField] private float _damagedCooldown;
        private float _currentCooldown;

        public int teamID { get; set; }

        // Start is called before the first frame update
        void Awake()
        {
            if (startWithMaxHp)
                _startingHp = _maxHp;

            _currentHp = _startingHp;

            _currentCooldown = _damagedCooldown;
        }

        void Update()
        {
            _currentCooldown += Time.deltaTime;

            if (_currentHp <= 0)
                Die();

            _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);
        }

        public void InteractFriend(IUseTeams other) { }
        public void InteractEnemy(IUseTeams other)
        {
            if (_currentCooldown >= _damagedCooldown)
            {
                _currentHp--;
                _currentCooldown = 0;
            }



        }

        void Die()
        {
            Destroy(gameObject);
        }

    }
}
