export interface ListFilter {
  take?: number,
  skip?: number,
  searchText?: string
}

export interface QuestionsInSubjectListFilterModel extends ListFilter {
  subjectId: number
}