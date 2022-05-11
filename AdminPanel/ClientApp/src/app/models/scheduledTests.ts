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