export interface SubjectReadModel {
  id: number,
  name: string
}

export interface SubjectEditModel {
  id: number,
  name: string
}

export interface SubjectCreateModel {
  name: string
}

export interface SubjectInListModel {
  id: number,
  name: string
}

export interface SubjectsListModel {
  totalCount: number,
  subjects: SubjectInListModel[]
}