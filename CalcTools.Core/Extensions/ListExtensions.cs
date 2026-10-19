namespace CalcTools.Core.Extensions;

public static class ListExtensions
{
    extension<T>(List<T> list)
    {
        public void Replace(T oldItem, T newItem)
        {
            if (!list.Contains(oldItem))
                return;

            var index = list.IndexOf(oldItem);
            list[index] = newItem;
        }

        public void Replace(T oldItem, T newItem, IEqualityComparer<T> comparer)
        {
            if (!list.Contains(oldItem, comparer))
                return;

            var index = list.FindIndex(x => x is not null && comparer.Equals(x, oldItem));
            list[index] = newItem;
        }
    }
}