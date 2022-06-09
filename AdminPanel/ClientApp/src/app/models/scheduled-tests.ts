import { CandidateReadModel } from "./candidates";

export interface ScheduledTestInList {
  id: number,
  testName: string,
  pin: string,
  keycode: string,
  candidate: CandidateReadModel,
  duration: number,
  startDateTime: string,
  expirationDateTime: string
}

export interface ScheduledTestsListModel {
  scheduledTests: ScheduledTestInList[],
  totalCount: number;
}

export interface ScheduledTestCreateModel {
  testId: number,
  startDateTime: string,
  expirationDateTime: string,
  duration: number,
  candidateIds: number[],
  destinationInstance: string
}

export interface ScheduledTestReadModel {
  id: number,
  candidates: CandidateReadModel[],
  duration: number,
  startDateTime: string,
  expirationDateTime: string
}