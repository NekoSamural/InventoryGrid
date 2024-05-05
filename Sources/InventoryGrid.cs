using UnityEngine;

namespace NekoSamurai.Inventory
{
    public class InventoryGrid : MonoBehaviour
    {
        private const int GridSizeX = 10;
        private const int GridSizeY = 20;
        private const int CellSize = 65;

        private CellState[,] _cells = new CellState[GridSizeX, GridSizeY];

        private void Awake()
        {
            ClearGrid();
        }

        private void OnTransformChildrenChanged()
        {
            ClearGrid();

            for (int i = 0; i < transform.childCount; i++)
            {
                SetItemPosition(i);
            }
        }

        private void SetItemPosition(int childIndex)
        {
            var rectTransform = transform.GetChild(childIndex).GetComponent<RectTransform>();
            var xSize = Mathf.CeilToInt(rectTransform.sizeDelta.x / CellSize);
            var ySize = Mathf.CeilToInt(rectTransform.sizeDelta.y / CellSize);

            for (int y = 0; y < GridSizeY; y++)
            {
                for (int x = 0; x < GridSizeX; x++)
                {
                    if (_cells[x, y] == CellState.empty)
                    {
                        if (CheckVoidCells(new Vector2Int(x, y), new Vector2Int(xSize, ySize)))
                        {
                            rectTransform.anchoredPosition = new Vector2(x * CellSize, -y * CellSize);
                            FillCells(new Vector2Int(x, y), new Vector2Int(xSize, ySize));
                            return;
                        }
                    }
                }
            }
        }

        private bool CheckVoidCells(Vector2Int startPos, Vector2Int checkSize)
        {
            if (startPos.x + checkSize.x > GridSizeX || startPos.y + checkSize.y > GridSizeY)
            {
                return false;
            }

            for (int y = startPos.y; y < startPos.y + checkSize.y; y++)
            {
                for (int x = startPos.x; x < startPos.x + checkSize.x; x++)
                {
                    if (_cells[x, y] == CellState.filled)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void FillCells(Vector2Int startPos, Vector2Int fillSize)
        {
            for (int y = startPos.y; y < startPos.y + fillSize.y; y++)
            {
                for (int x = startPos.x; x < startPos.x + fillSize.x; x++)
                {
                    _cells[x, y] = CellState.filled;
                }
            }
        }

        private void ClearGrid()
        {
            for (int i = 0; i < GridSizeX; i++)
            {
                for (int k = 0; k < GridSizeY; k++)
                {
                    _cells[i, k] = CellState.empty;
                }
            }
        }
    }
}