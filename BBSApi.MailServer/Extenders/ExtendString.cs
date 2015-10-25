namespace BBSApi.MailServer.Extenders
{
    public static class ExtendString
    {
        public static string Fmt(this string fmt, params object[] args)
        {
            return string.Format(fmt, args);
        }
    }
}