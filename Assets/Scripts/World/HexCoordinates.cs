using System;
using UnityEngine;

[Serializable]
public struct HexCoordinates
{
    [SerializeField] private int q;
    [SerializeField] private int r;

    public int Q => q;
    public int R => r;

    public HexCoordinates(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    public HexCoordinates GetNeighbor(HexDirection direction)
    {
        return direction switch
        {
            HexDirection.North =>
                new HexCoordinates(q, r - 1),

            HexDirection.NorthEast =>
                new HexCoordinates(q + 1, r - 1),

            HexDirection.SouthEast =>
                new HexCoordinates(q + 1, r),

            HexDirection.South =>
                new HexCoordinates(q, r + 1),

            HexDirection.SouthWest =>
                new HexCoordinates(q - 1, r + 1),

            HexDirection.NorthWest =>
                new HexCoordinates(q - 1, r),

            _ => this
        };
    }
}