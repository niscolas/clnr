using System;
using UnityEngine;

namespace _Scripts.Enemies {
    public class RandomEnemySpawner : EnemySpawner {

        private enum EnemyTier {
            WEAK = 0,
            MEDIUM = 1,
            STRONG = 2,
            ELITE = 3,
            BOSS = 4
        }

        [Serializable]
        private class EnemyTierStats {
            [SerializeField]
            private EnemyTier enemyTier;

            [Header ("Damage Range")]
            [SerializeField]
            private float minEnemyDamage;
            [SerializeField]
            private float maxEnemyDamage;

            [Header ("Health Range")]
            [SerializeField]
            private float minEnemyHealth;
            [SerializeField]
            private float maxEnemyHealth;

            [Header ("Speed Range")]
            [SerializeField]
            private float minEnemySpeed;
            [SerializeField]
            private float maxEnemySpeed;

            [Header ("Enemy per Tier Range")]
            [SerializeField]
            private float minEnemiesOfTier;
            [SerializeField]
            private float maxEnemiesOfTier;

            internal EnemyTier EnemyTier { get { return enemyTier; } }

            internal float MinEnemyDamage { get { return minEnemyDamage; } }
            internal float MaxEnemyDamage { get { return maxEnemyDamage; } }

            internal float MinEnemyHealth { get { return minEnemyHealth; } }
            internal float MaxEnemyHealth { get { return maxEnemyHealth; } }

            internal float MinEnemySpeed { get { return minEnemyHealth; } }
            internal float MaxEnemySpeed { get { return maxEnemySpeed; } }
        }

        [SerializeField]
        private EnemyTierStats[] enemyTierStats;

        [Serializable]
        private class EnemyOfTier {
            [SerializeField]
            private EnemyTier enemyTier;

            [SerializeField]
            private Enemy enemyPrefab;

            [Header ("Num of Enemies per Type")]
            [SerializeField]
            private int minEnemiesPerType;
            [SerializeField]
            private int maxEnemiesPerType;

            public EnemyTier EnemyTier { get { return enemyTier; } }
            public Enemy EnemyPrefab { get { return enemyPrefab; } }
            public int MinEnemiesOfTier { get { return minEnemiesPerType; } }
            public int MaxEnemiesOfTier { get { return maxEnemiesPerType; } }
        }

        [SerializeField]
        private EnemyOfTier[] enemiesOfTiers;

        [SerializeField]
        private Transform[] possiblePaths;

        [SerializeField]
        private Transform[] possibleSpawnPoints;

        private System.Random rand;

        private void Shuffle (System.Object[] mbArray) {
            for (int i = 0; i < mbArray.Length; i++) {
                System.Object temp = mbArray[i];
                int r = rand.Next (i, mbArray.Length);
                mbArray[i] = mbArray[r];
                mbArray[r] = temp;
            }
        }

        private void Awake () {
            rand = new System.Random ();

            Shuffle (enemiesOfTiers);

            foreach (EnemyOfTier eot in enemiesOfTiers) {
                SetEnemyStats (eot);

                WaveComponent wc = new WaveComponent ();
                wc.EnemyPrefab = eot.EnemyPrefab;
                wc.WaypointsParentGO = possiblePaths[rand.Next (0, possiblePaths.Length)];
                wc.NumOfEnemies = rand.Next (eot.MinEnemiesOfTier, eot.MaxEnemiesOfTier);
            }
        }

        private void SetEnemyStats (EnemyOfTier eot) {
            EnemyTierStats ets = enemyTierStats[(int) eot.EnemyTier];
            float damage = rand.Next ((int) ets.MinEnemyDamage, (int) ets.MaxEnemyDamage + 1);
            float health = rand.Next ((int) ets.MinEnemyHealth, (int) ets.MaxEnemyHealth + 1);
            float speed = rand.Next ((int) ets.MinEnemySpeed, (int) ets.MaxEnemySpeed + 1);

            eot.EnemyPrefab.Damage = damage;
            eot.EnemyPrefab.Health = health;
            eot.EnemyPrefab.Speed = speed;
        }
    }
}