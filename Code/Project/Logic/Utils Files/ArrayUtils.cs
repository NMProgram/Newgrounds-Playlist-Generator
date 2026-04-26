public static class ArrayUtils
{
    extension(byte[] bytes)
    {
        public string Title
        {
            get
            {
                using MemoryStream ms = new(bytes);
                var file = TagLib.File.Create(new PathAbstraction("in-memory.mp3", ms));
                return file.Tag.Title;
            }
        }
    }
    public static string FormatAudio(this object a) => (a as byte[])!.Title;
    public static void Deconstruct<T1, T2>(this object[] arr, out T1 first, out T2 second)
    {
        if (arr is null || arr.Length == 0)
        { throw new ArgumentOutOfRangeException(nameof(arr), "An empty or null array cannot be deconstructed."); }
        first = (T1)arr[0];
        second = (T2)arr[1];
    }
    public static void Deconstruct<T1, T2, T3>(this object[] arr, out T1 first, out T2 second, out T3 third)
    {
        arr.Deconstruct(out first, out second);
        third = (T3)arr[2];
    }
    public static void Deconstruct<T1, T2, T3, T4>(this object[] arr, out T1 first, out T2 second, out T3 third, out T4 fourth)
    {
        arr.Deconstruct(out first, out second, out third);
        fourth = (T4)arr[3];
    }
    public static void Deconstruct<T1, T2, T3, T4, T5>(this object[] arr, out T1 first, out T2 second, out T3 third, out T4 fourth, out T5 fifth)
    {
        arr.Deconstruct(out first, out second, out third, out fourth);
        fifth = (T5)arr[4];
    }
    public static void Deconstruct<T1, T2, T3, T4, T5, T6>(this object[] arr, out T1 first, out T2 second, out T3 third, out T4 fourth, out T5 fifth, out T6 sixth)
    {
        arr.Deconstruct(out first, out second, out third, out fourth, out fifth);
        sixth = (T6)arr[5];
    }
    public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7>(this object[] arr, out T1 first, out T2 second, out T3 third, out T4 fourth, out T5 fifth, out T6 sixth, out T7 seventh)
    {
        arr.Deconstruct(out first, out second, out third, out fourth, out fifth, out sixth);
        seventh = (T7)arr[6];
    }
}