import { QuestionReadModel } from "./questions"

export interface TestInListModel {
  id: number,
  name: string
}

export interface TestCreateModel {
  name: string,
  questionIds: number[]
}

export interface TestEditModel {
  id: number,
  name: string,
  questionIds: number[]
}

export interface TestReadModel {
  id: number,
  name: string,
  questionIds: QuestionReadModel[]
}

export interface TestsList {
  tests: TestInListModel[],
  totalCount: number
}