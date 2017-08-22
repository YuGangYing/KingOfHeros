using System.Collections.Generic;
using UnityEngine;
using SLG;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
[ExecuteInEditMode]
public class TerrainLayer : SingletonMonoBehaviour<TerrainLayer>
{
    Dictionary<BuildType, SolidityGrid> m_buildTypeList = new Dictionary<BuildType, SolidityGrid>();

    [HideInInspector]
    public SolidityGrid m_grid;

    [System.Serializable]
    public class GridData
    {
        public BuildType buildType; // 建筑类型
        public int cols;
        public int rows;
        public float size;
        [HideInInspector]
        public string value;
        public Vector3 startPos;

        public GridData()
        {

        }

        public GridData(GridData d)
        {
            buildType = d.buildType;
            cols = d.cols;
            rows = d.rows;
            size = d.size;
            value = d.value;
            startPos = d.startPos;
        }

        public bool Has()
        {
            return (cols != 0) && (rows != 0) && (size != 0);
        }

        public SolidityGrid ToGrid()
        {
            SolidityGrid grid = new SolidityGrid();
            grid.Awake(startPos, rows, cols, size, true);
            for (int i = 0; i < value.Length; ++i)
            {
                grid.SetSolidity(i, CharToShort(value[i]));
            }

            return grid;
        }

        public static GridData ToData(SolidityGrid grid)
        {
            GridData data = new GridData();
            ToData(grid, ref data);
            return data;
        }

        static char[] MemtryChar;

        static char ShortToChar(short v)
        {
            switch (v)
            {
            case 0: return '0';
            case 1: return '1';
            case 2: return '2';
            case 3: return '3';
            case 4: return '4';
            case 5: return '5';
            case 6: return '6';
            case 7: return '7';
            case 8: return '8';
            case 9: return '9';
            }

            return '0';
        }

        static short CharToShort(char v)
        {
            switch (v)
            {
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            }

            return 0;
        }

        public static void ToData(SolidityGrid grid, ref GridData data)
        {
            data.cols = grid.Columns;
            data.rows = grid.Rows;
            data.size = grid.CellSize;
            data.startPos = grid.Origin;

            int NumberOfCells = grid.NumberOfCells;
            if (MemtryChar == null || MemtryChar.Length < NumberOfCells)
                MemtryChar = new char[NumberOfCells];

            for (int i = 0; i < NumberOfCells; ++i)
            {
                MemtryChar[i] = ShortToChar(grid.GetGridValue(i));
            }

            data.value = new string(MemtryChar, 0, NumberOfCells);
        }
    }
    public List<GridData> m_gridList = new List<GridData>();

    public GridData m_gridData;
    public bool DeleteCurrent = false; // 是否删除当前索引
    public bool AddToList = false; // 是否添加进去
    public bool SetToCurrent = false; // 设置当前

    public SolidityGrid GetGridType(BuildType type)
    {
        SolidityGrid grid = null;
        if (m_buildTypeList.TryGetValue(type, out grid) == false)
            return null;

        return grid;
    }

    public bool IsCanBuild(BuildType type, Collider collider)
    {
        SolidityGrid grid = GetGridType(type);
        if (grid == null)
            return false;

        return CanBuild(collider, grid);
    }

    // 是否有璧碧
    public static bool CanBuild(Collider collider, SolidityGrid grid)
    {
        if (collider == null)
        {
            return false;
        }

        Bounds bounds = collider.bounds;
        Vector3 upperLeftPos = new Vector3(bounds.min.x, grid.Origin.y, bounds.max.z);
        Vector3 upperRightPos = new Vector3(bounds.max.x, grid.Origin.y, bounds.max.z);
        Vector3 lowerLeftPos = new Vector3(bounds.min.x, grid.Origin.y, bounds.min.z);
        Vector3 lowerRightPos = new Vector3(bounds.max.x, grid.Origin.y, bounds.min.z);
        Vector3 horizDir = (upperRightPos - upperLeftPos).normalized;
        Vector3 vertDir = (upperLeftPos - lowerLeftPos).normalized;
        float horizLength = bounds.size.x;
        float vertLength = bounds.size.z;

        int maxNumObstructedRows = (int)(bounds.size.z / grid.CellSize) + 2;
        int maxNumObstructedCols = (int)(bounds.size.x / grid.CellSize) + 2;
        int maxNumObstructedCells = maxNumObstructedRows * maxNumObstructedCols;
        int numObstructedCellPoolRows = maxNumObstructedRows;
        int numObstructedCellPoolColumns = maxNumObstructedCols;
        for (int rowCount = 0; rowCount < numObstructedCellPoolRows; rowCount++)
        {
            float currentVertLength = rowCount * grid.CellSize;

            for (int colCount = 0; colCount < numObstructedCellPoolColumns; colCount++)
            {
                float currentHorizLength = colCount * grid.CellSize;
                Vector3 testPos = lowerLeftPos + horizDir * currentHorizLength + vertDir * currentVertLength;
                testPos.x = Mathf.Clamp(testPos.x, bounds.min.x, bounds.max.x);
                testPos.z = Mathf.Clamp(testPos.z, bounds.min.z, bounds.max.z);
                if (grid.GetGridValue(testPos) != 1 && grid.GetGridValue(testPos) != 2)
                    return false;

                if (currentHorizLength > horizLength)
                    break;
            }

            if (currentVertLength > vertLength)
                break;
        }

        return true;
    }

    public GridItem.eValueType GetPosValue(BuildType type, Vector3 pos)
    {
        SolidityGrid grid = GetGridType(type);
        if (grid == null)
            return GridItem.eValueType.Null;

        return (GridItem.eValueType)grid.GetGridValue(pos);
    }

    public bool GetAroundCanBuildPos(BuildType type, Vector3 posBase, float sizeX, float sizeY, ref Vector3 posBuild)
    {
        SolidityGrid grid = GetGridType(type);
        if (grid == null)
            return false;

        int maxNumObstructedRows = (int)(sizeY / grid.CellSize) + 2;
        int maxNumObstructedCols = (int)(sizeX / grid.CellSize) + 2;

        int nIndexBase = grid.GetCellIndex(posBase);
        int nColBase = grid.GetColumn(nIndexBase);
        int nRowBase = grid.GetRow(nIndexBase);

        int maxCount = Mathf.Max(grid.Columns - nColBase, nColBase, grid.Rows - nRowBase, nRowBase);

        for (int offset = 0; offset < maxCount; offset++)
        {
            for (int x = -offset; x <= offset; x++)
            {
                for (int y = -offset; y <= offset; y++)
                {
                    if (x != offset && y != offset)
                    {
                        continue;
                    }

                    int nCol = nColBase + x;
                    int nRow = nRowBase + y;
                    Vector3 pos;

                    if (x == 0 && y == 0)
                    {
                        pos = posBase;
                    }
                    else
                    {
                        pos = grid.GetCellCenter(nCol, nRow);
                    }

                    if (PosCanBuild(grid, pos, sizeX, sizeY, maxNumObstructedRows, maxNumObstructedCols))
                    {
                        posBuild = pos;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    bool PosCanBuild(SolidityGrid grid, Vector3 pos, float sizeX, float sizeY, int numRows, int numColumns)
    {
        float radio = Mathf.Max(sizeX, sizeY);
        Collider[] colliders = Physics.OverlapSphere(pos, radio);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.layer != LayerMask.NameToLayer("Terrain") && c.gameObject.layer != LayerMask.NameToLayer("Tree") && c.gameObject.layer != 0)
                return false;
        }

        Vector3 upperLeftPos = new Vector3(pos.x - sizeX, grid.Origin.y, pos.z + sizeY);
        Vector3 upperRightPos = new Vector3(pos.x + sizeX, grid.Origin.y, pos.z + sizeY);
        Vector3 lowerLeftPos = new Vector3(pos.x - sizeX, grid.Origin.y, pos.z - sizeY);
        Vector3 lowerRightPos = new Vector3(pos.x + sizeX, grid.Origin.y, pos.z - sizeY);
        Vector3 horizDir = (upperRightPos - upperLeftPos).normalized;
        Vector3 vertDir = (upperLeftPos - lowerLeftPos).normalized;

        bool posCanBuild = true;
        for (int rowCount = 0; rowCount < numRows; rowCount++)
        {
            float currentVertLength = rowCount * grid.CellSize;

            for (int colCount = 0; colCount < numColumns; colCount++)
            {
                float currentHorizLength = colCount * grid.CellSize;
                Vector3 testPos = lowerLeftPos + horizDir * currentHorizLength + vertDir * currentVertLength;
                testPos.x = Mathf.Clamp(testPos.x, pos.x - sizeX, pos.x + sizeX);
                testPos.z = Mathf.Clamp(testPos.z, pos.z - sizeY, pos.z + sizeY);
                if (grid.GetGridValue(testPos) != 1 && grid.GetGridValue(testPos) != 2)
                {
                    posCanBuild = false;
                    break;
                }

                if (currentHorizLength > sizeX)
                    break;
            }

            if (!posCanBuild || currentVertLength > sizeY)
                break;
        }

        return posCanBuild;
    }

    void Awake()
    {
        m_gridData = null;
        foreach (GridData data in m_gridList)
            m_buildTypeList[data.buildType] = data.ToGrid();
		
        DontDestroyOnLoad(gameObject);
    }

    public void ToGridData()
    {
        if (m_grid == null)
        {
            m_gridData = null;
        }
        else
        {
            if (m_gridData != null)
            {
                GridData.ToData(m_grid, ref m_gridData);
            }
            else
            {
                m_gridData = GridData.ToData(m_grid);
            }
        }
    }

#if UNITY_EDITOR
    public Color m_insideColor = Color.yellow;
    public Color m_outsideColor = Color.grey;
    public bool m_show = true;

    public void OnGUI()
    {
        if (m_gridData == null)
            return;

        if (DeleteCurrent == true)
        {
            DeleteCurrent = false;
            for (int i = 0; i < m_gridList.Count; ++i)
            {
                if (m_gridList[i].buildType == m_gridData.buildType)
                {
                    m_gridList.RemoveAt(i);
                    break;
                }
            }
        }

        if (AddToList == true)
        {
            AddToList = false;
            bool bHas = false;
            for (int i = 0; i < m_gridList.Count; ++i)
            {
                if (m_gridList[i].buildType == m_gridData.buildType)
                {
                    m_gridList[i] = new GridData(m_gridData);
                    bHas = true;
                }
            }

            if (bHas == false)
            {
                m_gridList.Add(new GridData(m_gridData));
            }
        }

        if (SetToCurrent == true)
        {
            SetToCurrent = false;
            for (int i = 0; i < m_gridList.Count; ++i)
            {
                if (m_gridList[i].buildType == m_gridData.buildType)
                {
                    m_gridData = new GridData(m_gridList[i]);
                    m_grid = m_gridData.ToGrid();
                    break;
                }
            }
        }

        if (!m_gridData.Has())
            m_grid = null;
        else
        {
            if (m_grid.Columns != m_gridData.cols ||
                m_grid.Rows != m_gridData.rows)
            {
                m_grid = m_gridData.ToGrid();
            }

            m_grid.CellSize = m_gridData.size;
        }
    }

    void OnDrawGizmos()
    {
        if (m_show && m_grid != null)
        {
            for (int i = 0; i < m_grid.NumberOfCells; i++)
            {
                if (m_grid.GetGridValue(i) == (int)GridItem.eValueType.Null)
                    continue;

                Gizmos.color = (m_grid.GetGridValue(i) == ((int)GridItem.eValueType.Inside) ? m_insideColor : m_outsideColor);
                Vector3 cellPos = m_grid.GetCellCenter(i);
                Vector3 cellSize3 = new Vector3(m_grid.CellSize, 0.5f, m_grid.CellSize);
                Gizmos.DrawCube(cellPos, cellSize3);
            }

            if (m_grid != null)
            {
                m_grid.Origin = transform.position;
            }

            Gizmos.color = Color.white;
            Vector3 startPosition = m_grid.Origin;
            for (int i = 0; i < m_grid.Columns + 1; ++i)
            {
                Vector3 form = startPosition + new Vector3(i * m_grid.CellSize, 0f, 0f);
                Vector3 to = form + new Vector3(0f, 0f, m_grid.Rows * m_grid.CellSize);
                Gizmos.DrawLine(form, to);
            }

            for (int i = 0; i < m_grid.Rows + 1; ++i)
            {
                Vector3 form = startPosition + new Vector3(0f, 0f, i * m_grid.CellSize);
                Vector3 to = form + new Vector3(m_grid.Columns * m_grid.CellSize, 0f, 0f);
                Gizmos.DrawLine(form, to);
            }
        }
    }
#endif
}