namespace TestsDelivery.DAL.Exceptions.Candidate
{
    // TODO: move exceptions to BL
    public class CandidateNotFoundException : CandidateException
    {
        public CandidateNotFoundException(long id)
            : base($"Candidate with id = {id} is not found.")
        {
        }
    }
}
