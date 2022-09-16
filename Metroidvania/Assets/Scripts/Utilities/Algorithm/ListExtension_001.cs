namespace System.Collections.Generic
{
    public static partial class ListExtension
    {
        public static void CheckCapacity<T>(this List<T> array, int targetCapacity)
        {
            int currentCount = array.Count;

            for(int i = currentCount; i < targetCapacity; i++)
                array.Add(default(T));

            while(array.Count > targetCapacity)
                array.RemoveAt(targetCapacity);
        }
    }
}