import { ComponentType } from "../components";
import { SubjectReadModel } from "../subjects";

export interface SubjectsDialogData {
  type: ComponentType,
  subject?: SubjectReadModel
}