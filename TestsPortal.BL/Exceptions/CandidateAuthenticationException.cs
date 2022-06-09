namespace TestsPortal.BL.Exceptions
{
    public class CandidateAuthenticationException : Exception
    {
        public CandidateAuthenticationException()
            : base("Keycode and Pin are incorrect for this test")
        {
        }
    }
}
