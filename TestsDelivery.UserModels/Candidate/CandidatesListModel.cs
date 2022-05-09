namespace TestsDelivery.UserModels.Candidate
{
    public record CandidatesListModel
    {
        public IEnumerable<CandidateReadModel> Candidates { get; set; }

        public int TotalCount { get; set; }
    }
}
