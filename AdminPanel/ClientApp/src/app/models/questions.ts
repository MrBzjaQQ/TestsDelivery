export enum QuestionType {
  SingleChoice,
  MultipleChoice,
  Essay
}

export interface ShortQuestionModel {
  id: number,
  name: string,
  type: QuestionType
}

export interface QuestionsListModel {
  questions: ShortQuestionModel[],
  totalCount: number
}
