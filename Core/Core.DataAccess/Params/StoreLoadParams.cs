namespace Core.DataAccess.Params
{
    public class StoreLoadParams
    {
        public int Start { get; set; }
        public int Length { get; set; }
    }

    public class FilteredStoreLoadParams<T> : StoreLoadParams
    {
        public T Filter { get; set; }
    }
}
