import { AnswerOptionCreateModel, AnswerOptionReadModel, AnswerOptionEditModel, AnswerOption } from './answerOptions';
import { SubjectReadModel } from './subjects';

export enum QuestionType {
  SingleChoice,
  MultipleChoice,
  Essay,
  Unknown = 99
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

export interface ScqCreateModel {
  name: string,
  text: string,
  subjectId: number,
  answerOptions: AnswerOptionCreateModel[]
}

export interface ScqReadModel {
  id: number,
  name: string,
  text: string,
  subject: SubjectReadModel,
  answerOptions: AnswerOptionReadModel[]
}

export interface ScqEditModel {
  id: number,
  name: string,
  text: string,
  subjectId: number,
  answerOptions: AnswerOptionEditModel[]
}

export interface McqCreateModel {
  name: string,
  text: string,
  subjectId: number,
  answerOptions: AnswerOptionCreateModel[]
}

export interface McqReadModel {
  id: number,
  name: string,
  text: string,
  subject: SubjectReadModel,
  answerOptions: AnswerOptionReadModel[]
}

export interface McqEditModel {
  id: number,
  name: string,
  text: string,
  subjectId: number,
  answerOptions: AnswerOptionEditModel[]
}

export interface EssayCreateModel {
  name: string,
  text: string,
  subjectId: number
}

export interface EssayReadModel {
  id: number,
  name: string,
  text: string,
  subject: SubjectReadModel
}

export interface EssayEditModel {
  id: number,
  name: string,
  text: string,
  subjectId: number,
}

export interface QuestionReadModel {
  id: number,
  name: string,
  text: string,
  subject: SubjectReadModel
}

export interface QuestionInSubjectModel {
  id: number,
  name: string,
  type: QuestionType
}

export interface SubjectWithQuestionsModel {
  id: number,
  name: string,
  questions: QuestionInSubjectModel
}