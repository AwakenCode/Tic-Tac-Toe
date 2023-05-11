using System.Collections.Generic;
using Common;
using Components;
using Leopotam.EcsLite;
using NUnit.Framework;
using Systems;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public class ChainLengthTest
    { 
        private const int FieldHeight = 3;
        private const int FieldWidth = 3;
        
        private Dictionary<Vector2Int, int> _cells;
        private EcsWorld _world;
        private EcsPool<Position> _positionPool;
        private EcsPool<TakenCell> _takenCellPool;
        
        [Test]
        public void CheckChainZero()
        {
            InitWorld();
            
            int chainLength = _cells.GetLongestChain(Vector2Int.zero);
            
            DestroyWorld();
            Assert.AreEqual(0, chainLength);    
        }

        [Test]
        public void CheckHorizontalChainOne()
        {
            InitWorld();
            _takenCellPool.Add(_cells[Vector2Int.zero]).Value = SignType.Circle;
            int chainLength = _cells.GetLongestChain(Vector2Int.zero);

            DestroyWorld();
            Assert.AreEqual(1, chainLength);    
        }

        [Test]
        public void CheckHorizontalChainTwoLeft()
        {
            InitWorld();
            _takenCellPool.Add(_cells[new Vector2Int(2, 0)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(1, 0)]).Value = SignType.Circle;
            int chainLength = _cells.GetLongestChain(new Vector2Int(2, 0));

            DestroyWorld();
            Assert.AreEqual(2, chainLength);    
        }

        [Test]
        public void CheckHorizontalChainThreeLeft()
        {
            InitWorld();
            _takenCellPool.Add(_cells[new Vector2Int(2, 0)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(1, 0)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(0, 0)]).Value = SignType.Circle;
            int chainLength = _cells.GetLongestChain(new Vector2Int(2, 0));

            DestroyWorld();
            Assert.AreEqual(3, chainLength);    
        }

        [Test]
        public void CheckHorizontalChainTwoRight()
        {
            InitWorld();
            _takenCellPool.Add(_cells[new Vector2Int(2, 0)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(1, 0)]).Value = SignType.Circle;
            int chainLength = _cells.GetLongestChain(new Vector2Int(1, 0));

            DestroyWorld();
            Assert.AreEqual(2, chainLength);    
        }
        
        [Test]
        public void CheckHorizontalChainThreeRight()
        {
            InitWorld();
            _takenCellPool.Add(_cells[new Vector2Int(2, 0)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(1, 0)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(0, 0)]).Value = SignType.Circle;
            int chainLength = _cells.GetLongestChain(new Vector2Int(0, 0));

            DestroyWorld();
            Assert.AreEqual(3, chainLength);    
        }

        [Test]
        public void CheckVerticalChainTwoUp()
        {
            InitWorld();
            _takenCellPool.Add(_cells[new Vector2Int(0, 2)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(0, 1)]).Value = SignType.Circle;
            int chainLength = _cells.GetLongestChain(new Vector2Int(0, 1));

            DestroyWorld();
            Assert.AreEqual(2, chainLength);    
        }
        
        [Test]
        public void CheckVerticalChainTwoDown()
        {
            InitWorld();
            _takenCellPool.Add(_cells[new Vector2Int(0, 1)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(0, 0)]).Value = SignType.Circle;
            int chainLength = _cells.GetLongestChain(new Vector2Int(0, 1));

            DestroyWorld();
            Assert.AreEqual(2, chainLength);    
        }

        [Test]
        public void CheckDiagonalChainFromLeftToRight()
        {
            InitWorld();
            _takenCellPool.Add(_cells[new Vector2Int(2, 2)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(1, 1)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(0, 0)]).Value = SignType.Circle;
            int chainLength = _cells.GetLongestChain(new Vector2Int(1, 1));

            DestroyWorld();
            Assert.AreEqual(3, chainLength);    
        }

        [Test]
        public void CheckDiagonalChainFromRightToLeft()
        {
            InitWorld();
            _takenCellPool.Add(_cells[new Vector2Int(0, 2)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(1, 1)]).Value = SignType.Circle;
            _takenCellPool.Add(_cells[new Vector2Int(2, 0)]).Value = SignType.Circle;
            int chainLength = _cells.GetLongestChain(new Vector2Int(1, 1));

            DestroyWorld();
            Assert.AreEqual(3, chainLength);    
        }
        
        private void InitWorld()
        {
            _world = new EcsWorld();
            _positionPool = _world.GetPool<Position>();
            _takenCellPool = _world.GetPool<TakenCell>();
            InitCells();
            GameExtensions.Init(_world);
        }

        private void DestroyWorld()
        {
            _cells.Clear();
            _world.Destroy();
            _world = null;
        }

        private void InitCells()
        {
            _cells = new Dictionary<Vector2Int, int>();
            
            for (var x = 0; x < FieldWidth; x++)
            {
                for (var y = 0; y < FieldHeight; y++)
                {
                    var position = new Vector2Int(x, y);
                    _cells.Add(position, CreateCell(position));
                }
            }
        }

        private int CreateCell(Vector2Int position)
        {
            int entity = _world.NewEntity();
            _positionPool.Add(entity).Value = position;
            return entity;
        }
    }
}