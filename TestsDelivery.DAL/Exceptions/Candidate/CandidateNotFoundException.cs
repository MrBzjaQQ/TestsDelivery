namespace TestsDelivery.DAL.Exceptions.Candidate
{
    public class CandidateNotFoundException : CandidateException
    {
        public CandidateNotFoundException(long id)
            : base($"Candidate with id = {id} is not found.")
        {
        }
    }
}
