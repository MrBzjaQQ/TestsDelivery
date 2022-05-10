import { CandidateReadModel } from "./candidates";

export type DialogType = 'create' | 'edit';

export interface CandidatesDialogData {
    type: DialogType,
    candidate?: CandidateReadModel
}