import { CandidateReadModel } from "./candidates";
import { SubjectReadModel } from "./subjects";

export type DialogType = 'create' | 'edit';

export interface CandidatesDialogData {
    type: DialogType,
    candidate?: CandidateReadModel
}

export interface SubjectsDialogData {
    type: DialogType,
    subject?: SubjectReadModel
}