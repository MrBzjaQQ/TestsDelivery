interface AnswerOptionModelBase {
  text: string,
  isCorrect: boolean
}

export interface AnswerOption extends AnswerOptionModelBase {
  id?: number
}

export interface AnswerOptionCreateModel extends AnswerOptionModelBase {}

export interface AnswerOptionReadModel extends AnswerOptionModelBase {
  id: number
}

export interface AnswerOptionEditModel extends AnswerOptionModelBase {
  id?: number
}

export interface AnswerOptionInTestModel {
  id: number,
  text: string
}

export interface AnswerOptionUnverified {
  id: number,
  text: string
}