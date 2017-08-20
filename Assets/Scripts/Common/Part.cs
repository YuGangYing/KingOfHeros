
public class Pair<TKey, TValue>
{
    public Pair(TKey key, TValue value)
    {
        k = key;
        v = value;
    }

    public TKey first { get { return k; } set { k = value; } }
    public TValue second { get { return v; } set { v = value; } }

    private TKey k;
    private TValue v;
}