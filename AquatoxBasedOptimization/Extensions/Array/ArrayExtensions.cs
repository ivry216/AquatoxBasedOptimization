namespace AquatoxBasedOptimization.Extensions.Array
{
    static class ArrayExtensions
    {
        public static void CopyFrom<T>(this T[] current, T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                current[i] = array[i];
            }
        }
    }
}
