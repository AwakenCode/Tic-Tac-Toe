using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Common
{
    public static class GameExtensions
    {
        private static EcsPool<TakenCell> _takenCellPool;

        public static void Init(EcsWorld world)
        {
            _takenCellPool = world.GetPool<TakenCell>();
        }

        public static int GetLongestChain(this IReadOnlyDictionary<Vector2Int, int> cells, Vector2Int position)
        {
            int startCell = cells[position];

            if (_takenCellPool.Has(startCell) == false) return 0;

            var startSign = _takenCellPool.Get(startCell).Value;
            int horizontalLength = GetHorizontalLenght();
            int verticalLength = GetVerticalLength();
            int diagonalLeftToRightLength = GetDiagonalLeftToRight();
            int diagonalRightToLeftLength = GetDiagonalRightToLeft();

            return Mathf.Max(horizontalLength, verticalLength, diagonalLeftToRightLength, diagonalRightToLeftLength);

            int GetVerticalLength()
            {
                var increment = new Vector2Int(0, 1);
                var decrement = new Vector2Int(0, -1);
                return GetCount(cells, startSign, position, increment, decrement);
            }

            int GetHorizontalLenght()
            {
                var increment = new Vector2Int(1, 0);
                var decrement = new Vector2Int(-1, 0);
                return GetCount(cells, startSign, position, increment, decrement);
            }

            int GetDiagonalLeftToRight()
            {
                var increment = new Vector2Int(1, 1);
                var decrement = new Vector2Int(-1, -1);
                return GetCount(cells, startSign, position, increment, decrement);
            }

            int GetDiagonalRightToLeft()
            {
                var increment = new Vector2Int(-1, 1);
                var decrement = new Vector2Int(1, -1);
                return GetCount(cells, startSign, position, increment, decrement);
            }
        }

        private static int GetCount(IReadOnlyDictionary<Vector2Int, int> cells, SignType startSign, 
            Vector2Int position, Vector2Int increment, Vector2Int decrement)
        {
            var currentPosition = position + decrement;
            var length = 1;
                
            while (cells.TryGetValue(currentPosition, out int cell))
            {
                if (_takenCellPool.Has(cell) == false) break;

                var signType = _takenCellPool.Get(cell).Value;
                if(signType != startSign) break;
                
                length++;
                currentPosition += decrement;
            }

            currentPosition = position + increment;
            
            while (cells.TryGetValue(currentPosition, out int cell))
            {
                if (_takenCellPool.Has(cell) == false) break;
                
                var signType = _takenCellPool.Get(cell).Value;
                if(signType != startSign) break;
                
                length++;
                currentPosition += increment;
            }

            return length;
        }
    }
}