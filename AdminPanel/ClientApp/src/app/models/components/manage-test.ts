export interface QuestionsTreeNode {
  id: number,
	name: string,
	level: number,
	children: QuestionsTreeNode[]
}