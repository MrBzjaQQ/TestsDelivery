export interface ApiResponse<T> {
  isError: boolean;
  timestamp: string;
  message: string;
  data: T;
}

export interface StudentProfile {
  studentId: string;
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  group?: Group;
  profile?: StudentProfileDetails;
  statistics?: StudentStatistics;
  recentTests: RecentTest[];
}

export interface Group {
  id: string;
  name: string;
  startYear: number;
  endYear: number;
}

export interface StudentProfileDetails {
  avatarUrl?: string;
  bio?: string;
}

export interface StudentStatistics {
  totalTests: number;
  completedTests: number;
  inProgressTests: number;
  averageScore: number;
}

export interface RecentTest {
  testId: string;
  testTitle: string;
  status: string;
  score: number;
  maxScore: number;
  percentages: number;
  isPassed: boolean;
  completedAt?: string;
}

export interface AvailableTests {
  tests: AvailableTest[];
  totalCount: number;
}

export interface AvailableTest {
  testId: string;
  testTitle: string;
  description: string;
  durationMinutes: number;
  passingScore: number;
  maxAttempts: number;
  attemptsUsed: number;
  status: string;
  availableFrom?: string;
  availableUntil?: string;
  questionsCount: number;
  canTake: boolean;
  score?: number;
  maxScore?: number;
  percentage?: number;
  isPassed?: boolean;
  completedAt?: string;
}

export interface TestQuestions {
  testId: string;
  testTitle: string;
  testDescription: string;
  durationMinutes: number;
  passingScore: number;
  questions: TestQuestion[];
  totalQuestions: number;
  totalPoints: number;
}

export interface TestQuestion {
  questionId: string;
  text: string;
  category: string;
  difficulty: string;
  order: number;
  options: QuestionOption[];
  imageFileId?: string;
  answerType?: string;
  maxLength?: number;
}

export interface QuestionOption {
  optionId: string;
  text: string;
  order: number;
}

export interface StartTestResponse {
  testId: string;
  studentId: string;
  status: string;
  startedAt: string;
  deadline: string;
  attemptsUsed: number;
  questionsCount: number;
}

export interface SubmitTestRequest {
  answers: AnswerItem[];
}

export interface AnswerItem {
  questionId: string;
  selectedOptionIds: string[];
  textAnswer?: string;
}

export interface SubmitTestResponse {
  testAssignmentId: string;
  testId: string;
  studentId: string;
  status: string;
  submittedAt: string;
  attemptNumber: number;
  answersCount: number;
}

export interface TestResults {
  testId: string;
  testTitle: string;
  score: number;
  maxScore: number;
  percentage: number;
  isPassed: boolean;
  passedThreshold: number;
  attemptNumber: number;
  completedAt: string;
  answers: AnswerResult[];
  canRetake: boolean;
  attemptsRemaining: number;
}

export interface AnswerResult {
  questionId: string;
  questionText: string;
  selectedOptionIds: string[];
  isCorrect: boolean;
  pointsEarned: number;
  correctOptionIds: string[];
  textAnswer?: string;
  feedback?: string;
}

export interface GroupAnalytics {
  groupId: string;
  groupName: string;
  totalStudents: number;
  activeStudents: number;
  completedTests: number;
  averageScore: number;
  passRate: number;
  topPerformers: TopPerformer[];
  groupProgress?: GroupProgress;
}

export interface TopPerformer {
  studentId: string;
  studentName: string;
  averageScore: number;
}

export interface GroupProgress {
  testsCompleted: number;
  testsInProgress: number;
  totalTestsAssigned: number;
}
