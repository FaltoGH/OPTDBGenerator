namespace libKHOpenAPI
{
    public struct RqResult
    {
        public int Return;
        public string PrevNext;
        public RqResult(int retur, string prevNext)
        {
            Return = retur;
            PrevNext = prevNext;
        }
    }
}
