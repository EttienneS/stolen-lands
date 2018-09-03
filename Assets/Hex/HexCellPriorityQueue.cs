using System.Collections.Generic;

public class HexCellPriorityQueue
{
    private readonly List<HexCell> list = new List<HexCell>();
    private int minimum = int.MaxValue;

    public HexCellPriorityQueue()
    {
        Count = 0;
    }

    public int Count { get; private set; }

    public void Enqueue(HexCell cell)
    {
        Count += 1;
        var priority = cell.SearchPriority;

        if (priority < minimum)
        {
            minimum = priority;
        }

        while (priority >= list.Count)
        {
            list.Add(null);
        }

        cell.NextWithSamePriority = list[priority];

        list[priority] = cell;
    }

    public HexCell Dequeue()
    {
        Count -= 1;

        for (; minimum < list.Count; minimum++)
        {
            var cell = list[minimum];
            if (cell != null)
            {
                list[minimum] = cell.NextWithSamePriority;
                return cell;
            }
        }

        return null;
    }

    public void Change(HexCell cell, int oldPriority)
    {
        var current = list[oldPriority];
        var next = current.NextWithSamePriority;

        if (current == cell)
        {
            list[oldPriority] = next;
        }
        else
        {
            while (next != cell)
            {
                current = next;
                next = current.NextWithSamePriority;
            }

            current.NextWithSamePriority = cell.NextWithSamePriority;
        }

        Enqueue(cell);
        Count -= 1;
    }

    public void Clear()
    {
        list.Clear();
        Count = 0;
        minimum = int.MaxValue;
    }
}