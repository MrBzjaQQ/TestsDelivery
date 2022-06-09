export interface CandidateReadModel {
    id: number,
    firstName: number,
    lastName: number,
    email: number
}

export interface CandidateCreateModel {
    firstName: number,
    lastName: number,
    email: number
}

export interface CandiadteEditModel {
    id: number,
    firstName: number,
    lastName: number,
    email: number
}

export interface CandidatesList {
    candidates: CandidateReadModel[],
    totalCount: number
}