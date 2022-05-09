export interface CandidateReadModel {
    id: number,
    firstName: number,
    lastName: number,
    email: number
}

export interface CandidatesList {
    candidates: CandidateReadModel[],
    totalCount: number
}