import { CandidateReadModel } from "../candidates";
import { ComponentType } from "../components";

export interface CandidatesDialogData {
  type: ComponentType,
  candidate?: CandidateReadModel
}