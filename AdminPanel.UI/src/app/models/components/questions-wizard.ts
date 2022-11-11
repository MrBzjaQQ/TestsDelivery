import { AnswerOption } from "../answerOptions";

export interface ScqModel {
  name: string,
  text: string,
  answerOptions: AnswerOption[]
}

export interface McqModel {
  name: string,
  text: string,
  answerOptions: AnswerOption[]
}

export interface EssayModel {
  name: string,
  text: string
}