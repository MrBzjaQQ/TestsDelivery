import { CandidateReadModel } from "./candidates";
import { SubjectReadModel } from "./subjects";

export type ComponentType = 'create' | 'edit';

export interface CandidatesDialogData {
  type: ComponentType,
  candidate?: CandidateReadModel
}

export interface SubjectsDialogData {
  type: ComponentType,
  subject?: SubjectReadModel
}